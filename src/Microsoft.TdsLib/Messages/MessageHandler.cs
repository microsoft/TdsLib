using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Exceptions;
using Microsoft.TdsLib.Packets;
using Microsoft.TdsLib.Payloads;

namespace Microsoft.TdsLib.Messages
{
    /// <summary>
    /// Message handler.
    /// </summary>
    public class MessageHandler
    {

        private readonly TdsClient tdsClient;
        private ByteBuffer incomingMessageBuffer;

        internal MessageHandler(TdsClient tdsClient)
        {
            this.tdsClient = tdsClient;
        }

        /// <summary>
        /// Send messages to the SQL server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task.</returns>
        /// <exception cref="ConnectionClosedException">If the connection is unexpectedly closed.</exception>
        /// <exception cref="IOException">If an IO problem occur.</exception>
        public async Task SendMessage(Message message, CancellationToken cancellationToken = default)
        {
            foreach (var packet in message.GetPackets(tdsClient.Connection.Options.PacketSize))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await tdsClient.Connection.SendData(packet.Buffer).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receives a message from the SQL Server.
        /// </summary>
        /// <param name="payloadGenerator">The payload generator.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="P">The Payload type.</typeparam>
        /// <returns>Awaitable task. Message.</returns>
        /// <exception cref="ConnectionClosedException">If the connection is unexpectedly closed.</exception>
        /// <exception cref="IOException">If an IO problem occur.</exception>
        public async Task<Message> ReceiveMessage<P>(Func<ByteBuffer, P> payloadGenerator, CancellationToken cancellationToken = default) where P : Payload
        {
            try
            {
                (PacketType packetType, ByteBuffer payloadBuffer) = await ReceiveMessageRaw(cancellationToken).ConfigureAwait(false);

                Message message = new Message(packetType)
                {
                    Payload = payloadGenerator(payloadBuffer)
                };

                return message;
            }
            catch (OperationCanceledException)
            {
                incomingMessageBuffer = null;
                throw;
            }
        }

        /// <summary>
        /// Receives a message from the SQL Server.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task. Message.</returns>
        /// <exception cref="ConnectionClosedException">If the connection is unexpectedly closed.</exception>
        /// <exception cref="IOException">If an IO problem occur.</exception>
        public async Task<Message> ReceiveMessage(CancellationToken cancellationToken = default)
        {
            try
            {
                (PacketType packetType, ByteBuffer payloadBuffer) = await ReceiveMessageRaw(cancellationToken).ConfigureAwait(false);

                Message message = new Message(packetType)
                {
                    Payload = new RawPayload(payloadBuffer)
                };

                return message;
            }
            catch (OperationCanceledException)
            {
                incomingMessageBuffer = null;
                throw;
            }
        }

        private async Task<(PacketType, ByteBuffer)> ReceiveMessageRaw(CancellationToken cancellationToken = default)
        {
            if (incomingMessageBuffer is null)
            {
                incomingMessageBuffer = await tdsClient.Connection.ReceiveData().ConfigureAwait(false);
            }
            else
            {
                incomingMessageBuffer = incomingMessageBuffer.Concat(await tdsClient.Connection.ReceiveData().ConfigureAwait(false));
            }

            List<Packet> packetList = new List<Packet>();

            while (true)
            {
                await WaitForData(Packet.HeaderLength, cancellationToken).ConfigureAwait(false);

                ushort packetLength = incomingMessageBuffer.ReadUInt16BE(PacketOffset.Length);

                await WaitForData(packetLength, cancellationToken).ConfigureAwait(false);

                Packet packet = new Packet(incomingMessageBuffer.Slice(0, packetLength));
                packetList.Add(packet);

                if (packetLength < incomingMessageBuffer.Length)
                {
                    incomingMessageBuffer = incomingMessageBuffer.Slice(packetLength);
                }
                else
                {
                    incomingMessageBuffer = new ByteBuffer(0);
                }

                if (packet.Last)
                {
                    break;
                }

                cancellationToken.ThrowIfCancellationRequested();
            }

            ByteBuffer payloadBuffer = new ByteBuffer(packetList.Select(p => p.Data));
            return (packetList.FirstOrDefault()?.Type ?? PacketType.Unknown, payloadBuffer);
        }

        private async Task WaitForData(int size, CancellationToken cancellationToken = default)
        {
            while (incomingMessageBuffer.Length < size)
            {
                cancellationToken.ThrowIfCancellationRequested();
                incomingMessageBuffer = incomingMessageBuffer.Concat(await tdsClient.Connection.ReceiveData().ConfigureAwait(false));
            }
        }

    }
}

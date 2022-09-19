using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Exceptions;
using Microsoft.TdsLib.Messages;
using Microsoft.TdsLib.Packets;
using Microsoft.TdsLib.Payloads;

namespace Microsoft.TdsLib.IO
{
    internal class PreLoginTlsWrapperStream : Stream
    {
        private readonly ushort packetSize;
        private readonly Stream InnerStream;
        private ByteBuffer availableData;

        public override bool CanRead => InnerStream.CanRead;

        public override bool CanSeek => InnerStream.CanSeek;

        public override bool CanWrite => InnerStream.CanWrite;

        public override long Length => InnerStream.Length;

        public override long Position { get => InnerStream.Position; set => InnerStream.Position = value; }

        public PreLoginTlsWrapperStream(ushort packetSize, Stream stream)
        {
            this.packetSize = packetSize;
            InnerStream = stream;
        }

        public override void Flush() => InnerStream.Flush();

        private ByteBuffer ReadData()
        {
            byte[] buffer = new byte[packetSize];
            int size = InnerStream.Read(buffer, 0, buffer.Length);

            if (size == 0)
            {
                throw new ConnectionClosedException("Connection closed unexpectedly. Read returned 0 bytes.");
            }

            if (size != buffer.Length)
            {
                return new ByteBuffer(buffer, 0, size);
            }

            return new ByteBuffer(buffer);
        }

        private void WaitForData(ref ByteBuffer byteBuffer, int size)
        {
            while (byteBuffer.Length < size)
            {
                byteBuffer = byteBuffer.Concat(ReadData());
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (availableData != null)
            {
                int sizeToCopy = Math.Min(count, availableData.Length);
                availableData.CopyTo(buffer, offset, sizeToCopy);

                if (sizeToCopy == availableData.Length)
                {
                    availableData = null;
                }
                else
                {
                    availableData = availableData.Slice(sizeToCopy);
                }

                return sizeToCopy;
            }

            ByteBuffer byteBuffer = ReadData();

            List<Packet> packetList = new List<Packet>();

            while (true)
            {
                WaitForData(ref byteBuffer, Packet.HeaderLength);

                ushort packetLength = byteBuffer.ReadUInt16BE(PacketOffset.Length);

                WaitForData(ref byteBuffer, packetLength);

                Packet packet = new Packet(byteBuffer.Slice(0, packetLength));
                packetList.Add(packet);

                if (packet.Last)
                {
                    break;
                }

                if (packetLength < byteBuffer.Length)
                {
                    byteBuffer = byteBuffer.Slice(packetLength);
                }
                else
                {
                    byteBuffer = new ByteBuffer(0);
                }
            }

            availableData = new ByteBuffer(packetList.Select(p => p.Data));
            return Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);

        public override void SetLength(long value) => InnerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            Message message = new Message(PacketType.PreLogin)
            {
                Payload = new RawPayload(new ByteBuffer(buffer, offset, count))
            };

            foreach (var packet in message.GetPackets(packetSize))
            {
                packet.Buffer.CopyTo(InnerStream);
            }
        }

    }
}

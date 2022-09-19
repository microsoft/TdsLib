// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Packets;
using Microsoft.TdsLib.Payloads;

namespace Microsoft.TdsLib.Messages
{
    /// <summary>
    /// TDS Message. 
    /// A message contains both metadata such as the <see cref="PacketType"/>, <see cref="ResetConnection"/> and <see cref="Ignore"/> as well as the data payload. 
    /// A message is transmitted in several packets, the last packet has the <i>Last</i> bit set.
    /// 
    /// <code>
    /// Message structure in the network:<br/>
    /// <see cref="Packet"/> | <see cref="Packet"/> | ... | <see cref="Packet"/> [Packet.Last]<br/>
    /// </code>
    /// </summary>
    /// 
    public class Message
    {
        /// <summary>
        /// The <see cref="PacketType"/> of the packets of this message.
        /// </summary>
        public PacketType PacketType { get; set; }

        /// <summary>
        /// Indicates if the connection should be reset.
        /// </summary>
        public bool ResetConnection { get; set; }

        /// <summary>
        /// Indicates if the message should be ignored.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// The payload of the message.
        /// </summary>
        public Payload Payload { get; set; }

        /// <summary>
        /// Creates a new empty message.
        /// </summary>
        /// <param name="packetType">The packet type for the message.</param>
        /// <param name="resetConnection">Indicate if the connection should be reset.</param>
        public Message(PacketType packetType, bool resetConnection = false)
        {
            PacketType = packetType;
            ResetConnection = resetConnection;
        }

        /// <summary>
        /// Get the <see cref="Packet"/>s of this message.
        /// </summary>
        /// <param name="packetSize">The packet size.</param>
        /// <returns>Enumerable with all packets of the message.</returns>
        public IEnumerable<Packet> GetPackets(ushort packetSize)
        {
            int packetId = 1;
            int dataIndex = 0;

            int dataSize = packetSize - Packet.HeaderLength;
            ByteBuffer payloadData = Payload.BuildBuffer();

            while (payloadData.Length - dataIndex > dataSize)
            {
                ByteBuffer packetData = payloadData.Slice(dataIndex, dataSize);
                Packet packet = new Packet(PacketType);
                packet.SetPacketId(packetId++);
                packet.ResetConnection = ResetConnection;
                packet.Ignore = Ignore;
                packet.AddData(packetData);

                dataIndex += dataSize;
                yield return packet;
            }

            ByteBuffer lastPacketData = payloadData.Slice(dataIndex);
            Packet lastPacket = new Packet(PacketType);
            lastPacket.SetPacketId(packetId++);
            lastPacket.ResetConnection = ResetConnection;
            lastPacket.Ignore = Ignore;
            lastPacket.Last = true;
            lastPacket.AddData(lastPacketData);
            yield return lastPacket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Message=[Type={PacketType}, Reset={ResetConnection}, Ignore={Ignore}, Payload={Payload}]";
        }

    }
}

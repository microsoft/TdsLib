// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Data.TdsLib.Buffer;

namespace Microsoft.Data.TdsLib.Packets
{
    /// <summary>
    /// Represents a Packet in the TDS protocol.
    /// </summary>
    public class Packet
    {
        #region Constants

        /// <summary>
        /// Header length.
        /// </summary>
        public const byte HeaderLength = 8;

        /// <summary>
        /// Default SPID value.
        /// </summary>
        public const ushort DefaultSPID = 0;

        /// <summary>
        /// Default Packet Id.
        /// </summary>
        public const byte DefaultPacketId = 1;

        /// <summary>
        /// Default Window.
        /// </summary>
        public const byte DefaultWindow = 0;

        #endregion

        /// <summary>
        /// Buffer containing the packet (header + data).
        /// </summary>
        public ByteBuffer Buffer { get; private set; }

        /// <summary>
        /// Type of the packet.
        /// </summary>
        public PacketType Type => (PacketType)Buffer.ReadUInt8(PacketOffset.Type);

        /// <summary>
        /// Status of the packet.
        /// </summary>
        public PacketStatus Status => (PacketStatus)Buffer.ReadUInt8(PacketOffset.Status);

        /// <summary>
        /// Server Process Id.
        /// </summary>
        /// <see cref="DefaultSPID"/>
        public ushort SPId => Buffer.ReadUInt16BE(PacketOffset.SPID);

        /// <summary>
        /// Packet Id.
        /// </summary>
        /// <see cref="DefaultPacketId"/>
        public byte Id 
        { 
            get => Buffer.ReadUInt8(PacketOffset.PacketID); 
            set => Buffer.WriteUInt8(value, PacketOffset.PacketID); 
        }

        /// <summary>
        /// Window value.
        /// </summary>
        /// <see cref="DefaultWindow"/>
        public byte Window => Buffer.ReadUInt8(PacketOffset.Window);

        /// <summary>
        /// Total length of the packet (header + data).
        /// </summary>
        public ushort Length => Buffer.ReadUInt16BE(PacketOffset.Length);

        /// <summary>
        /// Indicates if this packet is the last packet for a Message.
        /// </summary>
        public bool Last
        {
            get => ((PacketStatus)Buffer.ReadUInt8(PacketOffset.Status) & PacketStatus.EOM) == PacketStatus.EOM;
            set
            {
                PacketStatus status = (PacketStatus)Buffer.ReadUInt8(PacketOffset.Status);

                if (value)
                {
                    status |= PacketStatus.EOM;
                }
                else
                {
                    status &= 0xFF - PacketStatus.EOM;
                }

                Buffer.WriteUInt8((byte)status, PacketOffset.Status);
            }
        }

        /// <summary>
        /// Indicates if the packet (and message) should be ignored.
        /// </summary>
        public bool Ignore
        {
            get => ((PacketStatus)Buffer.ReadUInt8(PacketOffset.Status) & PacketStatus.Ignore) == PacketStatus.Ignore;
            set
            {
                PacketStatus status = (PacketStatus)Buffer.ReadUInt8(PacketOffset.Status);

                if (value)
                {
                    status |= PacketStatus.Ignore;
                }
                else
                {
                    status &= 0xFF - PacketStatus.Ignore;
                }

                Buffer.WriteUInt8((byte)status, PacketOffset.Status);
            }
        }

        /// <summary>
        /// Indicates if the connection should be reset.
        /// </summary>
        public bool ResetConnection
        {
            get => ((PacketStatus)Buffer.ReadUInt8(PacketOffset.Status) & PacketStatus.ResetConnection) == PacketStatus.ResetConnection;
            set
            {
                PacketStatus status = (PacketStatus)Buffer.ReadUInt8(PacketOffset.Status);

                if (value)
                {
                    status |= PacketStatus.ResetConnection;
                }
                else
                {
                    status &= 0xFF - PacketStatus.ResetConnection;
                }

                Buffer.WriteUInt8((byte)status, PacketOffset.Status);
            }
        }

        /// <summary>
        /// Gets a copy of the data of this packet. 
        /// May be an empty buffer if there is no data in the packet.
        /// </summary>
        public ByteBuffer Data => Buffer.Length == HeaderLength ? ByteBuffer.Empty : Buffer.Slice(HeaderLength);

        /// <summary>
        /// Creates a new packet with the specified type.
        /// </summary>
        /// <param name="type">The packet type.</param>
        public Packet(PacketType type)
        {
            Buffer = new ByteBuffer(HeaderLength);

            Buffer.WriteUInt8((byte)type, PacketOffset.Type);
            Buffer.WriteUInt8((byte)PacketStatus.Normal, PacketOffset.Status);
            Buffer.WriteUInt16BE(DefaultSPID, PacketOffset.SPID);
            Buffer.WriteUInt8(DefaultPacketId, PacketOffset.PacketID);
            Buffer.WriteUInt8(DefaultWindow, PacketOffset.Window);
            UpdateLength();
        }

        /// <summary>
        /// Creates a packet from a buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="ArgumentNullException">If the provided buffer is null.</exception>
        /// <exception cref="ArgumentException">If the buffer length is less then the packet header length.</exception>
        public Packet(ByteBuffer buffer)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer.Length < HeaderLength)
            {
                throw new ArgumentException("Buffer length must be greater then the packet header length");
            }

            if (buffer.ReadUInt8(PacketOffset.Type) > (byte)PacketType.PreLogin)
            {
                throw new ArgumentException($"PacketType present in the buffer is invalid: 0x{buffer.ReadUInt8(PacketOffset.Type):X2}");
            }

            Buffer = buffer;
        }

        private void UpdateLength()
        {
            Buffer.WriteUInt16BE((ushort)Buffer.Length, PacketOffset.Length);
        }

        /// <summary>
        /// Sets the Packet Id of this packet with an integer. The value will be truncated (by modulo) to <see cref="byte.MaxValue"/>.
        /// </summary>
        /// <param name="packetId">The packet id to set.</param>
        /// <see cref="Id"/>
        public void SetPacketId(int packetId) => Id = (byte)(packetId % 256);

        /// <summary>
        /// Adds data to this packet.
        /// </summary>
        /// <param name="buffer">The buffer to add to this packet.</param>
        public void AddData(ByteBuffer buffer)
        {
            Buffer = Buffer.Concat(buffer);
            UpdateLength();
        }

        /// <summary>
        /// Returns a human readable string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return $"Packet[Type=0x{(uint)Type:X2}({Type}), Status=0x{(uint)Status:X2}({Status}), Length={Length}, SPID=0x{SPId:X4}, PacketId={Id}, Window=0x{Window:X2}]";
        }

    }
}

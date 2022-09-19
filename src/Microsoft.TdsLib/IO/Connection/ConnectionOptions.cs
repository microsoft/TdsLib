// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.TdsLib.Packets;

namespace Microsoft.TdsLib.IO.Connection
{
    /// <summary>
    /// Tds client connection options.
    /// </summary>
    public class ConnectionOptions
    {
        private ushort packetSize;

        /// <summary>
        /// Packet size in bytes.
        /// Greater than <see cref="Packet.HeaderLength"/>.
        /// </summary>
        public ushort PacketSize 
        { 
            get => packetSize;
            set => packetSize = value > Packet.HeaderLength ? value : throw new ArgumentOutOfRangeException(nameof(value), "Invalid packet size, must be greater than Packet header length.");
        }

        /// <summary>
        /// TDS Protocol version.
        /// </summary>
        public TdsVersion TdsVersion { get; set; }

        /// <summary>
        /// Creates a new connection options with default values. <br/>
        /// <see cref="PacketSize"/> = <see cref="TdsConstants.DefaultPacketSize"/>, <see cref="TdsVersion"/> = <see cref="TdsVersion.V7_4"/>.
        /// </summary>
        public ConnectionOptions()
        {
            PacketSize = TdsConstants.DefaultPacketSize;
            TdsVersion = TdsVersion.V7_4;
        }

    }
}
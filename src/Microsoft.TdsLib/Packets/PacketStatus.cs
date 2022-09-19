// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.TdsLib.Packets
{
    /// <summary>
    /// Packet status enum flags.
    /// </summary>
    [Flags]
    public enum PacketStatus
    {
        /// <summary>
        /// Normal Packet.
        /// </summary>
        Normal = 0x00,

        /// <summary>
        /// End of Message. The last packet in the message.
        /// </summary>
        EOM = 0x01,

        /// <summary>
        /// Packet/Message to be ignored.
        /// </summary>
        Ignore = 0x02,

        /// <summary>
        /// Reset connection.
        /// </summary>
        ResetConnection = 0x08,

        /// <summary>
        /// Reset connection but keep transaction state.
        /// </summary>
        ResetConnectionSkipTran = 0x10
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Data.TdsLib.Payloads.Login7
{
    /// <summary>
    /// Login7 options.
    /// </summary>
    public class Login7Options
    {
        /// <summary>
        /// Tds protocol version.
        /// </summary>
        public TdsVersion TdsVersion { get; set; }

        /// <summary>
        /// Packet size.
        /// </summary>
        public uint PacketSize { get; set; }

        /// <summary>
        /// Client program version.
        /// </summary>
        public uint ClientProgVer { get; set; }

        /// <summary>
        /// Process Id.
        /// </summary>
        public uint ClientPid { get; set; }

        /// <summary>
        /// Connection Id.
        /// </summary>
        public uint ConnectionId { get; set; }

        /// <summary>
        /// Timezone offset from UTC.
        /// </summary>
        public int ClientTimeZone { get; set; }

        /// <summary>
        /// Culture info identifier.
        /// </summary>
        public uint ClientLcid { get; set; }

        /// <summary>
        /// Create a new instance of this class with default values.
        /// </summary>
        public Login7Options()
        {
            TdsVersion = TdsVersion.V7_4;
            PacketSize = TdsConstants.DefaultPacketSize;
            ClientProgVer = 0;

            using (var process = Process.GetCurrentProcess())
            {
                ClientPid = (uint)process.Id;
            }

            ConnectionId = 0;
            ClientTimeZone = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
            ClientLcid = (uint)CultureInfo.CurrentCulture.LCID;
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"Options[TdsVersion={TdsVersion}, PacketSize={PacketSize}, ClientProgVer={ClientProgVer}, ClientPid={ClientPid}, ConnectionId={ConnectionId}, ClientTimeZone={ClientTimeZone}, ClientLcid={ClientLcid}]";
        }

    }
}

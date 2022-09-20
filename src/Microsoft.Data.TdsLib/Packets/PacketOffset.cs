// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Packets
{
    internal static class PacketOffset
    {
        public const int Type = 0;
        public const int Status = 1;
        public const int Length = 2;
        public const int SPID = 4;
        public const int PacketID = 6;
        public const int Window = 7;
    }
}

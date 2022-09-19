// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Payloads.PreLogin
{
    internal static class TokenType
    {
        public const int Version = 0x00;
        public const int Encryption = 0x01;
        public const int InstOpt = 0x02;
        public const int ThreadId = 0x03;
        public const int Mars = 0x04;
        public const int FedAuthRequired = 0x06;
        public const int Terminator = 0xff;
    }
}

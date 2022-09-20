// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Payloads.PreLogin
{
    internal struct PreLoginOption
    {
        public byte TokenType { get; set; }

        public ByteBuffer ByteBuffer { get; set; }

    }
}

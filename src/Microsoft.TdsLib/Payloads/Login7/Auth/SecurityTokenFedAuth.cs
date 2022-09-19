// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Text;
using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Payloads.Login7.Auth
{
    /// <summary>
    /// Security token federated authentication.
    /// </summary>
    public sealed class SecurityTokenFedAuth : FedAuth
    {
        /// <summary>
        /// Token.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Echo.
        /// </summary>
        public bool Echo { get; }

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="echo">Echo.</param>
        public SecurityTokenFedAuth(string token, bool echo = false)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            Token = token;
            Echo = echo;
        }

        internal override ByteBuffer GetBuffer()
        {
            ByteBuffer tokenBuffer = new ByteBuffer(Encoding.Unicode.GetBytes(Token));
            ByteBuffer buffer = new ByteBuffer(10);

            int offset = 0;
            offset = buffer.WriteUInt8(FeatureId, offset);
            offset = buffer.WriteUInt32LE((uint)tokenBuffer.Length + 4 + 1, offset);

            byte options = (byte)(LibrarySecurityToken | (Echo ? FedAuthEchoYes : FedAuthEchoNo));
            offset = buffer.WriteUInt8(options, offset);
            buffer.WriteInt32LE(tokenBuffer.Length, offset);

            return buffer.Concat(tokenBuffer);
        }
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Reset connection.
    /// </summary>
    public sealed class ResetConnectionEnvChangeToken : EnvChangeToken<ByteBuffer>
    {

        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.ResetConnection;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>

        public ResetConnectionEnvChangeToken(ByteBuffer oldValue, ByteBuffer newValue) : base(oldValue, newValue)
        {
        }

    }
}

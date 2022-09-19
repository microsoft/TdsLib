﻿using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Commit transaction token.
    /// </summary>
    public sealed class CommitTransactionEnvChangeToken : EnvChangeToken<ByteBuffer>
    {
        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.CommitTransaction;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public CommitTransactionEnvChangeToken(ByteBuffer oldValue, ByteBuffer newValue) : base(oldValue, newValue)
        {
        }

    }
}

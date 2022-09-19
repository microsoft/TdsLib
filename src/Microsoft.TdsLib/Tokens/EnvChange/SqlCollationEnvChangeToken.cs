﻿using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Tokens.EnvChange
{

    /// <summary>
    /// Sql collation change token.
    /// </summary>
    public sealed class SqlCollationEnvChangeToken : EnvChangeToken<ByteBuffer>
    {

        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.SqlCollation;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public SqlCollationEnvChangeToken(ByteBuffer oldValue, ByteBuffer newValue) : base(oldValue, newValue)
        {
        }

    }
}

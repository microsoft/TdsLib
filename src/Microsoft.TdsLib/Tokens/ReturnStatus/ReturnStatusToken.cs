﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Tokens.ReturnStatus
{
    /// <summary>
    /// Return status of an RPC or TSQL exec query.
    /// </summary>
    public sealed class ReturnStatusToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.ReturnStatus;

        /// <summary>
        /// Return status value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Create a new instance of this token with a status value.
        /// </summary>
        /// <param name="value">Return status value.</param>
        public ReturnStatusToken(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(ReturnStatusToken)}[{nameof(Value)}={Value}]";
        }

    }
}

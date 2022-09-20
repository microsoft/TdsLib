// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Data.Tools.TdsLib.Tokens.Done
{
    /// <summary>
    /// Done status for <see cref="Done"/>, <see cref="DoneProc"/>, <see cref="DoneInProc"/>.
    /// </summary>
    [Flags]
    public enum DoneStatus : ushort
    {
        /// <summary>
        /// Final.
        /// </summary>
        Final = 0x0000,

        /// <summary>
        /// More.
        /// </summary>
        More = 0x0001,

        /// <summary>
        /// Error.
        /// </summary>
        Error = 0x0002,

        /// <summary>
        /// In Transaction
        /// </summary>
        InXAct = 0x0004,

        /// <summary>
        /// Count.
        /// </summary>
        Count = 0x0010,

        /// <summary>
        /// Attention.
        /// </summary>
        Attn = 0x0020,

        /// <summary>
        /// Server Error.
        /// </summary>
        ServerError = 0x0100
    }
}

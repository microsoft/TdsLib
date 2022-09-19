// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Tokens.Done
{
    /// <summary>
    /// Indicates the completion status of a SQL statement.
    /// </summary>
    public class DoneToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.Done;

        /// <summary>
        /// Status.
        /// </summary>
        public DoneStatus Status { get; }

        /// <summary>
        /// Current command.
        /// </summary>
        public ushort CurrentCommand { get; }

        /// <summary>
        /// Row count.
        /// </summary>
        public ulong RowCount { get; }

        /// <summary>
        /// Create a new instance with a status, current command and row count.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <param name="currentCommand">Current command.</param>
        /// <param name="rowCount">Row count.</param>
        public DoneToken(DoneStatus status, ushort currentCommand, ulong rowCount)
        {
            Status = status;
            CurrentCommand = currentCommand;
            RowCount = rowCount;
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"DoneToken[Status=0x{Status:X}({Status}), CurrentCommand={CurrentCommand}, RowCount={RowCount}]";
        }
    }
}

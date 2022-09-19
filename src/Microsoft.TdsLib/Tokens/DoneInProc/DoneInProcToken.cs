using Microsoft.TdsLib.Tokens.Done;

namespace Microsoft.TdsLib.Tokens.DoneInProc
{
    /// <summary>
    /// Token indicating the completion status of a statement in a procedure.
    /// </summary>
    public sealed class DoneInProcToken : DoneToken
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.DoneInProc;

        /// <summary>
        /// Create a new instance with a status, current command and row count.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <param name="currentCommand">Current command.</param>
        /// <param name="rowCount">Row count.</param>
        public DoneInProcToken(DoneStatus status, ushort currentCommand, ulong rowCount)
            : base(status, currentCommand, rowCount)
        {
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"DoneInProcToken[Status=0x{Status:X}({Status}), CurrentCommand={CurrentCommand}, RowCount={RowCount}]";
        }
    }
}

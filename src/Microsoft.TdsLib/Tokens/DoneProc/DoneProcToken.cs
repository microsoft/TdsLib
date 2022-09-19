using Microsoft.TdsLib.Tokens.Done;

namespace Microsoft.TdsLib.Tokens.DoneProc
{
    /// <summary>
    /// Statement in a procedure done.
    /// </summary>
    public sealed class DoneProcToken : DoneToken
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.DoneProc;

        /// <summary>
        /// Create a new instance with a status, current command and row count.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <param name="currentCommand">Current command.</param>
        /// <param name="rowCount">Row count.</param>
        public DoneProcToken(DoneStatus status, ushort currentCommand, ulong rowCount)
            : base(status, currentCommand, rowCount)
        {
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"DoneProcToken[Status=0x{Status:X}({Status}), CurrentCommand={CurrentCommand}, RowCount={RowCount}]";
        }
    }
}

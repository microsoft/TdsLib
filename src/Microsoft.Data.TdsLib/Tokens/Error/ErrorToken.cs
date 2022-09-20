// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Tokens.Error
{
    /// <summary>
    /// Error message.
    /// </summary>
    public sealed class ErrorToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.Error;

        /// <summary>
        /// Error number.
        /// </summary>
        public uint Number { get; }

        /// <summary>
        /// State.
        /// </summary>
        public byte State { get; }

        /// <summary>
        /// Severity.
        /// </summary>
        public byte Severity { get; }

        /// <summary>
        /// Message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Server name.
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// Process name.
        /// </summary>
        public string ProcName { get; }

        /// <summary>
        /// Line number.
        /// </summary>
        public uint LineNumber { get; }

        /// <summary>
        /// Creates a new instance of the token.
        /// </summary>
        /// <param name="number">Error number.</param>
        /// <param name="state">State.</param>
        /// <param name="severity">Severity.</param>
        /// <param name="message">Message.</param>
        /// <param name="serverName">Server name.</param>
        /// <param name="procName">Process name.</param>
        /// <param name="lineNumber">Line number.</param>
        public ErrorToken(uint number, byte state, byte severity, string message, string serverName, string procName, uint lineNumber)
        {
            Number = number;
            State = state;
            Severity = severity;
            Message = message;
            ServerName = serverName;
            ProcName = procName;
            LineNumber = lineNumber;
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"{nameof(ErrorToken)}[{nameof(Number)}={Number}, {nameof(State)}={State}, {nameof(Severity)}={Severity}, {nameof(Message)}={Message}, {nameof(ServerName)}={ServerName}, {nameof(ProcName)}={ProcName}, {nameof(LineNumber)}={LineNumber}]";
        }
    }
}

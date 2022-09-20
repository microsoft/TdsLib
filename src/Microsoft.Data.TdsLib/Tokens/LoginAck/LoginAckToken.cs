// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.TdsLib.Tokens.LoginAck
{
    /// <summary>
    /// Login7 response.
    /// </summary>
    public sealed class LoginAckToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.LoginAck;

        /// <summary>
        /// SQL interface type.
        /// </summary>
        public SqlInterfaceType InterfaceType { get; }

        /// <summary>
        /// Tds version.
        /// </summary>
        public TdsVersion TdsVersion { get; }

        /// <summary>
        /// Program name.
        /// </summary>
        public string ProgName { get; }

        /// <summary>
        /// Program version.
        /// </summary>
        public ProgVersion ProgVersion { get; }

        /// <summary>
        /// Creates a new instance of this token.
        /// </summary>
        /// <param name="interfaceType">SQL interface type.</param>
        /// <param name="tdsVersion">Tds version.</param>
        /// <param name="progName">Program name.</param>
        /// <param name="progVersion">Program version.</param>
        public LoginAckToken(SqlInterfaceType interfaceType, TdsVersion tdsVersion, string progName, ProgVersion progVersion)
        {
            InterfaceType = interfaceType;
            TdsVersion = tdsVersion;
            ProgName = progName;
            ProgVersion = progVersion;
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"LoginAckToken[InterfaceType={InterfaceType}, TdsVersion={TdsVersion}, ProgName={ProgName}, ProgVersion={ProgVersion}]";
        }
    }
}

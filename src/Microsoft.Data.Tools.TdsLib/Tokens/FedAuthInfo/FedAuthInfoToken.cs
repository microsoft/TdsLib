// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Data.Tools.TdsLib.Tokens.FedAuthInfo
{
    /// <summary>
    /// Federate authentication information token.
    /// </summary>
    public sealed class FedAuthInfoToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.FedAuthInfo;

        /// <summary>
        /// Service principal name. 
        /// Can be null.
        /// </summary>
        public string SPN { get; }

        /// <summary>
        /// Token endpoint url. 
        /// Can be null.
        /// </summary>
        public string STSUrl { get; }

        /// <summary>
        /// Creates a new instance of the token.
        /// </summary>
        /// <param name="spn">Service principal name.</param>
        /// <param name="sTSUrl">Token endpoint url.</param>
        public FedAuthInfoToken(string spn, string sTSUrl)
        {
            SPN = spn;
            STSUrl = sTSUrl;
        }

        /// <summary>
        /// Gets a human readable string representation of this token.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"FedAuthInfo[SPN={SPN}, STSUrl={STSUrl}]";
        }

    }
}

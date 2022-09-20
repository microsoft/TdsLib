// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Data.Tools.TdsLib.Tokens.LoginAck
{
    internal class LoginAckTokenParser : TokenParser
    {
        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            _ = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false); // length

            byte type = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            SqlInterfaceType interfaceType = GetSqlInterfaceType(type);

            uint version = await tokenStreamHandler.ReadUInt32BE().ConfigureAwait(false);
            TdsVersion tdsVersion = GetTdsVersion(version);

            string progName = await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false);
            byte major = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            byte minor = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            byte buildHi = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            byte buildLow = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);
            ProgVersion progVersion = new ProgVersion(major, minor, buildHi, buildLow);

            return new LoginAckToken(interfaceType, tdsVersion, progName, progVersion);
        }

        private TdsVersion GetTdsVersion(uint tdsVersion)
        {
            if (Enum.IsDefined(typeof(TdsVersion), tdsVersion))
            {
                return (TdsVersion)tdsVersion;
            }

            throw new InvalidOperationException($"Unknown Tds Version: {tdsVersion:X}");
        }

        private SqlInterfaceType GetSqlInterfaceType(byte interfaceType)
        {
            if (interfaceType == (byte)SqlInterfaceType.Default)
            {
                return SqlInterfaceType.Default;
            }
            else if (interfaceType == (byte)SqlInterfaceType.TSql)
            {
                return SqlInterfaceType.TSql;
            }

            throw new InvalidOperationException($"Unknown Sql Interface type: {interfaceType}");
        }
    }
}

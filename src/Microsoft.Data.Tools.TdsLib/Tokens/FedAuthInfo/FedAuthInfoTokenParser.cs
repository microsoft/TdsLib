// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Tokens.FedAuthInfo
{
    internal class FedAuthInfoTokenParser : TokenParser
    {
        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            uint tokenLength = await tokenStreamHandler.ReadUInt32LE().ConfigureAwait(false);
            ByteBuffer buffer = await tokenStreamHandler.ReadBuffer((int)tokenLength);
            int offset = 0;

            uint countOfIds = buffer.ReadUInt32LE(offset);
            offset += sizeof(uint);

            string spn = null;
            string stsUrl = null;

            for (int i = 0; i < countOfIds; i++)
            {
                byte fedAuthInfoId = buffer.ReadUInt8(offset);
                offset += sizeof(byte);

                uint fedAuthInfoDataLength = buffer.ReadUInt32LE(offset);
                offset += sizeof(uint);

                uint fedAuthInfoDataOffset = buffer.ReadUInt32LE(offset);
                offset += sizeof(uint);

                if (fedAuthInfoId == (byte)FedAuthInfoId.SPN)
                {
                    spn = Encoding.Unicode.GetString(buffer.ToArraySegment().Array, (int)fedAuthInfoDataOffset, (int)fedAuthInfoDataLength);
                }
                else if (fedAuthInfoId == (byte)FedAuthInfoId.STSUrl)
                {
                    stsUrl = Encoding.Unicode.GetString(buffer.ToArraySegment().Array, (int)fedAuthInfoDataOffset, (int)fedAuthInfoDataLength);
                }
            }

            return new FedAuthInfoToken(spn, stsUrl);
        }
    }
}

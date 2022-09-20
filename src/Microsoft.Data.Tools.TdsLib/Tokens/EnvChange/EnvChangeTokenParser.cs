// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Tokens.EnvChange
{
    internal class EnvChangeTokenParser : TokenParser
    {

        private static readonly Dictionary<EnvChangeTokenSubType, Func<TokenStreamHandler, Task<Token>>> subTypeParsers = subTypeParsers = new Dictionary<EnvChangeTokenSubType, Func<TokenStreamHandler, Task<Token>>>
        {
            [EnvChangeTokenSubType.Database] = async (tokenStreamHandler) =>
            new DatabaseEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false)),

            [EnvChangeTokenSubType.PacketSize] = async (tokenStreamHandler) =>
            new PacketSizeEnvChangeToken(newValue: int.Parse(await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false)), oldValue: int.Parse(await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false))),

            [EnvChangeTokenSubType.CharacterSet] = async (tokenStreamHandler) =>
            new CharsetEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false)),

            [EnvChangeTokenSubType.DatabaseMirroringPartner] = async (tokenStreamHandler) =>
            new DatabaseMirroringPartnerEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false)),

            [EnvChangeTokenSubType.Language] = async (tokenStreamHandler) =>
            new LanguageEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarChar().ConfigureAwait(false)),

            [EnvChangeTokenSubType.SqlCollation] = async (tokenStreamHandler) =>
            new SqlCollationEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false)),

            [EnvChangeTokenSubType.BeginTransaction] = async (tokenStreamHandler) =>
            new BeginTransactionEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false)),

            [EnvChangeTokenSubType.CommitTransaction] = async (tokenStreamHandler) =>
            new CommitTransactionEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false)),

            [EnvChangeTokenSubType.RollbackTransaction] = async (tokenStreamHandler) =>
            new RollbackTransactionEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false)),

            [EnvChangeTokenSubType.ResetConnection] = async (tokenStreamHandler) =>
            new ResetConnectionEnvChangeToken(newValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false), oldValue: await tokenStreamHandler.ReadBVarByte().ConfigureAwait(false)),

            [EnvChangeTokenSubType.Routing] = ParseRoutingEnvChange

        };

        private static async Task<Token> ParseRoutingEnvChange(TokenStreamHandler tokenStreamHandler)
        {
            ushort length = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            ByteBuffer buffer = await tokenStreamHandler.ReadBuffer(length).ConfigureAwait(false);

            byte protocol = buffer.ReadUInt8();
            ushort port = buffer.ReadUInt16LE(1);
            ushort serverLength = buffer.ReadUInt16LE(3);
            string server = Encoding.Unicode.GetString(buffer.Slice(5, serverLength * 2).ToArraySegment().Array);

            RoutingInfo newRoutingInfo = new RoutingInfo(protocol, port, server);

            length = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            RoutingInfo oldRoutingInfo = null;

            if (length > 0)
            {
                buffer = await tokenStreamHandler.ReadBuffer(length).ConfigureAwait(false);

                protocol = buffer.ReadUInt8();
                port = buffer.ReadUInt16LE();
                serverLength = buffer.ReadUInt16LE(3);
                server = Encoding.Unicode.GetString(buffer.Slice(5, serverLength * 2).ToArraySegment().Array);

                oldRoutingInfo = new RoutingInfo(protocol, port, server);
            }

            return new RoutingEnvChangeToken(oldRoutingInfo, newRoutingInfo);
        }


        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            ushort length = await tokenStreamHandler.ReadUInt16LE().ConfigureAwait(false);
            byte subType = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);

            if (!Enum.IsDefined(typeof(EnvChangeTokenSubType), subType))
            {
                throw new InvalidOperationException($"Unsupported EnvChange Token type: {subType}");
            }

            EnvChangeTokenSubType tokenSubType = (EnvChangeTokenSubType)subType;

            if (!subTypeParsers.ContainsKey(tokenSubType))
            {
                throw new InvalidOperationException($"Unsupported EnvChange Token type: {tokenSubType}");
            }

            return await subTypeParsers[tokenSubType](tokenStreamHandler).ConfigureAwait(false);
        }
    }
}

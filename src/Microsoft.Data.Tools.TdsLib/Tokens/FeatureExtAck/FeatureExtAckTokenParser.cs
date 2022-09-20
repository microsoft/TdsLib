// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Data.Tools.TdsLib.Tokens.FeatureExtAck
{
    internal sealed class FeatureExtAckTokenParser : TokenParser
    {

        public override async Task<Token> Parse(TokenType tokenType, TokenStreamHandler tokenStreamHandler)
        {
            byte byteFeatureId = await tokenStreamHandler.ReadUInt8().ConfigureAwait(false);

            if (!Enum.IsDefined(typeof(FeatureId), byteFeatureId))
            {
                throw new InvalidOperationException($"Invalid FeatureId: 0x{byteFeatureId:X2}");
            }

            FeatureId featureId = (FeatureId)byteFeatureId;

            if (featureId == FeatureId.Terminator)
            {
                return new FeatureExtAckToken((FeatureId)featureId);
            }

            uint dataLength = await tokenStreamHandler.ReadUInt32LE().ConfigureAwait(false);
            return new FeatureExtAckToken((FeatureId)featureId, await tokenStreamHandler.ReadBuffer((int)dataLength).ConfigureAwait(false));
        }
    }
}

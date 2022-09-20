// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.Tools.TdsLib.Buffer;

namespace Microsoft.Data.Tools.TdsLib.Tokens.FeatureExtAck
{
    /// <summary>
    /// Feature extension acknowledge token.
    /// </summary>
    public sealed class FeatureExtAckToken : Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public override TokenType Type => TokenType.FeatureExtAck;

        /// <summary>
        /// Feature Id.
        /// </summary>
        public FeatureId FeatureId { get; }

        /// <summary>
        /// Data buffer.
        /// </summary>
        public ByteBuffer Buffer { get; }

        /// <summary>
        /// Creates a new instance of the token.
        /// </summary>
        /// <param name="featureId">Feature Id.</param>
        public FeatureExtAckToken(FeatureId featureId)
        {
            FeatureId = featureId;
        }

        /// <summary>
        /// Creates a new instance of the token.
        /// </summary>
        /// <param name="featureId">Feature Id.</param>
        /// <param name="buffer">Data buffer.</param>
        public FeatureExtAckToken(FeatureId featureId, ByteBuffer buffer)
        {
            FeatureId = featureId;
            Buffer = buffer;
        }

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"FeatureExtAckToken=[FeatureId={FeatureId}, Buffer={Buffer}]";
        }

    }
}

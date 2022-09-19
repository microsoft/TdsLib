using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Begin transaction token.
    /// </summary>
    public sealed class BeginTransactionEnvChangeToken : EnvChangeToken<ByteBuffer>
    {

        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.BeginTransaction;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public BeginTransactionEnvChangeToken(ByteBuffer oldValue, ByteBuffer newValue) : base(oldValue, newValue)
        {
        }

    }
}

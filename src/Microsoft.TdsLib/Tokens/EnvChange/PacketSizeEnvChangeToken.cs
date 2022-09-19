namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Packet size change.
    /// </summary>
    public sealed class PacketSizeEnvChangeToken : EnvChangeToken<int>
    {
        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.PacketSize;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public PacketSizeEnvChangeToken(int oldValue, int newValue)
            : base(oldValue, newValue)
        {

        }
    }
}

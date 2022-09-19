namespace Microsoft.TdsLib.Tokens.EnvChange
{

    /// <summary>
    /// Charset change token.
    /// </summary>
    public sealed class CharsetEnvChangeToken : EnvChangeToken<string>
    {

        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.CharacterSet;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        public CharsetEnvChangeToken(string oldValue, string newValue) : base(oldValue, newValue)
        {
        }

    }
}

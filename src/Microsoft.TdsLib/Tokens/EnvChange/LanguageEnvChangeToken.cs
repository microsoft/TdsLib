namespace Microsoft.TdsLib.Tokens.EnvChange
{
    /// <summary>
    /// Language environment change.
    /// </summary>
    public sealed class LanguageEnvChangeToken : EnvChangeToken<string>
    {
        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.Language;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value./</param>
        /// <param name="newValue">New value.</param>
        public LanguageEnvChangeToken(string oldValue, string newValue)
            : base(oldValue, newValue)
        {
        }

    }
}

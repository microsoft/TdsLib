// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.TdsLib.Tokens.EnvChange
{

    /// <summary>
    /// Routing change token.
    /// </summary>
    public sealed class RoutingEnvChangeToken : EnvChangeToken<RoutingInfo>
    {
        /// <summary>
        /// EnvChange token sub type.
        /// </summary>
        public override EnvChangeTokenSubType SubType => EnvChangeTokenSubType.Routing;

        /// <summary>
        /// Create a new instance of this token.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        public RoutingEnvChangeToken(RoutingInfo oldValue, RoutingInfo newValue) : base(oldValue, newValue)
        {
        }

    }
}

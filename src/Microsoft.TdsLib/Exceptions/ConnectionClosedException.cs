// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Microsoft.TdsLib.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a connection is unexpectedly closed.
    /// </summary>
    public class ConnectionClosedException : IOException
    {
        /// <summary>
        /// Creates a new instance with a default message.
        /// </summary>
        public ConnectionClosedException() : this("Connection unexpectedly closed")
        {
        }

        /// <summary>
        /// Creates a new instance with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ConnectionClosedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance with a custom message and an inner exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ConnectionClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}

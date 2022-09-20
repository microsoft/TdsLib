// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Data.TdsLib.IO.Connection;
using Xunit;

namespace Microsoft.Data.TdsLib.UnitTest
{
    public class TdsClientTest
    {
        [Fact]
        public void CreateTdsClientWithNullConnection()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IConnection connection = default;
                new TdsClient(connection);
            });
        }
    }
}

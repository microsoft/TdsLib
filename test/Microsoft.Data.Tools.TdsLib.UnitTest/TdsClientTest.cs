// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Data.Tools.TdsLib.IO.Connection;
using Xunit;

namespace Microsoft.Data.Tools.TdsLib.UnitTest
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

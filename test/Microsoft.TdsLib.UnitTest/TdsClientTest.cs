using System;
using Microsoft.TdsLib.IO.Connection;
using Xunit;

namespace Microsoft.TdsLib.UnitTest
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

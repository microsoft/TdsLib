// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Data.TdsLib.Buffer;
using Microsoft.Data.TdsLib.Payloads.PreLogin;
using Xunit;

namespace Microsoft.Data.TdsLib.UnitTest.Payloads.PreLogin
{
    public class PreLoginPayloadTest
    {
        private static readonly ByteBuffer DefaultPreLoginPayloadBuffer = new ByteBuffer(new byte[] { 0x00, 0x00, 0x1F, 0x00, 0x06, 0x01, 0x00, 0x25, 0x00, 0x01, 0x02, 0x00, 0x26, 0x00, 0x01, 0x03, 0x00, 0x27, 0x00, 0x04, 0x04, 0x00, 0x2B, 0x00, 0x01, 0x06, 0x00, 0x2C, 0x00, 0x01, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 });
        
        [Fact]
        public void TestCreateEmptyPayload()
        {
            PreLoginPayload preLoginPayload = new PreLoginPayload();
            ByteBuffer buffer = preLoginPayload.BuildBuffer();

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);
            Assert.Equal(DefaultPreLoginPayloadBuffer, buffer);
        }

        [Fact]
        public void TestCreateEmptyPayloadNoEncryption()
        {
            PreLoginPayload preLoginPayload = new PreLoginPayload();
            Assert.Equal(EncryptionType.NotSupported, preLoginPayload.Encryption);
        }

        [Fact]
        public void TestCreateEmptyPayloadEncryption()
        {
            PreLoginPayload preLoginPayload = new PreLoginPayload(true);
            Assert.Equal(EncryptionType.On, preLoginPayload.Encryption);
        }

    }
}

﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.IO.Connection.Tcp;
using Microsoft.TdsLib.Messages;
using Microsoft.TdsLib.Packets;
using Microsoft.TdsLib.Payloads.Login7;
using Microsoft.TdsLib.Payloads.PreLogin;
using Microsoft.TdsLib.Tokens.Error;
using Xunit;

namespace Microsoft.TdsLib.UnitTest.IO.Connection.Tcp
{
    public class TcpConnectionTest
    {
        private const string NonRoutableIP = "10.255.255.1";
        private const int Port = 1443;
        private const int ETimedOutErrorCode = 110;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void TestConnectTimeout(int timeoutSeconds)
        {
            DateTime startTime = DateTime.Now;
            var socketException = Assert.ThrowsAny<SocketException>(() =>
            {
                using TcpConnection connection = new TcpConnection(new TcpConnectionOptions { ConnectTimeout = TimeSpan.FromSeconds(timeoutSeconds) }, new ServerEndpoint(NonRoutableIP, Port));
            });
            DateTime endTime = DateTime.Now;

            bool isError = (socketException.ErrorCode == ETimedOutErrorCode || socketException.ErrorCode == (int)SocketError.NetworkUnreachable || socketException.ErrorCode == (int)SocketError.TimedOut);

            Assert.True(isError);
            Assert.True(endTime - startTime < TimeSpan.FromSeconds(timeoutSeconds * 2));
        }

        [Fact]
        public void TestDefaultConnectTimeout()
        {
            DateTime startTime = DateTime.Now;
            var socketException = Assert.ThrowsAny<SocketException>(() =>
            {
                using TcpConnection connection = new TcpConnection(new TcpConnectionOptions(), new ServerEndpoint(NonRoutableIP, Port));
            });
            DateTime endTime = DateTime.Now;

            bool isError = (socketException.ErrorCode == ETimedOutErrorCode || socketException.ErrorCode == (int)SocketError.NetworkUnreachable || socketException.ErrorCode == (int)SocketError.TimedOut);
            
            Assert.True(isError);
        }

        [Fact]
        public void TestConnect()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();

            try
            {
                IPEndPoint endpoint = tcpListener.LocalEndpoint as IPEndPoint;
                using TcpConnection connection = new TcpConnection(new TcpConnectionOptions(), new ServerEndpoint(endpoint.Address.ToString(), endpoint.Port));
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        [Fact]
        public async Task TestTdsClientWithTcpConnection()
        {
            #region Initialize data

            ByteBuffer PreLoginByteBuffer = new ByteBuffer(new byte[] { 0x12, 0x01, 0x00, 0x35, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1F, 0x00, 0x06, 0x01, 0x00, 0x25, 0x00, 0x01, 0x02, 0x00, 0x26, 0x00, 0x01, 0x03, 0x00, 0x27, 0x00, 0x04, 0x04, 0x00, 0x2B, 0x00, 0x01, 0x06, 0x00, 0x2C, 0x00, 0x01, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 });
            ByteBuffer Login7ByteBuffer = new ByteBuffer(new byte[] { 0x10, 0x01, 0x01, 0x03, 0x00, 0x00, 0x01, 0x00, 0xFB, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x74, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x44, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB0, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x09, 0x08, 0x00, 0x00, 0x5E, 0x00, 0x08, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x0E, 0x00, 0x8A, 0x00, 0x22, 0x00, 0xCE, 0x00, 0x04, 0x00, 0xD3, 0x00, 0x06, 0x00, 0xDF, 0x00, 0x0A, 0x00, 0xF3, 0x00, 0x04, 0x00, 0xE8, 0xFA, 0xBB, 0x2B, 0x09, 0xFF, 0xFB, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x00, 0x45, 0x00, 0x53, 0x00, 0x54, 0x00, 0x2D, 0x00, 0x45, 0x00, 0x4E, 0x00, 0x56, 0x00, 0x54, 0x00, 0x64, 0x00, 0x73, 0x00, 0x4C, 0x00, 0x69, 0x00, 0x62, 0x00, 0x55, 0x00, 0x6E, 0x00, 0x69, 0x00, 0x74, 0x00, 0x54, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x61, 0x00, 0x7A, 0x00, 0x75, 0x00, 0x72, 0x00, 0x65, 0x00, 0x73, 0x00, 0x71, 0x00, 0x6C, 0x00, 0x2D, 0x00, 0x74, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x2E, 0x00, 0x64, 0x00, 0x61, 0x00, 0x74, 0x00, 0x61, 0x00, 0x62, 0x00, 0x61, 0x00, 0x73, 0x00, 0x65, 0x00, 0x2E, 0x00, 0x77, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x64, 0x00, 0x6F, 0x00, 0x77, 0x00, 0x73, 0x00, 0x2E, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x74, 0x00, 0xD2, 0x00, 0x00, 0x00, 0xFF, 0x54, 0x00, 0x64, 0x00, 0x73, 0x00, 0x4C, 0x00, 0x69, 0x00, 0x62, 0x00, 0x75, 0x00, 0x73, 0x00, 0x5F, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x67, 0x00, 0x6C, 0x00, 0x69, 0x00, 0x73, 0x00, 0x68, 0x00, 0x74, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00 });

            ByteBuffer PreLoginReplyByteBuffer = new ByteBuffer(new byte[] { 0x04, 0x01, 0x00, 0x31, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1F, 0x00, 0x06, 0x01, 0x00, 0x25, 0x00, 0x01, 0x02, 0x00, 0x26, 0x00, 0x01, 0x03, 0x00, 0x27, 0x00, 0x00, 0x04, 0x00, 0x27, 0x00, 0x01, 0x06, 0x00, 0x28, 0x00, 0x01, 0xFF, 0x0C, 0x00, 0x07, 0x6C, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 });
            ByteBuffer Login7ReplyByteBuffer = new ByteBuffer(new byte[] { 0x04, 0x01, 0x00, 0xE7, 0x00, 0x00, 0x01, 0x00, 0xAD, 0x32, 0x00, 0x01, 0x74, 0x00, 0x00, 0x04, 0x14, 0x4D, 0x00, 0x69, 0x00, 0x63, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x73, 0x00, 0x6F, 0x00, 0x66, 0x00, 0x74, 0x00, 0x20, 0x00, 0x53, 0x00, 0x51, 0x00, 0x4C, 0x00, 0x20, 0x00, 0x53, 0x00, 0x65, 0x00, 0x72, 0x00, 0x76, 0x00, 0x65, 0x00, 0x72, 0x00, 0x0C, 0x00, 0x07, 0x6C, 0xE3, 0x84, 0x00, 0x14, 0x7F, 0x00, 0x00, 0x54, 0x2B, 0x3D, 0x00, 0x64, 0x00, 0x39, 0x00, 0x66, 0x00, 0x65, 0x00, 0x61, 0x00, 0x64, 0x00, 0x61, 0x00, 0x35, 0x00, 0x31, 0x00, 0x33, 0x00, 0x39, 0x00, 0x62, 0x00, 0x2E, 0x00, 0x74, 0x00, 0x72, 0x00, 0x32, 0x00, 0x39, 0x00, 0x33, 0x00, 0x31, 0x00, 0x2E, 0x00, 0x77, 0x00, 0x65, 0x00, 0x73, 0x00, 0x74, 0x00, 0x65, 0x00, 0x75, 0x00, 0x72, 0x00, 0x6F, 0x00, 0x70, 0x00, 0x65, 0x00, 0x31, 0x00, 0x2D, 0x00, 0x61, 0x00, 0x2E, 0x00, 0x77, 0x00, 0x6F, 0x00, 0x72, 0x00, 0x6B, 0x00, 0x65, 0x00, 0x72, 0x00, 0x2E, 0x00, 0x64, 0x00, 0x61, 0x00, 0x74, 0x00, 0x61, 0x00, 0x62, 0x00, 0x61, 0x00, 0x73, 0x00, 0x65, 0x00, 0x2E, 0x00, 0x77, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x64, 0x00, 0x6F, 0x00, 0x77, 0x00, 0x73, 0x00, 0x2E, 0x00, 0x6E, 0x00, 0x65, 0x00, 0x74, 0x00, 0x00, 0x00, 0xE3, 0x13, 0x00, 0x04, 0x04, 0x34, 0x00, 0x30, 0x00, 0x39, 0x00, 0x36, 0x00, 0x04, 0x34, 0x00, 0x30, 0x00, 0x39, 0x00, 0x36, 0x00, 0xFD, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

            ServerEndpoint dbServerEndpoint = new ServerEndpoint("azuresql-test.database.windows.net", 1433);

            Message preLoginMessage = new Message(PacketType.PreLogin) { Payload = new PreLoginPayload(true) };

            Login7Payload login7Payload = new Login7Payload()
            {
                Hostname = "TEST-ENV",
                ServerName = dbServerEndpoint.Hostname,
                AppName = "TdsLibUnitTest",
                Language = "us_english",
                Database = "test",
                ClientId = new ByteBuffer(new byte[] { 0xE8, 0xFA, 0xBB, 0x2B, 0x09, 0xFF })
            };

            login7Payload.TypeFlags.AccessIntent = OptionAccessIntent.ReadWrite;

            login7Payload.Options.ClientLcid = 0x00000809;
            login7Payload.Options.ClientPid = 0x00004440;
            login7Payload.Options.ClientProgVer = 0x00000000;
            login7Payload.Options.ClientTimeZone = 0x00000000;
            login7Payload.Options.ConnectionId = 0x00000000;
            login7Payload.Options.PacketSize = 0x00001000;
            login7Payload.Options.TdsVersion = TdsVersion.V7_4;

            Message login7Message = new Message(PacketType.Login7) { Payload = login7Payload };

            #endregion

            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();

            Thread serverThread = new Thread(() =>
            {
                using TcpClient tcpClient = tcpListener.AcceptTcpClient();
                using NetworkStream stream = tcpClient.GetStream();

                static byte[] Read(Stream stream, int size)
                {
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(1024);
                    List<byte> readData = new List<byte>();

                    try
                    {
                        while (readData.Count != size)
                        {
                            int read = stream.Read(buffer, 0, buffer.Length);

                            if (read == 0)
                            {
                                throw new InvalidOperationException("Invalid read");
                            }

                            readData.AddRange(buffer.AsEnumerable().Take(read));
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }

                    return readData.ToArray();
                }

                byte[] preLoginRaw = Read(stream, PreLoginByteBuffer.Length);
                ByteBuffer preLoginReceived = new ByteBuffer(preLoginRaw);

                if (preLoginReceived != PreLoginByteBuffer)
                {
                    throw new Xunit.Sdk.XunitException("Invalid PreLogin received");
                }

                stream.Write(PreLoginReplyByteBuffer.ToArray());

                byte[] login7Raw = Read(stream, Login7ByteBuffer.Length);
                ByteBuffer login7Received = new ByteBuffer(login7Raw);

                if (login7Received != Login7ByteBuffer)
                {
                    throw new Xunit.Sdk.XunitException("Invalid Login7 received");
                }

                stream.Write(Login7ReplyByteBuffer.ToArray());
            });
            serverThread.Start();

            try
            {
                IPEndPoint endpoint = tcpListener.LocalEndpoint as IPEndPoint;
                using TcpConnection connection = new TcpConnection(new TcpConnectionOptions(), new ServerEndpoint(endpoint.Address.ToString(), endpoint.Port));

                using TdsClient client = new TdsClient(connection);

                #region PreLogin Phase

                await client.MessageHandler.SendMessage(preLoginMessage);

                Message message = await client.MessageHandler.ReceiveMessage(b => new PreLoginPayload(b));

                #endregion

                // Even though we set encrypt=true in the PreLogin payload we do not perform the TLS handshake.

                #region Login Phase

                await client.MessageHandler.SendMessage(login7Message);

                await client.TokenStreamHandler.ReceiveTokensAsync((tokenEvent) =>
                {
                    if (tokenEvent.Token is ErrorToken)
                    {
                        throw new Xunit.Sdk.XunitException("ErrorToken received");
                    }
                });

                #endregion
            }
            finally
            {
                tcpListener.Stop();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TdsLib.Buffer;
using Microsoft.TdsLib.Messages;
using Microsoft.TdsLib.Tokens.Done;
using Microsoft.TdsLib.Tokens.DoneInProc;
using Microsoft.TdsLib.Tokens.DoneProc;
using Microsoft.TdsLib.Tokens.EnvChange;
using Microsoft.TdsLib.Tokens.Error;
using Microsoft.TdsLib.Tokens.FeatureExtAck;
using Microsoft.TdsLib.Tokens.FedAuthInfo;
using Microsoft.TdsLib.Tokens.Info;
using Microsoft.TdsLib.Tokens.LoginAck;
using Microsoft.TdsLib.Tokens.ReturnStatus;

namespace Microsoft.TdsLib.Tokens
{
    /// <summary>
    /// Token data stream handler.
    /// </summary>
    public sealed class TokenStreamHandler
    {

        private readonly TdsClient tdsClient;
        private readonly Dictionary<TokenType, TokenParser> parsers;

        private ByteBuffer incomingTokenBuffer;
        private int offset;

        internal ConnectionOptions Options => tdsClient.Connection.Options;

        internal TokenStreamHandler(TdsClient tdsClient)
        {
            this.tdsClient = tdsClient;

            parsers = new Dictionary<TokenType, TokenParser>
            {
                [TokenType.EnvChange] = new EnvChangeTokenParser(),
                [TokenType.LoginAck] = new LoginAckTokenParser(),
                [TokenType.FeatureExtAck] = new FeatureExtAckTokenParser(),
                [TokenType.Done] = new DoneTokenParser(),
                [TokenType.DoneInProc] = new DoneInProcTokenParser(),
                [TokenType.DoneProc] = new DoneProcTokenParser(),
                [TokenType.FedAuthInfo] = new FedAuthInfoTokenParser(),
                [TokenType.Info] = new InfoTokenParser(),
                [TokenType.Error] = new ErrorTokenParser(),
                [TokenType.ReturnStatus] = new ReturnStatusTokenParser()
            };
        }

        private async Task WaitForData(int size)
        {
            if (incomingTokenBuffer == null)
            {
                Message message = await tdsClient.MessageHandler.ReceiveMessage().ConfigureAwait(false);
                incomingTokenBuffer = message.Payload.Buffer;
            }

            while (incomingTokenBuffer.Length < offset + size)
            {
                Message message = await tdsClient.MessageHandler.ReceiveMessage().ConfigureAwait(false);
                incomingTokenBuffer = incomingTokenBuffer.Concat(message.Payload.Buffer);
            }
        }

        private bool DataAvailable()
        {
            return incomingTokenBuffer?.Length > offset;
        }

        private void TrimBuffer()
        {
            if (incomingTokenBuffer is null)
            {
                return;
            }

            if (offset == incomingTokenBuffer.Length)
            {
                incomingTokenBuffer = null;
            }
            else
            {
                incomingTokenBuffer = incomingTokenBuffer.Slice(offset);
            }

            offset = 0;
        }

        private void ClearBuffer()
        {
            incomingTokenBuffer = null;
        }

        /// <summary>
        /// Receive a token.
        /// </summary>
        /// <returns>Awaitable task. Token.</returns>
        public async Task<Token> ReceiveTokenAsync()
        {
            byte type = await ReadUInt8().ConfigureAwait(false);

            if (!Enum.IsDefined(typeof(TokenType), type))
            {
                throw new InvalidOperationException($"Unsupported Token type: 0x{type:X2}");
            }

            TokenType tokenType = (TokenType)type;

            if (!parsers.ContainsKey(tokenType))
            {
                throw new InvalidOperationException($"Unsupported Token type: {tokenType}");
            }

            Token token = await parsers[tokenType].Parse(tokenType, this).ConfigureAwait(false);
            TrimBuffer();
            return token;
        }

        /// <summary>
        /// Receives tokens until the end of data or the receiver exits.
        /// </summary>
        /// <param name="funcTokenReceiver">The token receiver. This action will be invoked for every token received.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task.</returns>
        public async Task ReceiveTokensAsync(Action<TokenEvent> funcTokenReceiver, CancellationToken cancellationToken = default)
        {
            TokenEvent tokenEvent = new TokenEvent();

            try
            {
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    tokenEvent.Token = await ReceiveTokenAsync().ConfigureAwait(false);
                    funcTokenReceiver(tokenEvent);

                    if (tokenEvent.Exit)
                    {
                        break;
                    }
                } while (DataAvailable());
            }
            finally
            {
                ClearBuffer();
            }
        }

        /// <summary>
        /// Read an Int8 from the stream. 
        /// This method ensures (and waits if necessary) that the correct amount of data is present in the buffer for reading.
        /// </summary>
        /// <returns>Int8 value.</returns>
        internal async Task<sbyte> ReadInt8()
        {
            try
            {
                await WaitForData(sizeof(sbyte)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt8(offset);
            }
            finally
            {
                offset += sizeof(sbyte);
            }
        }

        internal async Task<byte> ReadUInt8()
        {
            try
            {
                await WaitForData(sizeof(byte)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt8(offset);
            }
            finally
            {
                offset += sizeof(byte);
            }
        }

        internal async Task<short> ReadInt16LE()
        {
            try
            {
                await WaitForData(sizeof(short)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt16LE(offset);
            }
            finally
            {
                offset += sizeof(short);
            }
        }

        internal async Task<short> ReadInt16BE()
        {
            try
            {
                await WaitForData(sizeof(short)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt16BE(offset);
            }
            finally
            {
                offset += sizeof(short);
            }
        }

        internal async Task<ushort> ReadUInt16LE()
        {
            try
            {
                await WaitForData(sizeof(ushort)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt16LE(offset);
            }
            finally
            {
                offset += sizeof(ushort);
            }
        }

        internal async Task<ushort> ReadUInt16BE()
        {
            try
            {
                await WaitForData(sizeof(ushort)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt16BE(offset);
            }
            finally
            {
                offset += sizeof(ushort);
            }
        }

        internal async Task<int> ReadInt32LE()
        {
            try
            {
                await WaitForData(sizeof(int)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt32LE(offset);
            }
            finally
            {
                offset += sizeof(int);
            }
        }

        internal async Task<int> ReadInt32BE()
        {
            try
            {
                await WaitForData(sizeof(int)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt32BE(offset);
            }
            finally
            {
                offset += sizeof(int);
            }
        }

        internal async Task<uint> ReadUInt32LE()
        {
            try
            {
                await WaitForData(sizeof(uint)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt32LE(offset);
            }
            finally
            {
                offset += sizeof(uint);
            }
        }

        internal async Task<uint> ReadUInt32BE()
        {
            try
            {
                await WaitForData(sizeof(uint)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt32BE(offset);
            }
            finally
            {
                offset += sizeof(uint);
            }
        }

        internal async Task<long> ReadInt64LE()
        {
            try
            {
                await WaitForData(sizeof(long)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt64LE(offset);
            }
            finally
            {
                offset += sizeof(long);
            }
        }

        internal async Task<long> ReadInt64BE()
        {
            try
            {
                await WaitForData(sizeof(long)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadInt64BE(offset);
            }
            finally
            {
                offset += sizeof(long);
            }
        }

        internal async Task<ulong> ReadUInt64LE()
        {
            try
            {
                await WaitForData(sizeof(ulong)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt64LE(offset);
            }
            finally
            {
                offset += sizeof(ulong);
            }
        }

        internal async Task<ulong> ReadUInt64BE()
        {
            try
            {
                await WaitForData(sizeof(ulong)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadUInt64BE(offset);
            }
            finally
            {
                offset += sizeof(ulong);
            }
        }

        internal async Task<float> ReadFloatLE()
        {
            try
            {
                await WaitForData(sizeof(float)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadFloatLE(offset);
            }
            finally
            {
                offset += sizeof(float);
            }
        }

        internal async Task<float> ReadFloatBE()
        {
            try
            {
                await WaitForData(sizeof(float)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadFloatBE(offset);
            }
            finally
            {
                offset += sizeof(float);
            }
        }

        internal async Task<double> ReadDoubleLE()
        {
            try
            {
                await WaitForData(sizeof(double)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadDoubleLE(offset);
            }
            finally
            {
                offset += sizeof(double);
            }
        }

        internal async Task<double> ReadDoubleBE()
        {
            try
            {
                await WaitForData(sizeof(double)).ConfigureAwait(false);
                return incomingTokenBuffer.ReadDoubleBE(offset);
            }
            finally
            {
                offset += sizeof(double);
            }
        }

        internal async Task<uint> ReadUInt24LE()
        {
            try
            {
                await WaitForData(sizeof(ushort) + sizeof(byte)).ConfigureAwait(false);

                ushort low = incomingTokenBuffer.ReadUInt16LE(offset);
                byte high = incomingTokenBuffer.ReadUInt8(offset + sizeof(ushort));

                return low | ((uint)high << 16);
            }
            finally
            {
                offset += sizeof(ushort) + sizeof(byte);
            }
        }

        internal async Task<ulong> ReadUInt40LE()
        {
            try
            {
                await WaitForData(sizeof(uint) + sizeof(byte)).ConfigureAwait(false);

                uint low = incomingTokenBuffer.ReadUInt32LE(offset);
                byte high = incomingTokenBuffer.ReadUInt8(offset + sizeof(uint));

                return low | ((ulong)high << 32);
            }
            finally
            {
                offset += sizeof(uint) + sizeof(byte);
            }
        }

        internal async Task<ulong> ReadUNumeric64LE()
        {
            try
            {
                await WaitForData(sizeof(uint) + sizeof(uint)).ConfigureAwait(false);

                uint low = incomingTokenBuffer.ReadUInt32LE(offset);
                uint high = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint));

                return low | ((ulong)high << 32);
            }
            finally
            {
                offset += sizeof(uint) + sizeof(uint);
            }
        }

        internal async Task<BigInteger> ReadUNumeric96LE()
        {
            try
            {
                await WaitForData(sizeof(uint) + sizeof(uint) + sizeof(uint)).ConfigureAwait(false);

                uint dword1 = incomingTokenBuffer.ReadUInt32LE(offset);
                uint dword2 = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint));
                uint dword3 = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint) + sizeof(uint));

                return dword3 | ((ulong)dword2 << 32) | (new BigInteger(dword1) << 64);
            }
            finally
            {
                offset += sizeof(uint) + sizeof(uint) + sizeof(uint);
            }
        }

        internal async Task<BigInteger> ReadUNumeric128LE()
        {
            try
            {
                await WaitForData(sizeof(uint) + sizeof(uint) + sizeof(uint) + sizeof(uint)).ConfigureAwait(false);

                uint dword1 = incomingTokenBuffer.ReadUInt32LE(offset);
                uint dword2 = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint));
                uint dword3 = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint) + sizeof(uint));
                uint dword4 = incomingTokenBuffer.ReadUInt32LE(offset + sizeof(uint) + sizeof(uint) + sizeof(uint));

                return dword4 | ((ulong)dword3 << 32) | (new BigInteger(dword2) << 64) | (new BigInteger(dword1) << 128);
            }
            finally
            {
                offset += sizeof(uint) + sizeof(uint) + sizeof(uint) + sizeof(uint);
            }
        }

        internal async Task<ByteBuffer> ReadBuffer(int length)
        {
            try
            {
                await WaitForData(length).ConfigureAwait(false);
                return incomingTokenBuffer.Slice(offset, length);
            }
            finally
            {
                offset += length;
            }
        }

        internal async Task<string> ReadBVarChar()
        {
            await WaitForData(sizeof(byte)).ConfigureAwait(false);
            byte length = incomingTokenBuffer.ReadUInt8(offset);

            await WaitForData(length * 2).ConfigureAwait(false);
            ByteBuffer stringBuffer = incomingTokenBuffer.Slice(offset + sizeof(byte), length * 2);

            offset += sizeof(byte) + length * 2;
            return Encoding.Unicode.GetString(stringBuffer.ToArraySegment().Array);
        }

        internal async Task<string> ReadUsVarChar()
        {
            await WaitForData(sizeof(ushort)).ConfigureAwait(false);
            ushort length = incomingTokenBuffer.ReadUInt16LE(offset);

            await WaitForData(length * 2).ConfigureAwait(false);
            ByteBuffer stringBuffer = incomingTokenBuffer.Slice(offset + sizeof(ushort), length * 2);

            offset += sizeof(ushort) + length * 2;
            return Encoding.Unicode.GetString(stringBuffer.ToArraySegment().Array);
        }

        internal async Task<ByteBuffer> ReadBVarByte()
        {
            await WaitForData(sizeof(byte)).ConfigureAwait(false);
            byte length = incomingTokenBuffer.ReadUInt8(offset);

            await WaitForData(length).ConfigureAwait(false);
            ByteBuffer buffer = incomingTokenBuffer.Slice(offset + sizeof(byte), length);

            offset += sizeof(byte) + length;
            return buffer;
        }

        internal async Task<ByteBuffer> ReadUsVarByte()
        {
            await WaitForData(sizeof(ushort)).ConfigureAwait(false);
            ushort length = incomingTokenBuffer.ReadUInt16LE(offset);

            await WaitForData(length).ConfigureAwait(false);
            ByteBuffer buffer = incomingTokenBuffer.Slice(offset + sizeof(ushort), length);

            offset += sizeof(ushort) + length;
            return buffer;
        }

    }
}

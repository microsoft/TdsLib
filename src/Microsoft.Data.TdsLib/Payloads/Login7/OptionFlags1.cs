// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Data.TdsLib.Payloads.Login7
{

    /// <summary>
    /// Endianness.
    /// </summary>
    public enum OptionEndian
    {
        /// <summary>
        /// Little endian.
        /// </summary>
        LittleEndian,

        /// <summary>
        /// Big endian.
        /// </summary>
        BigEndian
    }

    /// <summary>
    /// Charset.
    /// </summary>
    public enum OptionCharset
    {
        /// <summary>
        /// Ascii.
        /// </summary>
        Ascii,

        /// <summary>
        /// Ebcdic.
        /// </summary>
        Ebcdic
    }

    /// <summary>
    /// Float.
    /// </summary>
    public enum OptionFloat
    {
        /// <summary>
        /// IEEE.
        /// </summary>
        IEEE,

        /// <summary>
        /// VAX.
        /// </summary>
        VAX,

        /// <summary>
        /// ND5000.
        /// </summary>
        ND5000
    }

    /// <summary>
    /// BCP Dumpload.
    /// </summary>
    public enum OptionBcpDumpload
    {
        /// <summary>
        /// On.
        /// </summary>
        On,

        /// <summary>
        /// Off.
        /// </summary>
        Off
    }

    /// <summary>
    /// Defines if the server sends warning messages on execution of the USE SQL statement.
    /// </summary>
    public enum OptionUseDb
    {
        /// <summary>
        /// On.
        /// </summary>
        On,

        /// <summary>
        /// Off.
        /// </summary>
        Off
    }

    /// <summary>
    /// Defines if the change to initial database needs to succeed if the connection is to succeed.
    /// </summary>
    public enum OptionInitDb
    {
        /// <summary>
        /// Warning.
        /// </summary>
        Warn,

        /// <summary>
        /// Fatal.
        /// </summary>
        Fatal
    }

    /// <summary>
    /// Defines if the client requires warning messages on execution of a language change statement.
    /// </summary>
    public enum OptionLangWarn
    {
        /// <summary>
        /// On.
        /// </summary>
        On,

        /// <summary>
        /// Off.
        /// </summary>
        Off
    }

    /// <summary>
    /// Option flags 1.
    /// </summary>
    public sealed class OptionFlags1
    {
        private const int OptionEndianBitIndex = 0x01;
        private const int OptionCharsetBitIndex = 0x02;
        private const int OptionFloatBitIndexVax = 0x04;
        private const int OptionFloatBitIndexND5000 = 0x08;
        private const int OptionBcpDumploadBitIndex = 0x10;
        private const int OptionUseDbBitIndex = 0x20;
        private const int OptionIndexDbBitIndex = 0x40;
        private const int OptionLangWarnBitIndex = 0x80;

        /// <summary>
        /// The option raw value.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// Endianness.
        /// </summary>
        public OptionEndian Endian
        {
            get
            {
                if ((Value & OptionEndianBitIndex) == OptionEndianBitIndex)
                {
                    return OptionEndian.BigEndian;
                }

                return OptionEndian.LittleEndian;
            }

            set
            {
                if (value == OptionEndian.LittleEndian)
                {
                    Value &= byte.MaxValue - OptionEndianBitIndex;
                }
                else
                {
                    Value |= OptionEndianBitIndex;
                }
            }
        }

        /// <summary>
        /// Charset.
        /// </summary>
        public OptionCharset Charset
        {
            get
            {
                if ((Value & OptionCharsetBitIndex) == OptionCharsetBitIndex)
                {
                    return OptionCharset.Ebcdic;
                }

                return OptionCharset.Ascii;
            }

            set
            {
                if (value == OptionCharset.Ascii)
                {
                    Value &= byte.MaxValue - OptionCharsetBitIndex;
                }
                else
                {
                    Value |= OptionCharsetBitIndex;
                }
            }
        }

        /// <summary>
        /// Float type.
        /// </summary>
        public OptionFloat Float
        {
            get
            {
                if ((Value & OptionFloatBitIndexVax) == OptionFloatBitIndexVax)
                {
                    return OptionFloat.VAX;
                }
                else if ((Value & OptionFloatBitIndexND5000) == OptionFloatBitIndexND5000)
                {
                    return OptionFloat.ND5000;
                }

                return OptionFloat.IEEE;
            }

            set
            {
                if (value == OptionFloat.IEEE)
                {
                    Value &= byte.MaxValue - OptionFloatBitIndexVax;
                    Value &= byte.MaxValue - OptionFloatBitIndexND5000;
                }
                else if (value == OptionFloat.VAX)
                {
                    Value |= OptionFloatBitIndexVax;
                    Value &= byte.MaxValue - OptionFloatBitIndexND5000;
                }
                else
                {
                    Value &= byte.MaxValue - OptionFloatBitIndexVax;
                    Value |= OptionFloatBitIndexND5000;
                }
            }
        }

        /// <summary>
        /// BCP Dumpload capabilities.
        /// </summary>
        public OptionBcpDumpload BcpDumpload
        {
            get
            {
                if ((Value & OptionBcpDumploadBitIndex) == OptionBcpDumploadBitIndex)
                {
                    return OptionBcpDumpload.Off;
                }

                return OptionBcpDumpload.On;
            }

            set
            {
                if (value == OptionBcpDumpload.On)
                {
                    Value &= byte.MaxValue - OptionBcpDumploadBitIndex;
                }
                else
                {
                    Value |= OptionBcpDumploadBitIndex;
                }
            }
        }

        /// <summary>
        /// Defines is client needs warning messages on execution of USE SQL statement.
        /// </summary>
        public OptionUseDb UseDb
        {
            get
            {
                if ((Value & OptionUseDbBitIndex) == OptionUseDbBitIndex)
                {
                    return OptionUseDb.Off;
                }

                return OptionUseDb.On;
            }

            set
            {
                if (value == OptionUseDb.On)
                {
                    Value &= byte.MaxValue - OptionUseDbBitIndex;
                }
                else
                {
                    Value |= OptionUseDbBitIndex;
                }
            }
        }

        /// <summary>
        /// Behavior when an initial database change needs to succeed.
        /// </summary>
        public OptionInitDb InitDb
        {
            get
            {
                if ((Value & OptionIndexDbBitIndex) == OptionIndexDbBitIndex)
                {
                    return OptionInitDb.Fatal;
                }

                return OptionInitDb.Warn;
            }

            set
            {
                if (value == OptionInitDb.Warn)
                {
                    Value &= byte.MaxValue - OptionIndexDbBitIndex;
                }
                else
                {
                    Value |= OptionIndexDbBitIndex;
                }
            }
        }

        /// <summary>
        /// Behavior when a language change statement is executed.
        /// </summary>
        public OptionLangWarn LangWarn
        {
            get
            {
                if ((Value & OptionLangWarnBitIndex) == OptionLangWarnBitIndex)
                {
                    return OptionLangWarn.On;
                }

                return OptionLangWarn.Off;
            }

            set
            {
                if (value == OptionLangWarn.Off)
                {

                    Value &= byte.MaxValue - OptionLangWarnBitIndex;
                }
                else
                {
                    Value |= OptionLangWarnBitIndex;
                }
            }
        }

        /// <summary>
        /// Create a new instance of this class from a raw value.
        /// </summary>
        /// <param name="value">Raw value.</param>
        public OptionFlags1(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new instance of this class with a default value.
        /// </summary>
        public OptionFlags1()
        {
            Endian = OptionEndian.LittleEndian;
            Charset = OptionCharset.Ascii;
            Float = OptionFloat.IEEE;
            BcpDumpload = OptionBcpDumpload.Off;
            UseDb = OptionUseDb.Off;
            LangWarn = OptionLangWarn.On;
            InitDb = OptionInitDb.Warn;
        }

        /// <summary>
        /// Convert the object explicitly into a byte.
        /// </summary>
        /// <param name="optionFlags1">The option instance.</param>
        public static explicit operator byte(OptionFlags1 optionFlags1) => optionFlags1.Value;

        /// <summary>
        /// Creates a new instance of this class from a byte value. 
        /// </summary>
        /// <param name="value">The raw value.</param>
        public static explicit operator OptionFlags1(byte value) => new OptionFlags1(value);

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"OptionFlags1[0b{Convert.ToString(Value, 2).PadLeft(8, '0')}(Endian={Endian}, Charset={Charset}, Float={Float}, BcpDumpload={BcpDumpload}, UseDb={UseDb}, InitDb={InitDb}, LangWarn={LangWarn})]";
        }

    }
}

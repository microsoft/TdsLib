// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Data.TdsLib.Payloads.Login7
{

    /// <summary>
    /// Define if the change to initial language needs to succeed if the connect is to succeed.
    /// </summary>
    public enum OptionInitLang
    {
        /// <summary>
        /// Only warn.
        /// </summary>
        Warn,

        /// <summary>
        /// Fail with fatal.
        /// </summary>
        Fatal
    }

    /// <summary>
    /// ODBC.
    /// </summary>
    public enum OptionOdbc
    {
        /// <summary>
        /// Off.
        /// </summary>
        Off,

        /// <summary>
        /// On.
        /// </summary>
        On
    }

    /// <summary>
    /// User.
    /// </summary>
    public enum OptionUser
    {
        /// <summary>
        /// Normal user login.
        /// </summary>
        Normal,

        /// <summary>
        /// Reserved.
        /// </summary>
        Server,

        /// <summary>
        /// Distributed Query login.
        /// </summary>
        RemUser,

        /// <summary>
        /// Replication login.
        /// </summary>
        SqlRepl
    }

    /// <summary>
    /// Integrated security.
    /// </summary>
    public enum OptionIntegratedSecurity
    {
        /// <summary>
        /// Off.
        /// </summary>
        Off,

        /// <summary>
        /// On.
        /// </summary>
        On
    }

    /// <summary>
    /// Option flags 2.
    /// </summary>
    public sealed class OptionFlags2
    {
        private const int OptionInitLangBitIndex = 0x01;
        private const int OptionOdbcBitIndex = 0x02;
        private const int OptionUserBitIndexServer = 0x10;
        private const int OptionUserBitIndexRemUser = 0x20;
        private const int OptionUserBitIndexSqlRepl = 0x40;
        private const int OptionIntegratedSecurityBitIndex = 0x80;

        /// <summary>
        /// The option raw value.
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// Init lang flag.
        /// </summary>
        public OptionInitLang InitLang
        {
            get
            {
                if ((Value & OptionInitLangBitIndex) == OptionInitLangBitIndex)
                {
                    return OptionInitLang.Fatal;
                }

                return OptionInitLang.Warn;
            }

            set
            {
                if (value == OptionInitLang.Warn)
                {
                    Value &= byte.MaxValue - OptionInitLangBitIndex;
                }
                else
                {
                    Value |= OptionInitLangBitIndex;
                }
            }
        }

        /// <summary>
        /// ODBC.
        /// </summary>
        public OptionOdbc ODBC
        {
            get
            {
                if ((Value & OptionOdbcBitIndex) == OptionOdbcBitIndex)
                {
                    return OptionOdbc.On;
                }

                return OptionOdbc.Off;
            }

            set
            {
                if (value == OptionOdbc.Off)
                {
                    Value &= byte.MaxValue - OptionInitLangBitIndex;
                }
                else
                {
                    Value |= OptionInitLangBitIndex;
                }
            }
        }

        /// <summary>
        /// User flag.
        /// </summary>
        public OptionUser User
        {
            get
            {
                if ((Value & OptionUserBitIndexServer) == OptionUserBitIndexServer)
                {
                    return OptionUser.Server;
                }

                if ((Value & OptionUserBitIndexRemUser) == OptionUserBitIndexRemUser)
                {
                    return OptionUser.RemUser;
                }

                if ((Value & OptionUserBitIndexSqlRepl) == OptionUserBitIndexSqlRepl)
                {
                    return OptionUser.SqlRepl;
                }

                return OptionUser.Normal;
            }

            set
            {
                if (value == OptionUser.Normal)
                {
                    Value &= byte.MaxValue - OptionUserBitIndexServer;
                    Value &= byte.MaxValue - OptionUserBitIndexRemUser;
                    Value &= byte.MaxValue - OptionUserBitIndexSqlRepl;
                }
                else if (value == OptionUser.Server)
                {
                    Value |= OptionUserBitIndexServer;
                    Value &= byte.MaxValue - OptionUserBitIndexRemUser;
                    Value &= byte.MaxValue - OptionUserBitIndexSqlRepl;
                }
                else if (value == OptionUser.RemUser)
                {
                    Value &= byte.MaxValue - OptionUserBitIndexServer;
                    Value |= OptionUserBitIndexRemUser;
                    Value &= byte.MaxValue - OptionUserBitIndexSqlRepl;
                }
                else if (value == OptionUser.SqlRepl)
                {
                    Value &= byte.MaxValue - OptionUserBitIndexServer;
                    Value &= byte.MaxValue - OptionUserBitIndexRemUser;
                    Value |= OptionUserBitIndexSqlRepl;
                }
            }
        }

        /// <summary>
        /// Integrated security flag.
        /// </summary>
        public OptionIntegratedSecurity IntegratedSecurity
        {
            get
            {
                if ((Value & OptionIntegratedSecurityBitIndex) == OptionIntegratedSecurityBitIndex)
                {
                    return OptionIntegratedSecurity.On;
                }

                return OptionIntegratedSecurity.Off;
            }

            set
            {
                if (value == OptionIntegratedSecurity.Off)
                {
                    Value &= byte.MaxValue - OptionIntegratedSecurityBitIndex;
                }
                else
                {
                    Value |= OptionIntegratedSecurityBitIndex;
                }
            }
        }

        /// <summary>
        /// Create a new instance of this class from a raw value.
        /// </summary>
        /// <param name="value">Raw value.</param>
        public OptionFlags2(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new instance of this class with a default value.
        /// </summary>
        public OptionFlags2()
        {
            InitLang = OptionInitLang.Warn;
            ODBC = OptionOdbc.Off;
            User = OptionUser.Normal;
            IntegratedSecurity = OptionIntegratedSecurity.Off;
        }

        /// <summary>
        /// Convert the object explicitly into a byte.
        /// </summary>
        /// <param name="optionFlags2">The option instance.</param>
        public static explicit operator byte(OptionFlags2 optionFlags2) => optionFlags2.Value;

        /// <summary>
        /// Creates a new instance of this class from a byte value. 
        /// </summary>
        /// <param name="value">The raw value.</param>
        public static explicit operator OptionFlags2(byte value) => new OptionFlags2(value);

        /// <summary>
        /// Gets a human readable string representation of this object.
        /// </summary>
        /// <returns>Human readable string representation.</returns>
        public override string ToString()
        {
            return $"OptionFlags2[0b{Convert.ToString(Value, 2).PadLeft(8, '0')}(InitLang={InitLang}, ODBC={ODBC}, User={User}, IntegratedSecurity={IntegratedSecurity})]";
        }

    }
}

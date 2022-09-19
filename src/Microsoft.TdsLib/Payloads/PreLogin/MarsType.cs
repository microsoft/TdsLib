namespace Microsoft.TdsLib.Payloads.PreLogin
{
    internal static class MarsType
    {
        public const int Off = 0x00;
        public const int On = 0x01;

        public static string GetString(uint type)
        {
            switch (type)
            {
                case Off:
                    return "Off";
                case On:
                    return "On";
                default:
                    return "Unknown";
            }
        }

    }
}

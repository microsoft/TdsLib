using Microsoft.TdsLib.Buffer;

namespace Microsoft.TdsLib.Payloads.Login7.Auth
{
    /// <summary>
    /// Federate authentication feature extension.
    /// </summary>
    public abstract class FedAuth
    {
        /// <summary>
        /// Feature Id.
        /// </summary>
        protected const byte FeatureId = 0x02;

        /// <summary>
        /// Security token.
        /// </summary>
        protected const byte LibrarySecurityToken = 0x02;

        /// <summary>
        /// Active Directory Authentication Library.
        /// </summary>
        protected const byte LibraryADAL = 0x04;

        /// <summary>
        /// Enable echo.
        /// </summary>
        protected const byte FedAuthEchoYes = 0x01;

        /// <summary>
        /// Disable echo.
        /// </summary>
        protected const byte FedAuthEchoNo = 0x00;

        internal FedAuth()
        {
        }

        internal abstract ByteBuffer GetBuffer();

    }
}

using System.Net;

namespace SagaBNS.Networking
{
    public static class IPAddressExtension
    {
        #region Methods

        public static IPAddress Localhost() => IPAddress.Parse("127.0.0.1");

        #endregion
    }
}
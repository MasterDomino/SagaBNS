using System.IO;
using System.Net.Sockets;

namespace SagaBNS.Networking
{
    public class SocketPacket
    {
        #region Members

        public const int BUFFER_SIZE = 1024;

        public byte[] Buffer = new byte[BUFFER_SIZE];

        public MemoryStream Stream;

        public Socket Socket;

        #endregion
    }
}
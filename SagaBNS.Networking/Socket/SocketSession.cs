using System.Net.Sockets;

namespace SagaBNS.Networking
{
    public class SocketSession
    {
        #region Members

        public byte[] Buffer = new byte[SocketPacket.BUFFER_SIZE];

        public long SessionId;

        public Socket Socket;

        #endregion
    }
}
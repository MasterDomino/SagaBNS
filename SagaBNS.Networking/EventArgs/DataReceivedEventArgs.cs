namespace SagaBNS.Networking
{
    public class DataReceivedEventArgs
    {
        #region Instantiation

        public DataReceivedEventArgs(SocketPacket packet) => Packet = packet;

        public DataReceivedEventArgs(SocketPacket packet, SocketSession session)
        {
            Packet = packet;
            Session = session;
        }

        #endregion

        #region Properties

        public SocketPacket Packet { get; }

        public SocketSession Session { get; }

        #endregion
    }
}
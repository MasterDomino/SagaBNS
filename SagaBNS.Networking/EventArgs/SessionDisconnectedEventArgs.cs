using System;

namespace SagaBNS.Networking
{
    public class SessionDisconnectedEventArgs
    {
        #region Members

        public Exception Exception;

        public SocketSession Session;

        #endregion

        #region Instantiation

        public SessionDisconnectedEventArgs(Exception ex) => Exception = ex;

        public SessionDisconnectedEventArgs(SocketSession session) => Session = session;

        #endregion
    }
}
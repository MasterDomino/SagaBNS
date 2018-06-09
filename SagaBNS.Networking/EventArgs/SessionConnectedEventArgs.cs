using System;

namespace SagaBNS.Networking
{
    public class SessionConnectedEventArgs
    {
        #region Members

        public bool ConnectionSuccessful;

        public Exception Exception;

        public SocketSession Session;

        #endregion

        #region Instantiation

        public SessionConnectedEventArgs(bool success, SocketSession session, Exception ex)
        {
            ConnectionSuccessful = success;
            Session = session;
            Exception = ex;
        }

        #endregion
    }
}
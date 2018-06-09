using System;
using System.IO;

namespace SagaBNS.Networking
{
    public interface INetworkServer
    {
        #region Events

        event EventHandler<DataReceivedEventArgs> DataReceived;

        event EventHandler<ExceptionEventArgs> ExceptionCaught;

        event EventHandler<SessionConnectedEventArgs> SessionConnected;

        event EventHandler<SessionDisconnectedEventArgs> SessionDisconnected;

        #endregion

        #region Properties

        bool IsDisposing { get; set; }

        #endregion

        #region Methods

        void Dispose();

        void DisposeClient(long sessionId);

        void ReceiveCallback(IAsyncResult async);

        void SendMessage(long sessionId, MemoryStream stream);

        void StartServer();

        #endregion
    }
}
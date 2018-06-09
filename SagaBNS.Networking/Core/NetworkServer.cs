using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SagaBNS.Networking
{
    public class NetworkServer : INetworkServer, IDisposable
    {
        #region Members

        private readonly IPEndPoint _ipEndPoint;
        private readonly ConcurrentDictionary<long, SocketSession> _clientSockets;
        private bool _disposed;
        private Socket _serverSocket;

        #endregion

        #region Instantiation

        public NetworkServer(IPEndPoint ipEndPoint/*, int maxClients*/)
        {
            _ipEndPoint = ipEndPoint;

            //_maxClients = maxClients;
            _clientSockets = new ConcurrentDictionary<long, SocketSession>();
        }

        #endregion

        #region Events

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public event EventHandler<ExceptionEventArgs> ExceptionCaught;

        //private int _maxClients;
        public event EventHandler<SessionConnectedEventArgs> SessionConnected;

        public event EventHandler<SessionDisconnectedEventArgs> SessionDisconnected;

        #endregion

        #region Properties

        public bool IsDisposing { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (!_disposed)
            {
                IsDisposing = true;
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        public void DisposeClient(long sessionId)
        {
            try
            {
                // maybe it doesnt remove properly, but sometimes the session in methods below isn't null.
                if (_clientSockets.TryRemove(sessionId, out SocketSession session))
                {
                    session.Socket.Shutdown(SocketShutdown.Both);
                    session.Socket.Close();
                    session = null;
                    _clientSockets[sessionId] = null;
                    SessionDisconnected?.Invoke(this, new SessionDisconnectedEventArgs(session));
                }
            }
            catch (Exception ex)
            {
                SessionDisconnected?.Invoke(this, new SessionDisconnectedEventArgs(ex));
            }
        }

        public void SendMessage(long sessionId, MemoryStream stream)
        {
            try
            {
                if (!IsDisposing)
                {
                    byte[] buffer = stream.GetBuffer();
                    _clientSockets[sessionId]?.Socket.BeginSend(buffer, 0, (int)stream.Length, SocketFlags.None, new AsyncCallback(s => HandleAsyncSend(s, sessionId)), this);
                }
            }
            catch (SocketException se)
            {
                DisposeClient(sessionId);
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(se));
            }
        }

        private void HandleAsyncSend(IAsyncResult async, long sessionId)
        {
            SocketSession client = _clientSockets[sessionId];

            try
            {
                int bytesSent = client.Socket.EndSend(async);

                //Logs.Log(LogType.Network, "Sent {0} bytes to client {1}", bytesSent, client.Socket.RemoteEndPoint);
            }
            catch (SocketException e)
            {
                // Swallow the exception, but make sure a log is created and close the client.
                //Log.ErrorException("Unhandled SocketException", e);
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(e));
                //client.Close();
            }
            catch (Exception e)
            {
                //Log.ErrorException("Failed to send message to a client. Exception", e);
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(e));
                // don't swallow, this is a bug.
                throw;
            }
        }

        public void StartServer()
        {
            try
            {
                // Create the socket instance
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // bind to IPEndPoint
                _serverSocket.Bind(_ipEndPoint);

                // start listening
                _serverSocket.Listen(100);

                // start accepting clients
                _serverSocket.BeginAccept(AcceptClient, null);
            }
            catch (SocketException se)
            {
                SessionConnected?.Invoke(this, new SessionConnectedEventArgs(false, null, se));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (SocketSession session in _clientSockets.Values)
                {
                    session.Socket.Shutdown(SocketShutdown.Both);
                    session.Socket.Close();
                }
            }
        }

        private void AcceptClient(IAsyncResult async)
        {
            try
            {
                long sessionId = _clientSockets.Count > 0 ? _clientSockets.Last().Key + 1 : 1;
                SocketSession clientSocket = new SocketSession()
                {
                    Socket = _serverSocket.EndAccept(async),
                    SessionId = sessionId
                };
                _clientSockets[sessionId] = clientSocket;

                SessionConnected?.Invoke(this, new SessionConnectedEventArgs(true, clientSocket, null));

                byte[] buffer = new byte[SocketPacket.BUFFER_SIZE];
                clientSocket.Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
                _serverSocket.BeginAccept(AcceptClient, null);
            }
            catch (SocketException e)
            {
                // Dispose socket on error
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(e));
            }
            catch (ObjectDisposedException)
            {
                // Dispose socket on error
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(new Exception("Socket has been disposed")));
            }
        }

        public void ReceiveCallback(IAsyncResult async)
        {
            try
            {
                SocketSession session = _clientSockets[((SocketSession)async.AsyncState).SessionId];
                if (session != null)
                {
                    int len = session.Socket.EndReceive(async);
                    if (len > 0)
                    {
                        byte[] data = new byte[len];
                        Array.Copy(session.Buffer, data, len);

                        using (MemoryStream stream = new MemoryStream(session.Buffer, 0, len, false, true))
                        {
                            DataReceived?.Invoke(this, new DataReceivedEventArgs(new SocketPacket()
                            {
                                Stream = stream,
                                Buffer = session.Buffer,
                                Socket = session.Socket
                            }, session));
                        }

                        session.Socket.BeginReceive(session.Buffer, 0, session.Buffer.Length, SocketFlags.None, ReceiveCallback, session);
                    }
                    else
                    {
                        // received packet was of length 0, ignore.
                    }
                }
                else
                {
                    // session doesn't exist, ignore.
                }
            }
            catch (ObjectDisposedException)
            {
                // Socket was disposed, no further action is required.
            }
            catch (Exception e)
            {
                ExceptionCaught?.Invoke(this, new ExceptionEventArgs(e));
            }
        }

        #endregion
    }
}
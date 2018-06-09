using SagaBNS.Core;
using SagaBNS.Core.Threading;
using SagaBNS.DAO.Factory;
using SagaBNS.Enums;
using SagaBNS.Master.Library.Client;
using SagaBNS.Networking;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SagaBNS.GameObject.Networking
{
    public class NetworkManager
    {
        #region Members

        protected ThreadSafeSortedList<long, ClientSession> _sessions = new ThreadSafeSortedList<long, ClientSession>();

        private readonly INetworkServer _server;

        private readonly ServerType _serverType;

        #endregion

        #region Instantiation

        public NetworkManager(string ipAddress, int port, ServerType serverType)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress ip))
            {
                _server = new NetworkServer(new IPEndPoint(ip, port));
                _server.SessionConnected += OnServerClientConnected;
                _server.SessionDisconnected += OnServerClientDisconnected;
                _server.DataReceived += OnPacketReceived;
                _server.ExceptionCaught += OnExceptionCaught;
                _server.StartServer();
                _serverType = serverType;
                if (serverType == ServerType.Lobby)
                {
                    CommunicationServiceClient.Instance.AuthenticateLobbyEvent += OnLobbyAuthenticate;
                }
            }
            else
            {
                Logger.Error("IP Could not be parsed!");
            }
        }

        #endregion

        #region Methods

        public void OnLobbyAuthenticate(object sender, EventArgs e)
        {
            Tuple<Guid, long> loggedInCharacter = (Tuple<Guid, long>)sender;

            ClientSession session = _sessions.SingleOrDefault(s => s.SessionId == loggedInCharacter.Item2);
            if (session != null)
            {
                session.Account = DAOFactory.AccountDAO.LoadById(loggedInCharacter.Item1);
                string token = session.Account.AccountId.ToString().ToUpper() + ":" + Guid.NewGuid().ToString().ToUpper();
                string key = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                using (MemoryStream stream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.AutoFlush = true;
                    writer.Write("<Reply>");
                    writer.Write("<AuthToken>");
                    writer.Write(key);
                    writer.Write("</AuthToken>");
                    writer.Write("</Reply>");
                    writer.Flush();

                    session.SendOkReplyStream(stream);
                }
                Logger.Debug("AuthToken Sent");
            }
        }

        public void OnPacketReceived(object sender, DataReceivedEventArgs e) => _sessions[e.Session.SessionId].HandlePacket(e.Packet.Stream);

        public void AddSession(SocketSession socketSession)
        {
            Logger.Info("New client connected: " + socketSession.SessionId);

            ClientSession session = new ClientSession(socketSession, this);

            // why would we initialize it twice?
            //customClient.SetClientSession(session);
            _sessions[socketSession.SessionId] = session;
            if (session != null && _sessions[socketSession.SessionId] != session)
            {
                Logger.Warn("FORCE Disconnection: " + socketSession.SessionId);
                socketSession.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                _sessions.Remove(socketSession.SessionId);
            }
        }

        public void DeleteSession(long sessionId)
        {
            _sessions.Remove(sessionId);
            _server.DisposeClient(sessionId);
        }

        public void DistributePacket(long sessionId, MemoryStream stream) => _server.SendMessage(sessionId, stream);

        public void OnExceptionCaught(object sender, ExceptionEventArgs e) => Logger.Error(e.Exception);

        public void StopServer()
        {
            _server.Dispose();
            _server.SessionConnected -= OnServerClientConnected;
            _server.SessionDisconnected -= OnServerClientDisconnected;
            _server.DataReceived -= OnPacketReceived;
            _server.ExceptionCaught -= OnExceptionCaught;
        }

        protected ClientSession RemoveSession(SocketSession client)
        {
            if (_sessions.Take(client.SessionId, out ClientSession session))
            {
                return session;
            }
            else
            {
                return null;
            }
        }

        private void OnServerClientConnected(object sender, SessionConnectedEventArgs e) => AddSession(e.Session);

        private void OnServerClientDisconnected(object sender, SessionDisconnectedEventArgs e)
        {
            _server.DisposeClient(e.Session.SessionId);
            ClientSession client = RemoveSession(e.Session);
            if (client != null)
            {
                // add dispose

                // save if has selected character

                client.Destroy();

                Logger.Info("Session " + client.SessionId + " disconnected.");

                // session = null;
            }
        }

        #endregion
    }
}
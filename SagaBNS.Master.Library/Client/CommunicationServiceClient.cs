using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using SagaBNS.Core;
using SagaBNS.DAO.Factory;
using SagaBNS.Master.Library.Data;
using SagaBNS.Master.Library.Interface;
using System;
using System.Configuration;

namespace SagaBNS.Master.Library.Client
{
    public class CommunicationServiceClient : ICommunicationService
    {
        #region Members

        private static CommunicationServiceClient _instance;

        private readonly IScsServiceClient<ICommunicationService> _client;

        private readonly CommunicationClient _commClient;

        #endregion

        #region Instantiation

        public CommunicationServiceClient()
        {
            string ip = ConfigurationManager.AppSettings["MasterIP"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["MasterPort"]);
            _commClient = new CommunicationClient();
            _client = ScsServiceClientBuilder.CreateClient<ICommunicationService>(new ScsTcpEndPoint(ip, port), _commClient);
            System.Threading.Thread.Sleep(1000);
            while (_client.CommunicationState != CommunicationStates.Connected)
            {
                try
                {
                    _client.Connect();
                }
                catch (Exception)
                {
                    Logger.Error("Connection Retry", memberName: nameof(CommunicationServiceClient));
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler CharacterConnectedEvent;

        public event EventHandler CharacterDisconnectedEvent;

        public event EventHandler RestartEvent;

        public event EventHandler SessionKickedEvent;

        public event EventHandler ShutdownEvent;

        public event EventHandler AuthenticateLobbyEvent;

        #endregion

        #region Properties

        public static CommunicationServiceClient Instance => _instance ?? (_instance = new CommunicationServiceClient());

        public CommunicationStates CommunicationState => _client.CommunicationState;

        #endregion

        #region Methods

        public bool Authenticate(string authKey) => _client.ServiceProxy.Authenticate(authKey);

        public void Cleanup() => _client.ServiceProxy.Cleanup();

        public void CleanupOutdatedSession() => _client.ServiceProxy.CleanupOutdatedSession();

        public bool ConnectAccount(Guid worldId, Guid accountId, long sessionId) => _client.ServiceProxy.ConnectAccount(worldId, accountId, sessionId);

        public bool ConnectCharacter(Guid worldId, long characterId) => _client.ServiceProxy.ConnectCharacter(worldId, characterId);

        public void DisconnectAccount(Guid accountId) => _client.ServiceProxy.DisconnectAccount(accountId);

        public void DisconnectCharacter(Guid worldId, long characterId) => _client.ServiceProxy.DisconnectCharacter(worldId, characterId);

        public bool IsAccountConnected(Guid accountId) => _client.ServiceProxy.IsAccountConnected(accountId);

        public bool IsCharacterConnected(long characterId) => _client.ServiceProxy.IsCharacterConnected(characterId);

        public bool IsLoginPermitted(Guid accountId, long sessionId) => _client.ServiceProxy.IsLoginPermitted(accountId, sessionId);

        public void KickSession(Guid? accountId, long? sessionId) => _client.ServiceProxy.KickSession(accountId, sessionId);

        public void LobbyAuthenticate(Guid accountId, long sessionId) => _client.ServiceProxy.LobbyAuthenticate(accountId, sessionId);

        public void PulseAccount(Guid accountId) => _client.ServiceProxy.PulseAccount(accountId);

        public void RegisterAccountLogin(Guid accountId, long sessionId, string ipAddress) => _client.ServiceProxy.RegisterAccountLogin(accountId, sessionId, ipAddress);

        public Guid? RegisterWorldServer(SerializableWorldServer worldServer) => _client.ServiceProxy.RegisterWorldServer(worldServer);

        public Guid? RegisterLobbyServer(SerializableLobbyServer lobbyServer) => _client.ServiceProxy.RegisterLobbyServer(lobbyServer);

        public void Restart() => _client.ServiceProxy.Restart();

        public void Shutdown() => _client.ServiceProxy.Shutdown();

        public void UnregisterWorldServer(Guid worldId) => _client.ServiceProxy.UnregisterWorldServer(worldId);

        internal void OnCharacterConnected(long characterId)
        {
            string characterName = DAOFactory.CharacterDAO.LoadById(characterId)?.Name;
            CharacterConnectedEvent?.Invoke(new Tuple<long, string>(characterId, characterName), null);
        }

        internal void OnCharacterDisconnected(long characterId)
        {
            string characterName = DAOFactory.CharacterDAO.LoadById(characterId)?.Name;
            CharacterDisconnectedEvent?.Invoke(new Tuple<long, string>(characterId, characterName), null);
        }

        internal void OnKickSession(Guid? accountId, long? sessionId) => SessionKickedEvent?.Invoke(new Tuple<Guid?, long?>(accountId, sessionId), null);

        internal void OnRestart() => RestartEvent?.Invoke(null, null);

        internal void OnShutdown() => ShutdownEvent?.Invoke(null, null);

        internal void OnAuthenticateLobby(Guid accountId, long sessionId) => AuthenticateLobbyEvent?.Invoke(new Tuple<Guid, long>(accountId, sessionId), null);

        #endregion
    }
}
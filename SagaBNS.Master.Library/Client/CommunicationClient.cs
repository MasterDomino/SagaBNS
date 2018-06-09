using SagaBNS.Master.Library.Interface;
using System;
using System.Threading.Tasks;

namespace SagaBNS.Master.Library.Client
{
    internal class CommunicationClient : ICommunicationClient
    {
        #region Methods

        public void LobbyAuthenticate(Guid accountId, long sessionId) => Task.Run(() => CommunicationServiceClient.Instance.OnAuthenticateLobby(accountId, sessionId));

        public void CharacterConnected(long characterId) => Task.Run(() => CommunicationServiceClient.Instance.OnCharacterConnected(characterId));

        public void CharacterDisconnected(long characterId) => Task.Run(() => CommunicationServiceClient.Instance.OnCharacterDisconnected(characterId));

        public void KickSession(Guid? accountId, long? sessionId) => Task.Run(() => CommunicationServiceClient.Instance.OnKickSession(accountId, sessionId));

        public void Restart() => Task.Run(() => CommunicationServiceClient.Instance.OnRestart());

        public void Shutdown() => Task.Run(() => CommunicationServiceClient.Instance.OnShutdown());

        #endregion
    }
}
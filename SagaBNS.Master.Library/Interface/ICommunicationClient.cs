using System;

namespace SagaBNS.Master.Library.Interface
{
    public interface ICommunicationClient
    {
        #region Methods

        void CharacterConnected(long characterId);

        void CharacterDisconnected(long characterId);

        void KickSession(Guid? accountId, long? sessionId);

        void Restart();

        void Shutdown();

        void LobbyAuthenticate(Guid accountId, long sessionId);

        #endregion
    }
}
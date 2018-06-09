using Hik.Communication.ScsServices.Service;
using SagaBNS.Master.Library.Data;
using System;

namespace SagaBNS.Master.Library.Interface
{
    [ScsService(Version = "1.1.0.0")]
    public interface ICommunicationService
    {
        #region Methods

        /// <summary>
        /// Authenticates a Client to the Service
        /// </summary>
        /// <param name="authKey">The private Authentication key</param>
        /// <returns>true if successful, else false</returns>
        bool Authenticate(string authKey);

        /// <summary>
        /// Cleanup, used when rebooting the Server
        /// </summary>
        void Cleanup();

        void CleanupOutdatedSession();

        /// <summary>
        /// Registers the Login of the given Account and removes the permission to login
        /// </summary>
        /// <param name="worldId">World the Account connects to</param>
        /// <param name="accountId">Id of the connecting Account</param>
        /// <param name="sessionId">Id of the Session requesting the Login</param>
        /// <returns>true if the Login was successful, otherwise false</returns>
        bool ConnectAccount(Guid worldId, Guid accountId, long sessionId);

        /// <summary>
        /// Registers the Login of the given Character
        /// </summary>
        /// <param name="worldId">World the Character connects to</param>
        /// <param name="characterId">Id of the connecting Character</param>
        /// <returns>true if the Login was successful, otherwise false</returns>
        bool ConnectCharacter(Guid worldId, long characterId);

        /// <summary>
        /// Registers the Logout of the given Account
        /// </summary>
        /// <param name="accountId">Id of the disconnecting Account</param>
        void DisconnectAccount(Guid accountId);

        /// <summary>
        /// Registers the Logout of the given Character
        /// </summary>
        /// <param name="worldId">World the Character was connected to</param>
        /// <param name="characterId">Id of the disconnecting Character</param>
        void DisconnectCharacter(Guid worldId, long characterId);

        /// <summary>
        /// Checks if the Account is already connected
        /// </summary>
        /// <param name="accountId">Id of the Account</param>
        /// <returns></returns>
        bool IsAccountConnected(Guid accountId);

        /// <summary>
        /// Checks if the Character is connected
        /// </summary>
        /// <param name="worldGroup">Name of the WorldGroup to look on</param>
        /// <param name="characterId">Id of the Character</param>
        /// <returns></returns>
        bool IsCharacterConnected(long characterId);

        /// <summary>
        /// Checks if the Account is allowed to login
        /// </summary>
        /// <param name="accountId">Id of the Account</param>
        /// <param name="sessionId">Id of the Session that should be validated</param>
        /// <returns></returns>
        bool IsLoginPermitted(Guid accountId, long sessionId);

        /// <summary>
        /// Kicks a Session by their Id or Account
        /// </summary>
        /// <param name="accountId">Id of the Account</param>
        /// <param name="sessionId">Id of the Session</param>
        void KickSession(Guid? accountId, long? sessionId);

        /// <summary>
        /// Refreshes the Pulse Timer for the given account
        /// </summary>
        /// <param name="accountId">Id of the Account</param>
        void PulseAccount(Guid accountId);

        /// <summary>
        /// Registers the Account for Login
        /// </summary>
        /// <param name="accountId">Id of the Account to register</param>
        /// <param name="sessionId">Id of the Session to register</param>
        /// <param name="ipAddress">Session ip address</param>
        void RegisterAccountLogin(Guid accountId, long sessionId, string ipAddress);

        /// <summary>
        /// Registers a WorldServer
        /// </summary>
        /// <param name="worldServer">
        /// SerializableWorldServer object of the Server that should be registered
        /// </param>
        /// <returns>ChannelId on success, else null</returns>
        Guid? RegisterWorldServer(SerializableWorldServer worldServer);

        /// <summary>
        /// Registers LobbySerber
        /// </summary>
        /// <param name="lobbyServer">SerializableLobbyServer object of the Server that should be registered</param>
        /// <returns></returns>
        Guid? RegisterLobbyServer(SerializableLobbyServer lobbyServer);

        /// <summary>
        /// Shutdown given WorldGroup or WorldServer
        /// </summary>
        /// <param name="worldGroup">WorldGroup that should be shut down</param>
        void Restart();

        /// <summary>
        /// Shutdown given WorldGroup or WorldServer
        /// </summary>
        /// <param name="worldGroup">WorldGroup that should be shut down</param>
        void Shutdown();

        /// <summary>
        /// Authenticates the client further
        /// </summary>
        /// <param name="accountId">Account guid stored in database</param>
        void LobbyAuthenticate(Guid accountId, long sessionId);

        /// <summary>
        /// Unregisters a previously registered World Server
        /// </summary>
        /// <param name="worldId">Id of the World to be unregistered</param>
        void UnregisterWorldServer(Guid worldId);

        #endregion
    }
}
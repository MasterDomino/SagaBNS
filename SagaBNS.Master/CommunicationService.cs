using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using SagaBNS.Master.Library.Data;
using SagaBNS.Master.Library.Interface;
using System;
using System.Configuration;
using System.Linq;

namespace SagaBNS.Master
{
    internal class CommunicationService : ScsService, ICommunicationService
    {
        #region Methods

        public bool Authenticate(string authKey)
        {
            if (string.IsNullOrWhiteSpace(authKey))
            {
                return false;
            }

            if (authKey == ConfigurationManager.AppSettings["MasterAuthKey"])
            {
                MSManager.Instance.AuthentificatedClients.Add(CurrentClient.ClientId);
                return true;
            }

            return false;
        }

        public void Cleanup()
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }

            MSManager.Instance.ConnectedAccounts.Clear();
            MSManager.Instance.WorldServers.Clear();
        }

        public void CleanupOutdatedSession()
        {
            AccountConnection[] tmp = new AccountConnection[MSManager.Instance.ConnectedAccounts.Count + 20];
            lock (MSManager.Instance.ConnectedAccounts)
            {
                MSManager.Instance.ConnectedAccounts.CopyTo(tmp);
            }
            foreach (AccountConnection account in tmp.Where(a => a?.LastPing.AddMinutes(5) <= DateTime.Now))
            {
                KickSession(account.AccountId, null);
            }
        }

        public bool ConnectAccount(Guid worldId, Guid accountId, long sessionId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return false;
            }

            AccountConnection account = MSManager.Instance.ConnectedAccounts.Find(a => a.AccountId.Equals(accountId) && a.SessionId.Equals(sessionId));
            if (account != null)
            {
                account.ConnectedWorld = MSManager.Instance.WorldServers.Find(w => w.Id.Equals(worldId));
            }
            return account.ConnectedWorld != null;
        }

        public bool ConnectCharacter(Guid worldId, long characterId)
        {
            // TODO: Maybe later idk for now
            //if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            //{
            //    return false;
            //}
            //
            ////Multiple WorldGroups not yet supported by DAOFactory
            //Guid accountId = DAOFactory.CharacterDAO.LoadById(characterId)?.AccountId ?? Guid.NewGuid();
            //
            //AccountConnection account = MSManager.Instance.ConnectedAccounts.Find(a => a.AccountId.Equals(accountId) && a.ConnectedWorld.Id.Equals(worldId));
            //if (account != null)
            //{
            //    account.CharacterId = characterId;
            //    foreach (WorldServer world in MSManager.Instance.WorldServers)
            //    {
            //        world.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().CharacterConnected(characterId);
            //    }
            //    return true;
            //}
            return false;
        }

        public void DisconnectAccount(Guid accountId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            if (!MSManager.Instance.ConnectedAccounts.Any(s => s.AccountId.Equals(accountId)))
            {
                MSManager.Instance.ConnectedAccounts.RemoveAll(c => c.AccountId.Equals(accountId));
            }
        }

        public void DisconnectCharacter(Guid worldId, long characterId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }

            foreach (AccountConnection account in MSManager.Instance.ConnectedAccounts.Where(c => c.CharacterId.Equals(characterId) && c.ConnectedWorld.Id.Equals(worldId)))
            {
                foreach (WorldServer world in MSManager.Instance.WorldServers)
                {
                    world.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().CharacterDisconnected(characterId);
                }
            }
        }

        public bool IsAccountConnected(Guid accountId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return false;
            }

            return MSManager.Instance.ConnectedAccounts.Any(c => c.AccountId == accountId && c.ConnectedWorld != null);
        }

        public bool IsCharacterConnected(long characterId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return false;
            }

            return MSManager.Instance.ConnectedAccounts.Any(c => c.ConnectedWorld != null && c.CharacterId == characterId);
        }

        public bool IsLoginPermitted(Guid accountId, long sessionId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return false;
            }

            return MSManager.Instance.ConnectedAccounts.Any(s => s.AccountId.Equals(accountId) && s.SessionId.Equals(sessionId) && s.ConnectedWorld == null);
        }

        public void KickSession(Guid? accountId, long? sessionId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }

            foreach (WorldServer world in MSManager.Instance.WorldServers)
            {
                world.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().KickSession(accountId, sessionId);
            }
            if (accountId.HasValue)
            {
                MSManager.Instance.ConnectedAccounts.RemoveAll(s => s.AccountId.Equals(accountId.Value));
            }
            else if (sessionId.HasValue)
            {
                MSManager.Instance.ConnectedAccounts.RemoveAll(s => s.SessionId.Equals(sessionId.Value));
            }
        }

        public void PulseAccount(Guid accountId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            AccountConnection account = MSManager.Instance.ConnectedAccounts.Find(a => a.AccountId.Equals(accountId));
            if (account != null)
            {
                account.LastPing = DateTime.Now;
            }
        }

        public void RegisterAccountLogin(Guid accountId, long sessionId, string ipAddress)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            MSManager.Instance.ConnectedAccounts.RemoveAll(a => a.AccountId.Equals(accountId));
            MSManager.Instance.ConnectedAccounts.Add(new AccountConnection(accountId, sessionId, ipAddress));
        }

        public void LobbyAuthenticate(Guid accountId, long sessionId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            if (MSManager.Instance.ConnectedAccounts.Any(s => s.AccountId.Equals(accountId)))
            {
                MSManager.Instance.LobbyServer.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().LobbyAuthenticate(accountId, sessionId);
            }
        }

        public Guid? RegisterLobbyServer(SerializableLobbyServer lobbyServer)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return null;
            }
            LobbyServer ls = new LobbyServer(lobbyServer.Id, new ScsTcpEndPoint(lobbyServer.EndPointIP, lobbyServer.EndPointPort))
            {
                CommunicationServiceClient = CurrentClient,
            };
            MSManager.Instance.LobbyServer = ls;
            return ls.Id;
        }

        public Guid? RegisterWorldServer(SerializableWorldServer worldServer)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return null;
            }
            WorldServer ws = new WorldServer(worldServer.Id, new ScsTcpEndPoint(worldServer.EndPointIP, worldServer.EndPointPort), worldServer.AccountLimit)
            {
                CommunicationServiceClient = CurrentClient,
            };
            MSManager.Instance.WorldServers.Add(ws);
            return ws.Id;
        }

        public void Restart()
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            foreach (WorldServer world in MSManager.Instance.WorldServers)
            {
                world.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().Restart();
            }
        }

        public void Shutdown()
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }
            foreach (WorldServer world in MSManager.Instance.WorldServers)
            {
                world.CommunicationServiceClient.GetClientProxy<ICommunicationClient>().Shutdown();
            }
        }

        public void UnregisterWorldServer(Guid worldId)
        {
            if (!MSManager.Instance.AuthentificatedClients.Any(s => s.Equals(CurrentClient.ClientId)))
            {
                return;
            }

            MSManager.Instance.ConnectedAccounts.RemoveAll(a => a?.ConnectedWorld?.Id.Equals(worldId) == true);
            MSManager.Instance.WorldServers = null;
        }

        #endregion
    }
}
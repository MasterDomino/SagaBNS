using Hik.Communication.ScsServices.Service;
using SagaBNS.Core.Threading;
using SagaBNS.Master.Library.Data;
using System.Collections.Generic;

namespace SagaBNS.Master
{
    internal class MSManager
    {
        #region Members

        private static MSManager _instance;

        #endregion

        #region Instantiation

        public MSManager()
        {
            WorldServers = new List<WorldServer>();
            LoginServers = new List<IScsServiceClient>();
            ConnectedAccounts = new ThreadSafeGenericList<AccountConnection>();
            AuthentificatedClients = new List<long>();
            ConfigurationObject = new ConfigurationObject();
        }

        #endregion

        #region Properties

        public static MSManager Instance => _instance ?? (_instance = new MSManager());

        public List<long> AuthentificatedClients { get; set; }

        public ConfigurationObject ConfigurationObject { get; set; }

        public ThreadSafeGenericList<AccountConnection> ConnectedAccounts { get; set; }

        public List<IScsServiceClient> LoginServers { get; set; }

        public List<WorldServer> WorldServers { get; set; }

        // for now lets make it easy
        public LobbyServer LobbyServer { get; set; }

        #endregion
    }
}
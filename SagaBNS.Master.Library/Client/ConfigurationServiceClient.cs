using SagaBNS.Core;
using SagaBNS.Master.Library.Data;
using SagaBNS.Master.Library.Interface;
using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using System;
using System.Configuration;

namespace SagaBNS.Master.Library.Client
{
    public class ConfigurationServiceClient : IConfigurationService
    {
        #region Members

        private static ConfigurationServiceClient _instance;

        private readonly IScsServiceClient<IConfigurationService> _client;

        private readonly ConfigurationClient _confClient;

        #endregion

        #region Instantiation

        public ConfigurationServiceClient()
        {
            string ip = ConfigurationManager.AppSettings["MasterIP"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["MasterPort"]);
            _confClient = new ConfigurationClient();
            _client = ScsServiceClientBuilder.CreateClient<IConfigurationService>(new ScsTcpEndPoint(ip, port), _confClient);
            System.Threading.Thread.Sleep(1000);
            while (_client.CommunicationState != CommunicationStates.Connected)
            {
                try
                {
                    _client.Connect();
                }
                catch (Exception)
                {
                    Logger.Error("Retry Connection", memberName: nameof(CommunicationServiceClient));
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler ConfigurationUpdate;

        #endregion

        #region Properties

        public static ConfigurationServiceClient Instance => _instance ?? (_instance = new ConfigurationServiceClient());

        public CommunicationStates CommunicationState => _client.CommunicationState;

        #endregion

        #region Methods

        public bool Authenticate(string authKey, Guid serverId) => _client.ServiceProxy.Authenticate(authKey, serverId);

        public void UpdateConfigurationObject(ConfigurationObject configurationObject) => _client.ServiceProxy.UpdateConfigurationObject(configurationObject);

        public ConfigurationObject GetConfigurationObject() => _client.ServiceProxy.GetConfigurationObject();

        internal void OnConfigurationUpdated(ConfigurationObject configurationObject) => ConfigurationUpdate?.Invoke(configurationObject, null);

        #endregion
    }
}
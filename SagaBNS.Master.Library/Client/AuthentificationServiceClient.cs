using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using SagaBNS.Master.Library.Interface;
using SagaBNS.Core;
using SagaBNS.DTO;
using System;
using System.Configuration;

namespace SagaBNS.Master.Library.Client
{
    public class AuthentificationServiceClient : IAuthentificationService
    {
        #region Members

        private static AuthentificationServiceClient _instance;

        private readonly IScsServiceClient<IAuthentificationService> _client;

        #endregion

        #region Instantiation

        public AuthentificationServiceClient()
        {
            string ip = ConfigurationManager.AppSettings["MasterIP"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["MasterPort"]);
            _client = ScsServiceClientBuilder.CreateClient<IAuthentificationService>(new ScsTcpEndPoint(ip, port));
            System.Threading.Thread.Sleep(1000);
            while (_client.CommunicationState != CommunicationStates.Connected)
            {
                try
                {
                    _client.Connect();
                }
                catch (Exception)
                {
                    Logger.Error("Retry Connection", memberName: nameof(AuthentificationServiceClient));
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        #endregion

        #region Properties

        public static AuthentificationServiceClient Instance => _instance ?? (_instance = new AuthentificationServiceClient());

        public CommunicationStates CommunicationState => _client.CommunicationState;

        #endregion

        #region Methods

        public bool Authenticate(string authKey) => _client.ServiceProxy.Authenticate(authKey);

        public AccountDTO RequestAccount(string name) => _client.ServiceProxy.RequestAccount(name);

        #endregion
    }
}
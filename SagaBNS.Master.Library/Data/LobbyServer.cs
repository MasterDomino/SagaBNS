using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using System;

namespace SagaBNS.Master.Library.Data
{
    public class LobbyServer
    {
        #region Instantiation

        public LobbyServer(Guid id, ScsTcpEndPoint endpoint)
        {
            Id = id;
            Endpoint = endpoint;
        }

        #endregion

        #region Properties

        public ScsTcpEndPoint Endpoint { get; set; }

        public Guid Id { get; set; }

        public SerializableLobbyServer Serializable { get; }

        public IScsServiceClient CommunicationServiceClient { get; set; }

        public IScsServiceClient ConfigurationServiceClient { get; set; }

        #endregion
    }
}
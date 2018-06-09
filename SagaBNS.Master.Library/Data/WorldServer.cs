using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using System;

namespace SagaBNS.Master.Library.Data
{
    public class WorldServer
    {
        #region Instantiation

        public WorldServer(Guid id, ScsTcpEndPoint endpoint, int accountLimit)
        {
            Id = id;
            Endpoint = endpoint;
            AccountLimit = accountLimit;
        }

        #endregion

        #region Properties

        public int AccountLimit { get; set; }

        public ScsTcpEndPoint Endpoint { get; set; }

        public Guid Id { get; set; }

        public SerializableWorldServer Serializable { get; }

        public IScsServiceClient CommunicationServiceClient { get; set; }

        public IScsServiceClient ConfigurationServiceClient { get; set; }

        #endregion
    }
}
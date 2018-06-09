using System;

namespace SagaBNS.Master.Library.Data
{
    [Serializable]
    public class SerializableLobbyServer
    {
        #region Instantiation

        public SerializableLobbyServer(Guid id, string epIP, int epPort)
        {
            Id = id;
            EndPointIP = epIP;
            EndPointPort = epPort;
        }

        #endregion

        #region Properties

        public string EndPointIP { get; set; }

        public int EndPointPort { get; set; }

        public Guid Id { get; set; }

        #endregion
    }
}
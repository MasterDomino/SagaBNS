using System;

namespace SagaBNS.Master.Library.Data
{
    [Serializable]
    public class SerializableWorldServer
    {
        #region Instantiation

        public SerializableWorldServer(Guid id, string epIP, int epPort, int accountLimit)
        {
            Id = id;
            EndPointIP = epIP;
            EndPointPort = epPort;
            AccountLimit = accountLimit;
        }

        #endregion

        #region Properties

        public int AccountLimit { get; set; }

        public string EndPointIP { get; set; }

        public int EndPointPort { get; set; }

        public Guid Id { get; set; }

        #endregion
    }
}
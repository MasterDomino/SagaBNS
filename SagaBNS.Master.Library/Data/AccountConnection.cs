using System;

namespace SagaBNS.Master.Library.Data
{
    public class AccountConnection
    {
        public AccountConnection(Guid accountId, long sessionId, string ipAddress)
        {
            AccountId = accountId;
            SessionId = sessionId;
            IpAddress = ipAddress;
            LastPing = DateTime.Now;
        }

        public Guid AccountId { get; }

        public long CharacterId { get; set; }

        public WorldServer ConnectedWorld { get; set; }

        public string IpAddress { get; }

        public DateTime LastPing { get; set; }

        public WorldServer OriginWorld { get; set; }

        public long SessionId { get; }
    }
}
using SagaBNS.Core.Serializing;
using SagaBNS.GameObject.Networking;
using SagaBNS.Master.Library.Client;
using System.IO;

namespace SagaBNS.Handler.STSPackets
{
    [PacketHeader("Sts", "Ping")]
    public class STSPingPacket
    {
        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            STSPingPacket pingPacket = new STSPingPacket();
            pingPacket?.ExecuteHandler(session as ClientSession);
        }

        public static void Register() => PacketFacility.AddHandler(typeof(STSPingPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            if (session.Account != null)
            {
                CommunicationServiceClient.Instance.PulseAccount(session.Account.AccountId);
                session.SendOkReply();
            }
        }

        #endregion
    }
}
using SagaBNS.Core.Serializing;
using SagaBNS.GameObject.Networking;
using System.IO;

namespace SagaBNS.Handler.AuthPackets
{
    [PacketHeader("Auth", "LogoutMyClient")]
    public class LogoutMyClientPacket
    {
        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            LogoutMyClientPacket request = new LogoutMyClientPacket();
            request?.ExecuteHandler(session as ClientSession);
        }

        public static void Register() => PacketFacility.AddHandler(typeof(LogoutMyClientPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session) => session.Destroy();

        #endregion
    }
}
using SagaBNS.Core;
using SagaBNS.Core.Serializing;
using SagaBNS.Enums;
using SagaBNS.GameObject.Networking;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SagaBNS.Handler.AuthPackets
{
    [Serializable]
    [XmlRoot("Request")]
    [PacketHeader("Auth", "GetMyUserInfo")]
    public class GetMyUserInfoPacket
    {
        #region Properties

        [XmlElement]
        public string Data { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GetMyUserInfoPacket));
            GetMyUserInfoPacket request = (GetMyUserInfoPacket)serializer.Deserialize(reader);
            if (request != null)
            {
                request?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(GetMyUserInfoPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            if (session.SessionState != SessionState.LoginFinish)
            {
                Logger.Error($"Client {session} Is still in login process");
                return;
            }

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Reply>");
                writer.Write($"<UserId>{session.Account.AccountId.ToString().ToUpper()}</UserId>");
                writer.Write("<UserCenter>1</UserCenter>");
                writer.Write($"<UserName>{session.Account.Name}</UserName>");
                writer.Write($"<LoginName>{session.Account.Name}@ncsoft.jp</LoginName>");
                writer.Write("<UserStatus>1</UserStatus>");
                writer.Write("</Reply>");
                writer.Flush();

                session.SendOkReplyStream(stream);
            }
        }

        #endregion
    }
}
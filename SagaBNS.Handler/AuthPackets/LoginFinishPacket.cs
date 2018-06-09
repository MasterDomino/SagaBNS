using SagaBNS.Core;
using SagaBNS.Core.Serializing;
using SagaBNS.Enums;
using SagaBNS.GameObject.Networking;
using SagaBNS.Master.Library.Client;
using System;
using System.IO;

namespace SagaBNS.Handler.AuthPackets
{
    [Serializable]
    [PacketHeader("Auth", "LoginFinish")]
    public class LoginFinishPacket
    {
        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            LoginFinishPacket request = new LoginFinishPacket();
            request?.ExecuteHandler(session as ClientSession);
        }

        public static void Register() => PacketFacility.AddHandler(typeof(LoginFinishPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            if (session.SessionState != SessionState.ReceivedKeyData)
            {
                Logger.Error($"Client {session} sent AuthLoginFinish but is in invalid state.");
                return;
            }

            CommunicationServiceClient.Instance.RegisterAccountLogin(session.Account.AccountId, session.SessionId, session.IPAddress);

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Message>");
                writer.Write($"<UserId>{session.Account.AccountId.ToString().ToUpper()}</UserId>");
                writer.Write("<UserCenter>1</UserCenter>");
                writer.Write($"<UserName>{session.Account.Name}</UserName>");
                writer.Write("<Status>online</Status>");
                writer.Write("<Aliases type=\"array\">");
                writer.Write($"<Alias>{session.Account.Name}</Alias>");
                writer.Write($"<Alias>bns:{session.Account.Name}</Alias>");
                writer.Write("</Aliases>");
                writer.Write("</Message>");
                writer.Flush();

                session.SendFormattedReply("POST /Presence/UserInfo STS/1.0", stream);
            }

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Reply>");
                writer.Write($"<UserId>{session.Account.AccountId.ToString().ToUpper()}</UserId>");
                writer.Write("<UserCenter>1</UserCenter>");
                writer.Write("<Roles type=\"array\"/>");
                writer.Write("<LocationId>32B0E246-1D5F-4AC1-AC30-E462AAF4C870</LocationId>"); // AF228A31-0230-4212-9E55-6E405728B795
                writer.Write("<AccessMask>1073741823</AccessMask>"); // this both values have to be checked soo we know whats going on
                writer.Write($"<UserName>{session.Account.Name}</UserName>");
                writer.Write("</Reply>");
                writer.Flush();

                session.SendOkReplyStream(stream);
            }

            session.SessionState = SessionState.LoginFinish;
        }

        #endregion
    }
}
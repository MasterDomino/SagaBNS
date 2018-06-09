using SagaBNS.Core;
using SagaBNS.Core.Serializing;
using SagaBNS.DAO.Factory;
using SagaBNS.DTO;
using SagaBNS.GameObject.Networking;
using SagaBNS.Master.Library.Client;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SagaBNS.Handler.AuthPackets
{
    [Serializable]
    [XmlRoot("Request")]
    [PacketHeader("Auth", "RequestGameToken")]
    public class RequestGameTokenPacket
    {
        #region Properties

        [XmlElement]
        public Guid AccountAlias { get; set; }

        [XmlElement]
        public string GameCode { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RequestGameTokenPacket));
            RequestGameTokenPacket request = (RequestGameTokenPacket)serializer.Deserialize(reader);
            if (request != null)
            {
                request?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(RequestGameTokenPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            session.Account.LoginToken = Guid.NewGuid();
            session.Account.TokenExpireTime = DateTime.Now.AddMinutes(10);

            // save it properly
            AccountDTO acc = session.Account;
            DAOFactory.AccountDAO.InsertOrUpdate(ref acc);

            string token = session.Account.LoginToken.ToString().ToUpper();

            //Logger.Debug("GameToken: " + token);
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Reply>");
                writer.Write("<Token>");
                writer.Write(token);
                writer.Write("</Token>");
                writer.Write("</Reply>");
                writer.Flush();

                session.SendOkReplyStream(stream);
            }

            session.Ready = true;
            CommunicationServiceClient.Instance.LobbyAuthenticate(session.Account.AccountId, session.SessionId);

            Logger.Debug("GameAuthKey Sent: " + token);
        }

        #endregion
    }
}
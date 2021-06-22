using SagaBNS.Core;
using SagaBNS.Core.Serializing;
using SagaBNS.GameObject.Networking;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SagaBNS.Handler.AuthPackets
{
    [Serializable]
    [XmlRoot("Request")]
    [PacketHeader("Auth", "RequestToken")]
    public class RequestTokenPacket
    {
        #region Properties

        [XmlElement]
        public Guid AppId { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RequestTokenPacket));
            RequestTokenPacket request = (RequestTokenPacket)serializer.Deserialize(reader);
            if (request != null)
            {
                request?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(RequestTokenPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            string token = session.Account.AccountId.ToString().ToUpper() + ":" + Guid.NewGuid().ToString().ToUpper();
            string key = Convert.ToBase64String(Encoding.Default.GetBytes(token));
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Reply>");
                writer.Write("<AuthnToken>");
                writer.Write(key);
                writer.Write("</AuthnToken>");
                writer.Write("</Reply>");
                writer.Flush();

                session.SendOkReplyStream(stream);
            }
            Logger.Debug("AuthKey sent: " + key);
        }

        #endregion
    }
}
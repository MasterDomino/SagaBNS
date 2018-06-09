using SagaBNS.Core.Serializing;
using SagaBNS.GameObject.Networking;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SagaBNS.Handler.GameAccountPackets
{
    [Serializable]
    [XmlRoot("Request")]
    [PacketHeader("GameAccount", "ListMyAccounts")]
    public class ListMyAccountsPacket
    {
        #region Properties

        [XmlElement]
        public string GameCode { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ListMyAccountsPacket));
            ListMyAccountsPacket request = (ListMyAccountsPacket)serializer.Deserialize(reader);
            if (request != null)
            {
                request?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(ListMyAccountsPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            // Content = $"<Reply type=\"array\">
            // \n<GameAccount>\n<Alias>{Account.AccountId.ToGUID().ToString().ToUpper()}</Alias>
            // \n<Created>2013-06-08T17:45:58.689+09:00</Created> \n</GameAccount> \n</Reply>\n" {client.Account.LastLoginTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz")}
            // TODO: Add account created datetime to database
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;
                writer.Write("<Reply type=\"array\">");
                writer.Write("<GameAccount>");
                writer.Write($"<Alias>{session.Account.AccountId.ToString().ToUpper()}</Alias>");
                writer.Write("<Created>2018-06-08T17:45:58.689+09:00</Created>");
                writer.Write("</GameAccount>");
                writer.Write("</Reply>");
                writer.Flush();

                session.SendOkReplyStream(stream);
            }
        }

        #endregion
    }
}
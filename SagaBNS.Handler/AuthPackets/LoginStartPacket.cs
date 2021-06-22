using SagaBNS.Core;
using SagaBNS.Core.Cryptography;
using SagaBNS.Core.Serializing;
using SagaBNS.DAO.Factory;
using SagaBNS.Enums;
using SagaBNS.GameObject.Networking;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SagaBNS.Handler.AuthPackets
{
    [Serializable]
    [XmlRoot("Request")]
    [PacketHeader("Auth", "LoginStart")]
    public class LoginStartPacket
    {
        #region Properties

        [XmlElement]
        public string LoginName { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LoginStartPacket));
                LoginStartPacket request = (LoginStartPacket)serializer.Deserialize(reader);
                if (request != null)
                {
                    request?.ExecuteHandler(session as ClientSession);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(LoginStartPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            Logger.Info($"LoginStart From: {LoginName}");
            string login = LoginName.Split('@')[0];

            session.Account = DAOFactory.AccountDAO.LoadByName(login);

            if (session.Account == null)
            {
                using (MemoryStream stream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    session.SendErrorReply("ErrAccountNotFound", "<Error code=\"3002\" server=\"1008\" module=\"1\" line=\"458\"/>");
                }

                //Logger.Info("AccountNotFound");
                // TODO: Session CLOSE!
                //session.Close();
                return;
            }

            using (MemoryStream keyDataStream = new MemoryStream(4 + 8 + 4 + 128))
            using (BinaryWriter keyDataWriter = new BinaryWriter(keyDataStream))
            {
                try
                {
                    session.SRP = new SRP6();
                    session.SRP.ReceiveLoginStartInfo(LoginName, session.Account.Password, keyDataWriter);
                }
                catch (SRP6InvalidStateException ex)
                {
                    // This is an issue we don't want to recover from. Log it and close client connection.
                    Logger.Error("Invalid operation occured.", ex);
                    // TODO: Session CLOSE!
                    //session.Close();
                    return;
                }

                //Byte[] data = keyDataStream.GetBuffer();
                //Logs.Log("Sending first key (len: {0:X2}):", data.Length);
                //Logs.LogBuffer(data);

                //AuthLoginStartReply reply = new AuthLoginStartReply
                //{
                //    KeyData = Convert.ToBase64String(keyDataStream.GetBuffer())
                //};
                //client.SendOkReply(reply);
                //"STS/1.0 200 OK\r\nl:42\r\ns:0R\r\n\r\n﻿<Reply><KeyData>QVNE</KeyData></Reply>\n"
                using (MemoryStream stream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.AutoFlush = true;
                    writer.Write("<Reply>");
                    writer.Write("<KeyData>");
                    writer.Write(Convert.ToBase64String(keyDataStream.GetBuffer()));
                    writer.Write("</KeyData>");
                    writer.Write("</Reply>");
                    writer.Flush();

                    session.SendOkReplyStream(stream);
                }
            }

            session.SessionState = SessionState.LoginStart;
        }

        #endregion
    }
}
using SagaBNS.Core;
using SagaBNS.Core.Cryptography;
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
    [PacketHeader("Auth", "KeyData")]
    public class KeyDataPacket
    {
        #region Properties

        [XmlElement]
        public string KeyData { get; set; }

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(KeyDataPacket));
            KeyDataPacket request = (KeyDataPacket)serializer.Deserialize(reader);
            if (request != null)
            {
                request?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(KeyDataPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            if (session.SessionState != SessionState.LoginStart)
            {
                Logger.Error($"Client {session} sent AuthKeyData but is in invalid state.");
                return;
            }

            //AuthKeyDataRequest request = new AuthKeyDataRequest();
            //using (XmlReader xmlReader = XmlReader.Create(reader))
            //{
            //    request.ReadFrom(xmlReader);
            //}

            using (MemoryStream clientKeyDataStream = new MemoryStream(Convert.FromBase64String(KeyData)))
            using (BinaryReader clientKeyDataReader = new BinaryReader(clientKeyDataStream))
            using (MemoryStream serverKeyDataStream = new MemoryStream(4 + 32))
            using (BinaryWriter serverKeyDataWriter = new BinaryWriter(serverKeyDataStream))
            {
                byte[] key;

                try
                {
                    session.Srp.ReceiveClientProof(clientKeyDataReader, serverKeyDataWriter, out key);
                }
                catch (SRP6InvalidStateException ex)
                {
                    // This is an issue we don't want to recover from. Log it and close client connection.
                    Logger.Error(ex.Message, ex);

                    // TODO: Kick session
                    //session.Close();
                    return;
                }
                catch (SRP6SafeguardException ex)
                {
                    // This could just mean that the password was wrong, don't even log it.
                    Logger.Warn(ex.Message);
                    session.SendErrorReply("ErrBadParam", "<Error code=\"10\"/>");
                    session.SessionState = SessionState.Connected;
                    return;
                }
                finally
                {
                    // Let GC take the mem back.
                    session.Srp = null;
                }

                //AuthKeyDataReply reply = new AuthKeyDataReply
                //{
                //    KeyData = Convert.ToBase64String(serverKeyDataStream.GetBuffer())
                //};
                byte[] data = serverKeyDataStream.GetBuffer();
                Logger.Debug($"Sending verif key (len: {data.Length:X2})");

                //Logs.LogBuffer(data);
                //"STS/1.0 200 OK\r\nl:42\r\ns:0R\r\n\r\n﻿<Reply><KeyData>QVNE</KeyData></Reply>\n"
                using (MemoryStream stream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.AutoFlush = true;
                    writer.Write("<Reply>");
                    writer.Write("<KeyData>");
                    writer.Write(Convert.ToBase64String(data));
                    writer.Write("</KeyData>");
                    writer.Write("</Reply>");
                    writer.Flush();

                    session.SendOkReplyStream(stream);
                }

                // new RC4: Filter
                //client.Filter = new RC4Filter(key);
                session.CryptIn = new RC4(key);
                session.CryptOut = new RC4(key);

                session.SessionState = SessionState.ReceivedKeyData;
            }
        }

        #endregion
    }
}
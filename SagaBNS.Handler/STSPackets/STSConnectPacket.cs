using SagaBNS.Core;
using SagaBNS.Core.Serializing;
using SagaBNS.Enums;
using SagaBNS.GameObject.Networking;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SagaBNS.Handler.STSPackets
{
    [Serializable]
    [XmlRoot("Connect")]
    [PacketHeader("Sts", "Connect")]
    public class STSConnectPacket
    {
        #region Members

        [XmlElement]
        public string Address;

        [XmlElement]
        public uint AppIndex;

        [XmlElement]
        public uint Build;

        [XmlElement(IsNullable = true)]
        public uint? ConnAppIndex;

        [XmlElement(IsNullable = true)]
        public uint? ConnDeployment;

        [XmlElement(IsNullable = true)]
        public uint? ConnEpoch;

        [XmlElement(IsNullable = true)]
        public uint? ConnProductType;

        [XmlElement]
        public uint ConnType;

        [XmlElement(IsNullable = true)]
        public uint? Deployment;

        [XmlElement]
        public uint Epoch;

        [XmlElement(IsNullable = true)]
        public uint? NotifyFlags;

        [XmlElement]
        public uint Process;

        [XmlElement]
        public uint ProductType;

        [XmlElement]
        public uint Program;

        [XmlElement(IsNullable = true)]
        public uint? VersionFlags;

        #endregion

        #region Methods

        public static void HandlePacket(object session, StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(STSConnectPacket));
            STSConnectPacket stsConnect = (STSConnectPacket)serializer.Deserialize(reader);
            if (stsConnect != null)
            {
                stsConnect?.ExecuteHandler(session as ClientSession);
            }
        }

        public static void Register() => PacketFacility.AddHandler(typeof(STSConnectPacket), HandlePacket);

        private void ExecuteHandler(ClientSession session)
        {
            if (session.SessionState != SessionState.None)
            {
                Logger.Error($"Client {session} sent StsConnect but is already in another state.");
                // TODO: Session CLOSE!
                //session.Close();
                return;
            }

            //Logger.Debug($"StsConnect: {Build} {Process} {Address}");
            session.SessionState = SessionState.Connected;
        }

        #endregion
    }
}
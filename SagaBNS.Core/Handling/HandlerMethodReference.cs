using SagaBNS.Core.Serializing;
using System;

namespace SagaBNS.Core.Handling
{
    public class HandlerMethodReference
    {
        #region Instantiation

        public HandlerMethodReference(Type packetBaseParameterType)
        {
            PacketDefinitionParameterType = packetBaseParameterType;
            PacketHeaderAttribute headerAttribute = (PacketHeaderAttribute)Array.Find(PacketDefinitionParameterType.GetCustomAttributes(true), ca => ca.GetType().Equals(typeof(PacketHeaderAttribute)));
            Command = headerAttribute?.Command;
            Protocol = headerAttribute?.Protocol;

            //Authority = headerAttribute?.Authority ?? AuthorityType.User;
        }

        #endregion

        #region Properties

        /// <summary>
        /// String identification of the Packet
        /// </summary>
        public string Command { get; }

        public string Protocol { get; set; }

        public Type PacketDefinitionParameterType { get; }

        #endregion
    }
}
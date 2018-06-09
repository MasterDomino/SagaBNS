using System;
using System.Linq;

namespace SagaBNS.Core.Serializing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PacketHeaderAttribute : Attribute
    {
        #region Instantiation

        public PacketHeaderAttribute(string protocol, string command)
        {
            Protocol = protocol;
            Command = command;
        }

        #endregion

        #region Properties

        /// <summary>
        /// String identification of the Packet
        /// </summary>
        public string Command { get; set; }

        public string Protocol { get; set; }

        #endregion
    }
}

namespace SagaBNS.Enums
{
    public enum STSClientStatus
    {
        /// <summary>
        /// The TCP connection is opened but nothing has happened yet.
        /// </summary>
        None,

        /// <summary>
        /// The client is connected via the StsConnect command.
        /// </summary>
        Connected,

        LoginStart,
        ReceivedKeyData, // packets are encrypted after first crossing this point.
        LoginFinish,
    }
}
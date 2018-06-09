using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class TeleportLocationDTO
    {
        #region Properties

        public long CharacterId { get; set; }

        public int TeleportId { get; set; }

        #endregion
    }
}
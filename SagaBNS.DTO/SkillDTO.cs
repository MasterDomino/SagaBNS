using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class SkillDTO
    {
        #region Properties

        public long CharacterId { get; set; }

        public byte Level { get; set; }

        public long SkillId { get; set; }

        #endregion
    }
}
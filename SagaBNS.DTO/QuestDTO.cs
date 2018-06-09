using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class QuestDTO
    {
        #region Properties

        public long CharacterId { get; set; }

        public int Count1 { get; set; }

        public int Count2 { get; set; }

        public int Count3 { get; set; }

        public int Count4 { get; set; }

        public int Count5 { get; set; }

        public byte NextStep { get; set; }

        public int QuestId { get; set; }

        public byte Step { get; set; }

        public byte StepStatus { get; set; }

        #endregion
    }
}
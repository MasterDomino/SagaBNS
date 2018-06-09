using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class CompletedQuestDTO
    {
        #region Properties

        public long CharacterId { get; set; }

        public int QuestId { get; set; }

        #endregion
    }
}
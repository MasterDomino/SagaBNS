using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class Quest
    {
        #region Properties

        public virtual Character Character { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Identification { get; set; }

        public long CharacterId { get; set; }

        public int Count1 { get; set; }

        public int Count2 { get; set; }

        public int Count3 { get; set; }

        public int Count4 { get; set; }

        public int Count5 { get; set; }

        public byte NextStep { get; set; }

        // should propably be guid unless its the quest identification according to the game
        public int QuestId { get; set; }

        public byte Step { get; set; }

        public byte StepStatus { get; set; }

        #endregion
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class CompletedQuest
    {
        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Identification { get; set; }

        public long CharacterId { get; set; }

        public virtual Character Character { get; set; }

        public int QuestId { get; set; }

        #endregion
    }
}
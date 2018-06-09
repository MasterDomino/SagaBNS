using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class Skill
    {
        #region Properties

        public virtual Character Character { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Identification { get; set; }

        public long CharacterId { get; set; }

        public byte Level { get; set; }

        public long SkillId { get; set; }

        #endregion
    }
}
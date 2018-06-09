using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class TeleportLocation
    {
        #region Properties

        public virtual Character Character { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Identification { get; set; }

        public long CharacterId { get; set; }

        public int TeleportId { get; set; }

        #endregion
    }
}
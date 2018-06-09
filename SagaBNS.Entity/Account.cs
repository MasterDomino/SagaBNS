using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class Account
    {
        #region Instantiation

        public Account() => Character = new HashSet<Character>();

        #endregion

        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }

        public virtual ICollection<Character> Character { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public byte ExtraSlots { get; set; }

        public Guid LoginToken { get; set; }

        public DateTime TokenExpireTime { get; set; }

        public byte GMLevel { get; set; }

        [MaxLength(45)]
        public string LastLoginIP { get; set; }

        public DateTime LastLoginTime { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }

        #endregion
    }
}
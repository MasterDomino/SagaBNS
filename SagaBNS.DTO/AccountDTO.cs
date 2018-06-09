using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class AccountDTO
    {
        #region Properties

        public Guid AccountId { get; set; }

        public string Email { get; set; }

        public byte ExtraSlots { get; set; }

        public byte GMLevel { get; set; }

        public string LastLoginIP { get; set; }

        public DateTime LastLoginTime { get; set; }

        public Guid LoginToken { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public DateTime TokenExpireTime { get; set; }

        #endregion
    }
}
using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class AccountMapper
    {
        #region Methods

        public static bool ToAccount(AccountDTO input, Account output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.AccountId = input.AccountId;
            output.Name = input.Name;
            output.Password = input.Password;
            output.Email = input.Email;
            output.ExtraSlots = input.ExtraSlots;
            output.GMLevel = input.GMLevel;
            output.LastLoginIP = input.LastLoginIP;
            output.LastLoginTime = input.LastLoginTime;
            output.LoginToken = input.LoginToken;
            output.TokenExpireTime = input.TokenExpireTime;
            return true;
        }

        public static bool ToAccountDTO(Account input, AccountDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.AccountId = input.AccountId;
            output.Name = input.Name;
            output.Password = input.Password;
            output.Email = input.Email;
            output.ExtraSlots = input.ExtraSlots;
            output.GMLevel = input.GMLevel;
            output.LastLoginIP = input.LastLoginIP;
            output.LastLoginTime = input.LastLoginTime;
            output.LoginToken = input.LoginToken;
            output.TokenExpireTime = input.TokenExpireTime;
            return true;
        }

        #endregion
    }
}
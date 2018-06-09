using SagaBNS.Core;
using SagaBNS.DAO.Interface;
using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using SagaBNS.Entity;
using SagaBNS.Entity.Helpers;
using SagaBNS.Mapper;
using System;
using System.Data.Entity;
using System.Linq;

namespace SagaBNS.DAO
{
    public class AccountDAO : IAccountDAO
    {
        #region Methods

        public DeleteResult Delete(Guid accountId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Account account = context.Account.FirstOrDefault(c => c.AccountId.Equals(accountId));

                    if (account != null)
                    {
                        context.Account.Remove(account);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Account Delete Error: AccountId: {accountId}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public SaveResult Insert(ref AccountDTO account)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Guid accountId = account.AccountId;
                    Account entity = context.Account.FirstOrDefault(c => c.AccountId.Equals(accountId));

                    if (entity == null)
                    {
                        account = Insert(account, context);
                        return SaveResult.Inserted;
                    }
                    return SaveResult.Error;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Account Update Error: AccountId: {account.AccountId}: {e.Message}", e);
                return SaveResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(ref AccountDTO account)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Guid accountId = account.AccountId;
                    Account entity = context.Account.FirstOrDefault(c => c.AccountId.Equals(accountId));

                    if (entity == null)
                    {
                        account = Insert(account, context);
                        return SaveResult.Inserted;
                    }
                    account = Update(entity, account, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Account Update Error: AccountId: {account.AccountId}: {e.Message}", e);
                return SaveResult.Error;
            }
        }

        private static AccountDTO Insert(AccountDTO account, SagaBNSContext context)
        {
            Account entity = new Account();
            AccountMapper.ToAccount(account, entity);
            context.Account.Add(entity);
            context.SaveChanges();
            AccountMapper.ToAccountDTO(entity, account);
            return account;
        }

        private static AccountDTO Update(Account entity, AccountDTO account, SagaBNSContext context)
        {
            if (entity != null)
            {
                AccountMapper.ToAccount(account, entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            if (AccountMapper.ToAccountDTO(entity, account))
            {
                return account;
            }

            return null;
        }

        public AccountDTO LoadById(Guid accountId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Account account = context.Account.FirstOrDefault(a => a.AccountId.Equals(accountId));
                    if (account != null)
                    {
                        AccountDTO accountDTO = new AccountDTO();
                        if (AccountMapper.ToAccountDTO(account, accountDTO))
                        {
                            return accountDTO;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        public AccountDTO LoadByName(string name)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Account account = context.Account.FirstOrDefault(a => a.Name.Equals(name));
                    if (account != null)
                    {
                        AccountDTO accountDTO = new AccountDTO();
                        if (AccountMapper.ToAccountDTO(account, accountDTO))
                        {
                            return accountDTO;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        #endregion
    }
}
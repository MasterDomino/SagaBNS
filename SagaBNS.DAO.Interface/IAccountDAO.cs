using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System;

namespace SagaBNS.DAO.Interface
{
    public interface IAccountDAO
    {
        #region Methods

        DeleteResult Delete(Guid accountId);

        SaveResult Insert(ref AccountDTO account);

        SaveResult InsertOrUpdate(ref AccountDTO account);

        AccountDTO LoadById(Guid accountId);

        AccountDTO LoadByName(string name);

        #endregion
    }
}
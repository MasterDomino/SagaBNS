using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface IItemDAO
    {
        #region Methods

        DeleteResult DeleteById(long id);

        SaveResult InsertOrUpdate(ref ItemDTO item);

        IEnumerable<ItemDTO> LoadAll();

        IEnumerable<ItemDTO> LoadByCharacterId(long characterId);

        ItemDTO LoadById(long id);

        long LoadHighestId();

        #endregion
    }
}
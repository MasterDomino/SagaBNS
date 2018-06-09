using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface ITeleportLocationDAO
    {
        #region Methods

        DeleteResult DeleteById(int teleportId);

        SaveResult InsertOrUpdate(TeleportLocationDTO teleportLocation);

        IEnumerable<TeleportLocationDTO> LoadAll();

        IEnumerable<TeleportLocationDTO> LoadByCharacterId(long characterId);

        TeleportLocationDTO LoadById(int teleportId);

        #endregion
    }
}
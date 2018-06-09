using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface ICharacterDAO
    {
        #region Methods

        DeleteResult Delete(Guid accountId, long characterId);

        DeleteResult DeleteBySlot(Guid accountId, byte characterSlot);

        SaveResult InsertOrUpdate(ref CharacterDTO character);

        IEnumerable<CharacterDTO> LoadAll();

        IEnumerable<CharacterDTO> LoadAllByAccount(Guid accountId);

        IEnumerable<CharacterDTO> LoadByAccount(Guid accountId);

        CharacterDTO LoadById(long characterId);

        CharacterDTO LoadByName(string name);

        CharacterDTO LoadByNameAndWorldId(string name, byte worldId);

        CharacterDTO LoadBySlot(Guid accountId, byte slot);

        #endregion
    }
}
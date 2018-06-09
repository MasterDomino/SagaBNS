using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface ISkillDAO
    {
        #region Methods

        DeleteResult Delete(long characterId, short skillId);

        DeleteResult Delete(Guid id);

        SkillDTO InsertOrUpdate(SkillDTO dto);

        IEnumerable<SkillDTO> InsertOrUpdate(IEnumerable<SkillDTO> dtos);

        IEnumerable<SkillDTO> LoadByCharacterId(long characterId);

        SkillDTO LoadById(Guid id);

        IEnumerable<Guid> LoadKeysByCharacterId(long characterId);

        #endregion
    }
}
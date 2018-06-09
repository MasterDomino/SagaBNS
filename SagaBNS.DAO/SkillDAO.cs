using SagaBNS.DAO.Interface;
using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System;
using System.Collections.Generic;

namespace SagaBNS.DAO
{
    public class SkillDAO : ISkillDAO
    {
        #region Methods

        public DeleteResult Delete(long characterId, short skillId) => throw new NotImplementedException();

        public DeleteResult Delete(Guid id) => throw new NotImplementedException();

        public SkillDTO InsertOrUpdate(SkillDTO dto) => throw new NotImplementedException();

        public IEnumerable<SkillDTO> InsertOrUpdate(IEnumerable<SkillDTO> dtos) => throw new NotImplementedException();

        public IEnumerable<SkillDTO> LoadByCharacterId(long characterId) => throw new NotImplementedException();

        public SkillDTO LoadById(Guid id) => throw new NotImplementedException();

        public IEnumerable<Guid> LoadKeysByCharacterId(long characterId) => throw new NotImplementedException();

        #endregion
    }
}
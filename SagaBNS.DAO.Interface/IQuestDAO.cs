using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface IQuestDAO
    {
        #region Methods

        DeleteResult DeleteById(int questId);

        QuestDTO InsertOrUpdate(QuestDTO quest);

        void InsertOrUpdateFromList(List<QuestDTO> questList);

        IEnumerable<QuestDTO> LoadAll();

        QuestDTO LoadById(int questId);

        IEnumerable<QuestDTO> LoadByCharacterId(long characterId);

        #endregion
    }
}
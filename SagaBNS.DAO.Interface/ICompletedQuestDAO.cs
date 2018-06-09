using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using System.Collections.Generic;

namespace SagaBNS.DAO.Interface
{
    public interface ICompletedQuestDAO
    {
        #region Methods

        DeleteResult DeleteById(int completedQuestId);

        CompletedQuestDTO InsertOrUpdate(CompletedQuestDTO completedQuest);

        void InsertOrUpdateFromList(List<CompletedQuestDTO> completedQuestList);

        IEnumerable<CompletedQuestDTO> LoadAll();

        CompletedQuestDTO LoadById(int completedQuestId);

        IEnumerable<CompletedQuestDTO> LoadByQuestId(int completedQuestId);

        IEnumerable<CompletedQuestDTO> LoadByCharacterId(long characterId);

        #endregion
    }
}
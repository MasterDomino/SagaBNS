using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class CompletedQuestMapper
    {
        #region Methods

        public static bool ToCompletedQuest(CompletedQuestDTO input, CompletedQuest output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.QuestId = input.QuestId;
            return true;
        }

        public static bool ToCompletedQuestDTO(CompletedQuest input, CompletedQuestDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.QuestId = input.QuestId;
            return true;
        }

        #endregion
    }
}

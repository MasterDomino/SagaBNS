using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class QuestMapper
    {
        #region Methods

        public static bool ToQuest(QuestDTO input, Quest output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.Count1 = input.Count1;
            output.Count2 = input.Count2;
            output.Count3 = input.Count3;
            output.Count4 = input.Count4;
            output.Count5 = input.Count5;
            output.NextStep = input.NextStep;
            output.QuestId = input.QuestId;
            output.Step = input.Step;
            output.StepStatus = input.StepStatus;
            return true;
        }

        public static bool ToQuestDTO(Quest input, QuestDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.Count1 = input.Count1;
            output.Count2 = input.Count2;
            output.Count3 = input.Count3;
            output.Count4 = input.Count4;
            output.Count5 = input.Count5;
            output.NextStep = input.NextStep;
            output.QuestId = input.QuestId;
            output.Step = input.Step;
            output.StepStatus = input.StepStatus;
            return true;
        }

        #endregion
    }
}
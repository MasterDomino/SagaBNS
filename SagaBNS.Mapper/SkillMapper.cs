using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class SkillMapper
    {
        #region Methods

        public static bool ToSkill(SkillDTO input, Skill output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.SkillId = input.SkillId;
            output.CharacterId = input.CharacterId;
            output.Level = input.Level;
            return true;
        }

        public static bool ToSkillDTO(Skill input, SkillDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.SkillId = input.SkillId;
            output.CharacterId = input.CharacterId;
            output.Level = input.Level;
            return true;
        }

        #endregion
    }
}

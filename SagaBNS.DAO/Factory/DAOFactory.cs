using SagaBNS.DAO.Interface;

namespace SagaBNS.DAO.Factory
{
    public static class DAOFactory
    {
        #region Members

        private static IAccountDAO _accountDAO;
        private static ICharacterDAO _characterDAO;
        private static ICompletedQuestDAO _completedQuestDAO;
        private static IItemDAO _itemDAO;
        private static IQuestDAO _questDAO;
        private static ISkillDAO _skillDAO;
        private static ITeleportLocationDAO _teleportLocationDAO;

        #endregion

        #region Properties

        public static IAccountDAO AccountDAO => _accountDAO ?? (_accountDAO = new AccountDAO());

        public static ICharacterDAO CharacterDAO => _characterDAO ?? (_characterDAO = new CharacterDAO());

        public static ICompletedQuestDAO CompletedQuestDAO => _completedQuestDAO ?? (_completedQuestDAO = new CompletedQuestDAO());

        public static IItemDAO ItemDAO => _itemDAO ?? (_itemDAO = new ItemDAO());

        public static IQuestDAO QuestDAO => _questDAO ?? (_questDAO = new QuestDAO());

        public static ISkillDAO SkillDAO => _skillDAO ?? (_skillDAO = new SkillDAO());

        public static ITeleportLocationDAO TeleportLocationDAO => _teleportLocationDAO ?? (_teleportLocationDAO = new TeleportLocationDAO());

        #endregion
    }
}
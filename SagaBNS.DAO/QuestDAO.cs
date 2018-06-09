using SagaBNS.Core;
using SagaBNS.DAO.Interface;
using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using SagaBNS.Entity;
using SagaBNS.Entity.Helpers;
using SagaBNS.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaBNS.DAO
{
    public class QuestDAO : IQuestDAO
    {
        #region Methods

        public DeleteResult DeleteById(int questId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Quest deleteEntity = context.Quest.Find(questId);
                    if (deleteEntity != null)
                    {
                        context.Quest.Remove(deleteEntity);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Quest Delete Error: QuestId:{questId}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public QuestDTO InsertOrUpdate(QuestDTO quest)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Quest entity = context.Quest.Find(quest.QuestId);

                    if (entity == null)
                    {
                        return Insert(quest, context);
                    }
                    return Update(entity, quest, context);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Quest Insert Error: QuestId:{quest.QuestId}: {e.Message}", e);
                return quest;
            }
        }

        public void InsertOrUpdateFromList(List<QuestDTO> questList)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    void Insert(QuestDTO quest)
                    {
                        Quest _entity = new Quest();
                        QuestMapper.ToQuest(quest, _entity);
                        context.Quest.Add(_entity);
                    }

                    void Update(Quest _entity, QuestDTO quest)
                    {
                        if (_entity != null)
                        {
                            QuestMapper.ToQuest(quest, _entity);
                            context.SaveChanges();
                        }
                    }

                    foreach (QuestDTO item in questList)
                    {
                        Quest entity = context.Quest.Find(item.QuestId);

                        if (entity == null)
                        {
                            Insert(item);
                        }
                        else
                        {
                            Update(entity, item);
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public IEnumerable<QuestDTO> LoadAll()
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<QuestDTO> result = new List<QuestDTO>();
                foreach (Quest entity in context.Quest)
                {
                    QuestDTO dto = new QuestDTO();
                    QuestMapper.ToQuestDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public QuestDTO LoadById(int questId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                QuestDTO dto = new QuestDTO();
                if (QuestMapper.ToQuestDTO(context.Quest.Find(questId), dto))
                {
                    return dto;
                }

                return null;
            }
        }

        private static QuestDTO Insert(QuestDTO quest, SagaBNSContext context)
        {
            Quest entity = new Quest();
            QuestMapper.ToQuest(quest, entity);
            context.Quest.Add(entity);
            context.SaveChanges();
            if (QuestMapper.ToQuestDTO(entity, quest))
            {
                return quest;
            }

            return null;
        }

        private static QuestDTO Update(Quest entity, QuestDTO quest, SagaBNSContext context)
        {
            if (entity != null)
            {
                QuestMapper.ToQuest(quest, entity);
                context.SaveChanges();
            }

            if (QuestMapper.ToQuestDTO(entity, quest))
            {
                return quest;
            }

            return null;
        }

        public IEnumerable<QuestDTO> LoadByCharacterId(long characterId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<QuestDTO> result = new List<QuestDTO>();
                foreach (Quest entity in context.Quest.Where(s=>s.CharacterId.Equals(characterId)))
                {
                    QuestDTO dto = new QuestDTO();
                    QuestMapper.ToQuestDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        #endregion
    }
}
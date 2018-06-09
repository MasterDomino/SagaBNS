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
    public class CompletedQuestDAO : ICompletedQuestDAO
    {
        #region Methods

        public DeleteResult DeleteById(int completedQuestId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CompletedQuest deleteEntity = context.CompletedQuest.Find(completedQuestId);
                    if (deleteEntity != null)
                    {
                        context.CompletedQuest.Remove(deleteEntity);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"CompletedQuest Delete Error: QuestId:{completedQuestId}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public CompletedQuestDTO InsertOrUpdate(CompletedQuestDTO completedQuest)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CompletedQuest entity = context.CompletedQuest.Find(completedQuest.QuestId);

                    if (entity == null)
                    {
                        return Insert(completedQuest, context);
                    }
                    return Update(entity, completedQuest, context);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"CompletedQuest Insert Error: QuestId:{completedQuest.QuestId}: {e.Message}", e);
                return completedQuest;
            }
        }

        public void InsertOrUpdateFromList(List<CompletedQuestDTO> completedQuestList)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    void Insert(CompletedQuestDTO completedQuest)
                    {
                        CompletedQuest _entity = new CompletedQuest();
                        CompletedQuestMapper.ToCompletedQuest(completedQuest, _entity);
                        context.CompletedQuest.Add(_entity);
                    }

                    void Update(CompletedQuest _entity, CompletedQuestDTO quest)
                    {
                        if (_entity != null)
                        {
                            CompletedQuestMapper.ToCompletedQuest(quest, _entity);
                            context.SaveChanges();
                        }
                    }

                    foreach (CompletedQuestDTO item in completedQuestList)
                    {
                        CompletedQuest entity = context.CompletedQuest.Find(item.QuestId);

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

        public IEnumerable<CompletedQuestDTO> LoadAll()
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CompletedQuestDTO> result = new List<CompletedQuestDTO>();
                foreach (CompletedQuest entity in context.CompletedQuest)
                {
                    CompletedQuestDTO dto = new CompletedQuestDTO();
                    CompletedQuestMapper.ToCompletedQuestDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public CompletedQuestDTO LoadById(int completedQuestId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                CompletedQuestDTO dto = new CompletedQuestDTO();
                if (CompletedQuestMapper.ToCompletedQuestDTO(context.CompletedQuest.Find(completedQuestId), dto))
                {
                    return dto;
                }

                return null;
            }
        }

        public IEnumerable<CompletedQuestDTO> LoadByQuestId(int completedQuestId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CompletedQuestDTO> result = new List<CompletedQuestDTO>();
                foreach (CompletedQuest entity in context.CompletedQuest.Where(c => c.QuestId.Equals(completedQuestId)))
                {
                    CompletedQuestDTO dto = new CompletedQuestDTO();
                    CompletedQuestMapper.ToCompletedQuestDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        private static CompletedQuestDTO Insert(CompletedQuestDTO completedQuest, SagaBNSContext context)
        {
            CompletedQuest entity = new CompletedQuest();
            CompletedQuestMapper.ToCompletedQuest(completedQuest, entity);
            context.CompletedQuest.Add(entity);
            context.SaveChanges();
            if (CompletedQuestMapper.ToCompletedQuestDTO(entity, completedQuest))
            {
                return completedQuest;
            }

            return null;
        }

        private static CompletedQuestDTO Update(CompletedQuest entity, CompletedQuestDTO completedQuest, SagaBNSContext context)
        {
            if (entity != null)
            {
                CompletedQuestMapper.ToCompletedQuest(completedQuest, entity);
                context.SaveChanges();
            }

            if (CompletedQuestMapper.ToCompletedQuestDTO(entity, completedQuest))
            {
                return completedQuest;
            }

            return null;
        }

        public IEnumerable<CompletedQuestDTO> LoadByCharacterId(long characterId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CompletedQuestDTO> result = new List<CompletedQuestDTO>();
                foreach (CompletedQuest entity in context.CompletedQuest.Where(c => c.CharacterId.Equals(characterId)))
                {
                    CompletedQuestDTO dto = new CompletedQuestDTO();
                    CompletedQuestMapper.ToCompletedQuestDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        #endregion
    }
}
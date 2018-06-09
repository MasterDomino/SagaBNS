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
    public class ItemDAO : IItemDAO
    {
        #region Methods

        public DeleteResult DeleteById(long id)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    Item deleteEntity = context.Item.Find(id);
                    if (deleteEntity != null)
                    {
                        context.Item.Remove(deleteEntity);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Item Delete Error: Id:{id}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(ref ItemDTO item)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    long id = item.Id;
                    Item entity = context.Item.FirstOrDefault(c => c.Id.Equals(id));
                    if (entity == null)
                    {
                        item = Insert(item, context);
                        return SaveResult.Inserted;
                    }
                    item = Update(entity, item, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Insert Error: Id:{item.Id}CharacterId:{item.CharacterId}: {e.Message}", e);
                return SaveResult.Error;
            }
        }

        public IEnumerable<ItemDTO> LoadAll()
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<ItemDTO> result = new List<ItemDTO>();
                foreach (Item entity in context.Item)
                {
                    ItemDTO dto = new ItemDTO();
                    ItemMapper.ToItemDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public IEnumerable<ItemDTO> LoadByCharacterId(long characterId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<ItemDTO> result = new List<ItemDTO>();
                foreach (Item entity in context.Item.Where(c => c.CharacterId.Equals(characterId)))
                {
                    ItemDTO dto = new ItemDTO();
                    ItemMapper.ToItemDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public ItemDTO LoadById(long id)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                ItemDTO dto = new ItemDTO();
                if (ItemMapper.ToItemDTO(context.Item.Find(id), dto))
                {
                    return dto;
                }

                return null;
            }
        }

        private static ItemDTO Insert(ItemDTO item, SagaBNSContext context)
        {
            Item entity = new Item();
            ItemMapper.ToItem(item, entity);
            context.Item.Add(entity);
            context.SaveChanges();
            if (ItemMapper.ToItemDTO(entity, item))
            {
                return item;
            }

            return null;
        }

        private static ItemDTO Update(Item entity, ItemDTO item, SagaBNSContext context)
        {
            if (entity != null)
            {
                ItemMapper.ToItem(item, entity);
                context.SaveChanges();
            }

            if (ItemMapper.ToItemDTO(entity, item))
            {
                return item;
            }

            return null;
        }

        public long LoadHighestId()
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    return context.Item.OrderByDescending(s => s.Id).FirstOrDefault().Id;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return 0;
        }

        #endregion
    }
}
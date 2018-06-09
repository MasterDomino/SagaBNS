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
    public class TeleportLocationDAO : ITeleportLocationDAO
    {
        #region Methods

        public DeleteResult DeleteById(int teleportId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    TeleportLocation deleteEntity = context.TeleportLocation.Find(teleportId);
                    if (deleteEntity != null)
                    {
                        context.TeleportLocation.Remove(deleteEntity);
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"TeleportLocation Delete Error: TeleportId:{teleportId}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(TeleportLocationDTO teleportLocation)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    int teleportId = teleportLocation.TeleportId;
                    TeleportLocation entity = context.TeleportLocation.FirstOrDefault(c => c.TeleportId.Equals(teleportId));
                    if (entity == null)
                    {
                        teleportLocation = Insert(teleportLocation, context);
                        return SaveResult.Inserted;
                    }
                    teleportLocation = Update(entity, teleportLocation, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Insert Error: TeleportId:{teleportLocation.TeleportId}CharacterId:{teleportLocation.CharacterId}: {e.Message}", e);
                return SaveResult.Error;
            }
        }

        private static TeleportLocationDTO Insert(TeleportLocationDTO teleportLocation, SagaBNSContext context)
        {
            TeleportLocation entity = new TeleportLocation();
            TeleportLocationMapper.ToTeleportLocation(teleportLocation, entity);
            context.TeleportLocation.Add(entity);
            context.SaveChanges();
            if (TeleportLocationMapper.ToTeleportLocationDTO(entity, teleportLocation))
            {
                return teleportLocation;
            }

            return null;
        }

        private static TeleportLocationDTO Update(TeleportLocation entity, TeleportLocationDTO teleportLocation, SagaBNSContext context)
        {
            if (entity != null)
            {
                TeleportLocationMapper.ToTeleportLocation(teleportLocation, entity);
                context.SaveChanges();
            }

            if (TeleportLocationMapper.ToTeleportLocationDTO(entity, teleportLocation))
            {
                return teleportLocation;
            }

            return null;
        }

        public IEnumerable<TeleportLocationDTO> LoadAll()
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<TeleportLocationDTO> result = new List<TeleportLocationDTO>();
                foreach (TeleportLocation entity in context.TeleportLocation)
                {
                    TeleportLocationDTO dto = new TeleportLocationDTO();
                    TeleportLocationMapper.ToTeleportLocationDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public IEnumerable<TeleportLocationDTO> LoadByCharacterId(long characterId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<TeleportLocationDTO> result = new List<TeleportLocationDTO>();
                foreach (TeleportLocation entity in context.TeleportLocation.Where(c => c.CharacterId.Equals(characterId)))
                {
                    TeleportLocationDTO dto = new TeleportLocationDTO();
                    TeleportLocationMapper.ToTeleportLocationDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public TeleportLocationDTO LoadById(int teleportId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                TeleportLocationDTO dto = new TeleportLocationDTO();
                if (TeleportLocationMapper.ToTeleportLocationDTO(context.TeleportLocation.Find(teleportId), dto))
                {
                    return dto;
                }

                return null;
            }
        }

        #endregion
    }
}
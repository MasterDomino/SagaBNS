using SagaBNS.Core;
using SagaBNS.DAO.Interface;
using SagaBNS.DTO;
using SagaBNS.DTO.Enums;
using SagaBNS.Entity;
using SagaBNS.Entity.Helpers;
using SagaBNS.Enums;
using SagaBNS.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaBNS.DAO
{
    public class CharacterDAO : ICharacterDAO
    {
        #region Methods

        public DeleteResult Delete(Guid accountId, long characterId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    // actually a Character wont be deleted, it just will be disabled for future traces
                    Character character = context.Character.SingleOrDefault(c => c.AccountId.Equals(accountId) && c.CharacterId.Equals(characterId) && c.State.Equals((byte)CharacterState.Active));

                    if (character != null)
                    {
                        //implement character state changing
                        character.State = CharacterState.Inactive;
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Character Delete Error: CharacterId:{characterId}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public long LoadHighestId()
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    return context.Character.OrderByDescending(s => s.CharacterId).FirstOrDefault().CharacterId;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return 0;
        }

        public DeleteResult DeleteBySlot(Guid accountId, byte characterSlot)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    // actually a Character wont be deleted, it just will be disabled for future traces
                    Character character = context.Character.SingleOrDefault(c => c.AccountId.Equals(accountId) && c.Slot.Equals(characterSlot) && c.State.Equals((byte)CharacterState.Active));

                    if (character != null)
                    {
                        //implement character state changing
                        character.State = CharacterState.Inactive;
                        context.SaveChanges();
                    }

                    return DeleteResult.Deleted;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Character Delete Error: Slot:{characterSlot}: {e.Message}", e);
                return DeleteResult.Error;
            }
        }

        public SaveResult InsertOrUpdate(ref CharacterDTO character)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    long characterId = character.CharacterId;
                    Character entity = context.Character.FirstOrDefault(c => c.CharacterId.Equals(characterId));
                    if (entity == null)
                    {
                        character = Insert(character, context);
                        return SaveResult.Inserted;
                    }
                    character = Update(entity, character, context);
                    return SaveResult.Updated;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Insert Error: Id:{character.CharacterId}Name:{character.Name}: {e.Message}", e);
                return SaveResult.Error;
            }
        }

        public IEnumerable<CharacterDTO> LoadAll()
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CharacterDTO> result = new List<CharacterDTO>();
                foreach (Character chara in context.Character)
                {
                    CharacterDTO dto = new CharacterDTO();
                    CharacterMapper.ToCharacterDTO(chara, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public IEnumerable<CharacterDTO> LoadAllByAccount(Guid accountId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CharacterDTO> result = new List<CharacterDTO>();
                foreach (Character entity in context.Character.Where(c => c.AccountId.Equals(accountId)).OrderByDescending(c => c.Slot))
                {
                    CharacterDTO dto = new CharacterDTO();
                    CharacterMapper.ToCharacterDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public IEnumerable<CharacterDTO> LoadByAccount(Guid accountId)
        {
            using (SagaBNSContext context = DataAccessHelper.CreateContext())
            {
                List<CharacterDTO> result = new List<CharacterDTO>();
                foreach (Character entity in context.Character.Where(c => c.AccountId.Equals(accountId) && c.State.Equals((byte)CharacterState.Active)).OrderByDescending(c => c.Slot))
                {
                    CharacterDTO dto = new CharacterDTO();
                    CharacterMapper.ToCharacterDTO(entity, dto);
                    result.Add(dto);
                }
                return result;
            }
        }

        public CharacterDTO LoadById(long characterId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CharacterDTO dto = new CharacterDTO();
                    if (CharacterMapper.ToCharacterDTO(context.Character.FirstOrDefault(c => c.CharacterId.Equals(characterId)), dto))
                    {
                        return dto;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public CharacterDTO LoadByName(string name)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CharacterDTO dto = new CharacterDTO();
                    if (CharacterMapper.ToCharacterDTO(context.Character.SingleOrDefault(c => c.Name.Equals(name)), dto))
                    {
                        return dto;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        public CharacterDTO LoadByNameAndWorldId(string name, byte worldId)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CharacterDTO dto = new CharacterDTO();
                    if (CharacterMapper.ToCharacterDTO(context.Character.SingleOrDefault(c => c.Name.Equals(name) && c.WorldId.Equals(worldId)), dto))
                    {
                        return dto;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        public CharacterDTO LoadBySlot(Guid accountId, byte slot)
        {
            try
            {
                using (SagaBNSContext context = DataAccessHelper.CreateContext())
                {
                    CharacterDTO dto = new CharacterDTO();
                    if (CharacterMapper.ToCharacterDTO(context.Character.SingleOrDefault(c => c.AccountId.Equals(accountId) && c.Slot.Equals(slot) && c.State.Equals((byte)CharacterState.Active)), dto))
                    {
                        return dto;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"There should be only 1 character per slot, AccountId: {accountId} Slot: {slot}", e);
            }
            return null;
        }

        private static CharacterDTO Insert(CharacterDTO character, SagaBNSContext context)
        {
            Character entity = new Character();
            CharacterMapper.ToCharacter(character, entity);
            context.Character.Add(entity);
            context.SaveChanges();
            if (CharacterMapper.ToCharacterDTO(entity, character))
            {
                return character;
            }
            return null;
        }

        private static CharacterDTO Update(Character entity, CharacterDTO character, SagaBNSContext context)
        {
            if (entity != null)
            {
                // State Updates should only occur upon deleting character, so outside of this method.
                CharacterState state = entity.State;
                CharacterMapper.ToCharacter(character, entity);
                entity.State = state;

                context.SaveChanges();
            }

            if (CharacterMapper.ToCharacterDTO(entity, character))
            {
                return character;
            }

            return null;
        }

        #endregion
    }
}
using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class CharacterMapper
    {
        #region Methods

        public static bool ToCharacter(CharacterDTO input, Character output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.AccountId = input.AccountId;
            output.Appearence1 = input.Appearence1;
            output.Appearence2 = input.Appearence2;
            output.CharacterId = input.CharacterId;
            output.DepositorySize = input.DepositorySize;
            output.Dir = input.Dir;
            output.Exp = input.Exp;
            output.Gender = input.Gender;
            output.Gold = input.Gold;
            output.HP = input.HP;
            output.InventorySize = input.InventorySize;
            output.Job = input.Job;
            output.Level = input.Level;
            output.MapID = input.MapID;
            output.MaxHP = input.MaxHP;
            output.MaxMP = input.MaxMP;
            output.MP = input.MP;
            output.Name = input.Name;
            output.Race = input.Race;
            output.Slot = input.Slot;
            output.UISettings = input.UISettings;
            output.WardrobeSize = input.WardrobeSize;
            output.WorldId = input.WorldId;
            output.X = input.X;
            output.Y = input.Y;
            output.Z = input.Z;
            return true;
        }

        public static bool ToCharacterDTO(Character input, CharacterDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.AccountId = input.AccountId;
            output.Appearence1 = input.Appearence1;
            output.Appearence2 = input.Appearence2;
            output.CharacterId = input.CharacterId;
            output.DepositorySize = input.DepositorySize;
            output.Dir = input.Dir;
            output.Exp = input.Exp;
            output.Gender = input.Gender;
            output.Gold = input.Gold;
            output.HP = input.HP;
            output.InventorySize = input.InventorySize;
            output.Job = input.Job;
            output.Level = input.Level;
            output.MapID = input.MapID;
            output.MaxHP = input.MaxHP;
            output.MaxMP = input.MaxMP;
            output.MP = input.MP;
            output.Name = input.Name;
            output.Race = input.Race;
            output.Slot = input.Slot;
            output.UISettings = input.UISettings;
            output.WardrobeSize = input.WardrobeSize;
            output.WorldId = input.WorldId;
            output.X = input.X;
            output.Y = input.Y;
            output.Z = input.Z;
            return true;
        }

        #endregion
    }
}
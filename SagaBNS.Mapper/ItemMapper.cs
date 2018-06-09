using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class ItemMapper
    {
        #region Methods

        public static bool ToItem(ItemDTO input, Item output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.Container = input.Container;
            output.Count = input.Count;
            output.Durability = input.Durability;
            output.Enchant = input.Enchant;
            output.Exp = input.Exp;
            output.Gem1 = input.Gem1;
            output.Gem2 = input.Gem2;
            output.Gem3 = input.Gem3;
            output.Gem4 = input.Gem4;
            output.Id = input.Id;
            output.ItemId = input.ItemId;
            output.Level = input.Level;
            output.Slot = input.Slot;
            output.Synthesis = input.Synthesis;
            return true;
        }

        public static bool ToItemDTO(Item input, ItemDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.Container = input.Container;
            output.Count = input.Count;
            output.Durability = input.Durability;
            output.Enchant = input.Enchant;
            output.Exp = input.Exp;
            output.Gem1 = input.Gem1;
            output.Gem2 = input.Gem2;
            output.Gem3 = input.Gem3;
            output.Gem4 = input.Gem4;
            output.Id = input.Id;
            output.ItemId = input.ItemId;
            output.Level = input.Level;
            output.Slot = input.Slot;
            output.Synthesis = input.Synthesis;
            return true;
        }

        #endregion
    }
}

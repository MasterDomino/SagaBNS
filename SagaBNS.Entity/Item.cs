using SagaBNS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SagaBNS.Entity
{
    public class Item
    {
        #region Properties

        public virtual Character Character { get; set; }

        public long CharacterId { get; set; }

        public Containers Container { get; set; }

        public int Count { get; set; }

        public byte Durability { get; set; }

        public long Enchant { get; set; }

        public long Exp { get; set; }

        public long Gem1 { get; set; }

        public long Gem2 { get; set; }

        public long Gem3 { get; set; }

        public long Gem4 { get; set; }

        // TODO: Move to GUID
        [Key]
        public long Id { get; set; }

        public long ItemId { get; set; }

        public byte Level { get; set; }

        public int Slot { get; set; }

        public byte Synthesis { get; set; }

        #endregion
    }
}
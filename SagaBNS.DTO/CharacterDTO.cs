using SagaBNS.Enums;
using System;

namespace SagaBNS.DTO
{
    [Serializable]
    public class CharacterDTO
    {
        #region Properties

        public Guid AccountId { get; set; }

        public string Appearence1 { get; set; }

        public string Appearence2 { get; set; }

        public long CharacterId { get; set; }

        public CharacterState State { get; set; }

        public byte DepositorySize { get; set; }

        public int Dir { get; set; }

        public long Exp { get; set; }

        public Gender Gender { get; set; }

        public int Gold { get; set; }

        public int HP { get; set; }

        public byte InventorySize { get; set; }

        public Job Job { get; set; }

        public byte Level { get; set; }

        public long MapID { get; set; }

        public int MaxHP { get; set; }

        public int MaxMP { get; set; }

        public int MP { get; set; }

        public string Name { get; set; }

        public Race Race { get; set; }

        public byte Slot { get; set; }

        public string UISettings { get; set; }

        public byte WardrobeSize { get; set; }

        public byte WorldId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        #endregion
    }
}
using SagaBNS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaBNS.Entity
{
    public class Character
    {
        #region Instantiation

        public Character()
        {
            Items = new HashSet<Item>();
            Quests = new HashSet<Quest>();
            Skills = new HashSet<Skill>();
            CompletedQuests = new HashSet<CompletedQuest>();
            TeleportLocations = new HashSet<TeleportLocation>();
        }

        #endregion

        #region Properties

        public virtual Account Account { get; set; }

        public Guid AccountId { get; set; }

        public string Appearence1 { get; set; }

        public string Appearence2 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CharacterId { get; set; }

        public virtual ICollection<CompletedQuest> CompletedQuests { get; set; }

        public byte DepositorySize { get; set; }

        public int Dir { get; set; }

        public long Exp { get; set; }

        public CharacterState State { get; set; }

        public Gender Gender { get; set; }

        public int Gold { get; set; }

        public int HP { get; set; }

        public byte InventorySize { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Job Job { get; set; }

        public byte Level { get; set; }

        public long MapID { get; set; }

        public int MaxHP { get; set; }

        public int MaxMP { get; set; }

        public int MP { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<Quest> Quests { get; set; }

        public Race Race { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public byte Slot { get; set; }

        public virtual ICollection<TeleportLocation> TeleportLocations { get; set; }

        public string UISettings { get; set; }

        public byte WardrobeSize { get; set; }

        public byte WorldId { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        #endregion
    }
}
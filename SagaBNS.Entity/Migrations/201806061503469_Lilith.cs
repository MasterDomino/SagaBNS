namespace SagaBNS.Entity.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Lilith : DbMigration
    {
        #region Methods

        public override void Down()
        {
            DropForeignKey("dbo.Character", "AccountId", "dbo.Account");
            DropForeignKey("dbo.TeleportLocation", "CharacterId", "dbo.Character");
            DropForeignKey("dbo.Skill", "CharacterId", "dbo.Character");
            DropForeignKey("dbo.Quest", "CharacterId", "dbo.Character");
            DropForeignKey("dbo.Item", "CharacterId", "dbo.Character");
            DropForeignKey("dbo.CompletedQuest", "CharacterId", "dbo.Character");
            DropIndex("dbo.TeleportLocation", new[] { "CharacterId" });
            DropIndex("dbo.Skill", new[] { "CharacterId" });
            DropIndex("dbo.Quest", new[] { "CharacterId" });
            DropIndex("dbo.Item", new[] { "CharacterId" });
            DropIndex("dbo.CompletedQuest", new[] { "CharacterId" });
            DropIndex("dbo.Character", new[] { "AccountId" });
            DropTable("dbo.TeleportLocation");
            DropTable("dbo.Skill");
            DropTable("dbo.Quest");
            DropTable("dbo.Item");
            DropTable("dbo.CompletedQuest");
            DropTable("dbo.Character");
            DropTable("dbo.Account");
        }

        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                {
                    AccountId = c.Guid(nullable: false, identity: true),
                    Email = c.String(maxLength: 255),
                    ExtraSlots = c.Byte(nullable: false),
                    LoginToken = c.Guid(nullable: false),
                    TokenExpireTime = c.DateTime(nullable: false),
                    GMLevel = c.Byte(nullable: false),
                    LastLoginIP = c.String(maxLength: 45),
                    LastLoginTime = c.DateTime(nullable: false),
                    Name = c.String(maxLength: 255),
                    Password = c.String(maxLength: 255, unicode: false),
                })
                .PrimaryKey(t => t.AccountId);

            CreateTable(
                "dbo.Character",
                c => new
                {
                    CharacterId = c.Long(nullable: false, identity: true),
                    AccountId = c.Guid(nullable: false),
                    Appearence1 = c.String(),
                    Appearence2 = c.String(),
                    DepositorySize = c.Byte(nullable: false),
                    Dir = c.Int(nullable: false),
                    Exp = c.Long(nullable: false),
                    State = c.Byte(nullable: false),
                    Gender = c.Int(nullable: false),
                    Gold = c.Int(nullable: false),
                    HP = c.Int(nullable: false),
                    InventorySize = c.Byte(nullable: false),
                    Job = c.Int(nullable: false),
                    Level = c.Byte(nullable: false),
                    MapID = c.Long(nullable: false),
                    MaxHP = c.Int(nullable: false),
                    MaxMP = c.Int(nullable: false),
                    MP = c.Int(nullable: false),
                    Name = c.String(maxLength: 255, unicode: false),
                    Race = c.Int(nullable: false),
                    Slot = c.Byte(nullable: false),
                    UISettings = c.String(),
                    WardrobeSize = c.Byte(nullable: false),
                    WorldId = c.Byte(nullable: false),
                    X = c.Int(nullable: false),
                    Y = c.Int(nullable: false),
                    Z = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.CharacterId)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => t.AccountId);

            CreateTable(
                "dbo.CompletedQuest",
                c => new
                {
                    Identification = c.Guid(nullable: false, identity: true),
                    CharacterId = c.Long(nullable: false),
                    QuestId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Identification)
                .ForeignKey("dbo.Character", t => t.CharacterId)
                .Index(t => t.CharacterId);

            CreateTable(
                "dbo.Item",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CharacterId = c.Long(nullable: false),
                    Container = c.Byte(nullable: false),
                    Count = c.Int(nullable: false),
                    Durability = c.Byte(nullable: false),
                    Enchant = c.Long(nullable: false),
                    Exp = c.Long(nullable: false),
                    Gem1 = c.Long(nullable: false),
                    Gem2 = c.Long(nullable: false),
                    Gem3 = c.Long(nullable: false),
                    Gem4 = c.Long(nullable: false),
                    ItemId = c.Long(nullable: false),
                    Level = c.Byte(nullable: false),
                    Slot = c.Int(nullable: false),
                    Synthesis = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Character", t => t.CharacterId)
                .Index(t => t.CharacterId);

            CreateTable(
                "dbo.Quest",
                c => new
                {
                    Identification = c.Guid(nullable: false, identity: true),
                    CharacterId = c.Long(nullable: false),
                    Count1 = c.Int(nullable: false),
                    Count2 = c.Int(nullable: false),
                    Count3 = c.Int(nullable: false),
                    Count4 = c.Int(nullable: false),
                    Count5 = c.Int(nullable: false),
                    NextStep = c.Byte(nullable: false),
                    QuestId = c.Int(nullable: false),
                    Step = c.Byte(nullable: false),
                    StepStatus = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.Identification)
                .ForeignKey("dbo.Character", t => t.CharacterId)
                .Index(t => t.CharacterId);

            CreateTable(
                "dbo.Skill",
                c => new
                {
                    Identification = c.Guid(nullable: false, identity: true),
                    CharacterId = c.Long(nullable: false),
                    Level = c.Byte(nullable: false),
                    SkillId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Identification)
                .ForeignKey("dbo.Character", t => t.CharacterId)
                .Index(t => t.CharacterId);

            CreateTable(
                "dbo.TeleportLocation",
                c => new
                {
                    Identification = c.Guid(nullable: false, identity: true),
                    CharacterId = c.Long(nullable: false),
                    TeleportId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Identification)
                .ForeignKey("dbo.Character", t => t.CharacterId)
                .Index(t => t.CharacterId);
        }

        #endregion
    }
}
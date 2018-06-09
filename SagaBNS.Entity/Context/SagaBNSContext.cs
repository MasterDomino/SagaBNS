using SagaBNS.Entity.Migrations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SagaBNS.Entity
{
    public class SagaBNSContext : DbContext
    {
        #region Instantiation

        public SagaBNSContext() : base("name=SagaBNSContext")
        {
            Configuration.LazyLoadingEnabled = true;

            // --DO NOT DISABLE, otherwise the mapping will fail only one time access to database so
            // no proxy generation needed, its just slowing down in our case
            Configuration.ProxyCreationEnabled = false;
        }

        #endregion

        #region Properties

        public virtual DbSet<Account> Account { get; set; }

        public virtual DbSet<Character> Character { get; set; }

        public virtual DbSet<CompletedQuest> CompletedQuest { get; set; }

        public virtual DbSet<Item> Item { get; set; }

        public virtual DbSet<Quest> Quest { get; set; }

        public virtual DbSet<Skill> Skill { get; set; }

        public virtual DbSet<TeleportLocation> TeleportLocation { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SagaBNSContext, Configuration>());

            // remove automatic pluralization
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Character)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.AccountId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Character>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Character>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Character>()
                .HasMany(e => e.Skills)
                .WithRequired(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Character>()
                .HasMany(e => e.Quests)
                .WithRequired(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Character>()
                .HasMany(e => e.CompletedQuests)
                .WithRequired(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Character>()
                .HasMany(e => e.TeleportLocations)
                .WithRequired(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .WillCascadeOnDelete(false);
        }

        #endregion
    }
}
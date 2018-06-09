using System.Data.Entity.Migrations;

namespace SagaBNS.Entity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SagaBNSContext>
    {
        #region Instantiation

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        #endregion

        #region Methods

        protected override void Seed(SagaBNSContext context)
        {
            // This method will be called after migrating to the latest version.

            // You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating
            // duplicate seed data.
        }

        #endregion
    }
}
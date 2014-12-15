using Memorialis.Core.Base;
using Memorialis.Core.Dev;
using Memorialis.Core.Identity;
using Memorialis.Core.Migrations;
using Memorialis.Core.Sys.Settings;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Memorialis.Core
{
    public partial class Context : DbContext
    {
        public Context()
            : base("Context")
        {
            Database.SetInitializer<Context>(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        #region Entity sets
        //Identity
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Token> Tokens { get; set; }
        

        //Dev
        public DbSet<DevLog> DevLogs { get; set; }


        //System
        public DbSet<Setting> Settings { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
        
        #region SaveChanges Tracking
        /// <summary>
        /// Retrack changes according to 
        /// </summary>
        private void TrackChanges()
        {
            foreach (DbEntityEntry entry in ChangeTracker.Entries())
            {
                //process only trackable entities
                if (entry.Entity is TrackableEntity)
                {
                    TrackableEntity entity = entry.Entity as TrackableEntity;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.Created = DateTime.UtcNow;
                            break;
                        case EntityState.Deleted:
                            //if entity marked for deletion, unmark and set timestamp
                            entry.State = EntityState.Modified;
                            entity.Deleted = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entity.Modified = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            TrackChanges();
            return await base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        #endregion

        public static Context New()
        {
            return new Context();
        }
        
    }
}

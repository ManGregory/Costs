using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostsWeb.Models
{
    public class CostsContext : IdentityDbContext<ApplicationUser>
    {
        public CostsContext() : base("DefaultConnection")
        {            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CostsJournal> CostsJournal { get; set; }

        public void Rollback()
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }
        }

        public static CostsContext Create()
        {
            return new CostsContext();
        }
    }
}
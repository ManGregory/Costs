using System.Data.Entity;
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

        public static CostsContext Create()
        {
            return new CostsContext();
        }
    }
}
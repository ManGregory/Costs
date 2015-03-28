using System.Data.Entity;

namespace CostsWeb.Models
{
    public class CostsContext : DbContext
    {
        public CostsContext() : base("DefaultConnection")
        {            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CostsJournal> CostsJournal { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostsWeb.Models
{
    public class CostsContext : DbContext
    {
        public CostsContext() : base("DefaultConnection")
        {            
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId);
        }
    }

    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Название категории")]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostsWeb.Models
{
    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [Display(Name="Название категории")]
        public string Name { get; set; }
    }
}
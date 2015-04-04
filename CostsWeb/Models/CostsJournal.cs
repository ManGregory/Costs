using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CostsWeb.Models
{
    public class CostsJournal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString="{0:yyyy-MM-dd}")]
        public DateTime? Date { get; set; }
        
        [DisplayName("Название категории")]
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [DisplayName("Название подкатегории")]
        public int? SubCategoryId { get; set; }

        [DisplayName("Название подкатегории")]
        [ForeignKey("SubCategoryId")]
        public Category SubCategory { get; set; }

        [Required]
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        
        [DisplayName("Примечание")]
        public string Note { get; set; }
       
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
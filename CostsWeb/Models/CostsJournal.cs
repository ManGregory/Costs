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
        public DateTime? Date { get; set; }
        [DisplayName("Название категории")]
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        [DisplayName("Примечание")]
        public string Note { get; set; }

        [NotMapped]
        public string DateForShow
        {
            get { return Date.HasValue ? Date.Value.ToString("yyyy-MM-dd") : null; }          
        }
    }
}
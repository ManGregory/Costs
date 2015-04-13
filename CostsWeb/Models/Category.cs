using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostsWeb.Models
{
    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name="Название категории")]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        private static readonly string[] _defaultCategoryNames =
        {
            "Ежедневные расходы",
            "Еда",
            "Стрижка",
            "Лекарства",
            "Контактные линзы",
            "Чай и кофе",
            "Одежда",
            "Химчистка",
            "Косметика",
            "Учеба",
            "Сладости",
            "Инструменты",
            "Массаж",
            "Жилье",
            "Аренда",
            "Ипотека",
            "Бензин",
            "Стационарный телефон",
            "Интернет",
            "Мобильный телефон",
            "Вода",
            "Электричество",
            "Страховка",
            "Медицинская страховка",
            "Кредитные карты",
            "Техника",
            "Ремонт",
            "Транспорт",
            "Автомобиль",
            "Страховка автомобиля",
            "Проездной на автобус",
            "Проездной на метро",
            "Бензин",
            "Налог на автомобиль",
            "Проезд в поезде",
            "Ремонт автомобиля",
            "Маршрутка",
            "Трамвай",
            "Скоростной",
            "Развлечения",
            "Кабельное телевидение",
            "Кино",
            "Клуб",
            "Покупка DVD-дисков",
            "Прокат DVD-дисков",
            "Кафе и рестораны",
            "Тренажерный зал",
            "Журналы",
            "Музыкальные диски",
            "Уроки музыки",
            "Спортивные мероприятия",
            "Билет на концерт",
            "Спортинвентарь",
            "Бильярд",
            "Подарок",
            "Книга",
            "Отдых",
            "Долг"
        };

        public static async Task AddDefaultCategories(CostsContext db, string userId)
        {
            db.Categories.AddRange(_defaultCategoryNames.Select(s => new Category {Name = s, UserId = userId}));
            await db.SaveChangesAsync();
        }
    }
}
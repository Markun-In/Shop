using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Shop_Task.Models.Category;

namespace Shop_Task.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название товара")]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Категория товара")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Описание товара")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Цена товара")]
        public double Price { get; set; }

        
        [Display(Name = "Фото товара")]
        public byte[]? ProductPicture { get; set; }

        public string? UserId { get; set; }

    }
}

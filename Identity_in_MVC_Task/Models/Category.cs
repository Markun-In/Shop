
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity_in_MVC_Task.Models
{
    public enum Category
    {
        [Display(Name = "Электроника")]
        Electronics,
        [Display(Name = "Автомобили")]
        Cars,
        [Display(Name = "Одежда")]
        Clothes
    }

}

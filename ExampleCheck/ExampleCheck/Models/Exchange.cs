using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExampleCheck
{
    public partial class Exchange
    {
        public Exchange()
        {
            ExchangeClient1 = new HashSet<ExchangeClient>();
            Orders = new HashSet<Order>();
        }
        [Display(Name = "Ідентифікатор біржи")]
        public int Id { get; set; }
        [Display(Name = "Податок біржи")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Range(0.0, 5.0, ErrorMessage = "Податок може бути між 0 та 5")]
        public string? Fee { get; set; }
        [Display(Name = "Назва біржі")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [MinLength(3)]
        public string? Name { get; set; }

        [Display(Name = "Клієнт")]
        public virtual ICollection<ExchangeClient> ExchangeClient1 { get; set; }
        [Display(Name = "Угода")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

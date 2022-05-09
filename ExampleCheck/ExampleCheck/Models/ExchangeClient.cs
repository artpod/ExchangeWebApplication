using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExampleCheck
{
    public partial class ExchangeClient
    {
        public int Id { get; set; }
        [Display(Name = "Назва біржі")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public int? Exchange { get; set; }
        [Display(Name = "Користувач")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public int? ExchangeClient1 { get; set; }
        [Display(Name = "Користувач")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public virtual Customer? ExchangeClient1Navigation { get; set; }
        [Display(Name = "Назва біржі")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public virtual Exchange? ExchangeNavigation { get; set; }
    }
}

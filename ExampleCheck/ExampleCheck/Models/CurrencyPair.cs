using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleCheck
{
    public partial class CurrencyPair
    {
        public CurrencyPair()
        {
            Orders = new HashSet<Order>();
           // Sum = FirstCurrencyNavigation.Name+SecondCurrencyNavigation.Name;
        }
        [Display(Name = "Ідентифікатор валютної пари")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Display(Name = "Перша валюта")]
        public int? FirstCurrency { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Display(Name = "Друга валюта")]
        public int? SecondCurrency { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Display(Name = "Ціна")]
        [Range(0, 1000000, ErrorMessage = "Ціна може бути між 0 та 1000000")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Display(Name = "Тип валютної пари")]
        public string? PairType { get; set; }
        


        [Display(Name = "Перша валюта")]
        public virtual Currency? FirstCurrencyNavigation { get; set; }
        [Display(Name = "Друга валюта")]
        public virtual Currency? SecondCurrencyNavigation { get; set; }

        [NotMapped]
        public string? Sum { get; set; }

       


        [Display(Name = "Угоди")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

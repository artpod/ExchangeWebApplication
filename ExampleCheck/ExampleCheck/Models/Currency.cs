using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExampleCheck
{
    public partial class Currency
    {
        public Currency()
        {
            CurrencyPairFirstCurrencyNavigations = new HashSet<CurrencyPair>();
            CurrencyPairSecondCurrencyNavigations = new HashSet<CurrencyPair>();
        }
        [Display(Name = "Ідентифікатор валюти")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Display(Name = "Назва валюти")]
        public string? Name { get; set; }

        [Display(Name = "Перша валюта")]
        public virtual ICollection<CurrencyPair> CurrencyPairFirstCurrencyNavigations { get; set; }
        [Display(Name = "Друга валюта")]
        public virtual ICollection<CurrencyPair> CurrencyPairSecondCurrencyNavigations { get; set; }
    }
}

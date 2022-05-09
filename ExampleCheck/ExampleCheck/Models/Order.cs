using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExampleCheck
{
    public partial class Order
    {
        [Display(Name = "Ідентифікатор угоди")]
        public int Id { get; set; }
        [Display(Name = "Ідентифікатор пари")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public int? PairId { get; set; }
        [Display(Name = "Об'єм угоди")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [Range(0,1000000, ErrorMessage = "Сума замовлення може бути між 0 та 1000000")]
        public int? Value { get; set; }
        [Display(Name = "Ідентифікатор біржі")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public int? Exchange { get; set; }

        [Display(Name = "Назва біржі")]
        public virtual Exchange? ExchangeNavigation { get; set; }
        [Display(Name = "Валютна пара")]
        public virtual CurrencyPair? Pair { get; set; }
    }
}

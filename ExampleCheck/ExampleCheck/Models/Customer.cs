using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExampleCheck
{
    public partial class Customer
    {
        public Customer()
        {
            ExchangeClient1 = new HashSet<ExchangeClient>();
        }
        [Display(Name = "Ідентифікатор користувача")]
        public int Id { get; set; }
        [Display(Name = "Ім'я користувача")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        public string? Name { get; set; }
        [Display(Name = "День нарождення")]
        [Required(ErrorMessage = "Поле не може бути порожнім!")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Клієнт")]
        public virtual ICollection<ExchangeClient> ExchangeClient1 { get; set; }
    }
}

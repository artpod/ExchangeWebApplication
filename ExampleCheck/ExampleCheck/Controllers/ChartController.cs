using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleCheck.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBLibraryContext _context;
        public ChartController(DBLibraryContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var exchanges = _context.Exchanges.Include(e => e.Orders).ToList();
            List<object> ExOrders = new List<object>();
            ExOrders.Add(new[] { "Біржа", "Кількість угод" });
            int i = 1;
             foreach (var e in exchanges)
             {

                 ExOrders.Add(new object[] { e.Name, e.Orders.Count() });
             }
            return new JsonResult(ExOrders);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var customers = _context.Customers.Include(c => c.ExchangeClient1).ToList();
            List<object> ExOrders = new List<object>();
            ExOrders.Add(new[] { "Клієнт", "Кількість реєстрацій на біржах" });
            int i = 1;
            foreach (var c in customers)
            {

                ExOrders.Add(new object[] { c.Name, c.ExchangeClient1.Count() });
            }
            return new JsonResult(ExOrders);
        }

    }
}

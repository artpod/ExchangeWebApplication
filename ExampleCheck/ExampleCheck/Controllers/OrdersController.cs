 #nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExampleCheck;
using Microsoft.AspNetCore.Authorization;

namespace ExampleCheck.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DBLibraryContext _context;

        public OrdersController(DBLibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MainIndex()
        {
            var dBLibraryContext = _context.Orders.Include(o => o.ExchangeNavigation).Include(o => o.Pair).Include(o => o.Pair.FirstCurrencyNavigation).Include(o => o.Pair.SecondCurrencyNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: Orders
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            var dBLibraryContext = _context.Orders.Where(o => o.Exchange == id).Include(o => o.ExchangeNavigation).Include(o => o.Pair).Include(o => o.Pair.FirstCurrencyNavigation).Include(o => o.Pair.SecondCurrencyNavigation);
            return View(await dBLibraryContext.ToListAsync()); 
        }

        public async Task<IActionResult> Indexer(int id)
        {
            ViewBag.Id = id;
            var dBLibraryContext = _context.Orders.Where(o => o.PairId == id).Include(o => o.ExchangeNavigation).Include(o => o.Pair).Include(o => o.Pair.FirstCurrencyNavigation).Include(o => o.Pair.SecondCurrencyNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.ExchangeNavigation)
                .Include(o => o.Pair)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name");
            ViewData["PairId"] = new SelectList(_context.CurrencyPairs, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,PairId,Value,Exchange")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           // string sum = order.Pair.FirstCurrencyNavigation.Name + order.Pair.SecondCurrencyNavigation.Name;
            
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", order.Exchange);
            ViewData["PairId"] = new SelectList(_context.CurrencyPairs, "Id", "Id", order.PairId);
           // ViewData["PairId"] = new SelectList(_context.Currencies, "Id", "Name", order.Pair.FirstCurrencyNavigation);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", order.Exchange);
            ViewData["PairId"] = new SelectList(_context.CurrencyPairs, "Id", "Id", order.PairId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PairId,Value,Exchange")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", order.Exchange);
            ViewData["PairId"] = new SelectList(_context.CurrencyPairs, "Id", "Id", order.PairId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.ExchangeNavigation)
                .Include(o => o.Pair)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

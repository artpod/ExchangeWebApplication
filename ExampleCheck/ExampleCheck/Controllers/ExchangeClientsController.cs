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
    public class ExchangeClientsController : Controller
    {
        private readonly DBLibraryContext _context;

        public ExchangeClientsController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: ExchangeClients
        public async Task<IActionResult> Index()
        {
            var dBLibraryContext = _context.ExchangeClients.Include(e => e.ExchangeClient1Navigation).Include(e => e.ExchangeNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> Indexer(int? id)
        {
            ViewBag.Id = id;
            var dBLibraryContext = _context.ExchangeClients.Where(e => e.ExchangeClient1 == id).Include(e => e.ExchangeClient1Navigation).Include(e => e.ExchangeNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: ExchangeClients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchangeClient = await _context.ExchangeClients
                .Include(e => e.ExchangeClient1Navigation)
                .Include(e => e.ExchangeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangeClient == null)
            {
                return NotFound();
            }

            return View(exchangeClient);
        }

        // GET: ExchangeClients/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["ExchangeClient1"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name");
            return View();
        }

        // POST: ExchangeClients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Exchange,ExchangeClient1")] ExchangeClient exchangeClient)
        {
           // if (ModelState.IsValid)
           if (!IsDuplicate(exchangeClient))
            {
                _context.Add(exchangeClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ExchangeClient1"] = new SelectList(_context.Customers, "Id", "Name", exchangeClient.ExchangeClient1);
                ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", exchangeClient.Exchange);
                ModelState.AddModelError("", "Цей користувач вже зареєстрований на цій біржі");
            }
          //  Console.Write("Aboba");
          // ViewData["ExchangeClient1"] = new SelectList(_context.Customers, "Id", "Name", exchangeClient.ExchangeClient1);
          //  ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", exchangeClient.Exchange);
            return View(exchangeClient);
        }

        // GET: ExchangeClients/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchangeClient = await _context.ExchangeClients.FindAsync(id);
            if (exchangeClient == null)
            {
                return NotFound();
            }
            ViewData["ExchangeClient1"] = new SelectList(_context.Customers, "Id", "Name", exchangeClient.ExchangeClient1);
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", exchangeClient.Exchange);
            return View(exchangeClient);
        }

        // POST: ExchangeClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Exchange,ExchangeClient1")] ExchangeClient exchangeClient)
        {
            if (id != exchangeClient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exchangeClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExchangeClientExists(exchangeClient.Id))
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
            ViewData["ExchangeClient1"] = new SelectList(_context.Customers, "Id", "Name", exchangeClient.ExchangeClient1);
            ViewData["Exchange"] = new SelectList(_context.Exchanges, "Id", "Name", exchangeClient.Exchange);
            return View(exchangeClient);
        }

        // GET: ExchangeClients/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchangeClient = await _context.ExchangeClients
                .Include(e => e.ExchangeClient1Navigation)
                .Include(e => e.ExchangeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchangeClient == null)
            {
                return NotFound();
            }

            return View(exchangeClient);
        }

        // POST: ExchangeClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exchangeClient = await _context.ExchangeClients.FindAsync(id);
            _context.ExchangeClients.Remove(exchangeClient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExchangeClientExists(int id)
        {
            return _context.ExchangeClients.Any(e => e.Id == id);
        }
        private bool IsDuplicate(ExchangeClient model)
        {
            var cat1 = _context.ExchangeClients.FirstOrDefault(c => c.ExchangeClient1.Equals(model.ExchangeClient1) && c.Exchange.Equals(model.Exchange));
           // var cat2 = _context.CurrencyPairs.Where(c => c.FirstCurrency == c.SecondCurrency);
            // var cat2 = _context.CurrencyPairs.FirstOrDefault(c => c.SecondCurrency.Equals(model.SecondCurrency));

            return (cat1 == null) ? false : true;
        }
    }
}

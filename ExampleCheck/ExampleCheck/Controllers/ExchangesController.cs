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
    public class ExchangesController : Controller
    {
        private readonly DBLibraryContext _context;

        public ExchangesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Exchanges
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exchanges.ToListAsync());
        }

        public IActionResult Orders(int id)
        {
            ViewBag.Id = id;
            return RedirectToAction("Index", "Orders", new { id = ViewBag.Id });
        }


        // GET: Exchanges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchanges.Include(ex => ex.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // GET: Exchanges/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exchanges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Fee,Name")] Exchange exchange)
        {
            if (!IsDuplicate(exchange))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(exchange);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(exchange);
            }
            else
            {
                ModelState.AddModelError("Name", "Така біржа уже існує");
            }
            return View(exchange);
        }

        // GET: Exchanges/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchanges.FindAsync(id);
            if (exchange == null)
            {
                return NotFound();
            }
            return View(exchange);
        }

        // POST: Exchanges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fee,Name")] Exchange exchange)
        {
            if (id != exchange.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exchange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExchangeExists(exchange.Id))
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
            return View(exchange);
        }

        // GET: Exchanges/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exchange = await _context.Exchanges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exchange == null)
            {
                return NotFound();
            }

            return View(exchange);
        }

        // POST: Exchanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exchange = await _context.Exchanges.FindAsync(id);
            _context.Exchanges.Remove(exchange);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExchangeExists(int id)
        {
            return _context.Exchanges.Any(e => e.Id == id);
        }

        private bool IsDuplicate(Exchange model)
        {
            var cat = _context.Exchanges.FirstOrDefault(c => c.Name.Equals(model.Name));

            return cat == null ? false : true;
        }
    }
}

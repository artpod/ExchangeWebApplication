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
    public class CurrencyPairsController : Controller
    {
        private readonly DBLibraryContext _context;

        public CurrencyPairsController(DBLibraryContext context)
        {
            _context = context;
        }


        public IActionResult Orders(int id)
        {
            ViewBag.Id = id;
            return RedirectToAction("Indexer", "Orders", new { id = ViewBag.Id });
        }

        // GET: CurrencyPairs
        public async Task<IActionResult> Index(int id, int name)
        {
            ViewBag.CurrencyId = id;
            ViewBag.CurrencyName = name;
            var dBLibraryContext = _context.CurrencyPairs.Include(c => c.FirstCurrencyNavigation).Include(c => c.SecondCurrencyNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }




        // GET: CurrencyPairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyPair = await _context.CurrencyPairs
                .Include(c => c.FirstCurrencyNavigation)
                .Include(c => c.SecondCurrencyNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currencyPair == null)
            {
                return NotFound();
            }

            return View(currencyPair);
        }

        // GET: CurrencyPairs/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {

            var names = _context.CurrencyPairs.Select(s => s.PairType).Distinct().Select(n => new { PairType = n }).ToList();
            ViewData["FirstCurrency"] = new SelectList(_context.Currencies, "Id", "Name");
            ViewData["SecondCurrency"] = new SelectList(_context.Currencies, "Id", "Name");
            ViewData["PairType"] = new SelectList(names, "PairType", "PairType");
            return View();
        }

        // POST: CurrencyPairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,FirstCurrency,SecondCurrency,Price,PairType")] CurrencyPair currencyPair)
        {
            if (!IsDuplicate(currencyPair)&&currencyPair.FirstCurrency!=currencyPair.SecondCurrency)
            {
                if (ModelState.IsValid)
                {
                   // currencyPair.Sum = currencyPair.FirstCurrencyNavigation.Name + currencyPair.SecondCurrencyNavigation.Name;
                    _context.Add(currencyPair);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                var names = _context.CurrencyPairs.Select(s => s.PairType).Distinct().Select(n => new { PairType = n }).ToList();

                ViewData["FirstCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.FirstCurrency);
                ViewData["SecondCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.SecondCurrency);
                ViewData["PairType"] = new SelectList(names, "PairType", "PairType", currencyPair.PairType);
                return View(currencyPair);
            }

            else  
            {
                var names = _context.CurrencyPairs.Select(s => s.PairType).Distinct().Select(n => new { PairType = n }).ToList();
                ViewData["FirstCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.FirstCurrency);
                ViewData["SecondCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.SecondCurrency);
                ViewData["PairType"] = new SelectList(names, "PairType", "PairType", currencyPair.PairType);
                ModelState.AddModelError("","Така пара вже існує або валюти однакові");
                //ModelState.AddModelError("SecondCurrencyNavigation", "Така пара уже існує");
                
            }
            return View(currencyPair);
        }

        // GET: CurrencyPairs/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyPair = await _context.CurrencyPairs.FindAsync(id);
            if (currencyPair == null)
            {
                return NotFound();
            }
            ViewData["FirstCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.FirstCurrency);
            ViewData["SecondCurrency"] = new SelectList(_context.Currencies, "Id", "Name", currencyPair.SecondCurrency);
            return View(currencyPair);
        }

        // POST: CurrencyPairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstCurrency,SecondCurrency,Price,PairType")] CurrencyPair currencyPair)
        {
            if (id != currencyPair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currencyPair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyPairExists(currencyPair.Id))
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
            ViewData["FirstCurrency"] = new SelectList(_context.Currencies, "Id", "Id", currencyPair.FirstCurrency);
            ViewData["SecondCurrency"] = new SelectList(_context.Currencies, "Id", "Id", currencyPair.SecondCurrency);
            return View(currencyPair);
        }

        // GET: CurrencyPairs/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyPair = await _context.CurrencyPairs
                .Include(c => c.FirstCurrencyNavigation)
                .Include(c => c.SecondCurrencyNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currencyPair == null)
            {
                return NotFound();
            }

            return View(currencyPair);
        }

        // POST: CurrencyPairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currencyPair = await _context.CurrencyPairs.FindAsync(id);
            _context.CurrencyPairs.Remove(currencyPair);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyPairExists(int id)
        {
            return _context.CurrencyPairs.Any(e => e.Id == id);
        }
        private bool IsDuplicate(CurrencyPair model)
        {
            var cat1 = _context.CurrencyPairs.FirstOrDefault(c => c.FirstCurrency.Equals(model.FirstCurrency)&&c.SecondCurrency.Equals(model.SecondCurrency));
          //  var cat2 = _context.CurrencyPairs.Where(c => c.FirstCurrency == c.SecondCurrency);
           // var cat2 = _context.CurrencyPairs.FirstOrDefault(c => c.SecondCurrency.Equals(model.SecondCurrency));

            return (cat1 == null) ? false : true;
        }
    }
}

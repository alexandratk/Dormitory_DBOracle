using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lb4.Models;

namespace lb4.Controllers
{
    public class ExemptionsController : Controller
    {
        private readonly ModelContext _context;

        public ExemptionsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Exemptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exemption.ToListAsync());
        }

        // GET: Exemptions/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemption = await _context.Exemption
                .FirstOrDefaultAsync(m => m.ExemptionId == id);
            if (exemption == null)
            {
                return NotFound();
            }

            return View(exemption);
        }

        // GET: Exemptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exemptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExemptionId,Description,Discount")] Exemption exemption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exemption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exemption);
        }

        // GET: Exemptions/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemption = await _context.Exemption.FindAsync(id);
            if (exemption == null)
            {
                return NotFound();
            }
            return View(exemption);
        }

        // POST: Exemptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ExemptionId,Description,Discount")] Exemption exemption)
        {
            if (id != exemption.ExemptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exemption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExemptionExists(exemption.ExemptionId))
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
            return View(exemption);
        }

        // GET: Exemptions/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemption = await _context.Exemption
                .FirstOrDefaultAsync(m => m.ExemptionId == id);
            if (exemption == null)
            {
                return NotFound();
            }

            return View(exemption);
        }

        // POST: Exemptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var exemption = await _context.Exemption.FindAsync(id);
            _context.Exemption.Remove(exemption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExemptionExists(short id)
        {
            return _context.Exemption.Any(e => e.ExemptionId == id);
        }
    }
}

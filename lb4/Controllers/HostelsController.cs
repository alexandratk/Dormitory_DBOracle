using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lb4.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace lb4.Controllers
{
    public class HostelsController : Controller
    {
        private readonly ModelContext _context;

        public HostelsController(ModelContext context)
        {
            _context = context;
        }

        public static string funcResult = "";

        // GET: Hostels
        public async Task<IActionResult> Index()
        {
            ViewBag.FunctionResult = funcResult;
            return View(await _context.Hostel.ToListAsync());
        }

        // GET: Hostels/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostel = await _context.Hostel.Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.HostelId == id);
            if (hostel == null)
            {
                return NotFound();
            }

            return View(hostel);
        }

        // GET: Hostels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hostels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HostelId,HostelNumber,University,Manager")] Hostel hostel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hostel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hostel);
        }

        // GET: Hostels/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostel = await _context.Hostel.FindAsync(id);
            if (hostel == null)
            {
                return NotFound();
            }
            return View(hostel);
        }

        // POST: Hostels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("HostelId,HostelNumber,University,Manager")] Hostel hostel)
        {
            if (id != hostel.HostelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hostel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HostelExists(hostel.HostelId))
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
            return View(hostel);
        }

        // POST: Students/Func/
        [HttpPost]
        public async Task<IActionResult> Func([Bind("PHostelId")] FunctionParametrs fp)
        {
            funcResult = "hello";

            var p_hostel_id = new OracleParameter("p_hostel_id", OracleDbType.Int64, ParameterDirection.Input);

            p_hostel_id.Value = fp.PHostelId;

            var outParamResult = new OracleParameter("OutParamResult", OracleDbType.Double, 100, 2, ParameterDirection.Output);
            outParamResult.Value = "";

            var res = _context.Database.ExecuteSqlCommand("BEGIN SELECT func(:p_hostel_id) INTO :outParamResult FROM DUAL; END;", outParamResult, p_hostel_id);
            Debug.WriteLine(outParamResult.Value);
            funcResult = outParamResult.Value.ToString();
            return RedirectToAction(nameof(Index));
        }

        // GET: Hostels/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostel = await _context.Hostel
                .FirstOrDefaultAsync(m => m.HostelId == id);
            if (hostel == null)
            {
                return NotFound();
            }

            return View(hostel);
        }

        // POST: Hostels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var hostel = await _context.Hostel.FindAsync(id);
            _context.Hostel.Remove(hostel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HostelExists(short id)
        {
            return _context.Hostel.Any(e => e.HostelId == id);
        }
    }
}

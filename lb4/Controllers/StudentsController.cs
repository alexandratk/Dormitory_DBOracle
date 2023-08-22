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
    public class StudentsController : Controller
    {
        private readonly ModelContext _context;

        public StudentsController(ModelContext context)
        {
            _context = context;
        }

        //public static string funcResult = "";

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Student.Include(s => s.Exemption).Include(s => s.Room);
            //ViewBag.FunctionResult = funcResult;
            return View(await modelContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Exemption)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["ExemptionId"] = new SelectList(_context.Exemption, "ExemptionId", "ExemptionId");
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,RoomId,ExemptionId,StudentName,DateOfBirth,Gender,PassportId,CheckInDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExemptionId"] = new SelectList(_context.Exemption, "ExemptionId", "ExemptionId", student.ExemptionId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ExemptionId"] = new SelectList(_context.Exemption, "ExemptionId", "ExemptionId", student.ExemptionId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("StudentId,RoomId,ExemptionId,StudentName,DateOfBirth,Gender,PassportId,CheckInDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["ExemptionId"] = new SelectList(_context.Exemption, "ExemptionId", "ExemptionId", student.ExemptionId);
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }


        //// POST: Students/Func/
        //[HttpPost]
        //public async Task<IActionResult> Func([Bind("PHostelId,PRoomNumber")] FunctionParametrs fp)
        //{
        //    funcResult = "hello";

        //    var p_hostel_id = new OracleParameter("p_hostel_id", OracleDbType.Int64, ParameterDirection.Input);

        //    p_hostel_id.Value = fp.PHostelId;

        //    var outParamResult = new OracleParameter("OutParamResult", OracleDbType.Double, 100, 2, ParameterDirection.Output);
        //    outParamResult.Value = "";

        //    var res = _context.Database.ExecuteSqlCommand("BEGIN SELECT func_find_students(:p_hostel_id) INTO :outParamResult FROM DUAL; END;", outParamResult, p_hostel_id);
        //    Debug.WriteLine(outParamResult.Value);
        //    funcResult = outParamResult.Value.ToString();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Exemption)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(short id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}

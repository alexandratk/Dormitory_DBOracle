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

namespace lb4.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ModelContext _context;

        public RoomsController(ModelContext context)
        {
            _context = context;
        }

        public static string exceptionMessage = "";

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Room.Include(r => r.Hostel);
            return View(await modelContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.Hostel)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["HostelId"] = new SelectList(_context.Hostel, "HostelId", "HostelId");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,HostelId,RoomNumber,Price,NumberOfBeds,Flat,Description,Type")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HostelId"] = new SelectList(_context.Hostel, "HostelId", "HostelId", room.HostelId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            ViewBag.ExceptionMessage = exceptionMessage;
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["HostelId"] = new SelectList(_context.Hostel, "HostelId", "HostelId", room.HostelId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("RoomId,HostelId,RoomNumber,Price,NumberOfBeds,Flat,Description,Type")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            var p_room_id = new OracleParameter("p_room_id", OracleDbType.Int64, ParameterDirection.Input);
            var p_price = new OracleParameter("p_price", OracleDbType.Double, ParameterDirection.Input);

            p_room_id.Value = room.RoomId;
            p_price.Value = room.Price;
            bool flag = true;

            try
            {
                var res = _context.Database.ExecuteSqlCommand("BEGIN change_price(:p_room_id, :p_price); END;", p_room_id, p_price);
            } catch(Exception e)
            {
                if (e.Message.Contains("Less than zero"))
                {
                    exceptionMessage =  "Error, price is less than zero";
                }
                if (e.Message.Contains("Big rise"))
                {
                    exceptionMessage = "Error, big rise";
                }
                if (e.Message.Contains("Big discount"))
                {
                    exceptionMessage = "Error, big discount";
                }
                return RedirectToAction(nameof(Edit));
            }
            exceptionMessage = "";
            if (ModelState.IsValid && flag)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["HostelId"] = new SelectList(_context.Hostel, "HostelId", "HostelId", room.HostelId);
            return View(room);
        }

        // POST: Rooms/Proc/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proc([Bind("MinPrice,MaxNumberOfBeds")] ProcedureParametrs pp)
        {
            Oracle.ManagedDataAccess.Client.OracleParameter room = new Oracle.ManagedDataAccess.Client.OracleParameter("room", "1");

            var min_price = new OracleParameter("min_price", OracleDbType.Int64, ParameterDirection.Input);
            var max_number_of_beds = new OracleParameter("max_number_of_beds", OracleDbType.Int64, ParameterDirection.Input);

            min_price.Value = pp.MinPrice;
            max_number_of_beds.Value = pp.MaxNumberOfBeds;

            // Oracle.ManagedDataAccess.Client.OracleParameter min_price = new Oracle.ManagedDataAccess.Client.OracleParameter("@min_price", "900");
            // Oracle.ManagedDataAccess.Client.OracleParameter max_number_of_beds = new Oracle.ManagedDataAccess.Client.OracleParameter("@max_number_of_beds", "3");
            _context.Database.ExecuteSqlCommand("BEGIN proc_change_description(:min_price, :max_number_of_beds); END;", min_price, max_number_of_beds);
            // _context.Database.ExecuteSqlCommand(@"SELECT * FROM Room WHERE room_id=:room", room);
            //_context.Database.ExecuteSqlCommand("BEGIN proc(); END;");
            //_context.Set().FromSqlRaw("dbo.SomeSproc @Id = {0}, @Name = {1}", min_price);
            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.Hostel)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(short id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }
    }
}

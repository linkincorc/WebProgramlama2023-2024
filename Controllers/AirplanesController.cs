using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneTicket.Data;
using PlaneTicket.Models;

namespace PlaneTicket.Controllers
{
    [Authorize(Policy = "AdminOrRedirect")]
    public class AirplanesController : Controller
    {
        private readonly AppDbContext _context;

        public AirplanesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Airplanes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var airplanes = _context.Airplanes.ToList();
            return View(airplanes);
        }

        // GET: Airplanes/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Airplanes == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes
                .Include(a => a.Seats) // Include Seats related to the Airplane
                .FirstOrDefaultAsync(m => m.Id == id);

            if (airplane == null)
            {
                return NotFound();
            }

            // Calculate the count of available seats
            var availableSeatsCount = airplane.Seats.Count(s => s.IsAvailable);

            // Pass this data to the view via ViewBag or a ViewModel
            ViewBag.AvailableSeatsCount = availableSeatsCount;

            return View(airplane);
        }


        // GET: Airplanes/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airplanes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SerialNumber")] Airplane airplane)
        {
            airplane.NumberOfRows = 10; // Fixed number of rows
            airplane.SeatsPerRow = 6; // Fixed seats per row

            _context.Add(airplane);
            await _context.SaveChangesAsync();

            // Generate seats for the airplane
            GenerateSeatsForAirplane(airplane);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private void GenerateSeatsForAirplane(Airplane airplane)
        {
            int totalRows = airplane.NumberOfRows;
            int seatsPerRow = airplane.SeatsPerRow;
            string[] columns = { "L", "L", "M", "M", "R", "R" }; // Column identifiers

            for (int row = 1; row <= totalRows; row++)
            {
                for (int seatNum = 1; seatNum <= seatsPerRow; seatNum++)
                {
                    string seatCode = $"{row}{columns[seatNum - 1]}{seatNum}";
                    Seat seat = new Seat
                    {
                        AirplaneId = airplane.Id,
                        SeatCode = seatCode,
                        IsAvailable = true,
                        Class = "Economy" // Default class, can be modified as needed
                    };
                    _context.Seats.Add(seat);
                }
            }
        }


        // GET: Airplanes/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // POST: Airplanes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,SerialNumber,NumberOfRows,SeatsPerRow")] Airplane airplane)
        {
            if (id != airplane.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(airplane);
            _context.Update(airplane);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Airplanes/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var airplane = await _context.Airplanes
                .Include(a => a.Seats) // Include associated seats
                .FirstOrDefaultAsync(a => a.Id == id);

            if (airplane == null)
            {
                return NotFound();
            }

            // Remove the seats associated with the airplane
            foreach (var seat in airplane.Seats)
            {
                _context.Seats.Remove(seat);
            }

            // Remove the airplane
            _context.Airplanes.Remove(airplane);

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
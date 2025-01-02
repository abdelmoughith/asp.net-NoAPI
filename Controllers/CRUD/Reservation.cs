using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication_Client.Models;
using Authentication_Client.Data;

namespace Authentication_Client.Controllers
{
    [Route("/rapi")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        private readonly JwtTokenService _jwtTokenService;

        public ReservationsController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.reservations
                .Include(r => r.Client)
                .Include(r => r.Chambre)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        // POST: api/Reservations
        [HttpPost("reserve")]
        public async Task<IActionResult> PostReservation([FromBody] ReservationRequest reservationRequest)
        {
            // Validate input
            if (reservationRequest == null || reservationRequest.DateDebut >= reservationRequest.DateFin)
            {
                return BadRequest("Invalid reservation details.");
            }

            // Check if the room is available
            var chambre = await _context.chambres.FindAsync(reservationRequest.ChambreId);
            if (chambre == null || !chambre.Disponibilite)
            {
                return BadRequest("The selected room is not available.");
            }

            // Save reservation
            var reservation = new Reservation
            {
                DateDebut = reservationRequest.DateDebut,
                DateFin = reservationRequest.DateFin,
                Etat = "Confirmed",
                ClientId = reservationRequest.ClientId,
                ChambreId = reservationRequest.ChambreId
            };

            _context.reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Generate facture
            var facture = new Facture
            {
                MontantTotal = reservation.Facture,
                DatePaiement = DateTime.Now,
                Etat = "Pending",
                ReservationId = reservation.Id
            };

            _context.factures.Add(facture);
            chambre.Disponibilite = false; // Mark room as unavailable
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.reservations.Any(e => e.Id == id);
        }
    }
}

public class ReservationRequest
{

    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public int ClientId { get; set; }
    public int ChambreId { get; set; }
}

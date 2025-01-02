using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication_Client.Models;
using Authentication_Client.Data;

namespace Authentication_Client.Controllers.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChambresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChambresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Chambres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chambre>>> GetChambres()
        {
            return await _context.chambres.ToListAsync();
        }

        // GET: api/Chambres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chambre>> GetChambre(int id)
        {
            var chambre = await _context.chambres.FindAsync(id);

            if (chambre == null)
            {
                return NotFound();
            }

            return chambre;
        }

        // POST: api/Chambres
        [HttpPost]
        public async Task<ActionResult<Chambre>> PostChambre(Chambre chambre)
        {
            if (chambre == null)
            {
                return BadRequest("Les données de la chambre sont invalides.");
            }

            try
            {
                _context.chambres.Add(chambre);
                await _context.SaveChangesAsync();

                // Retourne l'objet créé avec son id
                return CreatedAtAction(nameof(GetChambre), new { id = chambre.Id }, chambre);
            }
            catch (Exception ex)
            {
                // Capture l'exception et renvoie un message d'erreur détaillé
                return BadRequest($"Erreur lors de l'ajout de la chambre : {ex.Message}");
            }
        }


        // PUT: api/Chambres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChambre(int id, Chambre chambre)
        {
            if (id != chambre.Id)
            {
                return BadRequest();
            }

            _context.Entry(chambre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChambreExists(id))
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

        // DELETE: api/Chambres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            var chambre = await _context.chambres.FindAsync(id);
            if (chambre == null)
            {
                return NotFound();
            }

            _context.chambres.Remove(chambre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChambreExists(int id)
        {
            return _context.chambres.Any(e => e.Id == id);
        }
    }
}

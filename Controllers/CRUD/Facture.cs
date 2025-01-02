using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication_Client.Models;
using Authentication_Client.Data;

namespace Authentication_Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/factures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facture>>> GetFactures()
        {
            return await _context.factures.ToListAsync();
        }

        // GET: api/factures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facture>> GetFactures(int id)
        {
            var facture = await _context.factures.FindAsync(id);

            if (facture == null)
            {
                return NotFound();
            }

            return facture;
        }

        // POST: api/factures
        [HttpPost]
        public async Task<ActionResult<Facture>> PostFacture(Facture facture)
        {
            if (facture == null)
            {
                return BadRequest("Les données de la facture sont invalides.");
            }

            try
            {
                _context.factures.Add(facture);
                await _context.SaveChangesAsync();

                // Retourne l'objet créé avec son id
                return CreatedAtAction(nameof(GetFactures), new { id = facture.Id }, facture);
            }
            catch (Exception ex)
            {
                // Capture l'exception et renvoie un message d'erreur détaillé
                return BadRequest($"Erreur lors de l'ajout de la chambre : {ex.Message}");
            }
        }


        // PUT: api/factures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacture(int id, Facture facture)
        {
            if (id != facture.Id)
            {
                return BadRequest();
            }

            _context.Entry(facture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureExists(id))
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

        // DELETE: api/factures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacture(int id)
        {
            var facture = await _context.factures.FindAsync(id);
            if (facture == null)
            {
                return NotFound();
            }

            _context.factures.Remove(facture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FactureExists(int id)
        {
            return _context.factures.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Models;

namespace TrainingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public TraineesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Trainees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainee>>> Gettrainees()
        {
            return await _context.trainees.ToListAsync();
        }

        // GET: api/Trainees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainee>> GetTrainee(int id)
        {
            var trainee = await _context.trainees.FindAsync(id);

            if (trainee == null)
            {
                return NotFound();
            }

            return trainee;
        }

        // PUT: api/Trainees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainee(int id, Trainee trainee)
        {
            if (id != trainee.TraineeID)
            {
                return BadRequest();
            }

            _context.Entry(trainee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraineeExists(id))
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

        // POST: api/Trainees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trainee>> PostTrainee(Trainee trainee)
        {
            _context.trainees.Add(trainee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainee", new { id = trainee.TraineeID }, trainee);
        }

        // DELETE: api/Trainees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainee(int id)
        {
            var trainee = await _context.trainees.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }

            _context.trainees.Remove(trainee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TraineeExists(int id)
        {
            return _context.trainees.Any(e => e.TraineeID == id);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceMissionsTracker.Core.DTOs;
using SpaceMissionsTracker.Core.Entities;
using SpaceMissionsTracker.Infrastructure.DatabaseContext;

namespace SpaceMissionsTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AstronautsController : ControllerBase
    {
        private readonly SpaceMissionsTrackerDbContext _context;
        public AstronautsController(SpaceMissionsTrackerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all astronauts from the database.
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAllAstronauts()
        {
            var astronauts = await _context.Astronauts.ToListAsync();
            return Ok(astronauts);
        }

        /// <summary>
        /// Creates a new astronaut.
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> AddAstronaut(AddAstronautRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            var astronaut = new Astronaut
            {
                Name = request.Name,
                Nationality = request.Nationality,
                BirthYear = request.BirthYear
            };

            _context.Astronauts.Add(astronaut);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAstronautById), new { id = astronaut.Id }, astronaut);
        }

        /// <summary>
        /// Retrieves a single astronaut by its unique ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAstronautById(int id)
        {
            var astronaut = await _context.Astronauts.FindAsync(id);
            if (astronaut == null)
                return NotFound();
            return Ok(astronaut);
        }

        /// <summary>
        /// Deletes an astronaut by its ID.
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAstronaut(int id)
        {
            var astronaut = await _context.Astronauts.FindAsync(id);
            if (astronaut == null)
            {
                return NotFound($"Astronaut with ID {id} not found.");
            }
            _context.Astronauts.Remove(astronaut);
            await _context.SaveChangesAsync();


            return NoContent();
        }

        /// <summary>
        /// Fully updates an existing astronaut.
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAstronaut(int id, AddAstronautRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAstronaut = await _context.Astronauts.FindAsync(id);
            if (existingAstronaut == null)
                return NotFound($"Astronaut with ID {id} not found.");

            existingAstronaut.Name = request.Name;
            existingAstronaut.Nationality = request.Nationality;
            existingAstronaut.BirthYear = request.BirthYear;

            await _context.SaveChangesAsync();

            return Ok(existingAstronaut);
        }
    }
}
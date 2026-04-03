using Microsoft.AspNetCore.Authorization;
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
    public class RocketsController : ControllerBase
    {
        private readonly SpaceMissionsTrackerDbContext _context;

        public RocketsController(SpaceMissionsTrackerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all rockets with their associated agency.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRockets()
        {
            var rockets = await _context.Rockets
                .Include(r => r.Agency)
                .ToListAsync();

            return Ok(rockets);
        }

        /// <summary>
        /// Gets a single rocket by ID, including its agency.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRocketById(int id)
        {
            var rocket = await _context.Rockets
                .Include(r => r.Agency)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rocket == null)
                return NotFound($"Rocket with ID {id} not found.");

            return Ok(rocket);
        }

        /// <summary>
        /// Creates a new rocket.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddRocket([FromBody] AddRocketRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var agencyExists = await _context.Agencies.AnyAsync(a => a.Id == request.AgencyId);
            if (!agencyExists)
                return BadRequest($"Agency with ID {request.AgencyId} does not exist.");

            var rocket = new Rocket
            {
                Name = request.Name,
                AgencyId = request.AgencyId,
                FirstLaunch = request.FirstLaunch
            };

            _context.Rockets.Add(rocket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRocketById), new { id = rocket.Id }, rocket);
        }

        /// <summary>
        /// Updates an existing rocket.
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRocket(int id, [FromBody] AddRocketRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRocket = await _context.Rockets.FindAsync(id);
            if (existingRocket == null)
                return NotFound($"Rocket with ID {id} not found.");

            if (existingRocket.AgencyId != request.AgencyId)
            {
                var agencyExists = await _context.Agencies.AnyAsync(a => a.Id == request.AgencyId);
                if (!agencyExists)
                    return BadRequest($"Agency with ID {request.AgencyId} does not exist.");
            }

            existingRocket.Name = request.Name;
            existingRocket.AgencyId = request.AgencyId;
            existingRocket.FirstLaunch = request.FirstLaunch;

            await _context.SaveChangesAsync();

            return Ok(existingRocket);
        }

        /// <summary>
        /// Deletes a rocket by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRocket(int id)
        {
            var rocket = await _context.Rockets.FindAsync(id);
            if (rocket == null)
                return NotFound($"Rocket with ID {id} not found.");

            _context.Rockets.Remove(rocket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
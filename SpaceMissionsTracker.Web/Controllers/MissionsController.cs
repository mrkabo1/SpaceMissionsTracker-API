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
    public class MissionsController : ControllerBase
    {
        private readonly SpaceMissionsTrackerDbContext _context;

        public MissionsController(SpaceMissionsTrackerDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves all missions with their associated rocket.
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAllMissions()
        {
            var missions = await _context.Missions
                .Include(r => r.Rocket)
                .ToListAsync();

            return Ok(missions);
        }

        /// <summary>
        /// Retrieves a single mission by its unique ID, including the related rocket.
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMissionById(int id)
        {
            var mission = await _context.Missions
                .Include(r => r.Rocket)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (mission == null)
                return NotFound($"Mission with ID {id} not found.");

            return Ok(mission);
        }

        /// <summary>
        /// Creates a new mission.
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> AddMission([FromBody] AddMissionRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rocketExists = await _context.Rockets.AnyAsync(a => a.Id == request.RocketId);
            if (!rocketExists)
                return BadRequest($"Rocket with ID {request.RocketId} does not exist.");

            var mission = new Mission
            {
                Name = request.Name,
                RocketId = request.RocketId,
                LaunchDate = request.LaunchDate,
                Status = request.Status,
                Destination = request.Destination
            };

            _context.Missions.Add(mission);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMissionById), new { id = mission.Id }, mission);
        }

        /// <summary>
        /// Fully updates an existing mission.
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMission(int id, [FromBody] AddMissionRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMission = await _context.Missions.FindAsync(id);
            if (existingMission == null)
                return NotFound($"Mission with ID {id} not found.");

            if (existingMission.RocketId != request.RocketId)
            {
                var rocketExists = await _context.Rockets.AnyAsync(a => a.Id == request.RocketId);
                if (!rocketExists)
                    return BadRequest($"Rocket with ID {request.RocketId} does not exist.");
            }

            existingMission.Name = request.Name;
            existingMission.RocketId = request.RocketId;
            existingMission.LaunchDate = request.LaunchDate;
            existingMission.Status = request.Status;
            existingMission.Destination = request.Destination;

            await _context.SaveChangesAsync();

            return Ok(existingMission);
        }

        /// <summary>
        /// Deletes a mission by its ID.
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMission(int id)
        {
            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
                return NotFound($"Mission with ID {id} not found.");

            _context.Missions.Remove(mission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //----------------------------------
    }
}

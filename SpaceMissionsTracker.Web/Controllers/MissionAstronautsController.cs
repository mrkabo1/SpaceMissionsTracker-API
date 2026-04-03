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
    public class MissionAstronautsController : ControllerBase
    {
        private readonly SpaceMissionsTrackerDbContext _context;

        public MissionAstronautsController(SpaceMissionsTrackerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all mission-astronaut relationships.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMissionAstronauts()
        {
            var relationships = await _context.MissionAstronauts
                .Select(ma => new { ma.MissionId, ma.AstronautId })
                .ToListAsync();

            return Ok(relationships);
        }

        /// <summary>
        /// Gets a specific mission-astronaut relationship by composite key.
        /// </summary>
        [HttpGet("{missionId}/{astronautId}")]
        public async Task<IActionResult> GetMissionAstronautById(int missionId, int astronautId)
        {
            var relationship = await _context.MissionAstronauts
                .FirstOrDefaultAsync(ma => ma.MissionId == missionId && ma.AstronautId == astronautId);

            if (relationship == null)
                return NotFound($"Relationship with MissionId={missionId} and AstronautId={astronautId} not found.");

            return Ok(new { relationship.MissionId, relationship.AstronautId });
        }

        /// <summary>
        /// Creates a new mission-astronaut relationship.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddMissionAstronaut([FromBody] AddMissionAstronautRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var missionExists = await _context.Missions.AnyAsync(m => m.Id == request.MissionId);
            if (!missionExists)
                return BadRequest($"Mission with ID {request.MissionId} does not exist.");

            var astronautExists = await _context.Astronauts.AnyAsync(a => a.Id == request.AstronautId);
            if (!astronautExists)
                return BadRequest($"Astronaut with ID {request.AstronautId} does not exist.");


            var alreadyExists = await _context.MissionAstronauts
                .AnyAsync(ma => ma.MissionId == request.MissionId && ma.AstronautId == request.AstronautId);
            if (alreadyExists)
                return Conflict("This relationship already exists.");

            var newRelationship = new MissionAstronaut
            {
                MissionId = request.MissionId,
                AstronautId = request.AstronautId
            };

            _context.MissionAstronauts.Add(newRelationship);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMissionAstronautById),
                new { missionId = newRelationship.MissionId, astronautId = newRelationship.AstronautId },
                new { newRelationship.MissionId, newRelationship.AstronautId });
        }

        /// <summary>
        /// Deletes a mission-astronaut relationship.
        /// </summary>
        [HttpDelete("{missionId}/{astronautId}")]
        public async Task<IActionResult> DeleteMissionAstronaut(int missionId, int astronautId)
        {
            var relationship = await _context.MissionAstronauts
                .FirstOrDefaultAsync(ma => ma.MissionId == missionId && ma.AstronautId == astronautId);

            if (relationship == null)
                return NotFound($"Relationship with MissionId={missionId} and AstronautId={astronautId} not found.");

            _context.MissionAstronauts.Remove(relationship);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceMissionsTracker.Core.DTOs;
using SpaceMissionsTracker.Core.Entities;
using SpaceMissionsTracker.Infrastructure.DatabaseContext;

namespace SpaceMissionsTracker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgenciesController : ControllerBase
    {
        private readonly SpaceMissionsTrackerDbContext _context;
        public AgenciesController(SpaceMissionsTrackerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all agencies from the database.
        /// </summary>

        [HttpGet]

        public async Task<IActionResult> GetAllAgencies()
        {
            var agencies = await _context.Agencies.ToListAsync();
            return Ok(agencies);
        }
        /// <summary>
        /// Creates a new agency.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAgency(AddAgencyRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            var agency = new Agency
            {
                Name = request.Name,
                Country = request.Country,
                Founded = request.Founded
            };

            _context.Agencies.Add(agency);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgencyById), new { id = agency.Id }, agency);
        }

        /// <summary>
        /// Retrieves a single agency by its unique ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgencyById(int id)
        {
            var agency = await _context.Agencies.FindAsync(id);
            if (agency == null)
                return NotFound();
            return Ok(agency);
        }

        /// <summary>
        /// Deletes an agency by its ID.
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgency(int id)
        {
            var agency = await _context.Agencies.FindAsync(id);
            if (agency == null)
            {
                return NotFound($"Agency with ID {id} not found.");
            }
            _context.Agencies.Remove(agency);
            await _context.SaveChangesAsync();

            
            return NoContent();
        }

        /// <summary>
        /// Fully updates an existing agency.
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgency(int id, AddAgencyRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAgency = await _context.Agencies.FindAsync(id);
            if (existingAgency == null)
                return NotFound($"Agency with ID {id} not found.");

            existingAgency.Name = request.Name;
            existingAgency.Country = request.Country;
            existingAgency.Founded = request.Founded;

            await _context.SaveChangesAsync();

            return Ok(existingAgency);
        }
    }
}

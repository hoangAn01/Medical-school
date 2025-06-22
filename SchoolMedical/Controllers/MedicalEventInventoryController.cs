using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalEventInventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalEventInventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalEventInventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalEventInventoryDto>>> GetMedicalEventInventories()
        {
            var eventInventories = await _context.MedicalEventInventory
                .Join(_context.MedicalInventory,
                      e => e.ItemID,
                      i => i.ItemID,
                      (e, i) => new MedicalEventInventoryDto
                      {
                          EventInventoryID = e.EventInventoryID,
                          EventID = e.EventID,
                          ItemID = e.ItemID,
                          ItemName = i.ItemName,
                          QuantityUsed = e.QuantityUsed,
                          UsedTime = e.UsedTime
                      })
                .ToListAsync();

            return Ok(eventInventories);
        }

        // GET: api/MedicalEventInventory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalEventInventoryDto>> GetMedicalEventInventory(int id)
        {
            var eventInventory = await _context.MedicalEventInventory
                .Where(e => e.EventInventoryID == id)
                .Join(_context.MedicalInventory,
                      e => e.ItemID,
                      i => i.ItemID,
                      (e, i) => new MedicalEventInventoryDto
                      {
                          EventInventoryID = e.EventInventoryID,
                          EventID = e.EventID,
                          ItemID = e.ItemID,
                          ItemName = i.ItemName,
                          QuantityUsed = e.QuantityUsed,
                          UsedTime = e.UsedTime
                      })
                .FirstOrDefaultAsync();

            if (eventInventory == null)
            {
                return NotFound();
            }

            return Ok(eventInventory);
        }

        // GET: api/MedicalEventInventory/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<MedicalEventInventoryDto>>> GetMedicalEventInventoriesByEventId(int eventId)
        {
            var eventInventories = await _context.MedicalEventInventory
                .Where(e => e.EventID == eventId)
                .Join(_context.MedicalInventory,
                      e => e.ItemID,
                      i => i.ItemID,
                      (e, i) => new MedicalEventInventoryDto
                      {
                          EventInventoryID = e.EventInventoryID,
                          EventID = e.EventID,
                          ItemID = e.ItemID,
                          ItemName = i.ItemName,
                          QuantityUsed = e.QuantityUsed,
                          UsedTime = e.UsedTime
                      })
                .ToListAsync();

            return Ok(eventInventories);
        }

        // POST: api/MedicalEventInventory
        [HttpPost]
        public async Task<ActionResult<MedicalEventInventoryDto>> CreateMedicalEventInventory(CreateMedicalEventInventoryDto createDto)
        {
            var medicalEventInventory = new MedicalEventInventory
            {
                EventID = createDto.EventID,
                ItemID = createDto.ItemID,
                QuantityUsed = createDto.QuantityUsed,
                UsedTime = DateTime.UtcNow
            };

            _context.MedicalEventInventory.Add(medicalEventInventory);
            await _context.SaveChangesAsync();

            var resultDto = new MedicalEventInventoryDto
            {
                EventInventoryID = medicalEventInventory.EventInventoryID,
                EventID = medicalEventInventory.EventID,
                ItemID = medicalEventInventory.ItemID,
                QuantityUsed = medicalEventInventory.QuantityUsed,
                UsedTime = medicalEventInventory.UsedTime
            };

            return CreatedAtAction(nameof(GetMedicalEventInventory), new { id = medicalEventInventory.EventInventoryID }, resultDto);
        }

        // PUT: api/MedicalEventInventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalEventInventory(int id, UpdateMedicalEventInventoryDto updateDto)
        {
            var eventInventory = await _context.MedicalEventInventory.FindAsync(id);

            if (eventInventory == null)
            {
                return NotFound();
            }

            eventInventory.EventID = updateDto.EventID;
            eventInventory.ItemID = updateDto.ItemID;
            eventInventory.QuantityUsed = updateDto.QuantityUsed;
            eventInventory.UsedTime = updateDto.UsedTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalEventInventoryExists(id))
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

        // DELETE: api/MedicalEventInventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalEventInventory(int id)
        {
            var eventInventory = await _context.MedicalEventInventory.FindAsync(id);
            if (eventInventory == null)
            {
                return NotFound();
            }

            _context.MedicalEventInventory.Remove(eventInventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalEventInventoryExists(int id)
        {
            return _context.MedicalEventInventory.Any(e => e.EventInventoryID == id);
        }
    }
} 
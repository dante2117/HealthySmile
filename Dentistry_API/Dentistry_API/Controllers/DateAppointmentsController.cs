using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dentistry_API.Models;

namespace Dentistry_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateAppointmentsController : ControllerBase
    {
        private readonly DentistryContext _context;

        public DateAppointmentsController(DentistryContext context)
        {
            _context = context;
        }

        // GET: api/DateAppointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DateAppointment>>> GetDateAppointments()
        {
          if (_context.DateAppointments == null)
          {
              return NotFound();
          }
            return await _context.DateAppointments.ToListAsync();
        }

        // GET: api/DateAppointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DateAppointment>> GetDateAppointment(int? id)
        {
          if (_context.DateAppointments == null)
          {
              return NotFound();
          }
            var dateAppointment = await _context.DateAppointments.FindAsync(id);

            if (dateAppointment == null)
            {
                return NotFound();
            }

            return dateAppointment;
        }

        // PUT: api/DateAppointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDateAppointment(int? id, DateAppointment dateAppointment)
        {
            if (id != dateAppointment.IdDateAppointment)
            {
                return BadRequest();
            }

            _context.Entry(dateAppointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DateAppointmentExists(id))
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

        // POST: api/DateAppointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DateAppointment>> PostDateAppointment(DateAppointment dateAppointment)
        {
          if (_context.DateAppointments == null)
          {
              return Problem("Entity set 'DentistryContext.DateAppointments'  is null.");
          }
            _context.DateAppointments.Add(dateAppointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDateAppointment", new { id = dateAppointment.IdDateAppointment }, dateAppointment);
        }

        // DELETE: api/DateAppointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDateAppointment(int? id)
        {
            if (_context.DateAppointments == null)
            {
                return NotFound();
            }
            var dateAppointment = await _context.DateAppointments.FindAsync(id);
            if (dateAppointment == null)
            {
                return NotFound();
            }

            _context.DateAppointments.Remove(dateAppointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DateAppointmentExists(int? id)
        {
            return (_context.DateAppointments?.Any(e => e.IdDateAppointment == id)).GetValueOrDefault();
        }
    }
}

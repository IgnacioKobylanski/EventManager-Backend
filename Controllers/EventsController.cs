using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManager.Data;
using EventManager.Models;
using EventManager.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly EventContext _context;

        public EventsController(EventContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.User)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    ImageUrl = e.ImageUrl,
                    Capacity = e.Capacity,
                    User = new UserDto
                    {
                        Id = e.User.Id,
                        Username = e.User.Username,
                        Email = e.User.Email
                    }
                })
                .ToListAsync();

            return events;
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var ev = await _context.Events
                .Include(e => e.User)
                .Where(e => e.Id == id)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    ImageUrl = e.ImageUrl,
                    Capacity = e.Capacity,
                    User = new UserDto
                    {
                        Id = e.User.Id,
                        Username = e.User.Username,
                        Email = e.User.Email
                    }
                })
                .FirstOrDefaultAsync();

            if (ev == null)
                return NotFound();

            return ev;
        }

        // GET: api/events/user/3
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByUser(int userId)
        {
            var events = await _context.Events
                .Include(e => e.User)
                .Where(e => e.UserId == userId)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location,
                    ImageUrl = e.ImageUrl,
                    Capacity = e.Capacity,
                    User = new UserDto
                    {
                        Id = e.User.Id,
                        Username = e.User.Username,
                        Email = e.User.Email
                    }
                })
                .ToListAsync();

            return events;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ev);
        }

        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event ev)
        {
            if (id != ev.Id)
                return BadRequest();

            _context.Entry(ev).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

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
        public async Task<ActionResult<Event>> CreateEvent(EventCreateDto eventDto)
        {
            var user = await _context.Users.FindAsync(eventDto.UserId);
            if (user == null)
            {
                return BadRequest("El usuario especificado no existe.");
            }

            // Crea una instancia del modelo Event y asigna los valores del DTO
            var newEvent = new Event
            {
                Name = eventDto.Name,
                Description = eventDto.Description,
                Date = eventDto.Date,
                Location = eventDto.Location,
                ImageUrl = eventDto.ImageUrl,
                Capacity = eventDto.Capacity,
                UserId = eventDto.UserId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
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

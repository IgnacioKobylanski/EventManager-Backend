using System;

namespace EventManager.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date {  get; set; }
        public string Location { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int? Capacity { get; set; }
    }
}

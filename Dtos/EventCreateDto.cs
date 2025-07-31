using System;
using System.ComponentModel.DataAnnotations;


namespace EventManager.Dtos
{
    public class EventCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Location { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int? Capacity { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace EventManager.Models



{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}

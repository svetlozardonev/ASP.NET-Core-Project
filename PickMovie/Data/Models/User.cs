namespace PickMovie.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        public string Avatar { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        [MaxLength(6)]
        public string Gender { get; set; }

        public DateTime RegisteredOn { get; init; } = DateTime.UtcNow;

        [MaxLength(300)]
        public string AboutMe { get; set; }

        public bool IsCritic { get; set; } = false;

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();
        public ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();
    }
}

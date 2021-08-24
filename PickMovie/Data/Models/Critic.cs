namespace PickMovie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Critic
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Review> Reviews { get; init; } = new List<Review>();
    }
}

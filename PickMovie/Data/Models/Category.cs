using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PickMovie.Data.Models
{
    public class Category
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
    }
}

namespace TestProject.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        public ICollection<Movie> Movies { get; init; } = new List<Movie>();
    }
}

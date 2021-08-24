using System.ComponentModel.DataAnnotations;

namespace PickMovie.Models.Reviews
{
    public class AddReviewFormModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Add Your Review")]
        public string Content { get; set; }
    }
}

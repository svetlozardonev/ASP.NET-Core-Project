namespace PickMovie.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    public class UserViewModel
    {
        public string Id { get; init; }
        public string Username { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Phone Number")]

        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Display(Name = "Profile Picture")]
        public string Avatar { get; set; }
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
    }
}

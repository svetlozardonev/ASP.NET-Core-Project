namespace PickMovie.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    public class UserRegisterFormModel
    {
        //TO DO: more validation

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password doesn't match!")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }


    }
}

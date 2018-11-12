using System.ComponentModel.DataAnnotations;

namespace Sitecore.TechnicalChallenge.WebClient.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
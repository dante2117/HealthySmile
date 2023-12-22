using System.ComponentModel.DataAnnotations;

namespace Dentistry_API.Models
{
    public class LoginCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

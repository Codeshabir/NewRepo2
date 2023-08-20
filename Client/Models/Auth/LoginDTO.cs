using System.ComponentModel.DataAnnotations;

namespace Client.Models.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }
    }
}

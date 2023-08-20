using System.ComponentModel.DataAnnotations;

namespace Client.Models.Auth
{

        public class RegisterDTOS
        {
            [Required(ErrorMessage = "User Name is Required")]
            public string? Username { get; set; }

            [EmailAddress]
            public string? Email { get; set; }

            [Required(ErrorMessage = "User Password is Required")]
            public string? Password { get; set; }
            [Required(ErrorMessage = "Password not matched")]
            public string? ConfirmPassword { get; set; }

        }

    
}

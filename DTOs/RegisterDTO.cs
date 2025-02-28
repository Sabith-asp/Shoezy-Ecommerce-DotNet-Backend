using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phoneno { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

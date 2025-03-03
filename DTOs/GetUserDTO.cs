using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; } = "user";
    }
}

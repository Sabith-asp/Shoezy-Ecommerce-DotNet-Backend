using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class User
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string UserName { get; set; }

        public bool IsBlocked { get; set; } = false;
        [Required]
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "user";
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        public List<Wishlist> WishLists { get; set; }

        public Cart cart { get; set; }

        public List<Order> Orders { get; set; }
    }
}

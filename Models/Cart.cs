using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User users { get; set; }
        public List<CartItem> cartItem { get; set; }
    }
}

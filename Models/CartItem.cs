using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        [Required]
        public Guid CartId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public Product product { get; set; }
        public Cart cart { get; set; }

    }
}

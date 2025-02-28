using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class Wishlist
    {
        [Required]
        public Guid WishListId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User user { get; set; }
        public Product products { get; set; }
    }
}



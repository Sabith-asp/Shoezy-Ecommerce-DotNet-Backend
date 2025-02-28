using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class WishlistAddRemoveDTO
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int userId { get; set; }
    }
}

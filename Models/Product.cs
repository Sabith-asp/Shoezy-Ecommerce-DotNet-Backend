using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Range(0, 100)]

        public int Discount { get; set; } = 0;
        [Required]
        public int Quantity { get; set; }

        public bool? isDeleted { get; set; }=false;

        public Wishlist Whishlist { get; set; }

        public Category Category { get; set; }

       
    }
}

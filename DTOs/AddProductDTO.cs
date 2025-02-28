using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class AddProductDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile Image { get; set; }
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
    }
}

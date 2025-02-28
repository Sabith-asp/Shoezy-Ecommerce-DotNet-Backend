using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class WishlistGetDTO
    {
        public Guid WishListId { get; set; }
        public Guid Id { get; set; }

        public string Title { get; set; }
        
        public string Image { get; set; }
        
        public int Price { get; set; }
        
        public string Brand { get; set; }
        
        public string Model { get; set; }
        
        public string Color { get; set; }
        
    }
}

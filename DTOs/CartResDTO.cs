namespace Shoezy.DTOs
{
    public class CartResDTO
    {
        public List<CartViewDTO> cartitems { get; set; }
        public int totalItem { get; set; }
        public decimal totalPrice { get; set; }
    }
}

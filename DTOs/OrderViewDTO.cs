namespace Shoezy.DTOs
{
    public class OrderViewDTO
    {
        public Guid ProductId { get; set; }
        public Guid OrderItemId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }
}

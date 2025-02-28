namespace Shoezy.DTOs
{
    public class ViewUserOrderDetailDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid OrderId { get; set; }
        public int TotalPrice { get; set; }
        public string TransactionId { get; set; }
        public List<OrderViewDTO> OrderProducts { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }

        public Guid? ProductId { get; set; }

        public int Quantity { get; set; }

        public int TotalPrice { get; set; }
        public Product? Product { get; set; }
        public Order Order { get; set; }
    }
}

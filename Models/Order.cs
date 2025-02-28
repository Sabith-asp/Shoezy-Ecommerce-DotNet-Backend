using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        public int TotalPrice { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string TransactionId { get; set; }
        public List<OrderItem> OrderItems { get; set; } 

        public User User { get; set; }
        public Address Address { get; set; }
        

    }
}
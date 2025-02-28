using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        public int Totalamount { get; set; }
        //public string OrderString { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }
}

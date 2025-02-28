using System.ComponentModel.DataAnnotations;

namespace Shoezy.Models
{
    public class Address
    {
        public Guid AddressId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string HouseName { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public string Pincode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }

        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class AddressViewDTO
    {
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
    }
}

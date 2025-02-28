using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class RefreshDTO
    {
        [Required]
        public string refreshtoken { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace Shoezy.DTOs
{
    public class ProductGetDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Category { get; set; }
        [Range(0, 100)]

        public int Discount { get; set; } = 0;
    }
}

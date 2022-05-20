using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWaste.Models
{
    public class ProductAllergen
    {
        [Required]
        public Guid AllergenId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}

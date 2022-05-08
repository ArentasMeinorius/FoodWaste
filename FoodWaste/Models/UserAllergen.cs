using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWaste.Models
{
    public class UserAllergen
    {
        [Required]
        public Guid AllergenId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

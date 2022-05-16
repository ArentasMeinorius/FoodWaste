using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWaste.Models
{
    public class UserSelectedAllergens
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Guid AllergenId { get; set; }
        public string Name { get; set; }
    }
}

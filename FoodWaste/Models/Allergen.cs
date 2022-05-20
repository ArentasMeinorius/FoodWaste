using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWaste.Models
{
    public class Allergen
    {
        [Key]
        public Guid AllergenId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }
}

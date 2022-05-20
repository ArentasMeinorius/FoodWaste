using System;
using System.ComponentModel.DataAnnotations;

namespace FoodWaste.Models
{
    public class UserSelectedAllergens
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public Guid AllergenId { get; set; }
        public string Name { get; set; }
    }
}

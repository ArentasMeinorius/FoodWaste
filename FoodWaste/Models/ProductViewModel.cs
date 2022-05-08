using System.Collections.Generic;

namespace FoodWaste.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public Restaurant Restaurant { get; set; }
        public List<UserAllergen> Allergens { get; set; }
    }
}

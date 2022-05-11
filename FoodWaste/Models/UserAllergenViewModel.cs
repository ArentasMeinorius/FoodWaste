
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FoodWaste.Models
{
    public class UserAllergenViewModel
    {
        [BindProperty]
        public IEnumerable<Guid> Item { get; set; }
        public List<Allergen> Allergens { get; set; }
        public List<Allergen> UserAllergens { get; set; }
    }
}

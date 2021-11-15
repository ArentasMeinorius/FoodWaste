using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public Restaurant()
        {
        }
    }
}

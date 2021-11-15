using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.Models
{
    public class Product
    {
        public enum ProductState
        {
            Expired,
            Taken,
            Reserved,
            Listed
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ProductState State { get; set; }
        public int Restaurant_id { get; set; }
        public int? User_id { get; set; }

        public Product()
        {
        }
    }
}

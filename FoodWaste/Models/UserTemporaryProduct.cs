using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.Models
{
    public class UserTemporaryProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ProductState State { get; set; }
    }
}

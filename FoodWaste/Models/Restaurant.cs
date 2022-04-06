using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FoodWaste.Data.ApplicationDbContext;

namespace FoodWaste.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "ApplicationUser")]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}

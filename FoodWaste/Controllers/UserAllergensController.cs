using FoodWaste.Data;
using FoodWaste.Models;
using FoodWaste.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.Controllers
{
    public class UserAllergensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _userService;

        public UserAllergensController(ApplicationDbContext context,
            IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpGet]
        [Route("/UserAllergens/")]
        public async Task<IActionResult> Index()
        {
            var userAllergens = new UserAllergenViewModel
            {
                UserAllergens = GetUserAllergens().ToList(),
                Allergens = GetAllergens().ToList(),
                Item = new List<Guid> { Guid.NewGuid(), Guid.Empty }
            };

            ViewBag.Allergenss = new MultiSelectList(GetAllergens(), "AllergenId", "Name");

            return View(userAllergens);
        }

        public IEnumerable<Allergen> GetUserAllergens()
        {
            var userId = _userService.GetCurrentUserId(User);
            var userAllergens = from a in _context.UserAllergens
                                where a.UserId == userId
                                join al in _context.Allergens on a.AllergenId equals al.AllergenId into d
                                from al in d.DefaultIfEmpty()
                                select al;

            return userAllergens.ToList();
        }

        public IEnumerable<Allergen> GetAllergens()
        {
            var allergens = _context.Allergens.ToList();
            return allergens;
        }

        [HttpGet]// change to HttpDelete
        [Route("/UserAllergens/Remove/{allergenId}")]
        public async Task<IActionResult> RemoveAllergen(Guid allergenId)
        {
            var userId = _userService.GetCurrentUserId(User);
            var userAllergen = _context.UserAllergens.FirstOrDefault(p => p.AllergenId == allergenId && p.UserId == userId);
            if (userAllergen == null)
            {
                return NotFound();
            }

            _context.UserAllergens.Remove(userAllergen);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("/UserAllergens/Save")]
        public async Task<IActionResult> SaveAllergens([Bind("Item")] UserAllergenViewModel view)
        {
            var userId = _userService.GetCurrentUserId(User);
            foreach (var allergen in view.Item)
            {
                var userAllergen = _context.UserAllergens.FirstOrDefault(p => p.AllergenId == allergen && p.UserId == userId);
                if (userAllergen == null)
                {
                    _context.UserAllergens.Add(new UserAllergen { UserId = userId, AllergenId = allergen });
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

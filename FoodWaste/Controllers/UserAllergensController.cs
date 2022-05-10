using FoodWaste.Data;
using FoodWaste.Models;
using FoodWaste.Services;
using Microsoft.AspNetCore.Mvc;
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
            ViewData["Allergens"] = GetAllergens();
            ViewData["UserAllergens"] = GetUserAllergens();
            return View();
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

        public IEnumerable<Allergen>GetAllergens()
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

        [HttpGet]// TODO change to POST and implement as required
        [Route("/UserAllergens/{allergenId}")]
        public async Task<IActionResult> SaveAllergens(Guid allergenId)// todo this one
        {
            var userId = _userService.GetCurrentUserId(User);
            var userAllergen = await _context.UserAllergens.FindAsync(userId);
            _context.UserAllergens.Remove(userAllergen);
            await _context.SaveChangesAsync();

            return View(userAllergen);
        }

    }
}

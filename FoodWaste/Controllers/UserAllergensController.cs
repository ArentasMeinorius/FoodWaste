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
        public IActionResult Index()
        {
            var userAls = GetUserAllergens().ToList();
            var als = GetAllergens(userAls).ToList();
            var userAllergens = new UserAllergenViewModel
            {
                UserAllergens = userAls,
                Allergens = als,
            };

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

            return userAllergens.ToList().OrderBy(p=>p.Name);
        }

        public IEnumerable<Allergen> GetAllergens(IEnumerable<Allergen> userAllergens)
        {
            var allergens = _context.Allergens.ToList().Except(userAllergens).OrderBy(p=>p.Name);
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

        [HttpGet]
        [Route("/UserAllergens/Add/{allergenId}")]
        public async Task<IActionResult> AddAllergens(Guid allergenId)
        {
            var userId = _userService.GetCurrentUserId(User);

            var allergen = _context.Allergens.FirstOrDefault(p => p.AllergenId == allergenId);
            if (allergen == null)
            {
                return NotFound();
            }

            _context.UserAllergens.Add(new UserAllergen { UserId = userId, AllergenId = allergenId });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

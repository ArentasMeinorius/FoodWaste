using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodWaste.Data;
using FoodWaste.Models;
using FoodWaste.Services;

namespace FoodWaste.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string SearchString = "";
        private IUserService _userService;

        public RestaurantsController(ApplicationDbContext context,
            IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Restaurants
        public IActionResult Index(string sortOrder, string searchString, bool clearFilter)
        {
            Func<string, string, string> getSortOrder = (x, orderby) => ((x == orderby) ? (orderby + "_desc") : orderby);

            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["AddressSortParm"] = getSortOrder(sortOrder, "Address");
            ViewData["NumberSortParm"] = getSortOrder(sortOrder, "Number");
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentUserId"] = _userService.GetCurrentUserId(User);

            var restaurants = from r in _context.Restaurant
                              select r;

            if (!string.IsNullOrEmpty(searchString))
                SearchString = searchString;

            if (clearFilter)
                SearchString = "";

            restaurants = restaurants.Where(s => s.Name.Contains(SearchString));

            restaurants = sortOrder switch
            {
                "Name_desc" => restaurants.OrderByDescending(r => r.Name),
                "Address" => restaurants.OrderBy(r => r.Address),
                "Address_desc" => restaurants.OrderByDescending(r => r.Address),
                "Number" => restaurants.OrderBy(r => r.PhoneNumber),
                "Number_desc" => restaurants.OrderByDescending(r => r.PhoneNumber),
                _ => restaurants.OrderBy(p => p.Name),
            };

            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(m => m.UserId == id);
            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Address,PhoneNumber")] Restaurant restaurant)//nepriimt userid
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(p => p.Id == id);
            if (restaurant == null || restaurant.UserId != _userService.GetCurrentUserId(User))
                return NotFound();

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,Name,Address,PhoneNumber,Id")] Restaurant restaurant)//nepriimt userid
        {
            if (restaurant.UserId != _userService.GetCurrentUserId(User))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(m => m.UserId == id);
            // user id and delete? should fix on the next iteration
            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int? id)
        {
            return _context.Restaurant.Any(e => e.UserId == id);
        }
    }
}

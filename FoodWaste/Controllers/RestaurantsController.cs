using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodWaste.Data;
using FoodWaste.Models;

namespace FoodWaste.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Restaurant.ToListAsync());
            return View(await DataBaseOperations.GetRestaurant());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var restaurant = await _context.Restaurant
            //    .FirstOrDefaultAsync(m => m.User_Id == id);
            var result = await DataBaseOperations.GetRestaurant();
            var restaurant = result.FirstOrDefault(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

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
            {
                return NotFound();
            }

            //var restaurant = await _context.Restaurant.FindAsync(id);
            var result = await DataBaseOperations.GetRestaurant();
            var restaurant = result.Find(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("UserId,Name,Address,PhoneNumber")] Restaurant restaurant)//nepriimt userid
        {
            if (id != restaurant.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await RestaurantExists(restaurant.UserId))
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
            {
                return NotFound();
            }

            //var restaurant = await _context.Restaurant
            //    .FirstOrDefaultAsync(m => m.User_Id == id);
            var result = await DataBaseOperations.GetRestaurant();
            var restaurant = result.FirstOrDefault(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var restaurant = await _context.Restaurant.FindAsync(id);
            //_context.Restaurant.Remove(restaurant);
            //await _context.SaveChangesAsync();
            var result = await DataBaseOperations.GetRestaurant();
            var restaurant = result.FirstOrDefault(m => m.Id == id);
            await DataBaseOperations.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RestaurantExists(int? id)
        {
            //return _context.Restaurant.Any(e => e.User_Id == id);
            var result = await DataBaseOperations.GetRestaurant();
            return result.Any(e => e.Id == id);
        }
    }
}

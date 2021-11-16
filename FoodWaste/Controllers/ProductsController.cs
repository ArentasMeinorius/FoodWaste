using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodWaste.Data;
using FoodWaste.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodWaste.Controllers;

namespace FoodWaste.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string SearchString = "";

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool clearFilter)
        {
            ViewData["IsCurrentUserRestaurant"] = await IsCurrentUserRestaurant();
            ViewData["CurrentUserId"] = GetCurrentUserId();

            Func<string, string, string> getSortOrder = (x, orderby) => ((x == orderby) ? (orderby + "_desc") : orderby);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["DateSortParm"] = getSortOrder(sortOrder, "Date");
            ViewData["StateSortParm"] = getSortOrder(sortOrder, "State");
            ViewData["CurrentFilter"] = searchString;

            var products = await DataBaseOperations.GetProduct();

            if (!String.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }
            
            if (clearFilter)
            {
                SearchString = "";
            }

            products = products.Where(s => s.Name.Contains(SearchString)).ToList();

            products = sortOrder switch
            {
                "Name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                "Date" => products.OrderBy(p => p.ExpiryDate).ToList(),
                "Date_desc" => products.OrderByDescending(p => p.ExpiryDate).ToList(),
                "State" => products.OrderBy(p => p.State).ToList(),
                "State_desc" => products.OrderByDescending(p => p.State).ToList(),
                _ => products.OrderBy(p => p.Name).ToList(),
            };
            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Reserve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (! await IsCurrentUserRestaurant())
            {
                //var product = await _context.Product
                //    .FirstOrDefaultAsync(m => m.Id == id);
                var result = await DataBaseOperations.GetProduct();
                var product = result.FirstOrDefault(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                if (product.State == Product.ProductState.Listed)
                {
                    product.State = Product.ProductState.Reserved;
                    product.UserId = GetCurrentUserId();
                }
                else if (product.State == Product.ProductState.Reserved)
                {
                    if (product.UserId == GetCurrentUserId())
                    {
                        product.State = Product.ProductState.Listed;
                        product.UserId = null;
                    }
                }
                try
                {
                    await DataBaseOperations.PutProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var result = from p in _context.Product
            //             join r in _context.Restaurant on p.Restaurant_id equals r.User_Id into details
            //             from r in details.DefaultIfEmpty()
            //             select new ProductViewModel { Product = p, Restaurant = r };
            var pResult = await DataBaseOperations.GetProduct();
            var rResult = await DataBaseOperations.GetRestaurant();
            var result = from p in pResult
                         join r in rResult on p.RestaurantId equals r.UserId into details
                         from r in details.DefaultIfEmpty()
                         select new ProductViewModel { Product = p, Restaurant = r };
            var product = result.FirstOrDefault(m => m.Product.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var result = await DataBaseOperations.GetRestaurant();
            var res = result.FirstOrDefault(r => r.UserId.Equals(GetCurrentUserId()));

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
        {
            if (ModelState.IsValid)
            {
                product.State = Product.ProductState.Listed;
                product.RestaurantId = GetCurrentUserId();
                //_context.Add(product);
                //await _context.SaveChangesAsync();
                await DataBaseOperations.PostProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Product.FindAsync(id);
            var result = await DataBaseOperations.GetProduct();
            var product = result.FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await DataBaseOperations.PutProduct(product);
                    //_context.Update(product);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)// jei be id kreiptusi tai notfound grazintu be nullable
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Product
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var result = await DataBaseOperations.GetProduct();
            var product = result.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var product = await _context.Product.FindAsync(id);
            //_context.Product.Remove(product);
            //await _context.SaveChangesAsync();
            var result = await DataBaseOperations.GetProduct();
            var product = result.FirstOrDefault(m => m.Id == id);
            await DataBaseOperations.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductExists(int id)
        {
            var result = await DataBaseOperations.GetProduct();
            // return _context.Product.Any(e => e.Id == id);
            return result.Any(e => e.Id == id);
        }

        private int GetCurrentUserId()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return new int();
        }

        public async Task<bool> IsCurrentUserRestaurant()//base klase sitiems
        {
            var userId = GetCurrentUserId();
            if (userId == default)
            {
                return false;
            }
            var result = await DataBaseOperations.GetRestaurant();
            var rez = result.FirstOrDefault(r => r.UserId == userId);
            return result.FirstOrDefault(r => r.UserId == userId) != null;
        }
    }
}

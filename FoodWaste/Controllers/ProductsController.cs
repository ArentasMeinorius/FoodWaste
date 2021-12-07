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
using Microsoft.Extensions.Logging;
using FoodWaste.ActionFilters;

namespace FoodWaste.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private const string NotFoundPage = "/Products/NotFound";
        private static string SearchString = "";
        private const int PageSize = 5;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Products
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool clearFilter, int page, int pageSwitch)
        {
            ViewData["IsCurrentUserRestaurant"] = IsCurrentUserRestaurant();
            ViewData["CurrentRestaurantUserId"] = GetRestaurantId();

            Func<string, string, string> getSortOrder = (x, orderby) => ((x == orderby) ? (orderby + "_desc") : orderby);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["DateSortParm"] = getSortOrder(sortOrder, "Date");
            ViewData["StateSortParm"] = getSortOrder(sortOrder, "State");
            ViewData["CurrentFilter"] = searchString;

            var products = from p in _context.Product
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            if (clearFilter)
            {
                SearchString = "";
            }

            products = products.Where(s => s.Name.Contains(SearchString));

            products = sortOrder switch
            {
                "Name_desc" => products.OrderByDescending(p => p.Name),
                "Date" => products.OrderBy(p => p.ExpiryDate),
                "Date_desc" => products.OrderByDescending(p => p.ExpiryDate),
                "State" => products.OrderBy(p => p.State),
                "State_desc" => products.OrderByDescending(p => p.State),
                _ => products.OrderBy(p => p.Name),
            };

            int currentPage = page + pageSwitch;
            if (currentPage <= 0)
                currentPage = 0;
            if (currentPage >= (products.Count() / PageSize))
                currentPage = products.Count() / PageSize;
            ViewData["Page"] = currentPage;

            products = products.Skip(currentPage * PageSize);
            products = products.Take(PageSize);

            return View(products);
        }

        [Authorize]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Reserve(int? id)
        {
            _logger.LogInformation("Start: Reserving id: {Id}", id);
            if (id == null)
            {
                return NotFound();
            }
            if (IsCurrentUserRestaurant())
            {
                var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id); ;
                if (product == null)
                {
                    _logger.LogInformation("Completed: product not found {Id}", id);
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductExists(product.Id))
                    {
                        _logger.LogInformation("Completed: product already exist {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogInformation("Completed: database error");
                        throw;
                    }
                }
                _logger.LogInformation("Completed: product reserved {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("Start: product id: {Id}", id);
            if (id == null)
            {
                return NotFound();
            }
            var result = from p in _context.Product
                         join r in _context.Restaurant on p.RestaurantId equals r.Id into details
                         from r in details.DefaultIfEmpty()
                         select new ProductViewModel { Product = p, Restaurant = r };

            var product = result.FirstOrDefault(m => m.Product.Id == id);

            if (product == null)
            {
                _logger.LogInformation("Completed: product is not found");
                return NotFound();
            }
            _logger.LogInformation("Completed: product details: {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Create()
        {
            var res = await _context.Restaurant.SingleOrDefaultAsync(r => r.UserId.Equals(GetCurrentUserId()));
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Create([Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
        {
            if (ModelState.IsValid)
            {
                product.State = Product.ProductState.Listed;
                product.RestaurantId = GetCurrentUserId();
                product.Restaurants = await _context.Restaurant
                    .FirstOrDefaultAsync(m => m.UserId == GetCurrentUserId());
                _context.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Completed: posted product {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                _logger.LogInformation("Completed: product not found for editing {Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Completed: opening product editing page {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
        {
            _logger.LogInformation("Start: product details: {Product}", product);
            if (id != product.Id)
            {
                _logger.LogInformation("Completed: product id does not match {Id} {Id}", id, product.Id);
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.RestaurantId = GetCurrentUserId();
                    product.Restaurants = await _context.Restaurant
                    .FirstOrDefaultAsync(m => m.UserId == GetCurrentUserId());
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductExists(product.Id))
                    {
                        _logger.LogInformation("Completed: product already exists with the following id {Id}", product.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogInformation("Completed: database error");
                        throw;
                    }
                }
                _logger.LogInformation("Completed: edited product {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation("Completed: product information is not valid {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return View(product);
        }

        // GET: Products/Delete/5
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Delete(int? id)// jei be id kreiptusi tai notfound grazintu be nullable
        {
            _logger.LogInformation("Start: deleting product with id {Id}", id);
            if (id == null)
            {
                _logger.LogInformation("Completed: id is not found {Id}", id);
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                _logger.LogInformation("Completed: product is not found {Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Completed: product found for deleting {Id}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Statrt: deleting product {Id}", id);
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Completed: product deleted {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        private int GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return new int();
        }

        public bool IsCurrentUserRestaurant()//base klase sitiems
        {
            var userId = GetCurrentUserId();
            if (userId == default)
            {
                return false;
            }
            return _context.Restaurant.SingleOrDefault(r => r.UserId.Equals(userId)) != null;
        }
        private int? GetRestaurantId()
        {
            var userId = GetCurrentUserId();
            if (userId == default)
            {
                return null;
            }
            return _context.Restaurant.SingleOrDefault(r => r.UserId.Equals(userId)).Id;
        }
    }
}

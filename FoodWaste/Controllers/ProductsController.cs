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

namespace FoodWaste.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private const string NotFoundPage = "/Products/NotFound";
        private static string SearchString = "";

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool clearFilter)
        {
            _logger.LogInformation("Start: Opening products page");

            ViewData["IsCurrentUserRestaurant"] = IsCurrentUserRestaurant();
            ViewData["CurrentUserId"] = GetCurrentUserId();

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

            _logger.LogInformation("Completed: Product page is opened");

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Reserve(int? id)
        {
            _logger.LogInformation("Start: Reserving product with id", id);
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
                    _logger.LogInformation("Completed: product not found", id);
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
                        _logger.LogInformation("Completed: product already exist", product);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogInformation("Completed: database error");
                        throw;
                    }
                }
                _logger.LogInformation("Completed: product reserved",product);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("Start: opening product details", id);
            if (id == null)
            {
                return NotFound();
            }
            var result = from p in _context.Product
                         join r in _context.Restaurant on p.RestaurantId equals r.UserId into details
                         from r in details.DefaultIfEmpty()
                         select new ProductViewModel { Product = p, Restaurant = r };

            var product = result.FirstOrDefault(m => m.Product.Id == id);

            if (product == null)
            {
                _logger.LogInformation("Completed: product is not found");
                return NotFound();
            }
            _logger.LogInformation("Completed: opened product details", product);
            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Start: opened product create page");
            var res = await _context.Restaurant.SingleOrDefaultAsync(r => r.UserId.Equals(GetCurrentUserId()));
            _logger.LogInformation("Completed: opening product create page");
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
            _logger.LogInformation("Start: posting new product information");
            if (ModelState.IsValid)
            {
                product.State = Product.ProductState.Listed;
                product.RestaurantId = GetCurrentUserId();
                _context.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Completed: posted product", product);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation("Completed: product is not valid");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("Start: opening product editing page");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                _logger.LogInformation("Completed: product not found for editing", product);
                return NotFound();
            }
            _logger.LogInformation("Completed: opening product editing page", product);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
        {
            _logger.LogInformation("Start: opened product details", product);
            if (id != product.Id)
            {
                _logger.LogInformation("Completed: product id does not match", id, product.Id);
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.RestaurantId = GetCurrentUserId();
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductExists(product.Id))
                    {
                        _logger.LogInformation("Completed: product already exists with the following id", product.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogInformation("Completed: database error");
                        throw;
                    }
                }
                _logger.LogInformation("Completed: edited product", product);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation("Completed: product information is not valid", product);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)// jei be id kreiptusi tai notfound grazintu be nullable
        {
            _logger.LogInformation("Start: getting for deleting deleting", id);
            if (id == null)
            {
                _logger.LogInformation("Completed: id is not found", id);
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                _logger.LogInformation("Completed: product is not found", id);
                return NotFound();
            }
            _logger.LogInformation("Completed: product found for deleting", product);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Statrt: deleting product", id);
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Completed: product deleted", product);
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
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodWaste.Data;
using FoodWaste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FoodWaste.ActionFilters;
using FoodWaste.Services;
using System.Collections.Generic;

namespace FoodWaste.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private const string NotFoundPage = "/Products/NotFound";
        private static string SearchString = "";
        private const int PageSize = 5;
        private readonly IUserService _userService;

        public ProductsController(ApplicationDbContext context,
            IUserService userService,
            ILogger<ProductsController> logger)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        // GET: Products
        [ServiceFilter(typeof(LogMethod))]
        public IActionResult Index(string sortOrder, string searchString, bool clearFilter, int page, int pageSwitch)
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

            if (!string.IsNullOrEmpty(searchString))
                SearchString = searchString;

            if (clearFilter)
                SearchString = "";

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
        public async Task<IActionResult> Reserve(Guid? id)
        {
            _logger.LogInformation("Start: Reserving id: {Id}", id);
            if (id == null)
                return NotFound();

            if (IsCurrentUserRestaurant())
            {
                var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id); ;
                if (product == null)
                {
                    _logger.LogInformation("Completed: product not found {Id}", id);
                    return NotFound();
                }
                if (product.State == ProductState.Listed)
                {
                    product.State = ProductState.Reserved;
                    product.UserId = _userService.GetCurrentUserId(User);
                }
                else if (product.State == ProductState.Reserved)
                {
                    if (product.UserId == _userService.GetCurrentUserId(User))
                    {
                        product.State = ProductState.Listed;
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
        public IActionResult Details(Guid id)
        {
            _logger.LogInformation("Start: product id: {Id}", id);
            if (id == Guid.Empty)
                return NotFound();

            var result = from p in _context.Product
                         join r in _context.Restaurant on p.RestaurantId equals r.Id into details
                         from r in details.DefaultIfEmpty()
                         select new ProductViewModel { Product = p, Restaurant = r };

            var product = result.FirstOrDefault(m => m.Product.Id == id);

            var productAllergens = from a in _context.ProductAllergens
                                   join r in _context.Product on a.ProductId equals r.Id
                                   join al in _context.Allergens on a.AllergenId equals al.AllergenId into d
                                   from al in d.DefaultIfEmpty()
                                   select al;

            var userId = _userService.GetCurrentUserId(User);
            product.Allergens = _context.UserAllergens.Where(p => p.UserId == userId).ToList();

            product.Product.ProductAllergens = productAllergens.ToList();

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
            var res = await _context.Restaurant.SingleOrDefaultAsync(r => r.UserId.Equals(_userService.GetCurrentUserId(User)));
            var mergedLists = MergeTwoLists();
            ViewData["CurrentAllergens"] = mergedLists;
            var created = _context.UserCreatedAllergens.Where(p => p.Id == _userService.GetCurrentUserId(User));
            ViewData["Allergens"] = GetAllergens().Where(p => !mergedLists.Any(q => q.AllergenId == p.AllergenId)).Concat(created.Select(p => new Allergen { AllergenId = p.AllergenId, Name = p.Name }));
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Create([Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId,ProductAllergens")] Product product)//nepriimt userid
        {
            if (ModelState.IsValid)// multiple allergens, returns the same elements, fix
            {
                var userId = _userService.GetCurrentUserId(User);
                var selectedAllergens = _context.UserSelectedAllergens.Where(p => p.Id == userId).ToList();
                product.Id = Guid.NewGuid();
                product.State = ProductState.Listed;
                product.RestaurantId = userId;
                product.Restaurants = await _context.Restaurant
                    .FirstOrDefaultAsync(m => m.UserId == _userService.GetCurrentUserId(User));
                _context.Add(product);
                await _context.SaveChangesAsync();

                foreach (var selAl in selectedAllergens)
                {
                    if (!_context.Allergens.Any(p => p.AllergenId == selAl.AllergenId))
                    {
                        _context.Add(new Allergen { Name = selAl.Name, AllergenId = selAl.AllergenId });
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.ProductAllergens.Any(p => p.AllergenId == selAl.AllergenId && p.ProductId == product.Id))
                    {
                        _context.Add(new ProductAllergen { AllergenId = selAl.AllergenId, ProductId = product.Id });
                        await _context.SaveChangesAsync();
                    }
                }

                foreach (var selAl in selectedAllergens)
                {
                    _context.Remove(selAl);
                }

                var createdAllergens = _context.UserCreatedAllergens.Where(p => p.Id == userId);
                foreach (var allergen in createdAllergens)
                {
                    _context.Remove(allergen);
                }
                await _context.SaveChangesAsync();
                var userTempProductData = _context.UserTemporaryProducts.Where(p => p.Id == userId).FirstOrDefault();
                if (userTempProductData != null)
                {
                    _context.Remove(userTempProductData);
                }
                _logger.LogInformation("Completed: posted product {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                _logger.LogInformation("Completed: product not found for editing {Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Completed: opening product editing page {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            var mergedLists = MergeTwoLists();
            ViewData["CurrentAllergens"] = mergedLists;// top as a list of currently existing allergens and selected allergens
            ViewData["Allergens"] = GetAllergens().Where(p => !mergedLists.Any(q => q.AllergenId == p.AllergenId));

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,ExpiryDate,State,UserId,RestaurantId")] Product product)//nepriimt userid
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
                    product.RestaurantId = _userService.GetCurrentUserId(User);
                    product.Restaurants = await _context.Restaurant
                    .FirstOrDefaultAsync(m => m.UserId == _userService.GetCurrentUserId(User));
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
        public async Task<IActionResult> Delete(Guid id)// jei be id kreiptusi tai notfound grazintu be nullable
        {
            _logger.LogInformation("Start: deleting product with id {Id}", id);
            if (id == Guid.Empty)
            {
                _logger.LogInformation("Completed: id is not found {Id}", id);
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Statrt: deleting product {Id}", id);
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            _context.ProductAllergens.RemoveRange(_context.ProductAllergens.Where(p => p.AllergenId == id));

            await _context.SaveChangesAsync();
            _logger.LogInformation("Completed: product deleted {Product}", Newtonsoft.Json.JsonConvert.SerializeObject(product));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Products/Details/Modify/{productId}/{allergenId}")]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> ChangeUserAllergens(Guid productId, Guid allergenId)
        {
            try
            {
                var userId = _userService.GetCurrentUserId(User);
                var UserAllergen = await _context.UserAllergens
                .FirstOrDefaultAsync(m => m.UserId == userId && m.AllergenId == allergenId);
                if (UserAllergen == null)
                {
                    _context.Add(new UserAllergen { AllergenId = allergenId, UserId = userId });
                }
                else
                {
                    _context.Remove(UserAllergen);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Redirect("~/Products/Details/" + productId);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public IEnumerable<Allergen> GetAllergens(IEnumerable<Allergen> productAllergens = null)
        {
            if (productAllergens == null)
                return _context.Allergens.ToList().OrderBy(p => p.Name);

            return _context.Allergens.ToList().Except(productAllergens).OrderBy(p => p.Name);
        }

        public bool IsCurrentUserRestaurant()//base klase sitiems
        {
            var userId = _userService.GetCurrentUserId(User);
            if (userId == default)
                return false;

            return _context.Restaurant.SingleOrDefault(r => r.UserId.Equals(userId)) != null;
        }

        private int? GetRestaurantId()
        {
            var userId = _userService.GetCurrentUserId(User);
            if (userId == default || !IsCurrentUserRestaurant())
                return null;

            return _context.Restaurant.SingleOrDefault(r => r.UserId.Equals(userId)).Id;
        }

        [HttpGet]
        [Route("Products/Remove/AllergenC/{allergenId}")]
        public async Task<IActionResult> RemoveAllergen(Guid allergenId)
        {//somewhy it removes all
            var userId = _userService.GetCurrentUserId(User);
            var selectedAllergen = _context.UserSelectedAllergens.Where(p => p.Id == userId && p.AllergenId == allergenId).FirstOrDefault();
            if (selectedAllergen != null)
            {
                _context.Remove(selectedAllergen);// remove fails somewhy
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }

            var createdAllergen = _context.UserCreatedAllergens.Where(p => p.Id == userId && p.AllergenId == allergenId).FirstOrDefault();
            if (createdAllergen != null)
            {
                _context.Remove(createdAllergen);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        [Route("Products/Add/AllergenC/{allergenId}")]
        public async Task<IActionResult> AddAllergensCreate(Guid allergenId)
        {
            await AddAllergens(allergenId);

            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        [Route("Products/Add/AllergenE/{allergenId}")]
        public async Task<IActionResult> AddAllergensEdit(Guid allergenId)
        {
            await AddAllergens(allergenId);

            return RedirectToAction(nameof(Edit));
        }

        private async Task AddAllergens(Guid allergenId)
        {
            var userId = _userService.GetCurrentUserId(User);
            var tempAllergen = _context.Allergens.Where(p => p.AllergenId == allergenId).FirstOrDefault();

            if (tempAllergen == null)
            {
                tempAllergen = _context.UserCreatedAllergens.Where(p => p.Id == userId && p.AllergenId == allergenId).Select(p => new Allergen { Name = p.Name }).FirstOrDefault();
            }
            _context.UserSelectedAllergens.Add(new UserSelectedAllergens { Id = userId, AllergenId = allergenId, Name = tempAllergen.Name });
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        [Route("Products/Create/AllergenC/{productName}")]
        public async Task<IActionResult> CreateAllergenC(string name)
        {
            await CreateAllergen(name);
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        [Route("Products/Create/AllergenE/{productName}")]
        public async Task<IActionResult> CreateAllergenE(string name)
        {
            await CreateAllergen(name);
            return RedirectToAction(nameof(Edit));
        }

        public async Task CreateAllergen(string name)
        {
            var userId = _userService.GetCurrentUserId(User);

            _context.UserCreatedAllergens.Add(new UserCreatedAllergens { Id = userId, AllergenId = Guid.NewGuid(), Name = name });
            await _context.SaveChangesAsync();
        }

        IEnumerable<Allergen> MergeTwoLists()
        {
            var userId = _userService.GetCurrentUserId(User);
            var selectedAllergens = _context.UserSelectedAllergens.Where(p => p.Id == userId).Select(p => new Allergen { AllergenId = p.AllergenId, Name = p.Name });
            var createdAllergens = _context.UserCreatedAllergens.Where(p => p.Id == userId).Select(p => new Allergen { AllergenId = p.AllergenId, Name = p.Name });

            return selectedAllergens;//.Concat(createdAllergens).OrderBy(p => p.Name);
        }
    }
}

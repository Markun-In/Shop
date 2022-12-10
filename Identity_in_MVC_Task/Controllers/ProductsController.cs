using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity_in_MVC_Task.Data;
using Identity_in_MVC_Task.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Identity_in_MVC_Task.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public ProductsController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSort"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["CategorySort"] = sortOrder == "category_asc" ? "" : "category_asc";
            ViewData["Electronic"] = "electronic";
            ViewData["Cars"] = "cars";
            ViewData["Clothes"] = "clothes";
            ViewData["Art"] = "art";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;//ClaimsPrincipal currentUser = this.User;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }
            ViewData["CurrentFilter"] = searchString;
            var products = from s in _context.Product 
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "category_asc":
                    products = products.OrderByDescending(p => p.Category);
                    break;
                case "category_desc":
                    products = products.OrderBy(p => p.Category);
                    break;
                case "electronic":
                    products = products.Where(p => p.Category == Category.Electronics);
                    break;
                case "cars":
                    products = products.Where(p => p.Category == Category.Cars);
                    break;
                case "clothes":
                    products = products.Where(p => p.Category == Category.Clothes);
                    break;
                case "art":
                    products = products.Where(p => p.Category == Category.Art);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            int pageSize = 5;

            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,ProductPicture,Price")] Product product)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }
            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyTo(dataStream);
                        product.ProductPicture = dataStream.ToArray();
                    }
                }
                product.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,ProductPicture,Price")] Product product)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }

            if (id != product.Id)
            {
                return NotFound();
            }
            product.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyToAsync(dataStream);
                        product.ProductPicture = dataStream.ToArray();
                    }
                }
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userNowId = _context.Users.Find(currentUserId);
                ViewData["Balance"] = userNowId.Balance;
            }
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Product == null)
            {
                return Problem("Entity set 'ProductsContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Product.Any(e => e.Id == id);
        }

       
        public IActionResult Buy(int? id)
        {
            var product = _context.Product.Find(id); //Get product by Id
            var userGetPay = _context.Users.Find(product.UserId); //Get user, who get money from buy, by UserId in product

            if (_signInManager.IsSignedIn(User))
            {
                var userNowId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;   //Get authorized userId
                var userNow = _context.Users.Find(userNowId);   //Get authorized user
                ViewData["Balance"] = userNow.Balance;
            
                if (userNow.Balance - product.Price >= 0)
                {
                    ViewData["Buy"] = "Покупка состоялась!";
                    userNow.Balance = userNow.Balance - product.Price;
                    userGetPay.Balance += product.Price;
                    _context.Update(userGetPay);
                    _context.Update(userNow);
                    if (_context.Product == null)
                    {
                        return Problem("Entity set 'ProductsContext.Product'  is null.");
                    }
                    if (product != null)
                    {
                        _context.Product.Remove(product);
                    }
                    _context.SaveChanges();
                }
                else
                {
                    ViewData["Buy"] = "Недостаточно средств!";
                }
            }
            return View(User);
        }
    }
}

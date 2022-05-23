using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCocoon.Data;
using MvcCocoon.Models.Admin;

namespace MvcCocoon.Controllers
{
    [Authorize]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly WingtipToysContext _dbContext;

        public AdminController(WingtipToysContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("AdminPage")]
        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Category.CategoryName)
                .ThenBy(p => p.ProductName)
                .ToListAsync();
            var model = new AdminPageViewModel(products);
            return View(model);
        }

        [HttpGet("NewProduct")]
        public async Task<IActionResult> NewProduct()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var model = new AddProductViewModel
            {
                Categories = categories
            };
            return View(model);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            var product = new Product
            {
                CategoryID = model.CategoryId,
                ProductName = model.Name,
                Description = model.Description,
                UnitPrice = model.Price,
            };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
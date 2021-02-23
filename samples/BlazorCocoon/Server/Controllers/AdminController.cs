using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorCocoon.Server.Data;
using BlazorCocoon.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product = BlazorCocoon.Shared.Product;
using Category = BlazorCocoon.Shared.Category;
using ProductEntity = BlazorCocoon.Server.Data.Product;
using CategoryEntity = BlazorCocoon.Server.Data.Category;

namespace BlazorCocoon.Server.Controllers
{
    [Authorize]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly WingtipToysContext _context;

        public AdminController(WingtipToysContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public async Task<ActionResult<Product[]>> GetProducts()
        {
            var products = (await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToArrayAsync())
                .Select(ToDto)
                .ToArray();

            return products;
        }

        [HttpPost("products")]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            var entity = new ProductEntity
            {
                CategoryID = product.CategoryID,
                Description = product.Description,
                ImagePath = product.ImagePath,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
            };

            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ToDto(entity);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<Category[]>> GetCategories()
        {
            var categories = (await _context.Categories
                .ToArrayAsync())
                .Select(ToDto)
                .ToArray();

            return categories;
        }

        private static Product ToDto(ProductEntity source)
        {
            return new()
            {
                ProductID = source.ProductID,
                ProductName = source.ProductName,
                Description = source.Description,
                ImagePath = source.ImagePath,
                UnitPrice = source.UnitPrice,
                Category = new Category
                {
                    CategoryID = source.Category.CategoryID,
                    Description = source.Category.Description,
                    CategoryName = source.Category.CategoryName
                }
            };
        }

        private static Category ToDto(CategoryEntity source) =>
            new()
            {
                CategoryID = source.CategoryID,
                Description = source.Description,
                CategoryName = source.CategoryName
            };
    }
}
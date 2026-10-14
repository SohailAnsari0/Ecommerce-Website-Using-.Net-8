using EcommerceProject.Data;
using EcommerceProject.Models;
using EcommerceProject.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel viewModel)
        {
            var category = new Category
            {
                CategoryName = viewModel.CategoryName
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("List", "Category");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var category = await dbContext.Categories.ToListAsync();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category veiwModel)
        {
            var category = await dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == veiwModel.Id);
            if (category is not null)
            {
                dbContext.Categories.Remove(veiwModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Category");
        }
    }
}

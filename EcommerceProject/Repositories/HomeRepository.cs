using EcommerceProject.Data;
using EcommerceProject.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public HomeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await dbContext.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm="", int CategoryId = 0)
        {
            sTerm = sTerm.ToLower();
            var products = await (from product in dbContext.Products
                            join Category in dbContext.Categories
                            on product.CategoryId equals Category.Id
                            where string.IsNullOrWhiteSpace(sTerm) || (product!=null && product.Name.ToLower().StartsWith(sTerm))
                            select new Product
                            {
                                Id = product.Id,
                                Image = product.Image,
                                Name = product.Name,
                                Colour = product.Colour,
                                Price = product.Price,
                                Description = product.Description,
                                CategoryId = product.CategoryId,
                                CategoryName = product.Category.CategoryName
                            }
                            ).ToListAsync();

            if(CategoryId > 0)
            {
                 products =   products.Where(a => a.CategoryId == CategoryId).ToList();
            }

            return products;
            
        }
    }
}

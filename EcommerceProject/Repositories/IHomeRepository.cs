using EcommerceProject.Models.Entities;

namespace EcommerceProject
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int CategoryId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}
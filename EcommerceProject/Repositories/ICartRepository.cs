using EcommerceProject.Models.DTOs;
using EcommerceProject.Models.Entities;

namespace EcommerceProject.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int productId, int qty);

        Task<int> RemoveItem(int productId);

        Task<Cart> GetUserCart();

        Task<int> GetCartItemCount(string userId = "");

        Task<bool> DoCheckout(CheckoutModel model);


    }
}
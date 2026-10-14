using EcommerceProject.Models.DTOs;
using EcommerceProject.Models.Entities;

namespace EcommerceProject.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);

        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task TogglePaymentStatus(int orderId);
        Task<Order?> GetOrderById(int id);
        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
    }
}
using EcommerceProject.Data;
using EcommerceProject.Models.DTOs;
using EcommerceProject.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpcontextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public UserOrderRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpcontextAccessor, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.httpcontextAccessor = httpcontextAccessor;
            this.userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await dbContext.Orders.FindAsync(data.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{data.OrderId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await dbContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await dbContext.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = dbContext.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderItems)
                           .ThenInclude(x => x.Product)
                           .ThenInclude(x => x.Category).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }

        private string GetUserId()
        {
            var principal = httpcontextAccessor.HttpContext.User;
            string userId = userManager.GetUserId(principal);
            return userId;
        }
    }

}

using EcommerceProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{

    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            this.userOrderRepo = userOrderRepo;
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders = await userOrderRepo.UserOrders();
            return View(orders);
        }
    }
}

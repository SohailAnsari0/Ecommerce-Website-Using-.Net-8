using EcommerceProject.Data;
using EcommerceProject.Models.DTOs;
using EcommerceProject.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpcontextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public CartRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpcontextAccessor
            , UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.httpcontextAccessor = httpcontextAccessor;
            this.userManager = userManager;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    dbContext.Carts.Add(cart);
                }
                dbContext.SaveChanges();
                var cartItem = dbContext.CartItems.FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                } else
                {
                    var product = dbContext.Products.Find(productId);
                    cartItem = new CartItem
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = qty,
                        Price = product.Price
                    };
                    dbContext.CartItems.Add(cartItem);
                }
                dbContext.SaveChanges();
                transaction.Commit();


            } catch(Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
            
        }



        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            //using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Invalid Cart");
                }
                dbContext.SaveChanges();
                var cartItem = dbContext.CartItems.FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("No items in Cart");
                else if(cartItem.Quantity==1)
                    dbContext.CartItems.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                dbContext.SaveChanges();
                //transaction.Commit();

            }
            catch (Exception ex)
            {
              
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }



        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid UserId");
            var shoppingCart = await dbContext.Carts
                                .Include(a => a.CartItems)
                                .ThenInclude(a => a.Product)
                                .ThenInclude(a => a.Category)
                                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<Cart> GetCart(string userId)
        {
            var cart = await dbContext.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in dbContext.Carts
                              join cartItem in dbContext.CartItems
                              on cart.Id equals cartItem.CartId
                              where cart.UserId==userId
                              select new { cartItem.Id }
                              ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = dbContext.CartItems
                                    .Where(a => a.CartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var pendingRecord = dbContext.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.Id,
                };
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var OrderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    dbContext.OrderItems.Add(OrderItem);

                }
                     dbContext.SaveChanges();

                // removing the cartdetails
                dbContext.CartItems.RemoveRange(cartDetail);
                dbContext.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private string GetUserId()
        {
            var principal = httpcontextAccessor.HttpContext.User;
            string userId = userManager.GetUserId(principal);
            return userId;
        }
    }
}

using EcommerceProject.Models.DTOs;
using EcommerceProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EcommerceProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            this.cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect=0)
        {
            var cartCount = await cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }


        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await cartRepo.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}

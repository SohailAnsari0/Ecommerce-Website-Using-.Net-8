using EcommerceProject.Models;
using EcommerceProject.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }
       
        public async Task<IActionResult> Index(string sterm="", int categoryId=0)
        {
            var products = await _homeRepository.GetProducts(sterm, categoryId);
            var categorys = await _homeRepository.Categories();
            ProductDisplayModel ProductModel = new ProductDisplayModel
            {
                Products=products,
                Categories=categorys,
                STerm = sterm,
                CategoryId = categoryId
            };
            return View(ProductModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

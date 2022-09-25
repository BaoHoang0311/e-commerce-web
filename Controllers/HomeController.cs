using e_commerce_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_web.Data.ViewModel;

namespace e_commerce_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbMarketsContext _context;
        public HomeController(ILogger<HomeController> logger, dbMarketsContext context)
        {
            _logger = logger;
            _context = context;
        }
        // trang chủ , sp show 8 sp mới nhất
        [Route("/")]
        public IActionResult Index()
        {
            var LsProducts = _context.Products
                                        .Include(x => x.Cat)
                                        .Where(x => x.Active == true)
                                        .OrderByDescending(x => x.DateCreated);

            HomePageVM homepage = new()
            {
                All = LsProducts.ToList(),
                TraiCayNhapKhau = LsProducts.Where(x => x.CatId == 2).ToList(),
                RauCu = LsProducts.Where(x => x.CatId == 1).ToList(),
            };
            return View(homepage);
        }
        [Route("/About.html")]
        public IActionResult About()
        {
            return View();
        }
        [Route("/contact.html")]
        public IActionResult Contact()
        {
            return View();
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

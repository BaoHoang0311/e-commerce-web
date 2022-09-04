using e_commerce_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly dbMarketsContext _context;
        public ProductsController(dbMarketsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async  Task<IActionResult> Details(int id)
        {
            var singleproduct = await _context.Products
                .Include(x => x.Cat)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (singleproduct == null)
            {
                return View(nameof(Index));
            }
            return View(singleproduct);
        }
    }
}

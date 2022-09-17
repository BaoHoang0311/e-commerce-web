using e_commerce_web.Data;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly dbMarketsContext _context;
        private readonly ProductServices _services;
        public ProductsController(dbMarketsContext context, ProductServices services)
        {
            _context = context;
            _services = services;
        }
        public IActionResult AutoComplete(string prefix)
        {
            var pro = (from product in _context.Products
                       where product.ProductName.Contains(prefix)
                       orderby product.DateCreated descending
                       select new
                       {
                           label = product.ProductName,
                           avatar = product.Thumb,
                       }).Take(5).ToList();
            return Json(pro);
        }


        // trang sản phẩm, show 8 cái
        [Route("/trang-san-pham")]
        public IActionResult Index( string keySearch, int? page, int? CatId, string sortOrder)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 2;
            
            //var LsProducts = _context.Products
            //                            .Include(x => x.Cat)
            //                            .Where(p => p.Active == true)
            //                            .OrderByDescending(x => x.DateCreated);

            var LsProducts = _services.XL_trang_san_pham(keySearch, CatId, sortOrder);

            ViewBag.Count = LsProducts.Count();

            ViewBag.ID = sortOrder;

            ViewBag.CurrentPage = page;

            ViewBag.CATE = new SelectList(_context.Categories, "CatId", "CatName");
            ViewBag.CATE_ID = CatId;

            ViewBag.KKK = keySearch;

            if (CatId.HasValue)
            {
                var catname = _context.Categories.FirstOrDefault(m => m.CatId == CatId).CatName;
                ViewBag.CATE_Name = catname;
            }

            PagedList<Product> models = new PagedList<Product>(LsProducts.AsEnumerable(), pageNumber, pageSize);

            return View(models);
        }
        [Route("/san-pham/{Alias}.html", Name = "abc")]
        public async  Task<IActionResult> Details(int id, string Alias)
        {
            var singleproduct = await _context.Products
                                                .AsNoTracking()
                                                .Include(x => x.Cat)
                                                .FirstOrDefaultAsync(p => p.Alias == Alias);
            if (singleproduct == null)
            {
                return View(nameof(Index));
            }
            ViewData["RelatedProduct"] = await _context.Products.AsNoTracking()
                                                .Where(p => p.CatId == singleproduct.CatId
                                                            && p.ProductId != singleproduct.ProductId)
                                                .ToListAsync();
            return View(singleproduct);
        }
        [Route("/danh-muc/{Alias}.html")]
        public async Task<IActionResult> ListCategory(int id, string Alias)
        {
            var singleproduct = await _context.Products
                                                .AsNoTracking()
                                                .Include(x => x.Cat)
                                                .FirstOrDefaultAsync(p => p.Alias == Alias);
            if (singleproduct == null)
            {
                return View(nameof(Index));
            }
            ViewData["RelatedProduct"] = await _context.Products.AsNoTracking()
                                                .Where(p => p.CatId == singleproduct.CatId
                                                            && p.ProductId != singleproduct.ProductId)
                                                .ToListAsync();
            return View(singleproduct);
        }
    }
}

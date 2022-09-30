using e_commerce_web.Data;
using e_commerce_web.Data.ViewModel;
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
        public IActionResult Filter(int? SortPrice, string keyword,  string LeftCat )
        {
            // là cái sort giá , còn Cat là loại doanh mục
            var url = $"/trang-san-pham?sortOrder={SortPrice}&keySearch={keyword}&Cat={LeftCat}";
            var zzz = Json(new { status = "success", redirectUrl = url });
            return zzz;
        }
        // trang sản phẩm, show hết
        [Route("/trang-san-pham")]
        public IActionResult Index ( int? CatId, int? page, SearchVM searchVM)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 3;

            var LsProducts = _services.XL_trang_san_pham(searchVM);

            ViewBag.Count = LsProducts.Count();

            ViewBag.SortPrice =  searchVM.sortOrder;

            ViewBag.CurrentPage = page;

            // Cat-left search
            if (searchVM.Cat != null) ViewBag.cheeek = int.Parse(searchVM.Cat);

            ViewBag.A = (searchVM.A == null ) ? 1 : searchVM.A;
            ViewBag.B = (searchVM.B == null ) ? 1000: searchVM.B;
            ViewBag.Cat = (searchVM.Cat == null) ? null: searchVM.Cat;

            ViewBag.KKK = searchVM.keySearch;

            PagedList<Product> models = new PagedList<Product>(LsProducts.AsQueryable(), pageNumber, pageSize);

            return View(models);
        }
        [Route("/san-pham/{Alias}.html")]
        public async Task<IActionResult> Details(int id, string Alias)
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
        public IActionResult ListCategory(int id, string Alias)
        {
            return RedirectToAction("Index","Products", new { Alias =Alias });
        }
    }
}

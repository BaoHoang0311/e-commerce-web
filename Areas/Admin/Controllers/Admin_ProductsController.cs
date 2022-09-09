using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce_web.Models;
using PagedList.Core;
using e_commerce_web.Extension;
using Microsoft.AspNetCore.Http;

namespace e_commerce_web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Admin_ProductsController : Controller
    {
        private readonly dbMarketsContext _context;
        private readonly Saveimage _saveImages;

        public Admin_ProductsController(dbMarketsContext context, Saveimage saveImages)
        {
            _context = context;
            _saveImages = saveImages;
        }
        [HttpPost]
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
        // GET: Admin/Admin_Products
        public IActionResult Index([Bind("keySearch")] string keySearch, int? page)
        {
            IQueryable<Product> lsCus = _context.Products
                                                .AsNoTracking()
                                                .Include(m=>m.Cat)
                                                .OrderByDescending(x => x.DateCreated);

            if (keySearch != null) lsCus = lsCus.Where(p => p.ProductName.Contains(keySearch));

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;

            PagedList<Product> models = new PagedList<Product>(lsCus.AsQueryable(), pageNumber, pageSize);

            ViewBag.KKK = keySearch;

            return View(models);
        }

        // GET: Admin/Admin_Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Admin_Products/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }
        // POST: Admin/Admin_Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,BestSellers,HomeFlag,UnitsInStock")] Product product
           , IFormFile Thumb)
        {
            if (ModelState.IsValid)
            {
                product.DateModified = DateTime.Now;
                product.Thumb = await _saveImages.UploadImage(@"Products/images/", Thumb,product.ProductName+"_thumb_" );
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateCreated = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/Admin_Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/Admin_Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,BestSellers,HomeFlag,UnitsInStock")] Product product
            ,IFormFile Thumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pro = _context.Products.AsNoTracking().FirstOrDefault(x => x.ProductId == id);
                    product.Alias = Utilities.SEOUrl(product.ProductName);

                    if (Thumb != null) product.Thumb = await _saveImages.UploadImage(@"Products/images/", Thumb, product.ProductName + "_Thumb_");
                    else product.Thumb = pro.Thumb;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/Admin_Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/Admin_Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

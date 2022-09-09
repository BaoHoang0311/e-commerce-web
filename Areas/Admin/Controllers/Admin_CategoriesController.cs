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
    public class Admin_CategoriesController : Controller
    {
        private readonly dbMarketsContext _context;
        private readonly Saveimage _saveimages;

        public Admin_CategoriesController(dbMarketsContext context, Saveimage saveimages)
        {
            _context = context;
            _saveimages = saveimages;
        }
        [HttpPost]
        public IActionResult AutoComplete(string prefix)
        {
            var cus = (from cate in _context.Categories
                       where cate.CatName.Contains(prefix)
                       select new
                       {
                           label = cate.CatName,
                           avatar = cate.Cover
                       }).ToList();
            return Json(cus);
        }
        // GET: Admin/Admin_Categories
        public IActionResult Index( [Bind("keySearch")] string keySearch , int? page  )
        {
            IQueryable<Category> lsCat = _context.Categories;
            if (keySearch != null) lsCat = lsCat.Where(p => p.CatName.Contains(keySearch));
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            PagedList<Category> models = new PagedList<Category>(lsCat.AsQueryable(), pageNumber, pageSize);

            ViewBag.KKK = keySearch;

            return View(models);
        }

        // GET: Admin/Admin_Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        // GET: Admin/Admin_Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Admin_Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatName,Published,Cover")] 
        Category category , IFormFile Cover)
        {
            if (ModelState.IsValid)
            {
                category.Alias = Utilities.SEOUrl(category.CatName);
                category.Cover = await _saveimages.UploadImage(@"Categories/images/",Cover,category.CatName+"_cover_");
                
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Admin_Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Admin_Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatName,Published,Cover")] 
        Category category , IFormFile Cover)
        {
            if (id != category.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cat = _context.Categories.AsNoTracking().FirstOrDefault(x => x.CatId == id);
                    category.Alias = Utilities.SEOUrl(category.CatName);

                    if( Cover != null)category.Cover = await _saveimages.UploadImage(@"Categories/images/", Cover, category.CatName + "_cover_");
                    else category.Cover = cat.Cover;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CatId))
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
            return View(category);
        }

        // GET: Admin/Admin_Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Admin_Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CatId == id);
        }
    }
}

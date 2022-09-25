using e_commerce_web.Data.Enums;
using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewComponents
{
    public class Category_Nav : ViewComponent
    {
        private IMemoryCache memoryCache;
        private readonly dbMarketsContext _context;
        public Category_Nav(IMemoryCache _memoryCache, dbMarketsContext context)
        {
            memoryCache = _memoryCache;
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var _lsCategoryNav = memoryCache.GetOrCreate(CacheKey.Categories_Nav, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(3);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return GetlsCategory_Nav();
            });
            return View(_lsCategoryNav);
        }
        public ListCategoryVM GetlsCategory_Nav()
        {
            var LScategory = _context.Categories.AsNoTracking().ToList();

            ListCategoryVM cate = new();

            foreach (var item in LScategory)
            {
                cate.ListCate.Add
                (
                    new Category_check
                    {
                        Id = item.CatId,
                        Name = item.CatName,
                        Alias = item.Alias
                    }
                );
            }
            return cate;
        }
    }
}

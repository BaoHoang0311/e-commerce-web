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
    public class NewProductView : ViewComponent
    {
        private IMemoryCache memoryCache;
        private readonly dbMarketsContext _context;
        public NewProductView(IMemoryCache _memoryCache, dbMarketsContext context)
        {
            memoryCache = _memoryCache;
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var _lsTopProduct = memoryCache.GetOrCreate(CacheKey.News, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(3);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return GetlsTopProducts();
            });
            return View(_lsTopProduct);
        }
        public NewProductVM GetlsTopProducts()
        {
            NewProductVM topProduct = new()
            {
                LsTopProduct = _context.Products.AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .Where(p => p.Active == true && p.UnitsInStock > 0)
                                                    .OrderByDescending(x => x.DateCreated).Take(3).ToList()
            };
            return topProduct;
        }
    }
}

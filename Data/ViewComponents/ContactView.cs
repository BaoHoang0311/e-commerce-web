using e_commerce_web.Data.Enums;
using e_commerce_web.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewComponents
{
    public class ContactView : ViewComponent
    {
        private IMemoryCache memoryCache;
        private readonly IConfiguration config;
        public ContactView(IMemoryCache _memoryCache, IConfiguration _config)
        {
            memoryCache = _memoryCache;
            config = _config;
        }
        public IViewComponentResult Invoke()
        {
            var _contact = memoryCache.GetOrCreate(CacheKey.Contact, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(3);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return GetlsContact();
            });
            return View(_contact);
        }
        public ContactVM GetlsContact()
        {
            ContactVM contactVM = new ContactVM();
            contactVM.Phone = config.GetValue<string>("Contact:Sdt");
            contactVM.Email = config.GetValue<string>("Contact:Email");
            contactVM.Location = config.GetValue<string>("Contact:Diachi");
            return contactVM;
        }
    }
}

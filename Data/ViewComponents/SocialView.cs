﻿using e_commerce_web.Data.Enums;
using e_commerce_web.Data.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.ViewComponents
{
    public class SocialView : ViewComponent
    {
        private IMemoryCache memoryCache;
        private readonly IConfiguration config;
        public SocialView(IMemoryCache _memoryCache, IConfiguration _config)
        {
            memoryCache = _memoryCache;
            config = _config;
        }
        public  IViewComponentResult Invoke()
        {
            var _social = memoryCache.GetOrCreate(CacheKey.Social, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(3);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return GetlsSocials();
            });
            return View(_social);
        }
        public SocailVM GetlsSocials()
        {
            SocailVM socialVM = new SocailVM();
            socialVM.Facebook  = config.GetValue<string>("SocialLinks:facebook");
            socialVM.Twitter   = config.GetValue<string>("SocialLinks:twitter");
            socialVM.Instagram = config.GetValue<string>("SocialLinks:instagram");
            socialVM.Pinterest = config.GetValue<string>("SocialLinks:pinterest");
            socialVM.Youtube   = config.GetValue<string>("SocialLinks:youtube");
            return socialVM;
        }
    }
}

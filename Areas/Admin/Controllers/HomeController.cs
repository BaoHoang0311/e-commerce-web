
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public INotyfService _notifyService { get; }
        public HomeController(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }
        [Route("/trang-admin")]
        public IActionResult Index()
        {
            _notifyService.Success("Bạn đang truy cập vào trang quản lý");
            return View();
        }
    }
}




//_notyfyService.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoka", "ta ta-gear");
//_notyfyService.Custom("Custom Notification closes in 5 seconds.", 10, #135224" , "fa fa-gear");
using e_commerce_web.Data.Extension;
using e_commerce_web.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewComponents
{
    public class ListCart : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Gets<ListCartItemVM>("GioHang");
            return View(cart);
        }
    }
}

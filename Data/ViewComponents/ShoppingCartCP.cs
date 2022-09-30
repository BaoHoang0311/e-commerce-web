using e_commerce_web.Data.Extension;
using e_commerce_web.Data.Services;
using e_commerce_web.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewComponents
{
    public class ShoppingCartCP : ViewComponent
    {
        private readonly ShoppingCartService _shoppingcartservice;
        public ShoppingCartCP(ShoppingCartService shoppingcartservice)
        {
            _shoppingcartservice = shoppingcartservice;
        }
        // số ở giỏ hàng
        public IViewComponentResult Invoke()
        {
            var item = HttpContext.Session.Get<ListCartItemVM>("GioHang");
            if (item != null) return View(item.ListCart.Count);
            return View(0);
        }
    }
}

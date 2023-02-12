using AspNetCoreHero.ToastNotification.Abstractions;
using e_commerce_web.Data.Extension;
using e_commerce_web.Data.Services;
using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Controllers
{
    public class CartController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notifyService { get; }
        private ShoppingCartService _services { get; set; }
        public CartController(dbMarketsContext context, 
            INotyfService notifyService,
            ShoppingCartService service)
        {
            _context = context;
            _notifyService = notifyService;
            _services = service;

        }
        public ListCartItemVM GioHang
        {
            get
            {
                ListCartItemVM gh = new();
                gh = HttpContext.Session.Get<ListCartItemVM>("GioHang");
                if (gh == null)
                {
                    gh = new ();
                }
                return gh;
            }
        }
        // ViewCart
        public IActionResult Index()
        {
            var listshopping = GioHang;
            return View(listshopping);
        }
        //update giỏ hàng ko cần f5
        public IActionResult GetComponentGioHang()
        {
            return ViewComponent("ShoppingCartCP");
        }
        //update list Cart ko cần f5
        public IActionResult GetComponentListCart()
        {
            return ViewComponent("ListCart");
        }
        // (+) sản phẩm
        public IActionResult AddToCart(int ProductID, int? ammount,int Detail)
        {
            ListCartItemVM listcartVM = new();
            listcartVM = _services.Cong_SP(ProductID, ammount, Detail, GioHang);
            if (listcartVM== null)
            {
                _notifyService.Warning("Sản Phẩm ko tồn tại");
                return RedirectToAction("Index", "ShoppingCart");
            }
            HttpContext.Session.Set("GioHang", listcartVM);
            _notifyService.Success("Sản Phẩm thêm vào giỏ hàng thành công");
            return Json(new { status = "success" });
        }
        public IActionResult UpdateCart(int ProductID, int? ammount, int Detail)
        {
            ListCartItemVM listcartVM = new();
            listcartVM = _services.Cong_SP(ProductID, ammount, Detail, GioHang);
            if (listcartVM == null)
            {
                _notifyService.Warning("Sản Phẩm ko tồn tại");
                return RedirectToAction("Index", "Cart");
            }
            HttpContext.Session.Set("GioHang", listcartVM);
            return Json(new { status = "success" });
        }
        public IActionResult RemoveCartItem(int ProductID, int? ammount)
        {
            ListCartItemVM listcartVM = new();
            listcartVM = _services.RemoveCartItem(ProductID, ammount, GioHang);
            if (listcartVM.ListCart.Count == 0 && listcartVM.TongTien == 0 )
            {
                _notifyService.Warning("Giỏ hàng rỗng");
                HttpContext.Session.Set("GioHang", listcartVM);
                return Json(new { status = "success" });
            }
            HttpContext.Session.Set("GioHang", listcartVM);
            _notifyService.Success("Remove 1 sản phẩm thành công");
            return Json(new { status = "success" });
        }
    }
}

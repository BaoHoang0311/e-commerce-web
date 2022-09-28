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
        // (+) sản phẩm
        public IActionResult AddToCart(int ProductID, int? ammount)
        {
            ListCartItemVM listcartVM = new();
            listcartVM = _services.Cong_SP(ProductID, ammount, GioHang);
            if (listcartVM== null)
            {
                _notifyService.Warning("Sản Phẩm ko tồn tại");
                return RedirectToAction("Index", "ShoppingCart");
            }
            HttpContext.Session.Set("GioHang", listcartVM);
            _notifyService.Success("Sản Phẩm thêm vào giỏ hàng thành công");
            return Json(new { status = "success" });
        }
        public IActionResult GetComponent()
        {
            return ViewComponent("ShoppingCartCP");
        }
        //public IActionResult RemoveToCart(int ProductID, int? ammount)
        //{
        //    listcart = GioHang;
        //    CartItemVM item = new();
        //    // tìm sp có trong giỏ hàng chưa
        //    item = GioHang.SingleOrDefault(p => p.sanpham.ProductId == ProductID);
        //    if (item != null) // có rồi -> cập nhập số lượng
        //    {
        //        // nếu Addcart có số lượng thì thêm + với với lượng hiện tại
        //        if (ammount.HasValue)
        //        {
        //            item.amount += ammount.Value;
        //        }
        //        // nếu Addcart không có số lượng ( theo dấu (+) )
        //        else
        //        {
        //            item.amount++;
        //        }
        //    }
        //    else // chưa có
        //    {
        //        Product product = _context.Products.SingleOrDefault(p => p.ProductId == ProductID);
        //        if (product == null)
        //        {
        //            _notifyService.Warning("Sản Phẩm ko tồn tại");
        //        }
        //        else
        //        {
        //            item.sanpham = product;
        //            // số lượng
        //            // nếu Addcart có số lượng thì thêm + với với lượng hiện tại
        //            if (ammount.HasValue)
        //            {
        //                item.amount = ammount.Value;
        //            }
        //            // nếu Addcart không có số lượng ( theo dấu (+) )
        //            else
        //            {
        //                item.amount = 1;
        //            }
        //            item.TongTien = item.Total();
        //        }
        //        listcart.Add(item);
        //    }
        //    HttpContext.Session.Set("GioHang", listcart);
        //    return RedirectToAction("Index", "ShoppingCart");
        //}
        //public IActionResult ClearCart(int ProductID)
        //{
        //    HttpContext.Session.Remove("GioHang");
        //    return RedirectToAction("Index");
        //}
        //// xóa luôn sản phẩm
        //public IActionResult Remove(int ProductID)
        //{
        //    listcart = GioHang;
        //    CartItemVM item = new();

        //    // tìm sp có trong giỏ hàng chưa
        //    item = GioHang.SingleOrDefault(p => p.sanpham.ProductId == ProductID);

        //    if (item != null) listcart.Remove(item);

        //    // lưu lại session
        //    HttpContext.Session.Set("GioHang", listcart);
        //    return View();
        //}
    }
}

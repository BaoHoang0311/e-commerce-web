using AspNetCoreHero.ToastNotification.Abstractions;
using e_commerce_web.Data.Extension;
using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notifyService { get; }

        public CheckoutController(dbMarketsContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public ListCartItemVM GioHang
        {
            get
            {
                ListCartItemVM gh = new();
                gh = HttpContext.Session.Get<ListCartItemVM>("GioHang");
                if (gh == null)
                {
                    gh = new();
                }
                return gh;
            }
        }
        [Route("/checkout.html")]
        public IActionResult Index()
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<ListCartItemVM>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("KhachHang_Ma");
            ThanhToanVM checkout = new ThanhToanVM();
            if (taikhoanID == null) return RedirectToAction("Index", "Home");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == taikhoanID);
                checkout.cus.FullName = khachhang.FullName;
                checkout.cus.Email = khachhang.Email;
                checkout.cus.Phone = khachhang.Phone;
                checkout.cus.Address = khachhang.Address;

                checkout.ListCartItem.ListCart = cart.ListCart;
                checkout.ListCartItem.TongTien = cart.TongTien;
            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Location", "Name");
            return View(checkout);
        }
        [HttpPost]
        [Route("/checkout.html")]
        public IActionResult Index(ThanhToanVM checkout)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<ListCartItemVM>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("KhachHang_Ma");
            Order order = new()
            {
                CustomerId = taikhoanID,
                OrderDate = DateTime.Now,
                TotalMoney = cart.TongTien,
                Address = checkout.cus.Address,
                TransactStatusId = 1 ,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    OrderId = order.OrderId,
                    ProductId = item.sanpham.ProductId,
                    Amount = item.amount,
                    TotalMoney = item.Tien,
                    CreateDate = DateTime.Now,
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
            HttpContext.Session.Remove("GioHang");
            return View("ThankYou");
        }
    }
}

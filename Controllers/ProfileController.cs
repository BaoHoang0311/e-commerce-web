using AspNetCoreHero.ToastNotification.Abstractions;
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
    public class ProfileController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notifyService { get; }
        public ProfileController(dbMarketsContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public IActionResult Index(string id)
        {
            var cus = _context.Customers.AsNoTracking()
                                        .FirstOrDefault(x => x.CustomerId == id);
            if (cus == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ProfileVM profile = new()
            {
                customer = cus,
                changepass = new ChangePasswordVM(),
            };
            ViewBag.LocationId = new SelectList(_context.Locations, "LocationId", "Name");

            return View(profile);
        }
        [HttpPost]
        public IActionResult Edit(ProfileVM profile)
        {
            var cus = _context.Customers.AsNoTracking();

            if (cus.FirstOrDefault(x => x.Phone == profile.customer.Phone && x.CustomerId != profile.customer.CustomerId) != null)
            {
                _notifyService.Warning("Sdt bị trùng");
                return RedirectToAction("Index", "Profile", new { id = profile.customer.CustomerId });
            }
            if (cus.FirstOrDefault(x => x.Email == profile.customer.Email && x.CustomerId != profile.customer.CustomerId) != null)
            {
                _notifyService.Warning("Email bị trùng");
                return RedirectToAction("Index", "Profile", new { id = profile.customer.CustomerId });
            }
            _context.Update(profile.customer);
            _context.SaveChanges();
            _notifyService.Success("Chỉnh sửa thành công");
            return RedirectToAction("Index", "Profile", new { id = profile.customer.CustomerId });
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM changepass)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("KhachHang_Ma");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var taikhoan = _context.Customers.FirstOrDefault(x => x.CustomerId == taikhoanID);

                if (taikhoan == null) return RedirectToAction("Login", "Account");

                taikhoan.Password = changepass.NewPassword;

                _context.Update(taikhoan);

                _context.SaveChanges();
                _notifyService.Success("Thay mật khẩu thành công");
                return RedirectToAction("Index", "Profile", new { id = changepass.CustomerId });
            }
            catch
            {
                _notifyService.Warning("Thay mật khẩu thất bại");
                return RedirectToAction("Index", "Profile", new { id = changepass.CustomerId });
            }
        }
    }
}

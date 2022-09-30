using AspNetCoreHero.ToastNotification.Abstractions;
using e_commerce_web.Data.ViewModel;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace e_commerce_web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly dbMarketsContext _context;
        public INotyfService _notifyService { get; }
        public AccountsController(dbMarketsContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking()
                                            .FirstOrDefault(x => x.Phone == Phone);
                var quanly = _context.Accounts.AsNoTracking()
                                                .FirstOrDefault(x => x.Phone == Phone);
                if (khachhang == null && quanly == null)
                {
                    return Json(data: true);
                }
                return Json(data: "Số điện thoại :" + Phone + " đã tồn tại");
            }
            catch
            {
                return Json(data: false);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateName(string FullName)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking()
                                            .FirstOrDefault(x => x.FullName == FullName);
                var quanly = _context.Accounts.AsNoTracking()
                                                .FirstOrDefault(x => x.FullName == FullName);
                if (khachhang == null && quanly == null)
                {
                    return Json(data: true);
                }
                return Json(data: "Tên FullName: " + FullName + " đã tồn tại");
            }
            catch
            {
                return Json(data: false);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking()
                                                .FirstOrDefault(x => x.Email == Email);
                var quanly = _context.Accounts.AsNoTracking()
                                .FirstOrDefault(x => x.Email == Email);
                if (khachhang == null && quanly == null)
                {
                    return Json(data: true);
                }
                return Json(data: "Email: " + Email + " đã tồn tại");
            }
            catch
            {
                return Json(data: false);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/dang-nhap.html")]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-nhap.html")]
        public async Task<IActionResult> Login(LogInVM dangnhap, string returnUrl )
        {
            try
            {
                var kh = _context.Customers
                                    .AsNoTracking()
                                    .FirstOrDefault(x => x.FullName == dangnhap.UserName
                                    && x.Password == dangnhap.Password);
                if (kh == null)
                {
                    _notifyService.Warning("Bạn đã nhập thông tin sai");
                    return RedirectToAction("Login", "Accounts");
                }
                //update LastLogin
                kh.LastLogin = DateTime.Now;
                _context.Customers.Update(kh);
                _context.SaveChanges();
                // Lưu session
                HttpContext.Session.SetString("KhachHang_Ma", kh.CustomerId);
                // Identity
                var USERCLAIM = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kh.FullName),
                    new Claim("CustomerID", kh.CustomerId.ToString()),
                };
                ClaimsIdentity grandmaIdentity = new ClaimsIdentity(USERCLAIM, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(grandmaIdentity));
                _notifyService.Success("Bạn đăng nhập thành công");
                if(returnUrl == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            catch
            {
                return RedirectToAction("Login", "Accounts");
            }
        }
        [HttpGet]
        [Route("/dang-xuat.html")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("KhachHang_Ma");
            HttpContext.Session.Remove("GioHang");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("/dang-ky.html")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/dang-ky.html")]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var taikhoanID = HttpContext.Session.GetString("KhachHang_Ma");
                if (taikhoanID != null)
                {
                    return RedirectToAction("Index", "Products");
                }

                Customer cus = new Customer()
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    FullName = registerVM.FullName,
                    Password = registerVM.Password,
                    Phone = registerVM.Phone,
                    Email = registerVM.Email,
                    Active = true,
                    CreateDate = DateTime.Now,
                };
                _context.Customers.Add(cus);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                return RedirectToAction("Register", "Accounts");
            }
        }

    }
}

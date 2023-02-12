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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult getLastestPage(string returnUrl)
        {
            var ajaxSuccess = returnUrl == "/" || returnUrl == null || returnUrl == "/dang-ky.html" || returnUrl == "/dang-nhap.html"
                                    ? Json(new { status = "success", link = $"/dang-nhap.html" })
                                    : Json(new { status = "success", link = $"/dang-nhap.html?returnUrl={returnUrl}" });
            return ajaxSuccess;
        }
        [AllowAnonymous]
        [Route("/dang-nhap.html")]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated )
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-nhap.html")]
        public async Task<IActionResult> Login(LogInVM dangnhap, string returnUrl)
        {
            try
            {
                var kh = _context.Customers.AsNoTracking()
                                            .Include(m => m.Role)
                                            .FirstOrDefault(x => x.FullName == dangnhap.UserName
                                            && x.Password == dangnhap.Password);

                var ajaxSuccess = returnUrl == "/" || returnUrl == null 
                                                ? Json(new { status = "success", link = $"/dang-nhap.html" })
                                                : Json(new { status = "success", link = $"/dang-nhap.html?returnUrl={returnUrl}" });
                if (kh == null)
                {
                    ajaxSuccess = returnUrl == "/" || returnUrl == null
                                                    ? Json(new { status = "fail", link = $"/dang-nhap.html" })
                                                    : Json(new { status = "fail", link = $"/dang-nhap.html?returnUrl={returnUrl}" });
                    return ajaxSuccess;
                }
                else
                {
                    ajaxSuccess = returnUrl == "/" || returnUrl == null
                                                ? Json(new { status = "success", link = "/" })
                                                : Json(new { status = "success", link = returnUrl });
                }
                //update LastLogin
                kh.LastLogin = DateTime.Now;
                _context.Customers.Update(kh);
                _context.SaveChanges();
                // Lưu session 
                // Settring Default
                HttpContext.Session.SetString("KhachHang_Ma", kh.CustomerId);
                // Identity
                var USERCLAIM = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kh.FullName),
                    new Claim("CustomerID", kh.CustomerId.ToString()),
                    new Claim(ClaimTypes.Role, kh.Role.RoleName),
                };
                ClaimsIdentity grandmaIdentity = new ClaimsIdentity(USERCLAIM, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(grandmaIdentity));

                _notifyService.Success("Bạn đã đăng nhập thành công");
                return ajaxSuccess;
            }
            catch
            {
                var ajaxSuccess = returnUrl == "/" || returnUrl == "" ? Json(new { status = "success", link = $"/dang-nhap.html" })
                                : Json(new { status = "success", link = $"/dang-nhap.html?returnUrl={returnUrl}" });
                return ajaxSuccess;
            }
        }
        [Route("/dntb.html")]
        public IActionResult Fail()
        {
            _notifyService.Warning("Bạn đã nhập thông tin sai");
            return RedirectToAction("Login","Accounts");
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
                    RoleId = 3,
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

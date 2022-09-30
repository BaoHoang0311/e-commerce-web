using AspNetCoreHero.ToastNotification.Abstractions;
using e_commerce_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CheckoutController(dbMarketsContext context)
        {
            _context = context;
        }
        public IActionResult Complete()
        {
            return null;
        }
    }
}

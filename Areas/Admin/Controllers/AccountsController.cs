using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce_web.Models;

namespace e_commerce_web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly dbMarketsContext _context;

        public AccountsController(dbMarketsContext context)
        {
            _context = context;
        }
        public IActionResult Filter(int? roleID, string Status)
        {
            var url = $"/Admin/Accounts/Index?roleID={roleID}&status={Status}";
            var zzz = Json(new { status = "success", redirectUrl = url });
            return zzz;
        }
        // GET: Admin/Accounts
        public async Task<IActionResult> Index(int? roleID, string Status)
        {
            ViewData["RoleName"] = new SelectList(_context.Roles.Where(x => x.RoleId == 1 || x.RoleId == 2), "RoleId", "Description");

            IQueryable<Customer> dbMarketsContext = _context.Customers
                                                        .Include(a => a.Role)
                                                        .Where(m => m.Role.RoleName == "NV" 
                                                        || m.Role.RoleName=="Admin");
            
            if(roleID != null && roleID != 0)
            {
                dbMarketsContext = dbMarketsContext.Where(x => x.RoleId == roleID);
            }
            if(Status == "Active")
            {
                dbMarketsContext = dbMarketsContext.Where(x => x.Active == true);
            }
            if(Status == "Block")
            {
                dbMarketsContext = dbMarketsContext.Where(x => x.Active == false );
            }

            return View(await dbMarketsContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Customers
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["RoleName"] = new SelectList(_context.Roles.Where(x => x.RoleId == 1 || x.RoleId == 2), "RoleId", "Description");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Phone,Email,Password,Active,FullName,RoleId,LastLogin,CreateDate")] Customer account)
        {
            if (ModelState.IsValid)
            {
                account.CreateDate = DateTime.Now;
                account.CustomerId = Guid.NewGuid().ToString();
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleName"] = new SelectList(_context.Roles.Where(x=> x.RoleId==1 || x.RoleId == 2) , "RoleName", "Description", account.RoleId);
            return View(account);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Customers.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleName"] = new SelectList(_context.Roles, "RoleId", "Description", account.RoleId);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Phone,Email,Password,Active,FullName,RoleId,LastLogin,CreateDate")] Customer account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw ;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleName"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Customers
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var account = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

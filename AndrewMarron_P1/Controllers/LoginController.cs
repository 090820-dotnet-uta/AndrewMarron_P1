using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RevatureP1.Models;

namespace RevatureP1.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// This is the controller for views before logging in
        /// </summary>
        private IMemoryCache _cache;
        private DbContextClass _context;

        public LoginController(IMemoryCache cache, DbContextClass context)
        {
            /// <summary>
            /// The constructor
            /// </summary>
            _cache = cache;
            _context = context;
        }

        public IActionResult Index(string warn = "")
        {
            /// <summary>
            /// The login page
            /// </summary>
            //System.Diagnostics.Debug.WriteLine(_cache.Get("TestCacheVar"));
            //System.Diagnostics.Debug.WriteLine(credinvalid);
            if (warn == "credinvalid")
            {
                ViewData["warn"] = "credinvalid";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] LoginViewModel loginInfo)
        //public IActionResult Login(string usernamein, string passwordin)
        {
            /// <summary>
            /// Logs in, checking user name and password
            /// </summary>
            var thisCustomer = await _context.Customers
                .FirstOrDefaultAsync(m => m.UserName == loginInfo.Username);
            if (thisCustomer != null)
            {
                if (thisCustomer.Password == loginInfo.Password)
                {
                    _cache.Set("thisCustomer", thisCustomer);
                    return Redirect("/");
                }
            }

            return Redirect("/Login?warn=credinvalid");
        }

        public IActionResult Register(string warn = "")
        {
            /// <summary>
            /// The registration page
            /// </summary>
            if (warn == "duplicate")
            {
                ViewData["warn"] = "duplicate";
            }
            return View();
        }

        // POST: NewCustomer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("CustomerId,UserName,FirstName,LastName,Password,DateAdded")] Customer customer)
        {
            /// <summary>
            /// Registers a new customer
            /// </summary>
            if (ModelState.IsValid)
            {
                Customer checkCust = UtilMethods.GetCustomerByUserName(customer.UserName, _context);
                if (checkCust.UserName != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Customer with user name {customer.UserName} already exists");
                    return Redirect("/Login/Register?warn=duplicate");
                }
                else
                {
                    customer.DateAdded = DateTime.Now;
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(customer);
        }
    }
}

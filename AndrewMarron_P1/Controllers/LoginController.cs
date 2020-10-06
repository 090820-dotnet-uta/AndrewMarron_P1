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
        private IMemoryCache _cache;
        private DbContextClass _context;

        public LoginController(IMemoryCache cache, DbContextClass context)
        {
            _cache = cache;
            _context = context;
        }

        public IActionResult Index(string warn = "")
        {
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
            //var foundCustomers = from thisTableItem in _context.Customers
            //                     where thisTableItem.UserName == loginInfo.Username
            //                     select thisTableItem;
            //if(foundCustomers.Count() == 1)
            //{
            //    Customer thisCustomer = foundCustomers.First();
            //    if(thisCustomer.Password == loginInfo.Password)
            //    {
            //        _cache.Set("thisCustomer", thisCustomer);
            //        return Redirect("/");
            //    }
            //}
            //else if(foundCustomers.Count() > 1)
            //{
            //    System.Diagnostics.Debug.WriteLine("Error: Multiple customers with same name");
            //}

            //System.Diagnostics.Debug.WriteLine(_cache.Get("TestCacheVar"));
            //return View();
            //_cache.Set("IsLoggedIn", "true");
            //Customer inCustomer = new Customer();
            //inCustomer.UserName = "aaa";
            //_cache.Set("thisCustomer", inCustomer);

            //ModelState.AddModelError("<usernamein>", "<message>");
            //return View("Index");

            return Redirect("/Login?warn=credinvalid");
        }

        //public IActionResult GoToRegister(string usernamein, string passwordin)
        //{
        //    return Redirect("/Register");
        //}

        public IActionResult Register(string warn = "")
        {
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

        //public IActionResult Register(string usernamein, string passwordin, string firstnamein, string lastnamein)
        //{
        //    ///<summary>
        //    /// Registers a new customer
        //    ///</summary>
        //    Customer checkCust = UtilMethods.GetCustomerByUserName(usernamein, _context);
        //    if (checkCust.UserName != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Customer with user name {usernamein} already exists");
        //        return Redirect("/Register?warn=duplicate");
        //    }
        //    else
        //    {
        //        Customer newCust = new Customer(usernamein, firstnamein, lastnamein, passwordin);
        //        _context.Customers.Add(newCust);
        //        _context.SaveChanges();
        //        System.Diagnostics.Debug.WriteLine($"Customer account {usernamein} created");
        //        _cache.Set("thisCustomer", newCust);
        //    }
        //    return Redirect("/");
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(string credinvalid = "false")
        {
            //System.Diagnostics.Debug.WriteLine(_cache.Get("TestCacheVar"));
            //System.Diagnostics.Debug.WriteLine(credinvalid);
            if (credinvalid == "true")
            {
                ViewData["credinvalid"] = "true";
            }
            return View();
        }

        public IActionResult Login(string usernamein, string passwordin)
        {
            var foundCustomers = from thisTableItem in _context.Customers
                                 where thisTableItem.UserName == usernamein
                                 select thisTableItem;
            if(foundCustomers.Count() == 1)
            {
                Customer thisCustomer = foundCustomers.First();
                if(thisCustomer.Password == passwordin)
                {
                    _cache.Set("thisCustomer", thisCustomer);
                    return Redirect("/");
                }
            }
            else if(foundCustomers.Count() > 1)
            {
                System.Diagnostics.Debug.WriteLine("Error: Multiple customers with same name");
            }
            //System.Diagnostics.Debug.WriteLine(_cache.Get("TestCacheVar"));
            //return View();
            //_cache.Set("IsLoggedIn", "true");
            //Customer inCustomer = new Customer();
            //inCustomer.UserName = "aaa";
            //_cache.Set("thisCustomer", inCustomer);

            //ModelState.AddModelError("<usernamein>", "<message>");
            //return View("Index");

            //return Redirect("/Login?warn=credinvalid");
            return Redirect("/Login?credinvalid=true");
        }


        public IActionResult GoToRegister(string usernamein, string passwordin)
        {
            return Redirect("/Register");
        }
    }
}

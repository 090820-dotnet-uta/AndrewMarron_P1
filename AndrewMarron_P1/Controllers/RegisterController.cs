using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RevatureP1.Models;

namespace RevatureP1.Controllers
{
    public class RegisterController : Controller
    {
        private IMemoryCache _cache;
        private DbContextClass _context;
        public RegisterController(IMemoryCache cache, DbContextClass context)
        {
            _cache = cache;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register(string usernamein, string passwordin, string firstnamein, string lastnamein)
        {
            ///<summary>
            /// Registers a new customer
            ///</summary>
            Customer checkCust = UtilMethods.GetCustomerByUserNameUtil(usernamein, _context);
            if (checkCust.UserName != null)
            {
                System.Diagnostics.Debug.WriteLine($"Customer with user name {usernamein} already exists");
                return Redirect("/Register?warn=duplicate");
            }
            else
            {
                Customer newCust = new Customer(usernamein, firstnamein, lastnamein, passwordin);
                _context.Customers.Add(newCust);
                _context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"Customer account {usernamein} created");
                _cache.Set("thisCustomer", newCust);
            }
            return Redirect("/");
            return View();
        }

        //public static RunSession RegisterCustomer(RunSession runSess)
        //{
        //    ///<summary>
        //    /// Registers a new customer
        //    ///</summary>
        //    Console.WriteLine("Input new customer user name");
        //    string newUserName = GetStringInput(shortStringMaxLength);
        //    Customer checkCust = UtilMethods.GetCustomerByUserNameUtil(newUserName, runSess);
        //    if (checkCust.UserName != null)
        //    {
        //        Console.WriteLine($"Customer with user name {newUserName} already exists");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Input new customer password");
        //        string newPass = GetStringInput(shortStringMaxLength);
        //        Console.WriteLine("Input new customer first name");
        //        string firstName = GetStringInput(shortStringMaxLength);
        //        Console.WriteLine("Input new customer last name");
        //        string lastName = GetStringInput(shortStringMaxLength);
        //        Customer newCust = new Customer(newUserName, firstName, lastName, newPass);
        //        runSess.DbContext.Customers.Add(newCust);
        //        runSess.DbContext.SaveChanges();
        //        Console.WriteLine($"Customer account {newUserName} created");
        //    }
        //    return (runSess);
        //}
    }
}

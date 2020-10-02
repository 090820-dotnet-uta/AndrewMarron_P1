using System;
using System.Collections.Generic;
using RevatureP1.Models;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Bson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;

namespace RevatureP1
{
    public static class UtilMethods
    {
        private static int shortStringMaxLength = 40; // For names and passwords

        public static IMemoryCache InitializeCacheIfNeeded(IMemoryCache _cache)
        {
            /// <summary>
            /// Initializes the memory cache if it is not already initialized
            /// </summary>
            if (_cache.Get("thisCustomer") == null)
            {
                System.Diagnostics.Debug.WriteLine("Initializing cache");
                _cache.Set("thisCustomer", new Customer());
                _cache.Set("selectedLocation", new Location());
                _cache.Set("selectedStock", new StockItem());
                _cache.Set("customerCart", new List<OrderItem>());
                //_cache.Set("custLocalList", new List<Customer>());
                //_cache.Set("locLocalList", new List<Location>());
                //_cache.Set("prodLocalList", new List<Product>());

            }
            return (_cache);
        }

        public static IActionResult RedirectIfLoggedOut(IActionResult thisView, IActionResult thisRedirect, IMemoryCache _cache)
        {
            Customer thisCustomer = (Customer)_cache.Get("thisCustomer");
            if (thisCustomer.UserName != null)
            {
                return thisView;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Redirecting to login");
                return thisRedirect;
            }
        }

        public static Customer GetCustomerByUserNameUtil(string userName, DbContextClass context)
        {
            ///<summary>
            /// Used for logging in and for checking if username is taken
            ///</summary>
            Customer gottenCust = new Customer();
            var foundCustomers = from thisTableItem in context.Customers
                                 where thisTableItem.UserName == userName
                                 select thisTableItem;
            if (foundCustomers.Count() > 1)
            {
                throw (new Exception("Error: Duplicate user names found"));
            }
            else if (foundCustomers.Count() != 0)
            {
                gottenCust = foundCustomers.First();
            }
            return (gottenCust);
        }


        public static DbContextClass PopulateTestDb (DbContextClass context)
        {
            ///<summary>
            /// Recreates test data for when the database is reset for updating
            ///</summary>
            System.Diagnostics.Debug.WriteLine("Pobulating database with test data");
            var foundCustomers = from thisTableItem in context.Customers
                                 select thisTableItem;
            if (foundCustomers.Count() > 0)
            {
                System.Diagnostics.Debug.WriteLine("Aborting; database already populated");
                return (context);
            }
            System.Diagnostics.Debug.WriteLine("Adding Customers");
            // Customer(string UserNameIn, string FirstNameIn, string LastNameIn, string PasswordIn)
            context.Customers.Add(new Customer("aa", "Aron", "Anders", "aa"));
            context.SaveChanges();
            context.Customers.Add(new Customer("bb", "Bethany", "Blue", "bb"));
            context.SaveChanges();
            context.Customers.Add(new Customer("cc", "Carl", "CarlsBerg", "cc"));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Success");
            System.Diagnostics.Debug.WriteLine("Adding Locations");
            // Location(string LocationAddressIn)
            context.Locations.Add(new Location("1111 One Road"));
            context.SaveChanges();
            context.Locations.Add(new Location("2222 Two Street"));
            context.SaveChanges();
            context.Locations.Add(new Location("3333 Three Drive"));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Success");
            System.Diagnostics.Debug.WriteLine("Adding Products");
            // Product(string ProductNameIn, double ProductPriceIn, string ProductDescriptionIn) 
            context.Products.Add(new Product("Cheddar", 4.00, "A popular English yellowish cheese"));
            context.SaveChanges();
            context.Products.Add(new Product("Swiss", 5.00, "A Swiss cheese with holes in it"));
            context.SaveChanges();
            context.Products.Add(new Product("Provolone", 6.00, "An aged Iitalian cheese"));
            context.SaveChanges();
            context.Products.Add(new Product("Mozzarella", 5.00, "A smooth Italian cheese"));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Success");
            System.Diagnostics.Debug.WriteLine("Adding StockItems");
            // StockItem(int LocationIdIn, int ProductIdIn, int StockCountIn, bool IsBundleIn, float DiscountPercentIn)
            context.StockItems.Add(new StockItem(1, 1, 110, true, 20));
            context.StockItems.Add(new StockItem(1, 2, 0, true, 0));
            context.StockItems.Add(new StockItem(1, 4, 130, true, 0));
            context.StockItems.Add(new StockItem(2, 2, 50, true, 0));
            context.StockItems.Add(new StockItem(2, 3, 45, true, 0));
            context.StockItems.Add(new StockItem(2, 4, 40, true, 0));
            context.StockItems.Add(new StockItem(3, 1, 2000, true, 50));
            context.StockItems.Add(new StockItem(3, 2, 1000, true, 50));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Success");
            System.Diagnostics.Debug.WriteLine("Adding OrderItems");
            // OrderItem(int CustomerIdIn, int LocationIdIn, int ProductIdIn, double TotalPriceWhenOrderedIn, int OrderCountIn)
            context.OrderItems.Add(new OrderItem(2, 1, 1, 12.00, 3));
            context.OrderItems.Add(new OrderItem(2, 1, 2, 10.00, 2));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Success");
            System.Diagnostics.Debug.WriteLine("Adding BundleRelations");
            System.Diagnostics.Debug.WriteLine("(Placeholder; not implemented)");
            System.Diagnostics.Debug.WriteLine("Done");

            return (context);
        }
    }
}

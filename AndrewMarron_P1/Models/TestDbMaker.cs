using System;
using System.Collections.Generic;
using RevatureP1.Models;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Bson;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;

namespace RevatureP1.Models
{
    public static class TestDbMaker
    {
        public static DbContextClass PopulateTestDb(DbContextClass context)
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

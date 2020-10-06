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

        public static IMemoryCache InitializeCacheIfNeeded(IMemoryCache _cache)
        {
            /// <summary>
            /// Initializes the memory cache if it is not already initialized
            /// </summary>
            if (_cache.Get("thisCustomer") == null || (string)_cache.Get("doLogout") == "true")
            {
                System.Diagnostics.Debug.WriteLine("Initializing cache");
                _cache.Set("doLogout", "false");
                _cache.Set("thisCustomer", new Customer());
                _cache.Set("selectedLocation", new Location());
                _cache.Set("selectedStock", new StockItem());
                _cache.Set("customerCart", new List<OrderItemViewModel>());
                _cache.Set("currentViewedStock", new StockItemViewModel());
                //_cache.Set("custLocalList", new List<Customer>());
                //_cache.Set("locLocalList", new List<Location>());
                //_cache.Set("prodLocalList", new List<Product>());

            }
            return (_cache);
        }

        public static bool LogInCheck(IMemoryCache _cache)
        {
            Customer thisCustomer = (Customer)_cache.Get("thisCustomer");
            if (thisCustomer.UserName != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Customer GetCustomerByUserName(string userName, DbContextClass context)
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
                throw (new Exception("Error: Duplicate user name found"));
            }
            else if (foundCustomers.Count() != 0)
            {
                gottenCust = foundCustomers.First();
            }
            return (gottenCust);
        }

        public static Location GetLocationById(int locId, DbContextClass context)
        {
            ///<summary>
            /// Used for getting location for order history and such
            ///</summary>
            Location gottenLoc = new Location();
            var foundLocations = from thisTableItem in context.Locations
                                 where thisTableItem.LocationId == locId
                                 select thisTableItem;
            if (foundLocations.Count() > 1)
            {
                throw (new Exception("Error: Duplicate location found"));
            }
            else if (foundLocations.Count() != 0)
            {
                gottenLoc = foundLocations.First();
            }
            return (gottenLoc);
        }

        public static Product GetProductById(int prodId, DbContextClass context)
        {
            ///<summary>
            /// Used for getting product for order history and such
            ///</summary>
            Product gottenProd = new Product();
            var foundProducts = from thisTableItem in context.Products
                                where thisTableItem.ProductId == prodId
                                select thisTableItem;
            if (foundProducts.Count() > 1)
            {
                throw (new Exception("Error: Duplicate product found"));
            }
            else if (foundProducts.Count() != 0)
            {
                gottenProd = foundProducts.First();
            }
            return (gottenProd);
        }

        public static StockItem GetStockItemByIds(int locId, int prodId, DbContextClass context)
        {
            ///<summary>
            /// Used for getting stock items for checking card addition
            ///</summary>
            StockItem gottenStockItem = new StockItem();
            var foundStockItems = from thisTableItem in context.StockItems
                                where thisTableItem.LocationId == locId
                                where thisTableItem.ProductId == prodId
                                select thisTableItem;
            if (foundStockItems.Count() > 1)
            {
                throw (new Exception("Error: Duplicate stock item found"));
            }
            else if (foundStockItems.Count() != 0)
            {
                gottenStockItem = foundStockItems.First();
            }
            return (gottenStockItem);
        }

        public static bool CheckProductCount( int locId, int prodId, int orderCount, DbContextClass context)
        {
            StockItem gottenStockItem = UtilMethods.GetStockItemByIds(locId, prodId, context);
            if (orderCount <= gottenStockItem.StockCount)
            {
                return true;
            }
            return false;
        }

        public static double TrimPriceDigits(double priceIn)
        {
            return (Math.Truncate(priceIn * 100) / 100);
        }

        public static double GetDiscountedPrice(Product thisProduct, double thisDiscountPercent)
        {
            return (TrimPriceDigits(thisProduct.BasePrice * (100 - thisDiscountPercent) / 100));
        }

        public static int GetCartCount(IMemoryCache cache)
        {
            List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)cache.Get("customerCart");
            return thisCart.Count();
        }

        public static StockItemViewModel BuildStockItemViewModelFromLocStock(Location thisLocation, StockItem thisStockItem, DbContextClass context)
        {
            Product thisProduct = UtilMethods.GetProductById(thisStockItem.ProductId, context);
            double thisDiscountPercent = thisStockItem.DiscountPercent;
            double thisPrice = UtilMethods.GetDiscountedPrice(thisProduct, thisDiscountPercent);
            string stockString = "";
            if (thisStockItem.StockCount == 0)
            {
                stockString = "sold out";
            }
            else
            {
                stockString = $"{thisStockItem.StockCount} in stock";
            }
            string saleString = "";
            if (thisDiscountPercent != 0)
            {
                saleString = $"{thisDiscountPercent}% Off!";
            }
            else
            {
                saleString = "-";
            }
            string priceString = "";
            if (thisDiscountPercent != 0)
            {
                priceString = $"${thisPrice} each (after discount)";
            }
            else
            {
                priceString = $"${thisPrice} each";
            }
            StockItemViewModel thisStockItemViewModel = new StockItemViewModel(
                thisLocation.LocationId,
                thisProduct.ProductId,
                thisProduct.Name,
                stockString,
                false,
                saleString,
                priceString,
                thisPrice,
                thisLocation.LocationAddress,
                thisProduct.Description);
            return thisStockItemViewModel;
        }

        public static OrderItemViewModel BuildOrderItemViewModelFromCustOrder(Customer thisCustomer, OrderItem thisOrderItem, DbContextClass context)
        {
            Product thisProduct = UtilMethods.GetProductById(thisOrderItem.ProductId, context);
            Location thisLocation = UtilMethods.GetLocationById(thisOrderItem.LocationId, context);
            //string thisAddress = thisLocation.LocationAddress;
            double thisPrice = UtilMethods.TrimPriceDigits(thisOrderItem.TotalPriceWhenOrdered);
            OrderItemViewModel thisOrderItemViewModel = new OrderItemViewModel(
                thisCustomer.CustomerId,
                thisLocation.LocationId,
                thisProduct.ProductId,
                $"{thisCustomer.FirstName} {thisCustomer.LastName}",
                thisProduct.Name,
                thisOrderItem.OrderCount,
                thisPrice,
                thisLocation.LocationAddress,
                thisOrderItem.DateOrdered
                );
            return (thisOrderItemViewModel);
        }

    }
}

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
        /// <summary>
        /// Various static business logic methods
        /// </summary>
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
                _cache.Set("customerCart", new List<OrderItemViewModel>());
                _cache.Set("currentViewedStock", new StockItemViewModel());

            }
            return (_cache);
        }

        public static bool LogInCheck(IMemoryCache _cache)
        {
            /// <summary>
            /// Checks if user is logged in
            /// </summary>
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
            else
            {
                System.Diagnostics.Debug.WriteLine("Warning: No result found");
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
            else
            {
                System.Diagnostics.Debug.WriteLine("Warning: No result found");
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
            else
            {
                System.Diagnostics.Debug.WriteLine("Warning: No result found");
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
            else {
                System.Diagnostics.Debug.WriteLine("Warning: No result found");
            }
            return (gottenStockItem);
        }

        public static StockItem GetStockItemByStockId(int stockId, DbContextClass context)
        {
            ///<summary>
            /// Used for getting stock items for bundle checking
            ///</summary>
            StockItem gottenStockItem = new StockItem();
            var foundStockItems = from thisTableItem in context.StockItems
                                  where thisTableItem.StockItemId == stockId
                                  select thisTableItem;
            if (foundStockItems.Count() > 1)
            {
                throw (new Exception("Error: Duplicate stock item found"));
            }
            else if (foundStockItems.Count() != 0)
            {
                gottenStockItem = foundStockItems.First();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Warning: No result found");
            }
            return (gottenStockItem);
        }

        public static bool CheckProductCount( int locId, int prodId, int orderCount, DbContextClass context)
        {
            /// <summary>
            /// Checks if enough of product is in stock for order
            /// </summary>
            StockItem gottenStockItem = UtilMethods.GetStockItemByIds(locId, prodId, context);
            if (orderCount <= gottenStockItem.StockCount)
            {
                return true;
            }
            return false;
        }

        public static double TrimPriceDigits(double priceIn)
        {
            /// <summary>
            /// Trims double to a presentable 2 digits
            /// </summary>
            return (Math.Truncate(priceIn * 100) / 100);
        }

        public static double GetDiscountedPrice(Product thisProduct, double thisDiscountPercent)
        {
            /// <summary>
            /// Gets a discounted price trimmed to 2 digits
            /// </summary>
            return (TrimPriceDigits(thisProduct.BasePrice * (100 - thisDiscountPercent) / 100));
        }

        public static int GetCartCount(IMemoryCache cache)
        {
            /// <summary>
            /// Gets a count of the items in the cart
            /// </summary>
            List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)cache.Get("customerCart");
            return thisCart.Count();
        }

        public static StockItemViewModel BuildStockItemViewModelFromLocStock(Location thisLocation, StockItem thisStockItem, DbContextClass context)
        {
            /// <summary>
            /// Builds a stock item view model from location and stock item
            /// </summary>
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
            /// <summary>
            /// Builds an order item view model from customer and order item
            /// </summary>
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

        public static List<StockItem> GetChildrenIfBundleParent(OrderItem thisOrderItem, DbContextClass context)
        {
            /// <summary>
            /// Gets the parent stock items of a bundle stock item
            /// </summary>
            StockItem thisStockItem = GetStockItemByIds(thisOrderItem.LocationId, thisOrderItem.ProductId, context);
            var gottenBundleRelations = context.BundleRelations
                    .Where(x => x.StockBundleId == thisStockItem.StockItemId);
            List<StockItem> childStockItemList = new List<StockItem>();
            foreach (BundleRelation thisBundleRelation in gottenBundleRelations.ToList<BundleRelation>())
            {
                StockItem childStockItem = GetStockItemByIds(thisOrderItem.LocationId, thisBundleRelation.SubStockItemId, context);
                if (childStockItem.StockItemId != 0)
                {
                    childStockItemList.Add(childStockItem);
                }
            }
            return childStockItemList;
        }

        public static List<StockItem> GetParentsIfBundleChild(OrderItem thisOrderItem, DbContextClass context)
        {
            /// <summary>
            /// Gets bundle stock items containing a target stock item
            /// </summary>
            StockItem thisStockItem = GetStockItemByIds(thisOrderItem.LocationId, thisOrderItem.ProductId, context);
            var gottenBundleRelations = context.BundleRelations
                    .Where(x => x.SubStockItemId == thisStockItem.StockItemId);
            List<StockItem> parentStockItemList = new List<StockItem>();
            foreach (BundleRelation thisBundleRelation in gottenBundleRelations.ToList<BundleRelation>())
            {
                StockItem parentStockItem = GetStockItemByStockId(thisBundleRelation.StockBundleId, context);

                System.Diagnostics.Debug.WriteLine("--");
                System.Diagnostics.Debug.WriteLine(parentStockItem.StockCount);
                System.Diagnostics.Debug.WriteLine(parentStockItem.ProductId);
                if (parentStockItem.StockItemId != 0)
                {
                    parentStockItemList.Add(parentStockItem);
                }
            }
            return parentStockItemList;
        }

        public static DbContextClass DecrementChildrenIfBundleParent(StockItem targetStockItem, OrderItem thisOrderItem, DbContextClass context)
        {
            /// <summary>
            /// Decreases bundle count to the lowest of its child counts if relevant
            /// If chold count is not a limiting factor in that way, decreases child counts by the same amount
            /// that bundle count was decreased by
            /// </summary>
            List<StockItem> childStockItemList = GetChildrenIfBundleParent(thisOrderItem, context);
            if(childStockItemList.Count() > 0)
            {
                int minChildStockCount = -1;
                foreach(StockItem childStockItem in childStockItemList)
                {
                    if(minChildStockCount == -1 || childStockItem.StockCount < minChildStockCount)
                    {
                        minChildStockCount = childStockItem.StockCount;
                    }
                }
                if(targetStockItem.StockCount > minChildStockCount)
                {
                    System.Diagnostics.Debug.WriteLine("Bundle count exceeds lowest child count. Decreasing to match.");
                    targetStockItem.StockCount = minChildStockCount;
                }
                else
                {
                    foreach (StockItem childStockItem in childStockItemList)
                    {
                        System.Diagnostics.Debug.WriteLine("Decremented child of bundle");
                        childStockItem.StockCount = childStockItem.StockCount - thisOrderItem.OrderCount;
                    }
                }
            }
            return context;
        }

        public static DbContextClass DecrementParentsIfBundleChild(StockItem targetStockItem, OrderItem thisOrderItem, DbContextClass context)
        {
            /// <summary>
            /// Decreases bundle count if it has a child that now has a lower count
            /// </summary>
            List<StockItem> parentStockItemList = GetParentsIfBundleChild(thisOrderItem, context);
            if (parentStockItemList.Count() > 0)
            {
                foreach (StockItem parentStockItem in parentStockItemList)
                {
                    System.Diagnostics.Debug.WriteLine("-------");
                    System.Diagnostics.Debug.WriteLine(targetStockItem.StockCount);
                    System.Diagnostics.Debug.WriteLine(parentStockItem.StockCount);
                    if (targetStockItem.StockCount < parentStockItem.StockCount)
                    {
                        parentStockItem.StockCount = targetStockItem.StockCount;
                        System.Diagnostics.Debug.WriteLine("Decreased bundle count to match child count");
                    }
                }
            }
            return context;
        }

        public static DbContextClass DecrementStockItem(OrderItem thisOrderItem, DbContextClass context)
        {
            /// <summary>
            /// Decrements a stock item's count when ordered, and decrements bundles if relevant
            /// </summary>
            StockItem targetStockItem = context.StockItems
                    .Where(x => x.LocationId == thisOrderItem.LocationId)
                    .Where(x => x.ProductId == thisOrderItem.ProductId).FirstOrDefault();
            targetStockItem.StockCount = targetStockItem.StockCount - thisOrderItem.OrderCount;

            context = DecrementChildrenIfBundleParent(targetStockItem, thisOrderItem, context);
            context = DecrementParentsIfBundleChild(targetStockItem, thisOrderItem, context);
            return context;
        }
    }
}

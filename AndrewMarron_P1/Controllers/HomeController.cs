using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RevatureP1.Models;

namespace RevatureP1.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This is the controller for views after logging in
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;
        private DbContextClass _context;

        public HomeController(ILogger<HomeController> logger, IMemoryCache cache, DbContextClass context)
        {
            /// <summary>
            /// The constructor
            /// </summary>
            _logger = logger;
            _cache = UtilMethods.InitializeCacheIfNeeded(cache);
            _context = context;
        }

        public IActionResult Index()
        {
            /// <summary>
            /// The main user menu
            /// </summary>
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View();
        }

        public IActionResult Cart()
        {
            /// <summary>
            /// The cart page
            /// </summary>
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)_cache.Get("customerCart");
            ViewData["cartcount"] = thisCart.Count();
            return View(thisCart);
        }

        public async Task<IActionResult> Locations()
        {
            /// <summary>
            /// The store list page
            /// </summary>
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(await _context.Locations.ToListAsync());
        }

        public async Task<IActionResult> LocationDetails(int? id)
        {
            /// <summary>
            /// The store stocks page
            /// </summary>
            if (id == null)
            {
                return NotFound();
            }
            int thisLocId = (int)id;
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            var thisLocation = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == thisLocId);
            if (thisLocation == null)
            {
                return NotFound();
            }
            var foundStockItems = from thisTableItem in _context.StockItems
                                  where thisTableItem.LocationId == thisLocation.LocationId
                                  select thisTableItem;
            List<StockItem> theseStockItems = foundStockItems.ToList<StockItem>();
            List<StockItemViewModel> theseStockItemViewModels = new List<StockItemViewModel>();
            foreach (StockItem thisStockItem in theseStockItems)
            {
                StockItemViewModel thisStockItemViewModel = UtilMethods.BuildStockItemViewModelFromLocStock(thisLocation, thisStockItem, _context);
                theseStockItemViewModels.Add(thisStockItemViewModel);
            }
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(theseStockItemViewModels);
        }

        public async Task<IActionResult> StoreHistory(int? id)
        {
            /// <summary>
            /// The store order history page
            /// </summary>
            if (id == null)
            {
                return NotFound();
            }
            int thisLocId = (int)id;
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            var thisLocation = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == thisLocId);
            if (thisLocation == null)
            {
                return NotFound();
            }
            var foundOrderItems = from thisTableItem in _context.OrderItems
                                  where thisTableItem.LocationId == thisLocation.LocationId
                                  select thisTableItem;
            List<OrderItem> theseOrderItems = foundOrderItems.ToList<OrderItem>();
            List<OrderItemViewModel> theseOrderItemViewModels = new List<OrderItemViewModel>();
            foreach (OrderItem thisOrderItem in theseOrderItems)
            {
                Customer thisCustomer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == thisOrderItem.CustomerId);
                OrderItemViewModel thisOrderItemViewModel = UtilMethods.BuildOrderItemViewModelFromCustOrder(thisCustomer, thisOrderItem, _context);
                theseOrderItemViewModels.Add(thisOrderItemViewModel);
            }
            ViewData["storeaddress"] = thisLocation.LocationAddress;
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(theseOrderItemViewModels);
        }

        public async Task<IActionResult> ProductSelect(string locid = "", string prodid = "", string ovarload = "false")
        {
            /// <summary>
            /// The specific product selection page
            /// </summary>
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            if (locid == "")
            {
                return Redirect("/Locations");
            }
            int thisLocId = int.Parse(locid);
            if (prodid == "")
            {
                return Redirect($"/LocationDetails/{thisLocId}");
            }
            int thisProdId = int.Parse(prodid);
            var thisLocation = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == thisLocId);
            var thisProduct = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == thisProdId);
            var thisStockItem = await _context.StockItems
                .FirstOrDefaultAsync(m => m.LocationId == thisLocId && m.ProductId == thisProdId);
            if (thisLocation == null || thisProduct == null || thisStockItem == null)
            {
                return NotFound();
            }
            if (ovarload == "true")
            {
                ViewData["warn"] = "toomuch";
            }
            StockItemViewModel thisStockItemViewModel = UtilMethods.BuildStockItemViewModelFromLocStock(thisLocation, thisStockItem, _context);
            _cache.Set("currentViewedStock", thisStockItemViewModel);
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(thisStockItemViewModel);
        }

        public IActionResult AddToCart(int countin)
        {
            /// <summary>
            /// Adds order item view model to cart
            /// </summary>
            Customer thisCustomer = (Customer)_cache.Get("thisCustomer");
            StockItemViewModel vSt = (StockItemViewModel)_cache.Get("currentViewedStock");
            if (!UtilMethods.CheckProductCount(vSt.LocationId, vSt.ProductId, countin, _context))
            {
                StockItemViewModel thisSIVM = (StockItemViewModel)_cache.Get("currentViewedStock");
                return Redirect($"/Home/ProductSelect?locid={thisSIVM.LocationId}&prodid={thisSIVM.ProductId}&ovarload=true");
            }
            double totalPrice = vSt.Price * countin;
            OrderItemViewModel thisOrder = new OrderItemViewModel(
                thisCustomer.CustomerId,
                vSt.LocationId,
                vSt.ProductId,
                $"{thisCustomer.FirstName} {thisCustomer.LastName}",
                vSt.ProductName,
                countin,
                UtilMethods.TrimPriceDigits(totalPrice),
                vSt.LocationAddress,
                DateTime.Now);
            List<OrderItemViewModel> CurrentCart = (List<OrderItemViewModel>)_cache.Get("customerCart");
            CurrentCart.Add(thisOrder);
            _cache.Set("customerCart", CurrentCart);
            return Redirect("/Home/Cart");
        }

        public async Task<IActionResult> CheckoutCart()
        {
            /// <summary>
            /// Adds cart items to orderitems db then empties cart
            /// </summary>
            List<OrderItemViewModel> CurrentCart = (List<OrderItemViewModel>)_cache.Get("customerCart");
            foreach (OrderItemViewModel thisOIVM in CurrentCart)
            {
                //OrderItem(int CustomerIdIn, int LocationIdIn, int ProductIdIn, double TotalPriceWhenOrderedIn, int OrderCountIn)
                OrderItem thisOrderItem = new OrderItem(
                    thisOIVM.CustomerId,
                    thisOIVM.LocationId,
                    thisOIVM.ProductId,
                    thisOIVM.TotalPriceWhenOrdered,
                    thisOIVM.OrderCount
                    );
                _context.Add(thisOrderItem);
                _context = UtilMethods.DecrementStockItem(thisOrderItem, _context);
            }
            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine($"Orders updated");
            _cache.Set("customerCart", new List<OrderItemViewModel>());
            return Redirect("/Home/History");
        }

        public IActionResult DeleteCartItem(int? id)
        {
            /// <summary>
            /// Deletes a specific cart item
            /// </summary>
            if (id == null)
            {
                return Redirect("/Home/Cart");
            }
            int targetInc = (int)id;
            List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)_cache.Get("customerCart");
            thisCart.RemoveAt(targetInc);
            _cache.Set("customerCart", thisCart);
            return Redirect("/Home/Cart");
        }

        public IActionResult ClearCart()
        {
            /// <summary>
            /// Empties the cart
            /// </summary>
            _cache.Set("customerCart", new List<OrderItemViewModel>());
            return Redirect("/Home/Cart");
        }

        public IActionResult History()
        {
            /// <summary>
            /// User order history page
            /// </summary>
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            Customer thisCustomer = (Customer)_cache.Get("thisCustomer");
            var foundOrderItems = _context.OrderItems.Where(m => m.CustomerId == thisCustomer.CustomerId);
            List<OrderItem> theseOrderItems = foundOrderItems.ToList<OrderItem>();
            List<OrderItemViewModel> theseOrderItemViewModels = new List<OrderItemViewModel>();
            foreach (OrderItem thisOrderItem in theseOrderItems)
            {
                OrderItemViewModel thisOrderItemViewModel = UtilMethods.BuildOrderItemViewModelFromCustOrder(thisCustomer, thisOrderItem, _context);
                theseOrderItemViewModels.Add(thisOrderItemViewModel);
            }
            ViewData["userName"] = $"{thisCustomer.FirstName} {thisCustomer.LastName}";
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(theseOrderItemViewModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            /// <summary>
            /// Error message page
            /// </summary>
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LogOut()
        {
            /// <summary>
            /// Logs the user out and resets cache
            /// </summary>
            _cache.Set("doLogout", "true");
            UtilMethods.InitializeCacheIfNeeded(_cache);
            return Redirect("/Login");
        }
    }
}

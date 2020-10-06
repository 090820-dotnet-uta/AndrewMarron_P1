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
        private readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;
        private DbContextClass _context;

        public HomeController(ILogger<HomeController> logger, IMemoryCache cache, DbContextClass context)
        {
            _logger = logger;
            _cache = UtilMethods.InitializeCacheIfNeeded(cache);
            _context = context;
        }

        public IActionResult Index()
        {
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View();
        }

        public IActionResult Cart()
        {
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
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(await _context.Locations.ToListAsync());
        }
        public async Task<IActionResult> LocationDetails(int? id)
        {
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

        public async Task<IActionResult> ProductSelect(string locid = "", string prodid = "", string ovarload = "false")
        {
            if (!UtilMethods.LogInCheck(_cache))
            {
                return Redirect("/Login");
            }
            if (locid == "")
            {
                return Redirect("/Locations");
            }
            int thisLocId = int.Parse(locid);
            if (locid == "")
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
                StockItem targetStockItem = _context.StockItems
                    .Where(x => x.LocationId == thisOrderItem.LocationId)
                    .Where(x => x.ProductId == thisOrderItem.ProductId).FirstOrDefault();
                targetStockItem.StockCount = targetStockItem.StockCount - thisOrderItem.OrderCount;
            }
            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine($"Orders updated");
            _cache.Set("customerCart", new List<OrderItemViewModel>());
            return Redirect("/Home/History");
        }

        public IActionResult ClearCart()
        {
            _cache.Set("customerCart", new List<OrderItemViewModel>());
            return Redirect("/Home/Cart");
        }

        public IActionResult History()
        {
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
            ViewData["cartcount"] = UtilMethods.GetCartCount(_cache);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LogOut()
        {
            _cache.Set("doLogout", "true");
            UtilMethods.InitializeCacheIfNeeded(_cache);
            return Redirect("/Login");
        }
    }
}

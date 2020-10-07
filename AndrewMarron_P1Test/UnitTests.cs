using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RevatureP1;
using RevatureP1.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RevatureP1Test
{
    public class UnitTests
    {
        [Fact]
        public void InitializeCacheWorks()
        {
            // Act
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            // Arrange
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            var resultVal = (string)_cache.Get("doLogout");
            var expectedVal = "false";
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }
        [Fact]
        public void InitializeCacheOverwrites()
        {
            // Act
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            // Arrange
            _cache.Set("doLogout", "true");
            _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            var resultVal = (string)_cache.Get("doLogout");
            var expectedVal = "false";
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void LogInCheckTrueWithCustomer()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            //// Cache setup
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            //// Cache populating
            Customer testCustomer = new Customer();
            testCustomer.UserName = "testUserName";
            _cache.Set("thisCustomer", testCustomer);
            // Arrange
            var resultVal = RevatureP1.UtilMethods.LogInCheck(_cache);
            var expectedVal = true;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void LogInCheckFalseWithoutCustomer()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            //// Cache setup
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            // Arrange
            var resultVal = RevatureP1.UtilMethods.LogInCheck(_cache);
            var expectedVal = false;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetCustomerByUserNameWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// Cache setup
            //var memoryCache = serviceProvider.GetService<IMemoryCache>();
            //IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// Cache populating
            //_cache.Set("thisCustomer", testCustomer);
            //// DbContext populating
            Customer testCustomer = new Customer();
            string testString = "testUserNameA";
            testCustomer.UserName = testString;
            testContext.Add(testCustomer); // already done
            testContext.SaveChanges();
            // Arrange
            Customer gottenCustomer = RevatureP1.UtilMethods.GetCustomerByUserName(testString, testContext);
            var resultVal = gottenCustomer.UserName;
            var expectedVal = testString;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetCustomerByUserNameHandlesFailuteCorrectly()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Customer testCustomer = new Customer();
            string testString = "testUserName";
            testCustomer.UserName = testString;
            testContext.Add(testCustomer);
            testContext.SaveChanges();
            // Arrange
            Customer gottenCustomer = RevatureP1.UtilMethods.GetCustomerByUserName("otherName", testContext);
            var resultVal = gottenCustomer.UserName;
            var expectedVal = new Customer().UserName;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetLocationByIdWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Location testLocation = new Location();
            int testInt = 11;
            string testString = "testVal";
            testLocation.LocationId = testInt;
            testLocation.LocationAddress = testString;
            testContext.Add(testLocation); // Already done
            testContext.SaveChanges();
            // Arrange
            Location gottenLocation = RevatureP1.UtilMethods.GetLocationById(testInt, testContext);
            var resultVal = gottenLocation.LocationAddress;
            var expectedVal = testString;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetLocationByIdHandlesFailureCorrectly()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Location testLocation = new Location();
            int testInt = 12;
            string testString = "testVal";
            testLocation.LocationId = testInt;
            testLocation.LocationAddress = testString;
            testContext.Add(testLocation); // Already done
            testContext.SaveChanges();
            // Arrange
            Location gottenLocation = RevatureP1.UtilMethods.GetLocationById(111, testContext);
            var resultVal = gottenLocation.LocationAddress;
            var expectedVal = new Location().LocationAddress;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetProductByIdWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Product testProduct = new Product();
            int testInt = 13;
            string testString = "testVal";
            testProduct.ProductId = testInt;
            testProduct.Name = testString;
            testContext.Add(testProduct);
            testContext.SaveChanges();
            // Arrange
            Product gottenProduct = RevatureP1.UtilMethods.GetProductById(testInt, testContext);
            var resultVal = gottenProduct.Name;
            var expectedVal = testString;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetProductByIdHandlesFailureCorrectly()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Product testProduct = new Product();
            int testInt = 15;
            string testString = "testVal";
            testProduct.ProductId = testInt;
            testProduct.Name = testString;
            testContext.Add(testProduct);
            testContext.SaveChanges();
            // Arrange
            Product gottenProduct = RevatureP1.UtilMethods.GetProductById(2, testContext);
            var resultVal = gottenProduct.Name;
            var expectedVal = new Product().Name;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetStockItemByIdWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            StockItem testStockItem = new StockItem();
            int testInt = 101;
            int testInt2 = 201;
            int testInt3 = 301;
            //string testString = "testVal";
            testStockItem.LocationId = testInt;
            testStockItem.ProductId = testInt2;
            testStockItem.StockCount = testInt3;
            testContext.Add(testStockItem);
            testContext.SaveChanges();
            // Arrange
            StockItem gottenStockItem = RevatureP1.UtilMethods.GetStockItemByIds(testInt, testInt2, testContext);
            var resultVal = gottenStockItem.StockCount;
            var expectedVal = testInt3;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetStockItemByIdHandlesFailureCorrectly()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            StockItem testStockItem = new StockItem();
            int testInt = 102;
            int testInt2 = 202;
            int testInt3 = 302;
            //string testString = "testVal";
            testStockItem.LocationId = testInt;
            testStockItem.ProductId = testInt2;
            testStockItem.StockCount = testInt3;
            testContext.Add(testStockItem);
            testContext.SaveChanges();
            // Arrange
            StockItem gottenStockItem = RevatureP1.UtilMethods.GetStockItemByIds(4, 4, testContext);
            var resultVal = gottenStockItem.StockCount;
            var expectedVal = new StockItem().StockCount;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void ProductCountWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            StockItem testStockItem = new StockItem();
            int testInt = 103;
            int testInt2 = 203;
            int testInt3 = 303;
            int testint4 = 203;
            //string testString = "testVal";
            testStockItem.LocationId = testInt;
            testStockItem.ProductId = testInt2;
            testStockItem.StockCount = testInt3;
            testContext.Add(testStockItem);
            testContext.SaveChanges();
            // Arrange
            bool enoughInStock = RevatureP1.UtilMethods.CheckProductCount(testInt, testInt2, testint4, testContext);
            var resultVal = enoughInStock;
            var expectedVal = true;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void ProductCountReturnsFalseWhenItShould()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            StockItem testStockItem = new StockItem();
            int testInt = 104;
            int testInt2 = 204;
            int testInt3 = 304;
            int testint4 = 404;
            //string testString = "testVal";
            testStockItem.LocationId = testInt;
            testStockItem.ProductId = testInt2;
            testStockItem.StockCount = testInt3;
            testContext.Add(testStockItem);
            testContext.SaveChanges();
            // Arrange
            bool enoughInStock = RevatureP1.UtilMethods.CheckProductCount(testInt, testInt2, testint4, testContext);
            var resultVal = enoughInStock;
            var expectedVal = false;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void TrimPriceDigitsWorks()
        {
            // Act
            double testDouble = (double)1.011;
            // Arrange
            var resultVal = RevatureP1.UtilMethods.TrimPriceDigits(testDouble);
            var expectedVal = (double)1.01;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetDiscountedPriceWorks()
        {
            // Act
            double testDiscountPercent = (double)50;
            Product thisProduct = new Product();
            thisProduct.BasePrice = (double)6;
            // Arrange
            var resultVal = RevatureP1.UtilMethods.GetDiscountedPrice(thisProduct, testDiscountPercent);
            var expectedVal = (double)3;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetCartCountWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            //// Cache setup
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            //// Cache populating
            OrderItemViewModel testOrderItemViewModel = new OrderItemViewModel();
            List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)memoryCache.Get("customerCart");
            thisCart.Add(testOrderItemViewModel);
            memoryCache.Set("customerCart", thisCart);
            // Arrange
            var resultVal = RevatureP1.UtilMethods.GetCartCount(memoryCache);
            var expectedVal = 1;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void GetCartCountWorksWhenEmpty()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            //// Cache setup
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            IMemoryCache _cache = RevatureP1.UtilMethods.InitializeCacheIfNeeded(memoryCache);
            //// Cache populating
            //OrderItemViewModel testOrderItemViewModel = new OrderItemViewModel();
            //List<OrderItemViewModel> thisCart = (List<OrderItemViewModel>)memoryCache.Get("customerCart");
            //thisCart.Add(testOrderItemViewModel);
            //memoryCache.Set("customerCart", thisCart);
            // Arrange
            var resultVal = RevatureP1.UtilMethods.GetCartCount(memoryCache);
            var expectedVal = 0;
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void BuildStockItemViewModelFromLocStockWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Location testLocation = new Location();
            StockItem testStockItem = new StockItem();
            testStockItem.DiscountPercent = 50;
            Product testProduct = new Product();
            testProduct.BasePrice = 6;
            testContext.Add(testProduct);
            testContext.SaveChanges();
            // Arrange
            StockItemViewModel testStockItemViewModel = RevatureP1.UtilMethods.BuildStockItemViewModelFromLocStock(
                testLocation, testStockItem, testContext);
            var resultVal = testStockItemViewModel.SaleString;
            var expectedVal = "50% Off!";
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }

        [Fact]
        public void BuildOrderItemViewModelFromCustOrderWorks()
        {
            // Act
            //// Service setup
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddDbContext<DbContextClass>(options => options.UseInMemoryDatabase("testDb"));
            var serviceProvider = services.BuildServiceProvider();
            //// DbContext setup
            DbContextClass testContext = serviceProvider.GetService<DbContextClass>();
            //// DbContext populating
            Customer testCustomer = new Customer();
            testCustomer.FirstName = "fn";
            testCustomer.LastName = "ln";
            Location testLocation = new Location();
            StockItem testStockItem = new StockItem();
            testStockItem.DiscountPercent = 50;
            Product testProduct = new Product();
            OrderItem testOrderItem = new OrderItem();
            testOrderItem.TotalPriceWhenOrdered = 3;
            testProduct.BasePrice = 6;
            testContext.Add(testProduct);
            testContext.Add(testLocation);
            testContext.SaveChanges();
            // Arrange
            OrderItemViewModel testOrderItemViewModel = RevatureP1.UtilMethods.BuildOrderItemViewModelFromCustOrder(
                testCustomer, testOrderItem, testContext);
            var resultVal = testOrderItemViewModel.CustomerName;
            var expectedVal = "fn ln";
            //Assert
            Assert.Equal(expectedVal, resultVal);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RevatureP1.Models
{
    public class Product // Products (including Bundles in general (with instances at specific places instead being StickItems)
    {
        /// <summary>
        /// This stores information about each product
        /// This corresponds to the table Products in the database
        /// Information about stocks of products in stores is instead in StockItem
        /// </summary>
        private int productId;
        private string name;
        private double basePrice;
        private string description;
        private DateTime dateAdded;
        private List<StockItem> inventoryItems;
        public int ProductId { get => productId; set => productId = value; }
        public string Name { get => name; set => name = value; }
        public double BasePrice { get => basePrice; set => basePrice = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
        public List<StockItem> InventoryItems { get => inventoryItems; set => inventoryItems = value; }

        public Product() { }
        public Product(string ProductNameIn, double ProductPriceIn, string ProductDescriptionIn)
        {
            Name = ProductNameIn;
            BasePrice = ProductPriceIn;
            Description = ProductDescriptionIn;
            DateAdded = DateTime.Now;
        }
        public double GetTotalPrice(StockItem fromStock)
        {
            return (this.BasePrice * (100 - fromStock.DiscountPercent) / 100);
        }
    }
}

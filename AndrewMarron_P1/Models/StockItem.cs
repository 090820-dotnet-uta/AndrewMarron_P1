using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Models
{
    public class StockItem
    {
        /// <summary>
        /// This stores information about an instance of a product (with stock count) sold by a specific store
        /// This corresponds to the table StockItems in the database
        /// LocationId and ProductId are together a secondary composite key in the db, since each location can only have one instance of a product
        /// </summary>
        private int stockItemId;
        private int locationId;
        private int productId;
        private int stockCount;
        private bool isBundle = false;
        private float discountPercent = 0;
        private DateTime dateAdded;
        private Location thisLocation;
        private Product product;
        public int StockItemId { get => stockItemId; set => stockItemId = value; }
        public int LocationId { get => locationId; set => locationId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int StockCount { get => stockCount; set => stockCount = value; }
        public bool IsBundle { get => isBundle; set => isBundle = value; }
        public float DiscountPercent { get => discountPercent; set => discountPercent = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
        internal Location ThisLocation { get => thisLocation; set => thisLocation = value; }
        internal Product Product { get => product; set => product = value; }
        public StockItem()
        {

        }
        public StockItem(int LocationIdIn, int ProductIdIn, int StockCountIn, bool IsBundleIn, float DiscountPercentIn)
        {
            StockCount = StockCountIn;
            IsBundle = IsBundleIn;
            DiscountPercent = DiscountPercentIn;
            LocationId = LocationIdIn;
            ProductId = ProductIdIn;
            DateAdded = DateTime.Now;
        }
    }
}

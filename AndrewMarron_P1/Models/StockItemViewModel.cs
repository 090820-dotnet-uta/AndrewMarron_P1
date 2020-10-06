using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Models
{
    public class StockItemViewModel
    {
        /// <summary>
        /// A version of StockItem for outputting to the view (with properties from other tables added)
        /// </summary>
        private int locationId;
        private int productId;
        private string productName;
        private string stockCount;
        private bool isBundle = false;
        private string saleString;
        private double price;
        private string priceString;
        private string locationAddress;
        private string descriptionString;
        public int LocationId { get => locationId; set => locationId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public string StockCount { get => stockCount; set => stockCount = value; }
        public bool IsBundle { get => isBundle; set => isBundle = value; }
        public string SaleString { get => saleString; set => saleString = value; }
        public string PriceString { get => priceString; set => priceString = value; }
        public double Price { get => price; set => price = value; }
        public string LocationAddress { get => locationAddress; set => locationAddress = value; }
        public string DescriptionString { get => descriptionString; set => descriptionString = value; }

        public StockItemViewModel()
        {

        }
        public StockItemViewModel(
            int LocationIdIn,
            int ProductIdIn,
            string ProductNameIn,
            string StockCountIn,
            bool IsBundleIn,
            string SaleStringIn,
            string PriceStringIn,
            double PriceIn,
            string LocationAddressIn,
            string DescriptionStringIn
            )
        {
            LocationId = LocationIdIn;
            ProductId = ProductIdIn;
            ProductName = ProductNameIn;
            StockCount = StockCountIn;
            IsBundle = IsBundleIn;
            SaleString = SaleStringIn;
            PriceString = PriceStringIn;
            Price = PriceIn;
            LocationAddress = LocationAddressIn;
            DescriptionString = DescriptionStringIn;
        }
    }
}

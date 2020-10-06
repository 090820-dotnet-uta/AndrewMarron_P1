using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Models // A product orderd by a customer from a location
{
    public class OrderItemViewModel
    {
        /// <summary>
        /// A version of StockItem for outputting to the view (with properties from other tables added)
        /// </summary>
        private int orderItemId;
        private int customerId;
        private int locationId;
        private int productId;
        private string customerName;
        private string locationAddress;
        private string productName;
        private double totalPriceWhenOrdered; // This is stored seperately from Product or StockItem since those might change after order
        private int orderCount;
        private DateTime dateOrdered;
        public int OrderItemId { get => orderItemId; set => orderItemId = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int LocationId { get => locationId; set => locationId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public string CustomerName { get => customerName; set => customerName = value; }
        public string LocationAddress { get => locationAddress; set => locationAddress = value; }
        public string ProductName { get => productName; set => productName = value; }
        public int OrderCount { get => orderCount; set => orderCount = value; }
        public double TotalPriceWhenOrdered { get => totalPriceWhenOrdered; set => totalPriceWhenOrdered = value; }
        public DateTime DateOrdered { get => dateOrdered; set => dateOrdered = value; }

        public OrderItemViewModel()
        {

        }
        public OrderItemViewModel(
            int CustomerIdIn, 
            int LocationIdIn, 
            int ProductIdIn,
            string CustomerNameIn, 
            string ProductNameIn, 
            int OrderCountIn, 
            double TotalPriceWhenOrderedIn, 
            string LocationAddressIn,
            DateTime DateOrderedIn)
        {
            CustomerId = CustomerIdIn;
            LocationId = LocationIdIn;
            ProductId = ProductIdIn;
            CustomerName = CustomerNameIn;
            ProductName = ProductNameIn;
            OrderCount = OrderCountIn;
            TotalPriceWhenOrdered = TotalPriceWhenOrderedIn;
            LocationAddress = LocationAddressIn;
            DateOrdered = DateOrderedIn;
        }
    }
}

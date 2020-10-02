using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Models // A product orderd by a customer from a location
{
    public class OrderItem
    {
        /// <summary>
        /// This object stores information for each time a product (or several of a product) is ordered
        /// This corresponds to the OrderItems table in the database
        /// </summary>
        private int orderItemId;
        private int customerId;
        private int locationId;
        private int productId;
        private double totalPriceWhenOrdered; // This is stored seperately from Product or StockItem since those might change after order
        private int orderCount;
        private DateTime dateOrdered;
        private Location thisLocation;
        private Product product;
        private Customer customer;
        public int OrderItemId { get => orderItemId; set => orderItemId = value; }
        public int CustomerId { get => customerId; set => customerId = value; }
        public int LocationId { get => locationId; set => locationId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int OrderCount { get => orderCount; set => orderCount = value; }
        public double TotalPriceWhenOrdered { get => totalPriceWhenOrdered; set => totalPriceWhenOrdered = value; }
        public DateTime DateOrdered { get => dateOrdered; set => dateOrdered = value; }
        internal Location ThisLocation { get => thisLocation; set => thisLocation = value; }
        internal Product Product { get => product; set => product = value; }
        internal Customer Customer { get => customer; set => customer = value; }

        public OrderItem()
        {

        }
        public OrderItem(int CustomerIdIn, int LocationIdIn, int ProductIdIn, double TotalPriceWhenOrderedIn, int OrderCountIn)
        {
            OrderCount = OrderCountIn;
            TotalPriceWhenOrdered = TotalPriceWhenOrderedIn;
            CustomerId = CustomerIdIn;
            LocationId = LocationIdIn;
            ProductId = ProductIdIn;
            DateOrdered = DateTime.Now;
        }
    }
}

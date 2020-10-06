using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public int OrderItemId { get => orderItemId; set => orderItemId = value; }

        [Required]
        public int CustomerId { get => customerId; set => customerId = value; }

        [Required]
        public int LocationId { get => locationId; set => locationId = value; }

        [Required]
        public int ProductId { get => productId; set => productId = value; }

        [Range(0, 999999)]
        [Required]
        public int OrderCount { get => orderCount; set => orderCount = value; }

        [Range(0, 999999)]
        [Required]
        public double TotalPriceWhenOrdered { get => totalPriceWhenOrdered; set => totalPriceWhenOrdered = value; }
        public DateTime DateOrdered { get => dateOrdered; set => dateOrdered = value; }

        [Required]
        [StringLength(500, MinimumLength = 2)]
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

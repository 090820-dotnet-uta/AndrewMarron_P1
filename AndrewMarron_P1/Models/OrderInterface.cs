using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevatureP1.Models
{
    interface OrderInterface
    {
        /// <summary>
        /// OrderItem properties shared with order model view
        /// </summary>
        static int orderItemId;
        static int customerId;
        static int locationId;
        static int productId;
        static double totalPriceWhenOrdered; // This is stored seperately from Product or StockItem since those might change after order
        static int orderCount;
        static DateTime dateOrdered;
    }
}

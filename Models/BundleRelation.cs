using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

//Placeholder; not yer implemented

namespace RevatureP1.Models
{
    public class BundleRelation
    {
        /// <summary>
        /// Bundles were not implemented due to time constraints
        /// This table was designed to hold relationships between a Product that is a bundle and each other product within the bundle
        /// </summary>
        private int bundleRelationId;
        private int stockBundleId; // This is for the bundle stockItem
        private int subStockItemId; // This is for the contained sub-stockItem
        private StockItem stockBundle; // This is for the bundle stockItem
        private StockItem subStockItem; // This is for the bundle stockItem
        public int BundleRelationId { get => bundleRelationId; set => bundleRelationId = value; }
        public int StockBundleId { get => stockBundleId; set => stockBundleId = value; }
        public int SubStockItemId { get => subStockItemId; set => subStockItemId = value; }
        internal StockItem StockBundle { get => stockBundle; set => stockBundle = value; }
        internal StockItem SubStockItem { get => subStockItem; set => subStockItem = value; }
    }
}

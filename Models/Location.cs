using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RevatureP1.Models
{
    public class Location
    {
        /// <summary>
        /// This object stores information about each store location
        /// This corresponds with the database table Locations
        /// </summary>
        private int locationId;
        private string locationAddress;
        private DateTime dateAdded;
        private List<StockItem> inventoryItems;

        public int LocationId { get => locationId; set => locationId = value; }
        public string LocationAddress { get => locationAddress; set => locationAddress = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
        public List<StockItem> InventoryItems { get => inventoryItems; set => inventoryItems = value; }
        public Location() { }
        public Location(string LocationAddressIn)
        {
            LocationAddress = LocationAddressIn;
            DateAdded = DateTime.Now;
        }
    }
}

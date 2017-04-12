using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class CafeMenu
    {
        public int itemID { get; set; }
        public string itemImg { get; set; }
        public string itemName { get; set; }
        public string itemCategory { get; set; }
        public string itemSubCategory { get; set; }
        public decimal itemPrice { get; set; }
        public string itemDesc { get; set; }
        public string itemRequest { get; set; }
        public int itemQty { get; set; }
        public string itemStatus { get; set; }
        public int combo { get; set; }
        public int orders { get; set; }
        public string label { get; set; }

        public CafeMenu(string itemImg, string itemName, string itemCategory, string itemSubCategory, decimal itemPrice, string itemDesc, string itemRequest, int itemQty, string itemStatus, int combo, int orders, string label)
        {
            this.itemImg = itemImg;
            this.itemName = itemName;
            this.itemCategory = itemCategory;
            this.itemSubCategory = itemSubCategory;
            this.itemPrice = itemPrice;
            this.itemDesc = itemDesc;
            this.itemRequest = itemRequest;
            this.itemQty = itemQty;
            this.itemStatus = itemStatus;
            this.combo = combo;
            this.orders = orders;
            this.label = label;
        }

        public CafeMenu()
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class OrderItem
    {
        public int orderID { get; set; }
        public int itemID { get; set; }
        public string itemName { get; set; }
        public int itemQty { get; set; }
        public string itemRequest { get; set; }
        public string specialRequest { get; set; }
        public decimal itemPrice { get; set; }
        public int quantity { get; set; }
        public int orderItemID { get; set; }
        public string itemImg { get; set; }
        public decimal discount { get; set; }
        public decimal GST { get; set; }
        public decimal grandTotal { get; set; }
        public string orderType { get; set; }
        public string orderDateTime { get; set; }
        public string tableNumber { get; set; }
        public string preorderDateTime { get; set; }
        public int PIN { get; set; }
        public string InvoiceNo { get; set; }


        public OrderItem(int orderID, int itemID, string itemName, int itemQty, string itemRequest, string specialRequest, decimal itemPrice, int quantity)
        {
            this.orderID = orderID;
            this.itemID = itemID;
            this.itemName = itemName;
            this.itemQty = itemQty;
            this.itemRequest = itemRequest;
            this.specialRequest = specialRequest;
            this.itemPrice = itemPrice;
            this.quantity = quantity;
        }
        public OrderItem()
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class Order
    {
        public int orderID { get; set; }
        public int custID { get; set; }
        public decimal discount { get; set; }
        public decimal GST { get; set; }
        public decimal grandTotal { get; set; }
        public string orderDateTime { get; set; }
        public OrderItem[] items { get; set; }
        public string tableNumber { get; set; }
        public string orderType { get; set; }
        public string preorderDateTime { get; set; }
        public string InvoiceNo { get; set; }
        public int PIN { get; set; }

        public Order(int orderID, int custID, decimal discont, decimal GST, decimal grandTotal, string orderDateTime, OrderItem[] items, string tableNumber, string orderType, string InvoiceNo)
        {
            this.orderID = orderID;
            this.custID = custID;
            this.discount = discount;
            this.GST = GST;
            this.grandTotal = grandTotal;
            this.orderDateTime = orderDateTime;
            this.items = items;
            this.tableNumber = tableNumber;
            this.orderType = orderType;
            this.InvoiceNo = InvoiceNo;
        }

        public Order(int orderID, int custID, decimal discont, decimal GST, decimal grandTotal, string orderDateTime, OrderItem[] items, string tableNumber, string orderType, string preorderDateTime, string InvoiceNo)
        {
            this.orderID = orderID;
            this.custID = custID;
            this.discount = discount;
            this.GST = GST;
            this.grandTotal = grandTotal;
            this.orderDateTime = orderDateTime;
            this.items = items;
            this.tableNumber = tableNumber;
            this.orderType = orderType;
            this.preorderDateTime = preorderDateTime;
            this.InvoiceNo = InvoiceNo;
        }

        public Order(int orderID, int custID, decimal discont, decimal GST, decimal grandTotal, string orderDateTime, OrderItem[] items, string orderType, string InvoiceNo)
        {
            this.orderID = orderID;
            this.custID = custID;
            this.discount = discount;
            this.GST = GST;
            this.grandTotal = grandTotal;
            this.orderDateTime = orderDateTime;
            this.items = items;
            this.tableNumber = tableNumber;
            this.orderType = orderType;
            this.preorderDateTime = preorderDateTime;
            this.InvoiceNo = InvoiceNo;
        }
        public Order()
        {

        }
    }
}
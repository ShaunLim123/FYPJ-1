using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class OrdersDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();


        //Insert for Orders
        public int CreateDineInOrder(int custId, decimal grandTotal, decimal discount, decimal gst, string orderType, string tableNumber, OrderItem[] items)
        {
            int num = 0;
            DateTime now = DateTime.Now;
            Random generator = new Random();
            int r = generator.Next(1, 1000000);
            int PIN = Convert.ToInt32(r.ToString().PadLeft(6, '0'));

            Random rand = new Random();
            var x = rand.Next(0, 1000000);
            string InvoiceNo = "IN" + x.ToString("000000");

            string query = "INSERT into Orders(custID, grandTotal, orderDateTime, discount, gst, orderType, tableNumber, PIN, InvoiceNo) values (@custId, @grandTotal, @orderDateTime, @discount, @gst, @orderType, @tableNumber, @PIN, @InvoiceNo)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@custId", custId);
            cmd.Parameters.AddWithValue("@grandTotal", grandTotal);
            cmd.Parameters.AddWithValue("@orderDateTime", now);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@gst", gst);
            cmd.Parameters.AddWithValue("@orderType", orderType);
            cmd.Parameters.AddWithValue("@tableNumber", tableNumber);
            cmd.Parameters.AddWithValue("@PIN", PIN);
            cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);

            sc.Open();
            try
            {
                num += cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            if(num == 1)
            {
                int OrderId = OrdersDAL.retrieveOrderIdByDateTime(now);
                OrdersDAL odal = new OrdersDAL();
                foreach (var item in items)
                {
                    odal.CreateOrderItems(item, OrderId);
                }
            }

            sc.Close();
            sc.Dispose();
            return PIN;
        }

        public int CreatePreorderOrder(int custId, decimal grandTotal, decimal discount, decimal gst, string orderType, string preorderDateTime, OrderItem[] items)
        {
            Random rand = new Random();
            var x = rand.Next(0, 1000000);
            string InvoiceNo = "IN" + x.ToString("000000");
            int num = 0;
            DateTime now = DateTime.Now;
            string query = "INSERT into Orders(custID, grandTotal, orderDateTime, discount, gst, orderType, preorderDateTime, InvoiceNo) values (@custId,@grandTotal, @orderDateTime, @discount, @gst, @orderType, @preorderDateTime, @InvoiceNo)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@custId", custId);
            cmd.Parameters.AddWithValue("@grandTotal", grandTotal);
            cmd.Parameters.AddWithValue("@orderDateTime", now);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@gst", gst);
            cmd.Parameters.AddWithValue("@orderType", orderType);
            cmd.Parameters.AddWithValue("@preorderDateTime", preorderDateTime);
            cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);

            sc.Open();
            try
            {
                num += cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            if (num == 1)
            {
                int OrderId = OrdersDAL.retrieveOrderIdByDateTime(now);
                OrdersDAL odal = new OrdersDAL();
                foreach (var item in items)
                {
                    odal.CreateOrderItems(item, OrderId);
                }
            }

            sc.Close();
            sc.Dispose();
            return num;
        }

        // Insert for Order Items
        public int CreateOrderItems(OrderItem o,int orderID)
        {

            string query = "INSERT into OrderItem (orderID, itemID, itemQty, itemRequest, specialRequest, itemPrice) values (@orderID, @itemID, @quantity, @itemRequest, @specialRequest, @itemPrice)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@itemID", o.itemID);
            cmd.Parameters.AddWithValue("@itemName", o.itemName);
            cmd.Parameters.AddWithValue("@quantity", o.quantity);
            cmd.Parameters.AddWithValue("@itemRequest", o.itemRequest);
            cmd.Parameters.AddWithValue("@specialRequest", o.specialRequest);
            cmd.Parameters.AddWithValue("@itemPrice", o.itemPrice);

            sc.Open();
            int insert = cmd.ExecuteNonQuery();
            sc.Close();
            return insert;
        }

        //public int RetrieveQtyByItemID(int itemID)
        //{
        //    string query = "SELECT itemQty FROM Menu WHERE itemID = @itemID";
        //    SqlConnection sc = new SqlConnection(con);
        //    SqlCommand cmd = new SqlCommand(query, sc);

        //    cmd.Parameters.AddWithValue("@itemID", itemID);

        //    sc.Open();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    int itemQty = Convert.ToInt32(dt.Rows[0]["itemQty"]);
        //    sc.Close();
        //    return itemQty;

        //}
        public int UpdateMenuQtyByID(int itemID, int itemQty)
        {
            string query = "UPDATE Menu SET itemQty=@itemQty, orders = orders+1  WHERE itemID = @itemID";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);
            cmd.Parameters.AddWithValue("@itemQty", itemQty);
            cmd.Parameters.AddWithValue("@itemID", itemID);

            sc.Open();
            int update = cmd.ExecuteNonQuery();
            sc.Close();
            return update;
        }


        public static int retrieveOrderIdByDateTime(DateTime now)
        {
            int id = 0;
            string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from  Orders WHERE orderDateTime = @orderDateTime";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@orderDateTime", now);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        id = Convert.ToInt32(rr["orderID"].ToString());
                    }
                    myConnection.Close();
                }

            }
            return id;
        }



        //Retrieve for Orders
        public List<Order> retrieveOrderList(int id)
        {
            List<Order> orderList = new List<Order>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Orders WHERE custID = @custID";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@custID", id);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        Order order = new Order();
                        order.custID = Convert.ToInt32(rr["custID"].ToString());
                        order.orderID = Convert.ToInt32(rr["orderID"].ToString());                        
                        order.orderDateTime = rr["orderDateTime"].ToString();
                        order.orderType = rr["orderType"].ToString();
                        order.discount = Convert.ToDecimal(rr["discount"].ToString());
                        order.GST = Convert.ToDecimal(rr["gst"].ToString());
                        order.grandTotal = Convert.ToDecimal(rr["grandTotal"].ToString());
                        order.InvoiceNo = rr["InvoiceNo"].ToString();

                        if (order.orderType == "Dine-In")
                        {
                            order.PIN = Convert.ToInt32(rr["PIN"].ToString());
                            order.tableNumber = rr["tableNumber"].ToString();
                        }
                        else
                        {
                            order.preorderDateTime = rr["preorderDateTime"].ToString();
                        }

                        orderList.Add(order);
                    }
                    myConnection.Close();
                }

            }
            return orderList;
        }

        //Retrieve for Order Items
        public List<OrderItem> retrieveOrderItems(int orderID)
        {
            List<OrderItem> orderItem = new List<OrderItem>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * , m.itemImg, d.grandTotal, d.discount, d.gst, d.orderDateTime, d.tableNumber, d.orderType, d.preorderDateTime, d.PIN FROM OrderItem o INNER JOIN Menu m ON o.itemID = m.itemID INNER JOIN Orders d ON o.orderID = d.orderID WHERE o.orderID = @orderID";

                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@orderID", orderID);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        OrderItem items = new OrderItem();
                        items.itemImg = rr["itemImg"].ToString();
                        items.orderID = Convert.ToInt32(rr["orderID"].ToString());
                        items.orderItemID = Convert.ToInt32(rr["orderitemID"].ToString());
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemPrice = Convert.ToDecimal(rr["itemPrice"].ToString());
                        items.itemName = rr["itemName"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.PIN = 0;
                        items.preorderDateTime = rr["preorderDateTime"].ToString();
                        items.discount = Convert.ToDecimal(rr["discount"].ToString());
                        items.GST = Convert.ToDecimal(rr["gst"].ToString());
                        items.grandTotal = Convert.ToDecimal(rr["grandTotal"].ToString());
                        items.specialRequest = rr["specialRequest"].ToString();
                        items.orderDateTime = rr["orderDateTime"].ToString();
                        items.tableNumber = rr["tableNumber"].ToString();
                        items.orderType = rr["orderType"].ToString();
                        items.InvoiceNo = rr["InvoiceNo"].ToString();
                        if( rr["PIN"].ToString() == "")
                        {
                            items.PIN = 0;
                        }
                        else
                        {
                            items.PIN = Convert.ToInt32(rr["PIN"].ToString());
                        }
                      
     
                        


                        orderItem.Add(items);
                    }

                    myConnection.Close();
                }

            }
            return orderItem;
        }


        //Update for Orders
        public int updateOrders(int orderID, int custID, decimal orderTotalPrice, DateTime orderDateTime, decimal discount, decimal gst, int tableNumber)
        {
            int result = 0;

            string queryStr = "UPDATE Orders SET custID=@custID, grandTotal=@orderTotalPrice, orderDateTime=@orderDateTime, discount=@discount, gst=@gst, tableNumber=@tableNumber WHERE orderID=@orderID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@orderTotalPrice", orderTotalPrice);
            cmd.Parameters.AddWithValue("@orderDateTime", orderDateTime);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@gst", gst);
            cmd.Parameters.AddWithValue("@tableNumber", tableNumber);

            sc.Open();
            result += cmd.ExecuteNonQuery();
            sc.Close();
            return result;

        }

        //Update for Order Items
        public int updateOrderItems(int orderID, int itemID, string itemName, int orderQty, string itemRequest, string specialRequest, decimal itemPrice)
        {
            int result = 0;

            string queryStr = "UPDATE OrderItem SET itemID=@itemID, itemName=@itemName, orderQty=@orderQty, itemRequest=@itemRequest, specialRequest=@specialRequest, itemPrice=@itemPrice WHERE orderID=@orderID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@itemID", itemID);
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@orderQty", orderQty);
            cmd.Parameters.AddWithValue("@itemRequest", itemRequest);
            cmd.Parameters.AddWithValue("@specialRequest", specialRequest);
            cmd.Parameters.AddWithValue("@itemPrice", itemPrice);

            sc.Open();
            result += cmd.ExecuteNonQuery();
            sc.Close();
            return result;

        }

        //Delete for OrderItems
        public void itemDelete(int itemID)
        {
            string query = "DELETE * FROM OrderItem WHERE itemID = @itemID";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);
            cmd.Parameters.AddWithValue("@itemID", itemID);
        }

        public bool checkCustIDint(int custID)
        {
            string custID1 = "";
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Orders where custID = @custID";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@custID", custID);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        custID = Convert.ToInt32(rr["custID"]);
                        return true;
                    }
                    myConnection.Close();
                }
            }
            return false;
        }
    }

}
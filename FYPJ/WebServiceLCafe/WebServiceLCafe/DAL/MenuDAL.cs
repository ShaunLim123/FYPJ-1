using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;
using System.Diagnostics;

namespace WebServiceLCafe.DAL
{
    public class MenuDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();


        //CREATE
        public int CreateItem(CafeMenu m)
        {
            string query = "INSERT into Menu (itemImg, itemName, itemCategory, itemSubCategory, itemPrice, itemDesc, itemRequest, itemQty, itemStatus, combo, orders) values (@itemImg, @itemName, @itemCategory, @itemSubCategory, @itemPrice, @itemDesc, @itemRequest, @itemQty, @itemStatus, @combo,@orders)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@itemImg", m.itemImg);
            cmd.Parameters.AddWithValue("@itemName", m.itemName);
            cmd.Parameters.AddWithValue("@itemCategory", m.itemCategory);
            cmd.Parameters.AddWithValue("@itemSubCategory", m.itemSubCategory);
            cmd.Parameters.AddWithValue("@itemPrice", m.itemPrice);
            cmd.Parameters.AddWithValue("@itemDesc", m.itemDesc);
            cmd.Parameters.AddWithValue("@itemRequest", m.itemRequest);
            cmd.Parameters.AddWithValue("@itemQty", m.itemQty);
            cmd.Parameters.AddWithValue("@itemStatus", m.itemStatus);
            cmd.Parameters.AddWithValue("@combo", m.combo);
            cmd.Parameters.AddWithValue("@orders", m.combo);

            sc.Open();
            int insert = cmd.ExecuteNonQuery();
            sc.Close();
            return insert;
        }

        //RETRIEVE
        public List<CafeMenu> retrieveAllItems()
        {
            List<CafeMenu> allItems = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = Math.Round(decimal.Parse(rr["itemPrice"].ToString()), 2);
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();
                        items.combo = Convert.ToInt32(rr["combo"].ToString());
                        items.orders = Convert.ToInt32(rr["orders"].ToString());

                        allItems.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allItems;
        }
        // Retrieve drinks list
        /*public List<CafeMenu> retrieveBeverages()
        {
            List<CafeMenu> allBeverages = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu WHERE itemCategory = 'Beverages' ";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = decimal.Parse(rr["itemPrice"].ToString());
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();

                        allBeverages.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allBeverages;
        }
        // End retrieve drinks list


        // Retrieve food list
        public List<CafeMenu> retrieveFood()
        {
            List<CafeMenu> allFood = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu WHERE itemCategory = 'Food' ";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = decimal.Parse(rr["itemPrice"].ToString());
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();

                        allFood.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allFood;
        }
        // End retrieve food list
        */

        public List<CafeMenu> retrieveMenuByItemCategory(string itemCategory)
        {
            List<CafeMenu> allItems = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu WHERE itemCategory = @itemCategory AND label=@label";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@itemCategory", itemCategory);
                oCmd.Parameters.AddWithValue("@label", "none");
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = decimal.Parse(rr["itemPrice"].ToString());
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();
                        items.combo = Convert.ToInt32(rr["combo"].ToString());
                        items.orders = Convert.ToInt32(rr["orders"].ToString());

                        allItems.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allItems;
        }

        public int retrieveItemQty(int id)
        {
            int itemQty = 0;
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT itemQty from Menu WHERE itemID = @itemID";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@itemID", id);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                    }

                    myConnection.Close();
                }
            }
            return itemQty;
        }
        public List<CafeMenu> retrieveMenuByLabel(string label)
        {
            List<CafeMenu> allItems = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu WHERE label = @label";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@label", label);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = decimal.Parse(rr["itemPrice"].ToString());
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();
                        items.combo = Convert.ToInt32(rr["combo"].ToString());
                        items.orders = Convert.ToInt32(rr["orders"].ToString());

                        allItems.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allItems;
        }
        public List<CafeMenu> retrieveTop10MenuItems()
        {
            List<CafeMenu> allItems = new List<CafeMenu>();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT top 10 * from  Menu order by orders desc ";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafeMenu items = new CafeMenu();
                        items.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        items.itemImg = rr["itemImg"].ToString();
                        items.itemName = rr["itemName"].ToString();
                        items.itemCategory = rr["itemCategory"].ToString();
                        items.itemSubCategory = rr["itemSubCategory"].ToString();
                        items.itemPrice = Math.Round(decimal.Parse(rr["itemPrice"].ToString()),2);
                        items.itemDesc = rr["itemDesc"].ToString();
                        items.itemRequest = rr["itemRequest"].ToString();
                        items.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        items.itemStatus = rr["itemStatus"].ToString();
                        items.combo = Convert.ToInt32(rr["combo"].ToString());
                        items.orders = Convert.ToInt32(rr["orders"].ToString());

                        allItems.Add(items);
                    }

                    myConnection.Close();
                }
            }
            return allItems;
        }


        public List<CafeMenu> retrieveFavourites(int custID)
        {
            List<CafeMenu> allItems = new List<CafeMenu>();
            MenuDAL cafe = new MenuDAL(); 
            CustomerAccountDAL c = new CustomerAccountDAL();
            string favouriteItems = c.retrieveFavouriteItemsByCustID(custID);
            string[] favouriteArr = favouriteItems.Split(',');
            for (int i = 0; i < favouriteArr.Length-1; i++)
            {
                CafeMenu items = cafe.retrieveMenuItemById(Convert.ToInt32(favouriteArr[i]));
                allItems.Add(items);
            }
            return allItems;
        }

        public CafeMenu retrieveMenuItemById(int id)
        {
            CafeMenu menuItem = new CafeMenu();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Menu WHERE itemID = @itemID";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@itemID", id);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        menuItem.itemID = Convert.ToInt32(rr["itemID"].ToString());
                        menuItem.itemImg = rr["itemImg"].ToString();
                        menuItem.itemName = rr["itemName"].ToString();
                        menuItem.itemCategory = rr["itemCategory"].ToString();
                        menuItem.itemSubCategory = rr["itemSubCategory"].ToString();
                        menuItem.itemPrice = Math.Round(decimal.Parse(rr["itemPrice"].ToString()), 2);
                        menuItem.itemDesc = rr["itemDesc"].ToString();
                        menuItem.itemRequest = rr["itemRequest"].ToString();
                        menuItem.itemQty = Convert.ToInt32(rr["itemQty"].ToString());
                        menuItem.itemStatus = rr["itemStatus"].ToString();
                        menuItem.combo = Convert.ToInt32(rr["combo"].ToString());
                        menuItem.orders = Convert.ToInt32(rr["orders"].ToString());

                    }

                    myConnection.Close();
                }
            }
            return menuItem;
        }
        public int updateMenu(int itemID, string itemImg, string itemName, string itemCategory, string itemSubCategory, decimal itemPrice, string itemDesc, string itemRequest, string itemAddOn, int itemQty, string itemStatus, int combo)
        {
            int result = 0;

            string queryStr = "UPDATE Menu SET itemImg=@itemImg, itemName=@itemName, itemCategory=@itemCategory, itemSubCategory=@itemSubCategory, itemPrice=@itemPrice, itemDesc=@itemDesc, itemRequest=@itemRequest, itemQty=@itemQty, itemStatus=@itemStatus, combo=@combo WHERE itemID=@itemID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);
            cmd.Parameters.AddWithValue("@itemID", itemID);
            cmd.Parameters.AddWithValue("@itemImg", itemImg);
            cmd.Parameters.AddWithValue("@itemName", itemName);
            cmd.Parameters.AddWithValue("@itemCategory", itemCategory);
            cmd.Parameters.AddWithValue("@itemSubCategory", itemSubCategory);
            cmd.Parameters.AddWithValue("@itemPrice", itemPrice);
            cmd.Parameters.AddWithValue("@itemDesc", itemDesc);
            cmd.Parameters.AddWithValue("@itemRequest", itemRequest);
            cmd.Parameters.AddWithValue("@itemQty", itemQty);
            cmd.Parameters.AddWithValue("@itemStatus", itemStatus);
            cmd.Parameters.AddWithValue("@combo", combo);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }

        //DELETE
        public void itemDelete(int itemID)
        {
            string query = "DELETE * FROM Menu WHERE itemID = @itemID";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);
            cmd.Parameters.AddWithValue("@itemID", itemID);
        }


    }
}

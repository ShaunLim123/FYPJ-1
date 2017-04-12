using WebServiceLCafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using WebServiceLCafe.DAL;
using System.Configuration;
using Newtonsoft.Json;

namespace WebServiceLCafe
{
    /// <summary>
    /// Summary description for WebServiceCafe
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceCafe : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void getToken(string accessKey)
        {
            if (ConfigurationManager.AppSettings["AccessKey"].Equals(accessKey))
            {
                //generate the 24 characters token and store in database and return the token.
                TokenDAL t = new TokenDAL();
                string date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                Tokens tt = new Tokens();
                //random 24 characters
                string genToken = generateToken();
                //t.CreateToken(genToken);
                int tokenINT = t.createToken(genToken, date);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write("{\"token\":\"" + genToken + "\"}");
                this.Context.Response.Flush();

            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid AccessKey");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }
        public string generateToken()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveMenu(string itemCategory, string label, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                MenuDAL m = new MenuDAL();
                List<CafeMenu> menuList;
                if (label != string.Empty)
                {
                    menuList = m.retrieveMenuByLabel(label);
                }
                else
                {
                    menuList = m.retrieveMenuByItemCategory(itemCategory);
                }
                var json = new JavaScriptSerializer().Serialize(menuList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Flush();
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveTop10MenuItems(string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                MenuDAL m = new MenuDAL();
                List<CafeMenu> menuList = m.retrieveTop10MenuItems();
                var json = new JavaScriptSerializer().Serialize(menuList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Flush();
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveMenuItem(string itemID, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                MenuDAL m = new MenuDAL();
                int id = Convert.ToInt32(itemID);
                CafeMenu cm = m.retrieveMenuItemById(id);
                var json = new JavaScriptSerializer().Serialize(cm);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CreateFeedback(string subject, string name, string email, string comment, int service, int food, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                // get feedback DAL to access to database
                FeedbackDAL m = new FeedbackDAL();

                //get current date time and convert to string
                string date = DateTime.Now.ToString();

                //create feedback object
                Feedbacks f = new Feedbacks(name, subject, comment, email, date, service, food);

                //pass feedback object to feedback DAL to store in database
                int feedbackINT = m.CreateFeedback(f);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                if (feedbackINT == 1)
                {
                    this.Context.Response.StatusCode = 201;
                }
                else
                {
                    this.Context.Response.StatusCode = 400;
                }
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createAccount(string fullname, string email, string password, string phoneNumber, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                // get CustomerAccountDAL DAL to access to database
                CustomerAccountDAL a = new CustomerAccountDAL();


                //pass paramenters to customerAcc DAL to store in database
                int custINT = a.CreateAccount(fullname, email, password, phoneNumber);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                if (custINT == 1)
                {
                    this.Context.Response.StatusCode = 201;
                }
                else
                {
                    this.Context.Response.StatusCode = 400;
                }
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CreateOrder(string token, string jsonString)
            {
            int PIN = 0;
            bool notEnoughStock = false;
            TokenDAL t = new TokenDAL();
            OrdersDAL o = new OrdersDAL();
            MenuDAL m = new MenuDAL();
            if (t.checkTokenBOOL(token))
            {
                Order dorder = JsonConvert.DeserializeObject<Order>(jsonString);
                int id = dorder.custID;
                decimal grandTotal = dorder.grandTotal;
                decimal gst = dorder.GST;
                decimal discount = dorder.discount;
                string orderType = dorder.orderType;
                string invoiceno = dorder.InvoiceNo;
                OrderItem[] order = dorder.items;

                OrdersDAL odal = new OrdersDAL();

                for (int i = 0; i < order.Length; i++)
                {
                    if (m.retrieveItemQty(order[i].itemID) < order[i].itemQty)
                    {
                        notEnoughStock = true;
                        break;
                    }
                }

            if (!notEnoughStock)
                {
                    if (orderType.Equals("Dine-In", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string tableNum = dorder.tableNumber;
                        PIN = odal.CreateDineInOrder(id, grandTotal, discount, gst, orderType, tableNum, order);
                    }
                    else if (orderType.Equals("Pre-Order", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string preorderDateTime = dorder.preorderDateTime;
                        odal.CreatePreorderOrder(id, grandTotal, discount, gst, orderType, preorderDateTime, order);
                    }



                    for (int i = 0; i < order.Length; i++)
                    {
                        int newQty = order[i].itemQty - order[i].quantity;
                        o.UpdateMenuQtyByID(order[i].itemID, newQty);
                    }
                    this.Context.Response.StatusCode = 201;
                    this.Context.Response.Write("{\"message\":\"" + PIN + "\"}");
                }
                else
                {
                    PIN = -1;
                    this.Context.Response.StatusCode = 201;
                    this.Context.Response.Write("{\"message\":\"" + PIN + "\"}");
                }


                
            }
            else
            {
                this.Context.Response.StatusCode = 400;
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }

        }

  
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveAllInfo(string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CafeInfoDAL m = new CafeInfoDAL();
                List<Info> cafeInfoList = m.retrieveAllInfo();
                var json = new JavaScriptSerializer().Serialize(cafeInfoList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveAllTermBreaks(string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                TermBreakDAL m = new TermBreakDAL();
                List<TermBreak> cafeTermBreakList = m.retrieveAllTermBreaks();
                var json = new JavaScriptSerializer().Serialize(cafeTermBreakList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveAllPromos(string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                PromoDAL m = new PromoDAL();
                List<CafePromo> cafePromoList = m.retrieveAllPromos();
                var json = new JavaScriptSerializer().Serialize(cafePromoList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }


        // unused
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveOrderList(string token, int custID)
        {
            TokenDAL t = new TokenDAL();
            OrdersDAL o = new OrdersDAL();
            if (t.checkTokenBOOL(token))
            {
                OrdersDAL m = new OrdersDAL();
                List<Order> order = m.retrieveOrderList(custID);
                if (order.Count != 0)
                {
                    var json = new JavaScriptSerializer().Serialize(order);
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.StatusCode = 200;
                    this.Context.Response.Write(json);
                }
                else
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.StatusCode = 200;
                    var json = new JavaScriptSerializer().Serialize("Empty");
                    this.Context.Response.Write(json);
                }

            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token or Invalid CustomerID");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveOrderItemList(string token, int orderID)
        {
            TokenDAL t = new TokenDAL();
            OrdersDAL o = new OrdersDAL();
            if (t.checkTokenBOOL(token))
            {
                OrdersDAL m = new OrdersDAL();
                List<OrderItem> orderitem = m.retrieveOrderItems(orderID);
                var json = new JavaScriptSerializer().Serialize(orderitem);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void login(string custEmail, string custPassword, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CustomerAccountDAL mdal = new CustomerAccountDAL();
                int i = mdal.login(custEmail, custPassword);

                if(i == -1)
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.Flush();
                    this.Context.Response.Write("{\"message\": \"unsuccessful\"}");
                }
                else
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.Flush();
                    this.Context.Response.Write("{\"message\": \""+i+"\"}");
                }

            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveAccountById(int id, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CustomerAccountDAL mdal = new CustomerAccountDAL();
                CustAcc m = mdal.retrieveCustAccountbyId(id);
                var json = new JavaScriptSerializer().Serialize(m);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Flush();
                this.Context.Response.Write(json);

            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);

            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void updateCustAcc(int custID, string custFullName, string custEmail, string custPassword, string custPhoneNumber, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CustomerAccountDAL m = new CustomerAccountDAL();
                int updateCustAccINT = m.updateCustAcc(custID, custFullName, custEmail, custPassword, custPhoneNumber);
                var json = new JavaScriptSerializer().Serialize(updateCustAccINT);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void checkToken(string token)
        {
            TokenDAL m = new TokenDAL();
            bool checkTokenValid = m.checkTokenBOOL(token);
            if (checkTokenValid == true)
            {
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;
                this.Context.Response.Flush();
                this.Context.Response.Write("{\"message\": \"successful\"}");
            }
            else
            {
                //token expired, delete from database
                TokenDAL tdal = new TokenDAL();
                tdal.deleteTOkenBOOL(token);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;

                this.Context.Response.Write("{\"message\": \"unsuccessful\"} ");
            }


        }

        ////[WebMethod]
        ////[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        ////public void checkCustAcc(string email, string pass, string token)
        ////{
        ////    TokenDAL m = new TokenDAL();
        ////    CustomerAccountDAL c = new CustomerAccountDAL();
        ////    bool checkTokenValid = m.checkTokenBOOL(token);
        ////    bool checkCustEmailValid = c.checkCustPasswordBOOL(email);
        ////    bool checkCustPassValid = c.checkCustPassword(pass);
        ////    if (checkCustEmailValid == true || checkCustPassValid == true || checkTokenValid == true)
        ////    {
        ////        this.Context.Response.ContentType = "application/json; charset=utf-8";
        ////        this.Context.Response.StatusCode = 200;
        ////        this.Context.Response.Write("{\"message\": \"successful\"}");
        ////    }
        ////    else
        ////    {
        ////        //token expired, delete from database
        ////        TokenDAL tdal = new TokenDAL();
        ////        tdal.deleteTOkenBOOL(token);
        ////        this.Context.Response.ContentType = "application/json; charset=utf-8";
        ////        this.Context.Response.StatusCode = 200;
        ////        this.Context.Response.Write("{\"message\": \"unsuccessful\"} ");
        ////    }
        ////}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrievePromoActivation(string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                PromoActivationDAL pad = new PromoActivationDAL();
                List<PromoActivation> list = pad.retrieveAllPromoStatus();
                var json = new JavaScriptSerializer().Serialize(list);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void checkDuplicateEmail(string email, string token)
        {
            TokenDAL m = new TokenDAL();
            CustomerAccountDAL c = new CustomerAccountDAL();
            bool checkTokenValid = m.checkTokenBOOL(token);
            bool checkCustEmailDuplicate = c.checkCustEmail(email);
            if (checkTokenValid == true)
            {
                if (checkCustEmailDuplicate == true)
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.StatusCode = 200;
                    this.Context.Response.Write("{\"message\": \"unsuccessful\"}");
                }
                else
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.StatusCode = 200;
                    this.Context.Response.Write("{\"message\": \"successful\"} ");
                }
            }
            else
            {
                //token expired, delete from database
                TokenDAL tdal = new TokenDAL();
                tdal.deleteTOkenBOOL(token);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.StatusCode = 200;
                this.Context.Response.Write("{\"message\": \"successful\"} ");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void resetPassword(string custID, string custPassword, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
             
                CustomerAccountDAL m = new CustomerAccountDAL();
                int updateCustAccINT = m.changeCustPassword(custID, custPassword);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                if (updateCustAccINT == 1)
                {
                    this.Context.Response.StatusCode = 201;
                }
                else
                {
                    this.Context.Response.StatusCode = 400;
                }
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void checkCurrentPassword(string custID, string custPassword, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CustomerAccountDAL mdal = new CustomerAccountDAL();
                int i = mdal.checkCurrentPassword(custID, custPassword);

                if (i == -1)
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.Flush();
                    this.Context.Response.Write("{\"message\": \"unsuccessful\"}");
                }
                else
                {
                    this.Context.Response.ContentType = "application/json; charset=utf-8";
                    this.Context.Response.Flush();
                    this.Context.Response.Write("{\"message\": \"" + i + "\"}");
                }

            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void updateFavourites(string custID, string favouriteItemsID, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {

                CustomerAccountDAL m = new CustomerAccountDAL();
                int updatefavourites = m.updatefavourites(custID, favouriteItemsID);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                if (updatefavourites == 1)
                {
                    this.Context.Response.StatusCode = 201;
                }
                else
                {
                    this.Context.Response.StatusCode = 400;
                }
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void retrieveFav(int custID, string token)
        {
            TokenDAL t = new TokenDAL();
            if (t.checkTokenBOOL(token))
            {
                CustomerAccountDAL c = new CustomerAccountDAL();
                MenuDAL m = new MenuDAL();
                List<CafeMenu> menuList = m.retrieveFavourites(custID);
                var json = new JavaScriptSerializer().Serialize(menuList);
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Flush();
                this.Context.Response.Write(json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize("Invalid Token");
                this.Context.Response.ContentType = "application/json; charset=utf-8";
                this.Context.Response.Write(json);
            }
        }

    }
}

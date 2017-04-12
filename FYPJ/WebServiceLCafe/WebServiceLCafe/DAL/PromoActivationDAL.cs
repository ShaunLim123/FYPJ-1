using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class PromoActivationDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public PromoActivationDAL()
        {

        }

        public List<PromoActivation> retrieveAllPromoStatus()
        {
            List<PromoActivation> allPromoActivation = new List<PromoActivation>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from PromoActivation";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        PromoActivation pa = new PromoActivation();
                        pa.id = Convert.ToInt32(rr["Id"]);
                        pa.name= rr["Name"].ToString();
                        pa.status = Convert.ToBoolean(rr["Status"]);

                        allPromoActivation.Add(pa);

                    }

                    myConnection.Close();
                }
            }
            return allPromoActivation;
        }
    }
}
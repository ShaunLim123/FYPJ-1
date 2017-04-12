using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class PromoDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        private string promoName = "";
        private string promoDesc = "";
        private string promoImg = "";


        public int CreatePromo(CafePromo p)
        {
            string query = "INSERT into Promo (promoImg, promoName, promoDesc, status) values (@promoImg, @promoName, @promoDesc, @status)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@promoImg", p.promoImg);
            cmd.Parameters.AddWithValue("@promoName", p.promoName);
            cmd.Parameters.AddWithValue("@promoDesc", p.promoDesc);
            cmd.Parameters.AddWithValue("@status", p.status);

            sc.Open();
            int insert = cmd.ExecuteNonQuery();
            sc.Close();
            return insert;
        }

        public List<CafePromo> retrieveAllPromos()
        {
            List<CafePromo> allPromos = new List<CafePromo>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Promo";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CafePromo promos = new CafePromo();
                        promos.promoID = Convert.ToInt32(rr["promoID"].ToString());
                        promos.promoImg = rr["promoImg"].ToString();
                        promos.promoName = rr["promoName"].ToString();
                        promos.promoDesc = rr["promoDesc"].ToString();
                        promos.status = Convert.ToInt32(rr["status"].ToString());

                        allPromos.Add(promos);
                    }

                    myConnection.Close();
                }
            }
            return allPromos;
        }

        public int updatePromos(int promoID, string promoImg, string promoName, string promoDesc, int status)
        {
            int result = 0;

            string queryStr = "UPDATE Promo SET promoImg=@promoImg, promoName=@promoName, promoDesc=@promoDesc, status=@status WHERE promoID=@promoID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);
            cmd.Parameters.AddWithValue("@promoID", promoID);
            cmd.Parameters.AddWithValue("@promoImg", promoImg);
            cmd.Parameters.AddWithValue("@promoName", promoName);
            cmd.Parameters.AddWithValue("@promoDesc", promoDesc);
            cmd.Parameters.AddWithValue("@status", status);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }

        public void promoDelete(int promoID)
        {
            string query = "DELETE * FROM Promo WHERE promoID = @promoID";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);
            cmd.Parameters.AddWithValue("@promoID", promoID);
        }
    }
}
using WebServiceLCafe.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.DAL
{
    public class TokenDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public int createToken(string accessToken, string tokenDateTime)
        {
            int result = 0;
            string strCommandText = "INSERT INTO Token(accessToken, tokenDateTime)"
                + "values (@accessToken, @tokenDateTime)";

            SqlConnection myConnection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(strCommandText, myConnection);
            cmd.Parameters.AddWithValue("@accessToken", accessToken);
            cmd.Parameters.AddWithValue("@tokenDateTime", tokenDateTime);

            myConnection.Open();

            try
            {
                result += cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            myConnection.Close();
            myConnection.Dispose();

            return result;
        }

        public Tokens retrieveMenuItemById(int access)
        {
            Tokens t = new Tokens();
            //ItemDAL item = new ItemDAL();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Token WHERE accessToken = @accessToken";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                oCmd.Parameters.AddWithValue("@accessToken", access);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        t.tokenID = Convert.ToInt32(rr["tokenID"].ToString());
                        t.accessToken = rr["accessToken"].ToString();
                        t.tokenDateTime = rr["tokenDateTime"].ToString();
                    }

                    myConnection.Close();
                }
            }
            return t;
        }

        public bool checkTokenBOOL(string token)
        {
            string accesstoken = "";
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                //change SQL statement within 24 hours
                string jString = "SELECT * from Token where accessToken = @accessToken AND tokenDateTime >= DATEADD(day, -1, GETDATE())";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@accessToken", token);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        accesstoken = rr["accessToken"].ToString();
                        return true;
                    }
                    myConnection.Close();
                }
            }
            return false;
        }

        public void deleteTOkenBOOL(string tokenD)
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "DELETE * FROM Token WHERE accessToken = @accessToken";
                SqlConnection sc = new SqlConnection(con);
                SqlCommand cmd = new SqlCommand(jString, sc);
                cmd.Parameters.AddWithValue("@accessToken", tokenD);
            }
        }
        //public bool checkTokenBOOL(string Token)
        //{
        //    bool flag = false;
        //    string jString = "SELECT * from Token where Token = @Token";
        //    SqlConnection myConnection = new SqlConnection(con);
        //    SqlCommand jCmd = new SqlCommand(jString, myConnection);
        //    myConnection.Open();
        //    jCmd.Parameters.AddWithValue("@Token", Token);
        //    int count = Convert.ToInt32(jCmd.ExecuteScalar());
        //    if (count > 0)
        //    {
        //        flag = true;
        //    }
        //    myConnection.Close();


        //    return flag;
        //}
    }
}
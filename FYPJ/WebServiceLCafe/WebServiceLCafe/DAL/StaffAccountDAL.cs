using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class StaffAccountDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        //Declaration 
        public string staffName = "";
        public string staffPassword = "";
        public string staffNumber = "";
        public string staffEmail = "";

        public bool checkStaffEmail(string staffEmail)
        {
            bool flag = false;
            string jString = "SELECT * from Staff where staffEmail = @staffEmail";
            SqlConnection myConnection = new SqlConnection(con);
            SqlCommand jCmd = new SqlCommand(jString, myConnection);
            myConnection.Open();
            jCmd.Parameters.AddWithValue("@staffEmail", staffEmail);
            int count = Convert.ToInt32(jCmd.ExecuteScalar());
            if (count > 0)
            {
                flag = true;
            }
            myConnection.Close();
            return flag;
        }

        //CREATE
        public int CreateAccount(StaffAccounts s)
        {
            string query = "INSERT INTO Staff (staffName, staffPassword, staffNumber, staffEmail ) values (@staffName, @staffPassword, @staffNumber, @staffEmail)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            //CryptographyManager manager = new CryptographyManager();
            //string hashed = manager.CalculateSHA256Hash(staffPassword);


            cmd.Parameters.AddWithValue("@staffName", s.staffName);
            cmd.Parameters.AddWithValue("@staffPassword", s.staffPassword);
            cmd.Parameters.AddWithValue("@staffNumber", s.staffNumber);
            cmd.Parameters.AddWithValue("@staffEmail", s.staffEmail);
            sc.Open();
            int result = cmd.ExecuteNonQuery();
            sc.Close();
            return result;
        }


        public StaffAccounts login(string staffEmail, string staffPassword)
        {
            StaffAccounts acc = null;

            //CryptographyManager manager = new CryptographyManager();
            //string hashed = manager.CalculateSHA256Hash(staffPassword);

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from Staff where staffEmail = @staffEmail and staffPassword = @staffPassword";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@staffEmail", staffEmail);
                jCmd.Parameters.AddWithValue("@staffPassword", staffPassword);
                myConnection.Open();
                using (SqlDataReader jReader = jCmd.ExecuteReader())
                {
                    if (jReader.Read())
                    {
                        acc = new StaffAccounts();
                        acc.staffEmail = jReader["staffEmail"].ToString();
                        acc.staffPassword = jReader["staffPassword"].ToString();
                        acc.staffName = jReader["staffName"].ToString();

                    }
                    else
                    {
                        acc = new StaffAccounts();

                        if (checkStaffEmail(staffEmail) == true)
                        {
                            acc.message = "Password is invalid. Try again.";
                        }
                        else
                        {
                            acc.message = "Invalid account. Please try again.";
                        }
                    }
                    myConnection.Close();
                }
            }
            return acc;
        }

        //RETRIEVE
        public List<StaffAccounts> retrieveAccount()
        {
            List<StaffAccounts> acc = new List<StaffAccounts>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from Staff";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        StaffAccounts accounts = new StaffAccounts();
                        staffEmail = rr["staffEmail"].ToString();
                        staffPassword = rr["staffPassword"].ToString();
                        acc.Add(accounts);
                    }
                    myConnection.Close();
                }
            }
            return acc;
        }

    }
}

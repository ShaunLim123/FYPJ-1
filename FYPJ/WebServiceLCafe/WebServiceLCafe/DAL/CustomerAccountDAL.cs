using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;
using WebServiceLCafe.DAL;
using System.Security.Cryptography;
using System.Text;

namespace WebServiceLCafe.DAL
{
    public class CustomerAccountDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public bool checkCustEmail(string custEmail)
        {
            bool flag = false;
            string jString = "SELECT * from Customer where custEmail = @custEmail";
            SqlConnection myConnection = new SqlConnection(con);
            SqlCommand jCmd = new SqlCommand(jString, myConnection);
            myConnection.Open();
            jCmd.Parameters.AddWithValue("@custEmail", custEmail);
            int count = Convert.ToInt32(jCmd.ExecuteScalar());
            if (count > 0)
            {
                flag = true;
            }
            myConnection.Close();


            return flag;
        }

        //public bool checkCustPassword(string custPassword)
        //{
        //    bool flag = false;
        //    string jString = "SELECT * from Customer where custPassword = @custPassword";
        //    SqlConnection myConnection = new SqlConnection(con);
        //    SqlCommand jCmd = new SqlCommand(jString, myConnection);
        //    myConnection.Open();
        //    jCmd.Parameters.AddWithValue("@custPassword", custPassword);
        //    int count = Convert.ToInt32(jCmd.ExecuteScalar());
        //    if (count > 0)
        //    {
        //        flag = true;
        //    }
        //    myConnection.Close();
        //    return flag;
        //}

        //public int CreateAccount(string fullname, string email, string password, string phoneNumber)
        //{

        //    string query = "INSERT into Customer (custFullName, custEmail, custPassword, custPhoneNumber) values (@custFullName, @custEmail, @custPassword, @custPhoneNumber)";
        //    SqlConnection sc = new SqlConnection(con);
        //    SqlCommand cmd = new SqlCommand(query, sc);

        //    cmd.Parameters.AddWithValue("@custFullName", fullname);
        //    cmd.Parameters.AddWithValue("@custEmail", email);
        //    cmd.Parameters.AddWithValue("@custPassword", password);
        //    cmd.Parameters.AddWithValue("@custPhoneNumber", phoneNumber);

        //    sc.Open();
        //    int result = cmd.ExecuteNonQuery();
        //    sc.Close();
        //    return result;
        //}

        public int CreateAccount(string fullname, string email, string password, string phoneNumber)
        {

            // generate random sakt
            String salt = PasswordSalt;

            // hash the password+salt
            String hashing = EncodePassword(password, salt);

            string query = "INSERT into Customer (custFullName, custEmail, custPasswordHash, custPhoneNumber, salt) values (@custFullName, @custEmail, @custPasswordHash, @custPhoneNumber, @salt)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@custFullName", fullname);
            cmd.Parameters.AddWithValue("@custEmail", email);
            cmd.Parameters.AddWithValue("@custPasswordHash", hashing);
            cmd.Parameters.AddWithValue("@custPhoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@salt", salt);

            sc.Open();
            int result = cmd.ExecuteNonQuery();
            sc.Close();
            return result;
        }

        public List<CustAcc> retrieveCustAccount()
        {
            List<CustAcc> acc = new List<CustAcc>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from Customer";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        CustAcc accounts = new CustAcc();
                        accounts.custFullName = rr["custFullName"].ToString();
                        accounts.custEmail = rr["custEmail"].ToString();
                        accounts.custPassword = rr["custPasswordHash"].ToString();
                        accounts.custPhoneNumber = rr["custPhoneNumber"].ToString();


                        acc.Add(accounts);
                    }
                    myConnection.Close();
                }
            }
            return acc;
        }

        public CustAcc retrieveCustAccountbyId(int id)
        {
            CustAcc accounts = new CustAcc();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from Customer where custID = @id";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@id", id);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        accounts.custID = Convert.ToInt32(rr["custID"]);
                        accounts.custFullName = rr["custFullName"].ToString();
                        accounts.custEmail = rr["custEmail"].ToString();
                        accounts.custPassword = rr["custPasswordHash"].ToString();
                        accounts.custPhoneNumber = rr["custPhoneNumber"].ToString();
                        accounts.favouriteItemsID = rr["favouriteItemsID"].ToString();

                    }
                    myConnection.Close();
                }
            }
            return accounts;
        }

        public string retrieveFavouriteItemsByCustID(int custID)
        {
            string favouriteItems = "";
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select favouriteItemsID from Customer where custID = @custID";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@custID", custID);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        favouriteItems = rr["favouriteItemsID"].ToString();
                    }
                    myConnection.Close();
                }
            }
            return favouriteItems;
        }

        public int login(string custEmail, string custPassword)
        {
            CustAcc acc = new CustAcc();

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                int id = 0;
                string salt;
                string passwordHash;
                bool isValid = false;
                string jString = "SELECT custID, custEmail, custPasswordHash, salt FROM Customer where custEmail = @custEmail";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                jCmd.Parameters.AddWithValue("@custEmail", custEmail);
                myConnection.Open();
                using (SqlDataReader jReader = jCmd.ExecuteReader())
                {
                    if (jReader.Read())
                    {
                        salt = jReader["salt"].ToString();
                        passwordHash = jReader["custPasswordHash"].ToString();

                        // hash the password+salt
                        String attemptedPasswordHash = EncodePassword(custPassword, salt);

                        // compare PasswordHash with attemptedPasswordHash
                        if (passwordHash == attemptedPasswordHash)
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }

                        if (isValid)
                        {
                            id = Convert.ToInt32(jReader["custID"].ToString());
                        }
                        else
                        {
                            id = -1;
                        }

                        return id;

                        //acc.custFullName = jReader["custFullName"].ToString();
                        //acc.custPreferredName = jReader["custPreferredName"].ToString();
                        //acc.custEmail = jReader["custEmail"].ToString();
                        //acc.custPassword = jReader["custPassword"].ToString();
                        //acc.custPhoneNumber = jReader["custPhoneNumber"].ToString();
                        //acc.custStatus = "LoginSuccessful";
                        //acc.message = "";
                        myConnection.Close();
                    }
                    else
                    {
                        return -1;
                    }

                }
            }
        }
        //public int login(string custEmail, string custPassword)
        //{
        //    CustAcc acc = new CustAcc();

        //    using (SqlConnection myConnection = new SqlConnection(con))
        //    {
        //        int id = 0;
        //        string jString = "SELECT custID, custEmail, custPassword FROM Customer where custEmail = @custEmail and custPassword = @custPassword";
        //        SqlCommand jCmd = new SqlCommand(jString, myConnection);
        //        jCmd.Parameters.AddWithValue("@custEmail", custEmail);
        //        jCmd.Parameters.AddWithValue("@custPassword", custPassword);
        //        myConnection.Open();
        //        using (SqlDataReader jReader = jCmd.ExecuteReader())
        //        {
        //            if (jReader.Read())
        //            {

        //                id= Convert.ToInt32(jReader["custID"].ToString());


        //                return id;


        //                myConnection.Close();
        //            }
        //            else
        //            {
        //                return -1;
        //            }

        //        }
        //    }
        //}

        public int checkCurrentPassword(string custID, string custPassword)
        {
                CustAcc acc = new CustAcc();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    int id = 0;
                    string salt;
                    string passwordHash;
                    bool isValid = false;
                    string jString = "SELECT custPasswordHash, salt FROM Customer where custID = @custID";
                    SqlCommand jCmd = new SqlCommand(jString, myConnection);
                    jCmd.Parameters.AddWithValue("@custID", custID);
                    myConnection.Open();
                    using (SqlDataReader jReader = jCmd.ExecuteReader())
                    {
                        if (jReader.Read())
                        {
                            salt = jReader["salt"].ToString();
                            passwordHash = jReader["custPasswordHash"].ToString();

                            // hash the password+salt
                            String attemptedPasswordHash = EncodePassword(custPassword, salt);

                            // compare PasswordHash with attemptedPasswordHash
                            if (passwordHash == attemptedPasswordHash)
                            {
                                isValid = true;
                            }
                            else
                            {
                                isValid = false;
                            }

                            if (isValid)
                            {
                                id = 1;
                            }
                            else
                            {
                                id = -1;
                            }

                            return id;
                            myConnection.Close();
                        }
                        else
                        {
                            return -1;
                        }

                    }
                }
            }

        public int changeCustPassword(string custID, string custPassword)
        {
            int result = 0;
            string salt = PasswordSalt;
            string queryStr = "UPDATE Customer SET custPasswordHash=@custPasswordHash, salt=@custSalt WHERE custID=@custID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);

            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@custPasswordHash", EncodePassword(custPassword, salt));
            cmd.Parameters.AddWithValue("@custSalt", salt);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }

        public int updatefavourites(string custID, string favouriteItemsID)
        {
            int result = 0;
            string queryStr = "UPDATE Customer SET favouriteItemsID=@favouriteItemsID WHERE custID=@custID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);

            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@favouriteItemsID", favouriteItemsID);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }
        public int updateCustAcc(int custID, string custFullName, string custEmail, string custPassword, string custPhoneNumber)
        {
            int result = 0;
            string salt = PasswordSalt;
            string queryStr = "UPDATE Customer SET custFullName=@custFullName, custEmail=@custEmail, custPassword=@custPasswordHash, custPhoneNumber=@custPhoneNumber, salt=@custSalt WHERE custID=@custID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);

            //CryptographyManager manager = new CryptographyManager();
            //string hashed = manager.CalculateSHA256Hash(custPassword);

            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@custFullName", custFullName);
            cmd.Parameters.AddWithValue("@custEmail", custEmail);
            cmd.Parameters.AddWithValue("@custPasswordHash", EncodePassword(custPassword, salt));  
            cmd.Parameters.AddWithValue("@custPhoneNumber", custPhoneNumber);
            cmd.Parameters.AddWithValue("@custSalt", salt);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }

        private string PasswordSalt
        {
            get
            {
                // variable for salt
                var rng = new RNGCryptoServiceProvider();
                // instantiate new bytes          
                var buff = new byte[32];
                // random bytes
                rng.GetBytes(buff);
                // convert bytes to human readable string
                return Convert.ToBase64String(buff);
            }
        }

        private string EncodePassword(string password, string salt)
        {
            // convert string of password to bytes
            byte[] bytes = Encoding.Unicode.GetBytes(password);

            // convert string of salt to bytes
            byte[] src = Encoding.Unicode.GetBytes(salt);

            // combine password bytes and salt bytes to come out with the final bytes
            byte[] dst = new byte[src.Length + bytes.Length];

            // put in the buffer
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            // declare hashing algorithm ---> SHA1
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");

            // hash the final bytes
            byte[] inarray = algorithm.ComputeHash(dst);

            // convert the bytes back to string after hashing
            return Convert.ToBase64String(inarray);
        }
    }
}

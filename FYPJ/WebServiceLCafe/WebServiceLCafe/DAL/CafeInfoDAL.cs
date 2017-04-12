using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class CafeInfoDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        //public int CreateCafeInfo(Info i)
        //{
        //    string query = "INSERT INTO Info (openinghours, closinghours, day, infoStatus, remarks) values (@infoOpeningHours, @infoClosingHours, @infoOpeningDays, @infoStatus, @remarks)";
        //    SqlConnection sc = new SqlConnection(con);
        //    SqlCommand cmd = new SqlCommand(query, sc);

        //    cmd.Parameters.AddWithValue("@infoOpeningHours", i.openinghours);
        //    cmd.Parameters.AddWithValue("@infoClosingHours", i.closinghours);
        //    cmd.Parameters.AddWithValue("@infoOpeningDays", i.day);
        //    cmd.Parameters.AddWithValue("@infoStatus", i.infoStatus);
        //    cmd.Parameters.AddWithValue("@remarks", i.remarks);

        //    sc.Open();
        //    int result = cmd.ExecuteNonQuery();
        //    sc.Close();
        //    return result;
        //}

        public List<Info> retrieveAllInfo()
        {
            List<Info> i = new List<Info>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from Info";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        Info infos = new Info();
                        infos.infoID = Convert.ToInt32(rr["infoID"].ToString());
                        infos.openinghours = rr["openinghours"].ToString();
                        infos.closinghours = rr["closinghours"].ToString();
                        infos.day = rr["day"].ToString();
                        infos.remarks = rr["remarks"].ToString();

                        i.Add(infos);
                    }
                    myConnection.Close();
                }
            }
            return i;
        }


        public int updateInfo(int infoID, string openinghours, string closinghours, string day, string remarks)
        {
            int result = 0;

            string queryStr = "UPDATE Info SET openinghours=@infoOpeningHours, closinghours=@infoClosingHours, day=@infoOpeningDays, remarks=@remarks WHERE infoID=@infoID";

            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(queryStr, sc);
            cmd.Parameters.AddWithValue("@infoID", infoID);
            cmd.Parameters.AddWithValue("@infoOpeningHours", openinghours);
            cmd.Parameters.AddWithValue("@infoClosingHours", closinghours);
            cmd.Parameters.AddWithValue("@infoOpeningDays", day);
            //cmd.Parameters.AddWithValue("@currentStatus", currentStatus);
            cmd.Parameters.AddWithValue("@remarks", remarks);

            sc.Open();
            result += cmd.ExecuteNonQuery();

            sc.Close();

            return result;
        }

        //public int updateStatus(string infoStatus)
        //{
        //    int result = 0;
        //    string queryStr = "UPDATE Info SET currentStatus=@infoStatus";

        //    SqlConnection sc = new SqlConnection(con);
        //    SqlCommand cmd = new SqlCommand(queryStr, sc);
        //    cmd.Parameters.AddWithValue("@infoStatus", infoStatus);

        //    sc.Open();
        //    result += cmd.ExecuteNonQuery();
        //    sc.Close();
        //    return result;
        //}
    }
}
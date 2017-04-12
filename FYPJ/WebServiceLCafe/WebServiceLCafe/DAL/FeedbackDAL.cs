using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class FeedbackDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public int CreateFeedback(Feedbacks f)
        {
            string query = "INSERT into Feedback (custName, feedbackSubject, feedbackMessage, feedbackDateTime, feedbackEmail, feedbackServiceRating, feedbackFoodRating) values (@custName, @feedbackSubject, @feedbackMessage, @feedbackDateTime, @feedbackEmail, @feedbackServiceRating, @feedbackFoodRating)";
            SqlConnection sc = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(query, sc);

            cmd.Parameters.AddWithValue("@custName", f.custName);
            cmd.Parameters.AddWithValue("@feedbackSubject", f.feedbackSubject);
            cmd.Parameters.AddWithValue("@feedbackMessage", f.feedbackMessage);
            cmd.Parameters.AddWithValue("@feedbackDateTime", f.feedbackDateTime);
            cmd.Parameters.AddWithValue("@feedbackEmail", f.feedbackEmail);
            cmd.Parameters.AddWithValue("@feedbackServiceRating", f.feedbackServiceRating);
            cmd.Parameters.AddWithValue("@feedbackFoodRating", f.feedbackFoodRating);
           

            sc.Open();
            int insert = cmd.ExecuteNonQuery();
            sc.Close();
            return insert;

        }

        public List<Feedbacks> retrieveAllFeedbacks()
        {
            List<Feedbacks> allFeedbacks = new List<Feedbacks>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "SELECT * from Feedback";
                SqlCommand oCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = oCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        Feedbacks fb = new Feedbacks();
                        fb.feedbackID = Convert.ToInt32(rr["feedbackID"].ToString());
                        fb.custName = rr["custName"].ToString();
                        fb.feedbackSubject = rr["feedbackSubject"].ToString();
                        fb.feedbackMessage = rr["feedbackMessage"].ToString();
                        fb.feedbackDateTime = rr["feedbackDateTime"].ToString();
                        fb.feedbackEmail = rr["feedbackEmail"].ToString();
                        fb.feedbackServiceRating = Convert.ToInt32(rr["feedbackServiceRating"].ToString());
                        fb.feedbackFoodRating = Convert.ToInt32(rr["feedbackFoodRating"].ToString());

                        allFeedbacks.Add(fb);

                    }

                    myConnection.Close();
                }
            }
            return allFeedbacks;
        }
    }
}
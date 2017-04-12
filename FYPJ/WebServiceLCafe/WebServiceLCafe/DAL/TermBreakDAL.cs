using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebServiceLCafe.Model;

namespace WebServiceLCafe.DAL
{
    public class TermBreakDAL
    {
        string con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

        public List<TermBreak> retrieveAllTermBreaks()
        {
            List<TermBreak> t = new List<TermBreak>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string jString = "Select * from TermBreak";
                SqlCommand jCmd = new SqlCommand(jString, myConnection);
                myConnection.Open();
                using (SqlDataReader rr = jCmd.ExecuteReader())
                {
                    while (rr.Read())
                    {
                        TermBreak termbreaks = new TermBreak();
                        termbreaks.breakID = Convert.ToInt32(rr["breakID"].ToString());
                        termbreaks.DateFrom = rr["DateFrom"].ToString();
                        termbreaks.DateTo = rr["DateTo"].ToString();
                        termbreaks.Message = rr["Message"].ToString();

                        t.Add(termbreaks);
                    }
                    myConnection.Close();
                }
            }
            return t;
        }
    }
}
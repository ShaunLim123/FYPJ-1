using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class Feedbacks
    {
        public int feedbackID { get; set; }
        public string custName { get; set; }
        public string feedbackSubject { get; set; }
        public string feedbackMessage { get; set; }
        public string feedbackDateTime { get; set; }
        public string feedbackEmail { get; set; }
        public int feedbackServiceRating { get; set; }
        public int feedbackFoodRating { get; set; }

        public Feedbacks(int feedbackID, string custName, string feedbackSubject, string feedbackMessage, string feedbackEmail, string feedbackDateTime, int feedbackServiceRating, int feedbackFoodRating)
        {
            this.feedbackID = feedbackID;
            this.custName = custName;
            this.feedbackSubject = feedbackSubject;
            this.feedbackMessage = feedbackMessage;
            this.feedbackDateTime = feedbackDateTime;
            this.feedbackEmail = feedbackEmail;
            this.feedbackServiceRating = feedbackServiceRating;
            this.feedbackFoodRating = feedbackFoodRating;
        }

        public Feedbacks(string custName, string feedbackSubject, string feedbackMessage, string feedbackEmail, string feedbackDateTime, int feedbackServiceRating, int feedbackFoodRating)
        {
            this.custName = custName;
            this.feedbackSubject = feedbackSubject;
            this.feedbackMessage = feedbackMessage;
            this.feedbackDateTime = feedbackDateTime;
            this.feedbackEmail = feedbackEmail;
            this.feedbackServiceRating = feedbackServiceRating;
            this.feedbackFoodRating = feedbackFoodRating;
        }

        public Feedbacks()
        {
        }
    }
}
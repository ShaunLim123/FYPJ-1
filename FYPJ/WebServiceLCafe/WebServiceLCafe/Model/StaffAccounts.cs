using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class StaffAccounts
    {
        public int staffID { get; set; }
        public string staffName { get; set; }
        public string staffPassword { get; set; }
        public string staffNumber { get; set; }
        public string staffEmail { get; set; }
        public string message { get; set; }

        public StaffAccounts(int staffID, string staffName, string staffPassword, string staffNumber, string staffEmail, string message)
        {
            this.staffID = staffID;
            this.staffName = staffName;
            this.staffPassword = staffPassword;
            this.staffNumber = staffNumber;
            this.staffEmail = staffEmail;
            this.message = message;
        }

        public StaffAccounts()
        {
            this.staffEmail = staffEmail;
            this.staffPassword = staffPassword;

        }
    }
}

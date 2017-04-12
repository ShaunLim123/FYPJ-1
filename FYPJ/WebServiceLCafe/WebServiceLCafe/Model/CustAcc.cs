using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class CustAcc
    {
        public int custID { get; set; }
        public string custFullName { get; set; }
        public string custEmail { get; set; }
        public string custPassword { get; set; }
        public string custPhoneNumber { get; set; }
        public string favouriteItemsID { get; set; }

        public CustAcc(string custFullName, string custEmail, string custPassword, string custPhoneNumber, string favouriteItemsID)
        {
            this.custFullName = custFullName;
            this.custEmail = custEmail;
            this.custPassword = custPassword;
            this.custPhoneNumber = custPhoneNumber;
            this.favouriteItemsID = favouriteItemsID;
        }

        public CustAcc(string custEmail, string custPassword)
        {
            this.custEmail = custEmail;
            this.custPassword = custPassword;
        }

        public CustAcc(int custID)
        {
            this.custID = custID;
        }

        public CustAcc()
        {

        }
    }
}
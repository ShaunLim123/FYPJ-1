using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class Inventories
    {
        public int invID { get; set; }
        public string itemName { get; set; }
        public string invCategory { get; set; }
        public int invQty { get; set; }
        public string invDateTime { get; set; }
    }

    //    public Inventory(int invID, string itemName, string invCategory, int invQty, string invDateTime)
    //    {
    //        this.itemName = itemName;
    //        this.staffPassword = staffPassword;
    //        this.staffNumber = staffNumber;
    //        this.staffEmail = staffEmail;
    //    }

}

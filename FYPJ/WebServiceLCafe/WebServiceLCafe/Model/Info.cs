using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class Info
    {
        public int infoID { get; set; }
        public string openinghours { get; set; }
        public string closinghours { get; set; }
        public string day { get; set; }
        public string remarks { get; set; }

        public Info(string infoOpeningHours, string infoClosingHours, string infoOpeningDays, string remarks)
        {
            this.openinghours = infoOpeningHours;
            this.closinghours = infoClosingHours;
            this.day = infoOpeningDays;
            this.remarks = remarks;
        }
        public Info()
        {

        }
        public Info(int infoID)
        {
            this.infoID = infoID;
        }

        public Info(int infoID, string infoOpeningHours, string infoClosingHours, string infoOpeningDays, string remarks)
        {
            this.infoID = infoID;
            this.openinghours = infoOpeningHours;
            this.closinghours = infoClosingHours;
            this.day = infoOpeningDays;
            this.remarks = remarks;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class TermBreak
    {
        public int breakID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Message { get; set; }
        public TermBreak(int breakID, string DateFrom, string DateTo, string Message)
        {
            this.breakID = breakID;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.Message = Message;
        }
        public TermBreak()
        {

        }
    }
}
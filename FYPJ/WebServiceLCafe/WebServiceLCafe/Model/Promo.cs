using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class CafePromo
    {
        public int promoID { get; set; }
        public string promoImg { get; set; }
        public string promoName { get; set; }
        public string promoDesc { get; set; }
        public int status { get; set; }


        public CafePromo(string promoImg, string promoName, string promoDesc, int status)
        {
            this.promoName = promoName;
            this.promoDesc = promoDesc;
            this.promoImg = promoImg;
            this.status = status;
        }

        public CafePromo()
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class PromoActivation
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }

        public PromoActivation()
        {

        }

        public PromoActivation(int id,string name,bool status)
        {
            this.id = id;
            this.name = name;
            this.status = status;
        }

    }
}
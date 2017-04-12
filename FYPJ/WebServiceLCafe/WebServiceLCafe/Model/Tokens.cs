using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceLCafe.Model
{
    public class Tokens : System.Web.Services.Protocols.SoapHeader
    {
        public int tokenID { get; set; }
        public string accessToken { get; set; }
        public string tokenDateTime { get; set; }


        public Tokens(int tokenID, string accessToken, string tokenDateTime)
        {
            this.tokenID = tokenID;
            this.accessToken = accessToken;
            this.tokenDateTime = tokenDateTime;
        }

        //public bool IsUserCredentialsValid(Tokens SoapHeader)
        //{
        //    if (SoapHeader == null)
        //        return false;

        //    // check the token exists in Cache
        //    if (!string.IsNullOrEmpty(SoapHeader.AuthenticationToken))
        //        return (HttpRuntime.Cache[SoapHeader.AuthenticationToken] != null);

        //    return false;
        //}
        public Tokens()
        {

        }
    }
}
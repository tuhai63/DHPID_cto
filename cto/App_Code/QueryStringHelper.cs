using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;


namespace cto
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    public static class QueryStringHelper
    {
        #region queryString
  
        public static string GetLang(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "lang") ? queryString["lang"].Trim() : "en";
        }
        public static string GetEstablishmentName(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "estname") ? queryString["estName"] : string.Empty;
        }

        public static string GetReferenceNumber(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "ref") ? queryString["ref"] : string.Empty;
        }
        public static string GetProvince(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "prov") ? queryString["prov"] : string.Empty;
        }
        public static string GetRating(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "rate") ? queryString["rate"] : string.Empty;
        }

        public static string GetCurrentlyRegistered(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "reg") ? queryString["reg"] : string.Empty;
        }

        public static string GetRegistrationNumber(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "regnum") ? queryString["regnum"] : string.Empty;
        }

        public static string GetInspectionStartDateFrom(this NameValueCollection queryString)
        {
           return queryString.AllKeys.Any(x => x.ToLower() == "insfrom") ? queryString["insFrom"] : string.Empty;
        }

        public static string GetInspectionStartDateTo(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "insto") ? queryString["insTo"] : string.Empty;
        }
        public static string GetCategory(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "cat") ? queryString["cat"] : string.Empty;
        }
        public static string GetInspectionNumber(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "insnumber") ? queryString["insNumber"] : string.Empty;
        }
        public static string GetActivity(this NameValueCollection queryString)
        {
            return queryString.AllKeys.Any(x => x.ToLower() == "act") ? queryString["act"] : string.Empty;
        }

        #endregion

    }

}



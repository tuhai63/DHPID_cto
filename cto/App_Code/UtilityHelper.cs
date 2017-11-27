
using System;
using System.Globalization;
using System.Linq;
using System.Threading;


namespace cto
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    public static class UtilityHelper
    {
        public static void SetDefaultCulture(string lang)
        {
            if (lang == "en")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-CA");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-CA");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
            }
        }

        public static string GetRating(string rating)
        {
            var returnValue = string.Empty;
            var lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            switch (rating.ToLower().Trim())
            {
                case "nc":
                    returnValue = Resources.Resource.NonCompliant;
                    break;
                case "c":
                    returnValue = Resources.Resource.Compliant;
                    break;
                case "i":
                    returnValue = Resources.Resource.Inprogress;
                    break;
                case "sr":
                    returnValue = Resources.Resource.SummaryReport;
                    break;
                case "ct":
                    returnValue = Resources.Resource.CompliantTermsConditions;
                    break;
                default:
                    returnValue = Resources.Resource.Inprogress;
                    break;
            }
            return returnValue;
        }

        public static string GetProvince(string province)
        {
            var returnValue = string.Empty;
            switch (province.ToLower().Trim())
            {
                case "alberta":
                case "ab":
                    returnValue = "AB";
                    break;
                case "british columbia":
                case "bc":
                    returnValue = "BC";
                    break;
                case "manitoba":
                case "mb":
                    returnValue = "MB";
                    break;
                case "new brunswick":
                case "nb":
                    returnValue = "NB";
                    break;
                case "newfoundland and labrador":
                case "nl":
                    returnValue = "NL";
                    break;
                case "northwest territories":
                case "nt":
                    returnValue = "NT";
                    break;
                case "nova scotia":
                case "ns":
                    returnValue = "NS";
                    break;
                case "nunavut":
                case "nu":
                    returnValue = "NU";
                    break;
                case "ontario":
                case "on":
                    returnValue = "ON";
                    break;
                case "prince edward island":
                case "pe":
                    returnValue = "PE";
                    break;
                case "quebec":
                case "qc":
                    returnValue = "QC";
                    break;
                case "saskatchewan":
                case "sk":
                    returnValue = "SK";
                    break;
                case "yukon":
                case "yt":
                    returnValue = "YT";
                    break;
                default:
                    returnValue = String.Empty;
                    break;
            }
            return returnValue;
        }

        public static string GetProvinceDesc(string province)
        {
            var returnValue = string.Empty;
            switch (province.Trim())
            {
                case "AB":
                    returnValue = Resources.Resource.province_AB;
                    break;
                case "BC":
                    returnValue = Resources.Resource.province_BC;
                    break;
                case "MB":
                    returnValue = Resources.Resource.province_MB;
                    break;
                case "NB":
                    returnValue = Resources.Resource.province_NB;
                    break;
                case "NL":
                    returnValue = Resources.Resource.province_NL;
                    break;
                case "NT":
                    returnValue = Resources.Resource.province_NT;
                    break;
                case "NS":
                    returnValue = Resources.Resource.province_NS;
                    break;
                case "NU":
                    returnValue = Resources.Resource.province_NU;
                    break;
                case "ON":
                    returnValue = Resources.Resource.province_ON;
                    break;
                case "PE":
                    returnValue = Resources.Resource.province_PE;
                    break;
                case "QC":
                    returnValue = Resources.Resource.province_QC;
                    break;
                case "SK":
                    returnValue = Resources.Resource.province_SK;
                    break;
                case "YT":
                    returnValue = Resources.Resource.province_YT;
                    break;
                default:
                    returnValue = string.Empty;
                    break;
            }
            return returnValue;
        }

        public static DateTime ConvertToDateTime(string strDateTime)
        {
            DateTime dtFinaldate;
            string sDateTime;
            try
            {
                dtFinaldate = Convert.ToDateTime(strDateTime);
            }
            catch (Exception e)
            {
                if (strDateTime.Contains("-"))
                {
                    string[] sDate = strDateTime.Split('-');
                    sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                }
                else
                {
                    string[] sDate = strDateTime.Split('/');
                    sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                }
                dtFinaldate = Convert.ToDateTime(sDateTime);
            }
            return dtFinaldate.Date;
        }

        public static string GetRatingCode(string rating)
        {
            var returnValue = string.Empty;
            switch (rating.ToLower().Trim())
            {
                case "non-compliant":
                case "nc":
                    returnValue = "NC";
                    break;
                case "compliant":
                case "c":
                    returnValue = "C";
                    break;
                case "progress":
                case "inspection in progress":
                case "in progress":
                case "inspection en cours":
                case "i":
                    returnValue = "I";
                    break;
                default:
                    returnValue = string.Empty;
                    break;
            }
            return returnValue;
        }

        public static string GetInspectionTypeFrench(string insType)
        {
            var returnValue = string.Empty;
            string[] keys = new string[] { "regular", "regular inspection", "re-inspection", "reinspection", "assessment", "re-assessment", "reassessment" };
            string sKeyResult = keys.FirstOrDefault<string>(s => insType.ToLower().Trim().Contains(s));

            switch (sKeyResult)
            {
                case "regular":
                case "regular inspection":
                    returnValue = "Inspection Régulière";
                    break;
                case "re-inspection":
                case "reinspection":
                    returnValue = "Ré-inspection";
                    break;
                case "assessment":
                case "re-assessment":
                case "reassessment":
                    returnValue = "Ré-évaluation";
                    break;
                default:
                    returnValue = "Inspection Régulière";
                    break;
            }
            return returnValue;
        }
    }
}


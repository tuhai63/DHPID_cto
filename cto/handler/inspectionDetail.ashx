<%@ WebHandler Language="C#" Class="cto.inspectionDetail" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace cto
{
    public class inspectionDetail : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            InspectionItem item = new InspectionItem();
            try
            {
                //Get All the QueryStrings
                item.insepctionNumber = Convert.ToInt32(context.Request.QueryString["insNumber"].ToString());
                item.lang = context.Request.QueryString["lang"];
                if (item.lang == "en")
                {
                    UtilityHelper.SetDefaultCulture("en");
                }
                else
                {
                    UtilityHelper.SetDefaultCulture("fr");
                }

                var readDB = new DBConnection(item.lang);
                var inspectionDataTable = new DataTable();
                var resultString = string.Empty;
                inspectionDataTable = readDB.GetInspectionByInspectionNumber(item.insepctionNumber);

                if (inspectionDataTable.Rows.Count > 0)
                {
                    DataRow row = inspectionDataTable.Rows[0];

                    item.reportCard = false;
                    // street
                    item.street = row["street"] == DBNull.Value ? string.Empty :  row["street"].ToString().Trim();
                    // city_name
                    item.city = row["city"] == DBNull.Value ? string.Empty :  row["city"].ToString().Trim();
                    // province
                    item.province = row["province"] == DBNull.Value ? string.Empty :  UtilityHelper.GetProvinceDesc(row["province"].ToString().Trim());
                    // country_desc  
                    item.country = row["country"] == DBNull.Value ? string.Empty :  row["country"].ToString().Trim();
                    // postal_code
                    item.postalCode = row["postalCode"] == DBNull.Value ? string.Empty :  row["postalCode"].ToString().Trim();
                    item.establishmentName = row["estName"].ToString().Trim();
                    item.referenceNumber = row["refNumber"].ToString().Trim();
                    item.registrationNumber = row["regNumber"].ToString().Trim();
                    item.currentlyRegistered = row["curRegistered"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(row["curRegistered"].ToString().Trim());

                    //Get all of ActivityList
                    var activities = new List<string>();
                    if (row["activity"] != DBNull.Value && !string.IsNullOrEmpty(row["activity"].ToString().Trim()))
                    {
                        if (row["activity"].ToString().Trim().Contains(","))
                        {
                            activities = new List<string>(row["activity"].ToString().Split(new string[] { "," }, StringSplitOptions.None));
                        }
                        else
                        {
                            activities.Add(row["activity"].ToString().Trim());
                        }
                    }
                    item.activityList = activities;
                    item.inspectionList = new List<InspectionSubItem>();
                    item.inspectionStartDate = row["insStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["insStartDate"].ToString().Trim());
                    item.inspectionType = row["insType"].ToString().Trim();
                    item.rating = row["rating"] == DBNull.Value ? string.Empty :  row["rating"].ToString().Trim();
                    item.ratingDesc = row["rating"] == DBNull.Value ? string.Empty :  UtilityHelper.GetRating(row["rating"].ToString().Trim());
                    item.reportCard = row["reportCard"] == DBNull.Value ? false : Convert.ToBoolean(row["reportCard"].ToString().Trim());

                    var insSubItem = new InspectionSubItem();
                    insSubItem.insepctionNumber = item.insepctionNumber;
                    insSubItem.inspectionStartDate = item.inspectionStartDate;
                    insSubItem.rating = item.rating;
                    insSubItem.ratingDesc = item.ratingDesc;
                    insSubItem.inspectionType = item.inspectionType;
                    insSubItem.reportCard = item.reportCard;
                    item.inspectionList.Add(insSubItem);

                    //Get all of Inspections'Date list by same referenceNumber.
                    List<InspectionSubItem> list = new List<InspectionSubItem>();
                    var dtInspections = readDB.GetInspectionByRefNumber(item.insepctionNumber, item.referenceNumber, item.registrationNumber);
                    var results = (from p in dtInspections.AsEnumerable()
                                   where p.Field<string>("estName").ToLower().Trim().Equals(item.establishmentName.ToLower().Trim())
                                   select p).ToList();
                    if (results  !=null && results.Count > 0)
                    {
                        foreach (DataRow dr in results)
                        {
                            var item2 = new InspectionSubItem();
                            item2.insepctionNumber = Convert.ToInt32(dr["insNumber"].ToString().Trim());
                            item2.inspectionStartDate = Convert.ToDateTime(dr["insStartDate"]).AddHours(12);
                            item2.inspectionType = dr["insType"].ToString().Trim();
                            item2.rating = dr["rating"] != DBNull.Value ? dr["rating"].ToString().Trim() : string.Empty;
                            item2.ratingDesc = dr["rating"] != DBNull.Value ? UtilityHelper.GetRating(dr["rating"].ToString().Trim()) : string.Empty;
                            item2.reportCard = (dr["reportCard"] != DBNull.Value ? Convert.ToBoolean(dr["reportCard"]) : false);
                            item.inspectionList.Add(item2);
                        }
                    }
                    if( item.inspectionList.Count > 0)
                    {
                        item.totalInspecitions = (  from n in item.inspectionList
                                                    where n.inspectionStartDate.Value.Year >= DateTime.Now.AddYears(-3).Year
                                                    select n).Count();
                    }

                    resultString = JsonHelper.JsonSerializer<InspectionItem>(item);
                    context.Response.Write(resultString);
                }
                else
                {
                    string errorMessage = string.Format("InspectionDetail.ashx - There is no data for Inspection Number : {0}", item.insepctionNumber);
                    ExceptionHelper.LogException(new Exception(), errorMessage);
                    context.Response.Write("{\"insepctionNumber\":\"\"}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "InspectionDetail.ashx");
                context.Response.Write("{\"insepctionNumber\":\"\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
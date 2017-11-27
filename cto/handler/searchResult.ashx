<%@ WebHandler Language="C#" Class="cto.searchResult" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;


namespace cto
{
    public class searchResult : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            SearchItem searchItem = new SearchItem();
            try
            {
                var lang = string.IsNullOrEmpty(context.Request.QueryString.GetLang().Trim()) ? "en" : context.Request.QueryString.GetLang().Trim();
                if (lang == "en")
                {
                    UtilityHelper.SetDefaultCulture("en");
                }
                else
                {
                    UtilityHelper.SetDefaultCulture("fr");
                }

                //Get All the QueryStrings
                searchItem.establishmentName = context.Request.QueryString.GetEstablishmentName().Trim();
                searchItem.referenceNumber = context.Request.QueryString.GetReferenceNumber().Trim();
                searchItem.province = context.Request.QueryString.GetProvince().Trim();
                searchItem.rating = context.Request.QueryString.GetRating().Trim();
                searchItem.currentlyRegistered = string.IsNullOrEmpty(context.Request.QueryString.GetCurrentlyRegistered().Trim()) ? (int?)null : Convert.ToInt32(context.Request.QueryString.GetCurrentlyRegistered().Trim());
                searchItem.registrationNumber = string.IsNullOrEmpty(context.Request.QueryString.GetRegistrationNumber().Trim()) ? string.Empty : context.Request.QueryString.GetRegistrationNumber().Trim();
                searchItem.activity = string.IsNullOrEmpty(context.Request.QueryString.GetActivity().Trim()) ? string.Empty : context.Request.QueryString.GetActivity().Trim();
                searchItem.inspectionStartDateFrom = string.IsNullOrEmpty(context.Request.QueryString.GetInspectionStartDateFrom().Trim()) ? (DateTime?)null : Convert.ToDateTime(context.Request.QueryString.GetInspectionStartDateFrom().Trim());
                searchItem.inspectionStartDateTo = string.IsNullOrEmpty(context.Request.QueryString.GetInspectionStartDateTo().Trim()) ? (DateTime?)null : Convert.ToDateTime(context.Request.QueryString.GetInspectionStartDateTo().Trim());


                //Connect DB and Get data from inspections' Table
                var readDB = new DBConnection(lang);
                var inspectionDataTable = new DataTable();
                var resultString = string.Empty;
                inspectionDataTable = readDB.GetAllInspections(searchItem);

                if (inspectionDataTable.Rows.Count > 0)
                {
                    var resultList = new List<InspectionItem>(inspectionDataTable.Rows.Count);

                    foreach (DataRow row in inspectionDataTable.Rows)
                    {
                        var itemArray = row.ItemArray;
                        if ( itemArray[0] != DBNull.Value && !string.IsNullOrEmpty(itemArray[0].ToString().Trim()))
                        {
                            var item = new InspectionItem()
                            {
                                insepctionNumber = Convert.ToInt32(itemArray[0].ToString().Trim()),
                                referenceNumber = itemArray[1].ToString().Trim(),
                                establishmentName = itemArray[2].ToString().Trim(),
                                province =  itemArray[3] == DBNull.Value ? string.Empty :  UtilityHelper.GetProvinceDesc(itemArray[3].ToString().Trim()),
                                inspectionStartDate = itemArray[4] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(itemArray[4].ToString().Trim()),
                                rating = itemArray[5].ToString().Trim(),
                                ratingDesc = UtilityHelper.GetRating(itemArray[5].ToString().Trim()),
                                currentlyRegistered = itemArray[6] == DBNull.Value ? (bool?)null : Convert.ToBoolean(itemArray[6].ToString().Trim()),
                                reportCard = itemArray[7] == DBNull.Value ? false : Convert.ToBoolean(itemArray[7].ToString().Trim()),
                            };                            
                            resultList.Add(item);

                        }
                    }
                    resultString = JsonHelper.JsonSerializer<List<InspectionItem>>(resultList);
                    resultString = "{\"data\":" + resultString + "}";
                    context.Response.Write(resultString);
                }
                else
                {
                    context.Response.Write("{\"data\":[]}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "SearchResult.ashx");
                context.Response.Write("{\"data\":[]}");
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
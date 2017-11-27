<%@ WebHandler Language="C#" Class="cto.reportResult" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace cto
{
    public class reportResult : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");


            try
            {
                var lang = context.Request.QueryString.GetLang();
                if (lang == "en")
                {
                    UtilityHelper.SetDefaultCulture("en");
                }
                else
                {
                    UtilityHelper.SetDefaultCulture("fr");
                }

                var ctoDB = new DBConnection(lang);
                var inspectionList = ctoDB.GetAllInspections();
                var filteredList = inspectionList.Where(x => x.createdDate >= new DateTime(2016, 03, 29)).ToList();
               
                string outputTxt = string.Empty;
                var resultList = new List<InspectionItem>();
                if (filteredList != null && filteredList.Count > 0)
                {
                    outputTxt = JsonHelper.JsonSerializer<List<InspectionItem>>(filteredList);
                    outputTxt = "{\"data\":" + outputTxt + "}";
                    context.Response.Write(outputTxt);
                }
                else
                {
                    context.Response.Write("{\"data\":[]}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "reportResult.ashx");
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
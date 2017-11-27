<%@ WebHandler Language="C#" Class="cto.initialReportCard" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace cto
{
    public class initialReportCard : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            InitialReportItem item = new InitialReportItem();
            try
            {
                item.insepctionNumber = Convert.ToInt32(context.Request.QueryString["insNumber"]);
                var lang = context.Request.QueryString.GetLang();

                if (String.Compare(lang, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (lang == "en")
                    {
                        UtilityHelper.SetDefaultCulture("en");
                    }
                    else
                    {
                        UtilityHelper.SetDefaultCulture("fr");
                    }
                }

                var dhpidDB = new DBConnection(lang);
                var dt = dhpidDB.GetIIDReportByInspectionNumber(item.insepctionNumber);
                var allIRCs = dhpidDB.GetIRCs();
                string outputTxt = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    //Get the first row from the DataTable
                    DataRow row = dt.Rows[0];
                    item.referenceNumber = row["refNumber"].ToString().Trim();
                    item.establishmentName = row["estName"].ToString().Trim();
                    item.insStartDate = Convert.ToDateTime(row["insStartDate"]).AddHours(12);
                    if(row["insEndDate"] != DBNull.Value && !string.IsNullOrEmpty(row["insEndDate"].ToString().Trim()) )
                    {
                        item.insEndDate = Convert.ToDateTime(row["insEndDate"]).AddHours(12);
                    }
                    item.insType = row["insType"].ToString().Trim();
                    item.reportCard = (row["reportCard"] != DBNull.Value ? Convert.ToBoolean(row["reportCard"]) : false);
                    item.ircInspectionNumber = 0;
                    if (item.reportCard)
                    {
                        item.ircInspectionNumber = allIRCs.FirstOrDefault(p => p == item.insepctionNumber);
                    }
                    var groupList = from g in dt.AsEnumerable()
                                    group g by g.Field<int>("orderNo") into Group1
                                    select new { No = Group1.Key, Items = Group1 };

                    item.summaryItemList = new List<SummaryItem>();
                    foreach (var p in groupList)
                    {
                        var sItem = new SummaryItem();
                        sItem.no = p.No.ToString();
                        sItem.summaryList = new List<string>();
                        foreach (var c in p.Items)
                        {
                            sItem.regulation = c["reg"].ToString().Trim();
                            sItem.summaryList.Add(c["observation"].ToString().Trim());
                        }

                        item.summaryItemList.Add(sItem);
                    }

                    outputTxt = JsonHelper.JsonSerializer<InitialReportItem>(item);
                    outputTxt = outputTxt.Replace("summaryItemList", "data");
                    context.Response.Write(outputTxt);

                }
                else
                {
                    string errorMessage = string.Format("InitialReportCard.ashx - There is no data for Insepction Number : {0}", item.insepctionNumber);
                    ExceptionHelper.LogException(new Exception(), errorMessage);
                    context.Response.Write("{\"gcpID\":\"\", \"data\":[]}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "InitialReportCard.ashx");
                context.Response.Write("{\"gcpID\":\"\", \"data\":[]}");
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


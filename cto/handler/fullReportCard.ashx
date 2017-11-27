<%@ WebHandler Language="C#" Class="cto.fullReportCard" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace cto
{
    public class fullReportCard : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            FullReportItem item = new FullReportItem();
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
                var dt = dhpidDB.GetFullReportByInspectionNumber(item.insepctionNumber );
                // var allInspections = dhpidDB.GetAllInspections();
                var allIIDs = dhpidDB.GetIIDs();
                string outputTxt = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    //Get the first row from the DataTable
                    DataRow row = dt.Rows[0];
                    item.referenceNumber = row["refNumber"].ToString().Trim();
                    item.establishmentName = row["estName"].ToString().Trim();
                    item.insStartDate = Convert.ToDateTime(row["insStartDate"]).AddHours(12);
                    item.insType = row["insType"].ToString().Trim();
                    item.rating = row["rating"] != DBNull.Value ? UtilityHelper.GetRating(row["rating"].ToString().Trim()) : string.Empty;
                    item.reportCard = (row["reportCard"] != DBNull.Value ? Convert.ToBoolean(row["reportCard"]) : false);
                    item.iidInspectionNumber = 0;
                    if (item.reportCard)
                    {
                         item.iidInspectionNumber = allIIDs.FirstOrDefault(p=> p == item.insepctionNumber);                        
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        var values = dr.ItemArray;
                        {
                            if (values[11] != DBNull.Value && !string.IsNullOrEmpty(values[11].ToString().Trim()))
                            {
                                //var outCome = new List<string>(values[11].ToString().Split(new string[] { "\"," }, StringSplitOptions.None));
                                var outCome =  values[11].ToString().Replace(@"""", "");
                                string[] arrayOutcome = outCome.Trim().Split('|');
                                item.insOutcomeList = new List<string>(arrayOutcome.Length);
                                item.insOutcomeList.AddRange(arrayOutcome);
                            }
                            if (values[12] != DBNull.Value && !string.IsNullOrEmpty(values[12].ToString().Trim())) {
                                var measureTaken =  values[12].ToString().Replace(@"""", "");
                                string[] arrayMeasureTaken = measureTaken.Trim().Split('|');
                                item.measureTakenList = new List<string>(arrayMeasureTaken.Length);
                                item.measureTakenList.AddRange(arrayMeasureTaken);
                            }

                            if( item.insOutcomeList != null && item.insOutcomeList.Count > 0)
                            {
                                break;
                            }
                        }
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
                    outputTxt = JsonHelper.JsonSerializer<FullReportItem>(item);
                    outputTxt = outputTxt.Replace("summaryItemList", "data");
                    context.Response.Write(outputTxt);
                }
                else
                {
                    string errorMessage = string.Format("FullReportCard.ashx - There is no data for insepction Number : {0}", item.insepctionNumber);
                    ExceptionHelper.LogException(new Exception(), errorMessage);
                    context.Response.Write("{\"insNumber\":\"\", \"data\":[]}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "fullReportCard.ashx");
                context.Response.Write("{\"insNumber\":\"\", \"data\":[]}");
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

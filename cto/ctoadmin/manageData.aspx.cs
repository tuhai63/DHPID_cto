using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace cto
{
    public partial class ctoadmin_manageData : System.Web.UI.Page
    {
        public string excelDataLocation { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            excelDataLocation = ConfigurationManager.AppSettings["excelDataLocation"].ToString();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            var redirectUrl = string.Empty;
            if (Page.IsValid && !string.IsNullOrEmpty(this.workType.SelectedValue))
            {
                switch (this.workType.SelectedValue)
                {
                    case "1":
                        insertInspections_Click(sender, e);
                        break;
                    //case "2":
                    //    insertIIDs_Click(sender, e);
                    //    break;
                    //case "3":
                    //    insertIRCs_Click(sender, e);
                    //    break;
                    case "4":
                        insertInspections_Click(sender, e);
                        insertIIDs_Click(sender, e);
                        break;
                    case "5":
                        insertInspections_Click(sender, e);
                        insertIRCs_Click(sender, e);
                        break;
                }
            }
        }

        private int GetLastThreeNumbers(string inspectionNumber)
        {
            int threeNumber = 1;
            var currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');

            var lastInspectionMonth = inspectionNumber.Substring(4, 2);
            if (currentMonth.Equals(lastInspectionMonth))
            {
                threeNumber = Convert.ToInt32(inspectionNumber.Substring(inspectionNumber.Length - 3, 3)) + 1;
            }
            return threeNumber;
        }

        protected void insertInspections_Click(object sender, EventArgs e)
        {
            if ((file1.PostedFile != null) && (file1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(file1.PostedFile.FileName);
                var excelData = excelDataLocation + fn;
                try
                {
                    file1.Dispose();
                    var items = new List<InspectionItem>();
                    var trialPhase = string.Empty;
                    var ctoDB = new DBConnection("en");
                    var updatedRowCount = 0;
                    var tempNumber = string.Format("{0}{1}", DateTime.Now.Year.ToString(),DateTime.Now.Month.ToString().PadLeft(2, '0'));
                    var tempInsNumber = string.Empty;

                    using (DataSet excelds = ExcelHelper.ReadClinicalExcelData(excelData))
                    {
                        if (excelds != null && excelds.Tables.Count > 0)
                        {
                            DataTable dt = excelds.Tables[0];
                            items = new List<InspectionItem>(dt.Rows.Count);
                            var oldItems = ctoDB.GetAllInspections();
                            var tempLastThreeNumber = 1;
                            if (oldItems.Count > 0)
                            {
                                var lastInspection = oldItems.OrderByDescending(x => x.insepctionNumber).Take(1);
                                 tempLastThreeNumber = GetLastThreeNumbers(lastInspection.FirstOrDefault().insepctionNumber.ToString());
                            }

                            foreach (DataRow row in dt.Rows)
                            {
                                var values = row.ItemArray;
                                var item = new InspectionItem();
                                tempInsNumber = string.Format("{0}{1}", tempNumber, tempLastThreeNumber.ToString().PadLeft(3, '0'));

                                //Referenc Number
                                if (values[0] != DBNull.Value && !string.IsNullOrEmpty(values[0].ToString().Trim()))
                                {
                                    item.referenceNumber = values[0].ToString().Trim();
                                }

                                if (item.referenceNumber != null && !string.IsNullOrWhiteSpace(item.referenceNumber.Trim()))
                                { 
                                    //Establishment Name
                                    if (values[1] != DBNull.Value && !string.IsNullOrEmpty(values[1].ToString().Trim()))
                                    {
                                        item.establishmentName = values[1].ToString().Trim();
                                    }
                                    //CURRENTLY_registered
                                    if (values[2] != DBNull.Value && !string.IsNullOrEmpty(values[2].ToString().Trim()))
                                    {
                                        item.currentlyRegistered = values[2].ToString().Trim().ToLower().Equals("y");
                                    }
                                    else
                                    {
                                        item.currentlyRegistered = (bool?)null;
                                    }
                                    //Registration  Number
                                    if (values[3] != DBNull.Value && !string.IsNullOrEmpty(values[3].ToString().Trim()))
                                    {
                                        item.registrationNumber = values[3].ToString().Trim();
                                    }
                                    //INSP_TYPE
                                    if (values[4] != DBNull.Value && !string.IsNullOrEmpty(values[4].ToString().Trim()))
                                    {
                                        item.inspectionTypeFr = values[4].ToString().Trim();
                                      //  item.inspectionTypeFr = UtilityHelper.GetInspectionTypeFrench(values[4].ToString().Trim()) ;
                                    }

                                    //INSP_TYPE
                                    if (values[5] != DBNull.Value && !string.IsNullOrEmpty(values[5].ToString().Trim()))
                                    {
                                        item.inspectionType = values[5].ToString().Trim();
                                        //  item.inspectionTypeFr = UtilityHelper.GetInspectionTypeFrench(values[4].ToString().Trim()) ;
                                    }
                                    //Rating
                                    if (values[6] != DBNull.Value && !string.IsNullOrEmpty(values[6].ToString().Trim()))
                                    {
                                        item.rating = UtilityHelper.GetRatingCode(values[6].ToString().Trim());
                                    }

                                    //InspectionStartDate
                                    if (values[7] != DBNull.Value && !string.IsNullOrEmpty(values[7].ToString().Trim()))
                                    {
                                        item.inspectionStartDate = Convert.ToDateTime(values[7].ToString().Trim());
                                        if (item.inspectionStartDate > DateTime.MaxValue || item.inspectionStartDate < DateTime.MinValue)
                                        {
                                            item.inspectionStartDate = new DateTime(2011, 12, 31);   //TEMPORARY.
                                        }
                                    }
                                    else
                                    {
                                        item.inspectionStartDate = new DateTime(2011, 12, 31);   //TEMPORARY.
                                    }
                                    //Inspection end date
                                    if (values[8] != DBNull.Value && !string.IsNullOrEmpty(values[8].ToString().Trim()))
                                    {
                                        item.inspectionEndDate = Convert.ToDateTime(values[8].ToString().Trim());
                                        if (item.inspectionEndDate > DateTime.MaxValue || item.inspectionEndDate < DateTime.MinValue)
                                        {
                                            item.inspectionEndDate = (DateTime?)null;   //TEMPORARY.
                                        }
                                    }

                                    //Street
                                    if (values[9] != DBNull.Value && !string.IsNullOrEmpty(values[9].ToString().Trim()))
                                    {
                                        item.street = values[9].ToString().Trim();
                                    }
                                    //City
                                    if (values[10] != DBNull.Value && !string.IsNullOrEmpty(values[10].ToString().Trim()))
                                    {
                                        item.city = values[10].ToString().Trim();
                                    }
                                    //Province
                                    if (values[11] != DBNull.Value && !string.IsNullOrEmpty(values[11].ToString().Trim()))
                                    {
                                        item.province = UtilityHelper.GetProvince(values[11].ToString().Trim());
                                    }
                                    //Country
                                    if (values[12] != DBNull.Value && !string.IsNullOrEmpty(values[12].ToString().Trim()))
                                    {
                                        item.country = values[12].ToString().Trim();
                                    }
                                    //Postal code 
                                    if (values[13] != DBNull.Value && !string.IsNullOrEmpty(values[13].ToString().Trim()))
                                    {
                                        item.postalCode = values[13].ToString().Trim();
                                    }

                                    //ACTIVITY_EN     
                                    if (values[14] != DBNull.Value && !string.IsNullOrEmpty(values[14].ToString().Trim()))
                                    {
                                        item.activityEn = values[14].ToString().Trim();
                                    }
                                    //ACTIVITY_FR  
                                    if (values[15] != DBNull.Value && !string.IsNullOrEmpty(values[15].ToString().Trim()))
                                    {
                                        item.activityFr = values[15].ToString().Trim();
                                    }
                                                               
                                    //ReportCard
                                    item.reportCard = false;                               

                                    if (oldItems != null && oldItems.Count > 0  && item.rating != null && !item.rating.ToLower().Equals("i"))
                                    {
                                        var foundInspection = (from c in oldItems
                                                               where c.referenceNumber.Equals(item.referenceNumber)
                                                                     && c.inspectionStartDate == item.inspectionStartDate
                                                                     && c.registrationNumber.Equals(item.registrationNumber)
                                                                     && c.rating.ToLower().Equals("i")
                                                               select c).FirstOrDefault();

                                        if (foundInspection != null && foundInspection.insepctionNumber > 0)
                                        {
                                            item.insepctionNumber = foundInspection.insepctionNumber;
                                            updatedRowCount += ctoDB.InspectionsUpdate(item.insepctionNumber, item.rating);
                                            outputTxt.Text = string.Format(" {0} updated to CTO inspection", updatedRowCount);
                                        }
                                        else
                                        {
                                            item.insepctionNumber = Convert.ToInt32(tempInsNumber);                                           
                                            items.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        item.insepctionNumber = Convert.ToInt32(tempInsNumber);
                                            items.Add(item);
                                    }
                                    tempLastThreeNumber++;
                                }
                            }

                            if (items != null && items.Count > 0)
                            {
                                var insertedRowCount = ctoDB.InspectionsCreate(items);
                                if (insertedRowCount > 0)
                                {
                                    outputTxt.Text = string.Format(" {0} inserted in CTO inspection", insertedRowCount);
                                }
                            }

                        }
                        else
                        {
                            outputTxt.Text = "Please check your excel file.";
                        }
                    }

                }
                catch (Exception ex)
                {
                    outputTxt.Text = ex.Message;
                }
            }
            else
            {
                outputTxt.Text = "Please select a file to upload.";
            }

        }

        protected void insertIRCs_Click(object sender, EventArgs e)
       {
            if ((file1.PostedFile != null) && (file1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(file1.PostedFile.FileName);
                var excelData = excelDataLocation + fn;
                try
                {
                    file1.Dispose();
                    var items = new List<FullReportItem>();
                    var ctoDB = new DBConnection("en");
                    using (DataSet excelds = ExcelHelper.ReadClinicalExcelData(excelData))
                    {
                        if (excelds != null && excelds.Tables.Count > 0)
                        {
                            DataTable dt = excelds.Tables[1];
                            items = new List<FullReportItem>(dt.Rows.Count);
                            foreach (DataRow row in dt.Rows)
                            {
                                var values = row.ItemArray;
                                var item = new FullReportItem();
                                try
                                {
                                    //Reference Number
                                    if (values[0] != DBNull.Value && !string.IsNullOrEmpty(values[0].ToString().Trim()))
                                    {
                                        item.referenceNumber = values[0].ToString().Trim();
                                    }

                                    //Establishment Name
                                    if (values[1] != DBNull.Value && !string.IsNullOrEmpty(values[1].ToString().Trim()))
                                    {
                                        item.establishmentName = values[1].ToString().Trim();
                                    }
                                    //Inspection Type
                                    if (values[2] != DBNull.Value && !string.IsNullOrEmpty(values[2].ToString().Trim()))
                                    {
                                        item.insType = values[2].ToString().Trim();
                                    }
                                    // rating
                                    if (values[4] != DBNull.Value && !string.IsNullOrEmpty(values[4].ToString().Trim()))
                                    {
                                        item.rating = values[4].ToString().Trim();
                                    }
                                    //InspectionStartDate
                                    if (values[5] != DBNull.Value && !string.IsNullOrEmpty(values[5].ToString().Trim()))
                                    {
                                        item.insStartDate = UtilityHelper.ConvertToDateTime(values[5].ToString().Trim());
                                    }

                                    //OrderID
                                    if (values[6] != DBNull.Value && !string.IsNullOrEmpty(values[6].ToString().Trim()))
                                    {
                                        item.orderNo = Convert.ToInt32(values[6].ToString().Trim());
                                    }
                                    //SubOrderID
                                    if (values[7] != DBNull.Value && !string.IsNullOrEmpty(values[7].ToString().Trim()))
                                    {
                                        item.subOrderNo = Convert.ToInt32(values[7].ToString().Trim());
                                    }
                                    //Regulation (EN)
                                    if (values[8] != DBNull.Value && !string.IsNullOrEmpty(values[8].ToString().Trim()))
                                    {
                                        item.reg = values[8].ToString().Trim();
                                    }
                                    //Regulation (FR)
                                    if (values[9] != DBNull.Value && !string.IsNullOrEmpty(values[9].ToString().Trim()))
                                    {
                                        item.regFr = values[9].ToString().Trim();
                                    }
                                    //Observation(EN)
                                    if (values[10] != DBNull.Value && !string.IsNullOrEmpty(values[10].ToString().Trim()))
                                    {
                                        item.obsDesc = values[10].ToString().Trim();
                                    }
                                    //Observation(FR)
                                    if (values[11] != DBNull.Value && !string.IsNullOrEmpty(values[11].ToString().Trim()))
                                    {
                                        item.obsDescFr = values[11].ToString().Trim();
                                    }

                                    //Inspection Outcome(EN)
                                    if (values[12] != DBNull.Value && !string.IsNullOrEmpty(values[12].ToString().Trim()))
                                    {
                                        item.insOutcome = values[12].ToString().Trim();
                                    }
                                    //Inspection Outcome(FR)
                                    if (values[13] != DBNull.Value && !string.IsNullOrEmpty(values[13].ToString().Trim()))
                                    {
                                        item.insOutcomeFr = values[13].ToString().Trim();
                                    }
                                    //Measure Taken(EN)
                                    if (values[14] != DBNull.Value && !string.IsNullOrEmpty(values[14].ToString().Trim()))
                                    {
                                        item.measureTaken = values[14].ToString().Trim();
                                    }
                                    //Measure Taken(FR)
                                    if (values[15] != DBNull.Value && !string.IsNullOrEmpty(values[15].ToString().Trim()))
                                    {
                                        item.measureTakenFr = values[15].ToString().Trim();
                                    }
   
                                    items.Add(item);
                                }
                                catch (Exception ex)
                                {
                                    items.Clear();
                                    var errorMessages = string.Format("Default.aspx - IRC Upload()- reference Number:{0}, inspection start Date : {1}", item.referenceNumber, item.insStartDate);
                                    ExceptionHelper.LogException(ex, errorMessages);
                                    break;
                                }

                            }
                        }
                    }

                    if (items != null && items.Count > 0)
                    {
                        //1. Grouping by controlNumber, insStartDate, and rating
                        var itemsGroup = items.GroupBy(p => new { p.referenceNumber, p.insStartDate, p.rating },
                                         (key, group) => new
                                         {
                                             referenceNumber = key.referenceNumber,
                                             insStartDate = key.insStartDate,
                                             rating = key.rating,
                                             count = group.Count(),
                                             ircList = group.ToList()
                                         });


                        //2. Get all of Inspections
                        var allInspections = ctoDB.GetAllInspections();

                        //3. Filter inspections only match between Inspections and IRC data based on controlNumber, insStartDate, Rating                    
                        var filteredIRCItems = new List<FullReportItem>();
                        foreach (var v in itemsGroup)
                        {
                            if (v.count > 0)
                            {
                                var filteredInspectionList = (from c in allInspections
                                                              where c.referenceNumber.Equals(v.referenceNumber)
                                                                && c.inspectionStartDate == v.insStartDate
                                                                && c.rating.Equals(v.rating)
                                                              select c).FirstOrDefault();

                                var outComeBulider = new StringBuilder();
                                var outComeBuliderFr = new StringBuilder();
                                var measureBulider = new StringBuilder();
                                var measureBuliderFr = new StringBuilder();
                                foreach (var newItem in v.ircList)
                                {
                                    if (!string.IsNullOrEmpty(newItem.insOutcome))
                                    {
                                        outComeBulider.Append(@"""");
                                        outComeBulider.Append(newItem.insOutcome.ToString().Trim());
                                        outComeBulider.Append(@"""");
                                        outComeBulider.Append("|");
                                    }
                                    if (!string.IsNullOrEmpty(newItem.insOutcomeFr))
                                    {
                                        outComeBuliderFr.Append(@"""");
                                        outComeBuliderFr.Append(newItem.insOutcomeFr.ToString().Trim());
                                        outComeBuliderFr.Append(@"""");
                                        outComeBuliderFr.Append("|");
                                    }
                                    if (!string.IsNullOrEmpty(newItem.measureTaken))
                                    {
                                        measureBulider.Append(@"""");
                                        measureBulider.Append(newItem.measureTaken.ToString().Trim());
                                        measureBulider.Append(@"""");
                                        measureBulider.Append("|");
                                    }

                                    if (!string.IsNullOrEmpty(newItem.measureTakenFr))
                                    {
                                        measureBuliderFr.Append(@"""");
                                        measureBuliderFr.Append(newItem.measureTakenFr.ToString().Trim());
                                        measureBuliderFr.Append(@"""");
                                        measureBuliderFr.Append("|");
                                    }
                                }

                                v.ircList.ForEach(x =>
                                {
                                    x.insOutcome = "";
                                    x.insOutcomeFr = "";
                                    x.measureTaken = "";
                                    x.measureTakenFr = "";
                                });
                                v.ircList[0].insOutcome = outComeBulider.ToString().TrimEnd('|');
                                v.ircList[0].insOutcomeFr = outComeBuliderFr.ToString().TrimEnd('|');
                                v.ircList[0].measureTaken = measureBulider.ToString().TrimEnd('|');
                                v.ircList[0].measureTakenFr = measureBuliderFr.ToString().TrimEnd('|');

                                //4. Get GCPID from filteredInspections
                                if (filteredInspectionList != null && filteredInspectionList.insepctionNumber > 0)
                                {
                                    v.ircList.ForEach(x =>
                                    {
                                        x.insepctionNumber = filteredInspectionList.insepctionNumber;
                                    });
                                    filteredIRCItems.AddRange(v.ircList);
                                }
                            }
                        }
						var removeOrderNo0 = filteredIRCItems.RemoveAll(x => x.orderNo == 0);
                        var ctoIDs = filteredIRCItems.Select(x => x.insepctionNumber).Distinct().ToList();
                        var insertedRowCount = ctoDB.ReportSummaryCreate(filteredIRCItems);
                        var updatedReportCard = ctoDB.InspectionReportCardUpdate(ctoIDs);

                        if (insertedRowCount > 0 && updatedReportCard > 0)
                        {
                            outputTxt.Text = string.Format("{0} inserted in Report Card Summary <br/> {1} updated in Report Card of CTO inspection", insertedRowCount, updatedReportCard);
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputTxt.Text = ex.Message;
                }
            }
            else
            {
                outputTxt.Text = "Please select a file to upload.";
            }

        }
       

        protected void insertIIDs_Click(object sender, EventArgs e)
        {
            if ((file1.PostedFile != null) && (file1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(file1.PostedFile.FileName);
                var excelData = excelDataLocation + fn;
                try
                {
                    file1.Dispose();
                    var items = new List<InitialReportItem>();
                    var ctoDB = new DBConnection("en");

                    using (DataSet excelds = ExcelHelper.ReadClinicalExcelData(excelData))
                    {
                        if (excelds != null && excelds.Tables.Count > 0)
                        {
                            DataTable dt = excelds.Tables[1];
                            items = new List<InitialReportItem>(dt.Rows.Count);
                            foreach (DataRow row in dt.Rows)
                            {
                                var values = row.ItemArray;
                                var item = new InitialReportItem();
                                //Ref Number
                                if (values[0] != DBNull.Value && !string.IsNullOrEmpty(values[0].ToString().Trim()))
                                {
                                    item.referenceNumber = values[0].ToString().Trim();
                                }
                                if (item.referenceNumber != null && !string.IsNullOrWhiteSpace(item.referenceNumber.Trim()))
                                {
                                    //Establishment name
                                    if (values[1] != DBNull.Value && !string.IsNullOrEmpty(values[1].ToString().Trim()))
                                    {
                                        item.establishmentName = values[1].ToString().Trim();
                                    }
                                    //inspection Type
                                    if (values[3] != DBNull.Value && !string.IsNullOrEmpty(values[3].ToString().Trim()))
                                    {
                                        item.insType = values[3].ToString().Trim();
                                    }
                                    //InspectionStartDate
                                    if (values[4] != DBNull.Value && !string.IsNullOrEmpty(values[4].ToString().Trim()))
                                    {
                                        item.insStartDate = UtilityHelper.ConvertToDateTime(values[4].ToString().Trim());
                                    }
                                    //InspectionEndDate
                                    if (values[5] != DBNull.Value && !string.IsNullOrEmpty(values[5].ToString().Trim()))
                                    {
                                        item.insEndDate = UtilityHelper.ConvertToDateTime(values[5].ToString().Trim());
                                    }

                                    //OrderID
                                    if (values[6] != DBNull.Value && !string.IsNullOrEmpty(values[6].ToString().Trim()))
                                    {
                                        item.orderNo = Convert.ToInt32(values[6].ToString().Trim());
                                    }
                                    //SubOrderID
                                    if (values[7] != DBNull.Value && !string.IsNullOrEmpty(values[7].ToString().Trim()))
                                    {
                                        item.subOrderNo = Convert.ToInt32(values[7].ToString().Trim());
                                    }
                                    //Regulation (EN)
                                    if (values[8] != DBNull.Value && !string.IsNullOrEmpty(values[8].ToString().Trim()))
                                    {
                                        item.regEn = values[8].ToString().Trim();
                                    }
                                    //Regulation (FR)
                                    if (values[9] != DBNull.Value && !string.IsNullOrEmpty(values[9].ToString().Trim()))
                                    {
                                        item.regFr = values[9].ToString().Trim();
                                    }
                                    //Observation(EN)
                                    if (values[10] != DBNull.Value && !string.IsNullOrEmpty(values[10].ToString().Trim()))
                                    {
                                        item.obsDescEn = values[10].ToString().Trim();
                                    }
                                    //Observation(FR)
                                    if (values[11] != DBNull.Value && !string.IsNullOrEmpty(values[11].ToString().Trim()))
                                    {
                                        item.obsDescFr = values[11].ToString().Trim();
                                    }

                                    items.Add(item);                                    
                                }
                            }
                        }
                    }

                    if (items != null && items.Count > 0)
                    {
                        items = (from p in items
                                 orderby (p.referenceNumber)
                                 select p).ToList();
                        items.OrderBy(x => x.insStartDate).ThenBy(x => x.insType);

                        // 1. Grouping by licenseNumber, insStartDate, site, and rating
                        var itemsGroup = items.GroupBy(p => new { p.referenceNumber, p.insStartDate, p.insType },
                                  (key, group) => new
                                  {
                                      referenceNumber = key.referenceNumber,
                                      insStartDate = key.insStartDate,
                                      insType = key.insType,
                                      count = group.Count(),
                                      iiDList = group.ToList()
                                  });

                        var newItems = new List<InitialReportItem>();

                        //3. Get existing inspection Numbers from IID table.
                        //var oldItems = gcpDB.GetIIDs();

                        //4. filter inspection table for only rating is I
                        var inspectionList = ctoDB.GetAllInspections();
                        var inspectionIIDList = inspectionList.Where(w => w.rating.ToLower().Equals("i")).ToList();
                        var filteredIIDItems = new List<InitialReportItem>();
                        foreach (var v in itemsGroup)
                        {
                            var filteredInspection = (from c in inspectionList
                                                      where c.referenceNumber.Equals(v.referenceNumber)
                                                            && c.inspectionStartDate == v.insStartDate                                                           
                                                            && c.rating.ToLower().Equals("i")
                                                      select c).FirstOrDefault();

                            if (filteredInspection != null && filteredInspection.insepctionNumber  > 0)
                            {
                                v.iiDList.ForEach(x =>
                                {
                                    x.insepctionNumber = filteredInspection.insepctionNumber;
                                });
                                filteredIIDItems.AddRange(v.iiDList);
                            }
                        }

                        if (filteredIIDItems != null && filteredIIDItems.Count > 0)
                        {
                            var insNumbers = filteredIIDItems.Select(x => x.insepctionNumber).Distinct().ToList();

                            var insertedRowCount = ctoDB.InitialDeficienciesCreate(filteredIIDItems);
                            if (insertedRowCount == 0)
                            {
                                outputTxt.Text = "There is no inserted in Initial inspection deficiencies. Please check your data.";
                            }
                            else
                            {
                                var updatedReportCard = ctoDB.InspectionReportCardUpdate(insNumbers);
                                outputTxt.Text += string.Format("<br/> {0} inserted in Initial inspection deficiencies <br/> {1} updated in inspection reportCard of CTO inspection", insertedRowCount, updatedReportCard);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputTxt.Text = ex.Message;
                }
            }
            else
            {
                outputTxt.Text = "Please select a file to upload.";
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var adminPass = ConfigurationManager.AppSettings["adminPassword"].ToString();
            if (String.Equals(args.Value, adminPass, StringComparison.Ordinal) == true)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void btnCreateExcel_Click(object sender, EventArgs e)
        {
            var ctoDB = new DBConnection("en");
            var inspectionList = ctoDB.GetAllInspectionsforExcel();
            if (inspectionList != null && inspectionList.Rows.Count > 0)
            {
                var filename = string.Format("cto_report_{0}{1}{2}",
                   DateTime.Now.Year.ToString(),
                   DateTime.Now.Month.ToString().PadLeft(2, '0'),
                   DateTime.Now.Day.ToString().PadLeft(2, '0'));

                //Clears all content output from the buffer stream.  
                Response.ClearContent();
                //Adds HTTP header to the output stream  
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", filename));
                // Gets or sets the HTTP MIME type of the output stream  
                Response.ContentType = "application/vnd.ms-excel";
                string space = string.Empty;

                foreach (DataColumn dcolumn in inspectionList.Columns)
                {
                    Response.Write(space + dcolumn.ColumnName);
                    space = "\t";
                }
                Response.Write("\n");
                int countcolumn;
                foreach (DataRow dr in inspectionList.Rows)
                {
                    space = string.Empty;
                    for (countcolumn = 0; countcolumn < inspectionList.Columns.Count; countcolumn++)
                    {
                        if (countcolumn > 2)
                        {
                            Response.Write(space + Convert.ToDateTime(dr[countcolumn]).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            Response.Write(space + dr[countcolumn].ToString());
                        }
                        space = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
        }
    }
}
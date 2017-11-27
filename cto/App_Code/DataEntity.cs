using System;
using System.Collections.Generic;



namespace cto
{

    public class SearchItem
    {
        public string establishmentName { get; set; }
        public string referenceNumber { get; set; }
        public string province { get; set; }
        public string rating { get; set; }
        public int? currentlyRegistered { get; set; }
        public string registrationNumber { get; set; }
        public string activity { get; set; }
        public DateTime? inspectionStartDateFrom { get; set; }
        public DateTime? inspectionStartDateTo { get; set; }
        public string category { get; set; }
    }

    public class InspectionItem
    {
        public string lang { get; set; }
        public int insepctionNumber { get; set; }
        public string referenceNumber { get; set; }
        public string establishmentName { get; set; }
        public string registrationNumber { get; set; }
        public bool? currentlyRegistered { get; set; }
        public DateTime? inspectionStartDate { get; set; }
        public DateTime? inspectionEndDate { get; set; }
        public string rating { get; set; }
        public string ratingDesc { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string activityEn { get; set; }
        public string activityFr { get; set; }
        public List<string> activityList { get; set; }
        public string inspectionType { get; set; }
        public string inspectionTypeFr { get; set; }
        public bool reportCard { get; set; }
        public int totalInspecitions { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public List<InspectionSubItem> inspectionList { get; set; }
        public bool? currentlyRegsitered { get; internal set; }
    }

    public class InspectionSubItem
    {
        public int insepctionNumber { get; set; }
        public DateTime? inspectionStartDate { get; set; }
        public string inspectionType { get; set; }
        public string rating { get; set; }
        public string ratingDesc { get; set; }
        public bool reportCard { get; set; }
    }

    public class InitialReportItem
    {
        public int insepctionNumber { get; set; }
        public string referenceNumber { get; set; }
        public string establishmentName { get; set; }
        public DateTime insStartDate { get; set; }
        public DateTime? insEndDate { get; set; }
        public string insType { get; set; }
        public bool reportCard { get; set; }
        public int ircInspectionNumber { get; set; }        
        //public string licenseNumber { get; set; }
        //public string site { get; set; }
        public int orderNo { get; set; }
        public int subOrderNo { get; set; }
        public string regEn { get; set; }
        public string regFr { get; set; }
        public string obsDescEn { get; set; }
        public string obsDescFr { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public List<SummaryItem> summaryItemList { get; set; }
    }

    public class FullReportItem
    {
        public int insepctionNumber { get; set; }
        public string referenceNumber { get; set; }
        public string establishmentName { get; set; }
        public DateTime insStartDate { get; set; }
        public string insType { get; set; }
        public string rating { get; set; }
        public bool reportCard { get; set; }
        public int iidInspectionNumber { get; set; }
        public List<SummaryItem> summaryItemList { get; set; }
        public int orderNo { get; set; }
        public int subOrderNo { get; set; }
        public string reg { get; set; }
        public string regFr { get; set; }
        public string obsDesc { get; set; }
        public string obsDescFr { get; set; }
        public string insOutcome { get; set; }
        public string insOutcomeFr { get; set; }
        public string measureTaken { get; set; }
        public string measureTakenFr { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }

        public List<string> insOutcomeList { get; set; }
        public List<string> insOutcomeListFr { get; set; }
        public List<string> measureTakenList { get; set; }
        public List<string> measureTakenListFr { get; set; }
        public List<string> summaryEn { get; set; }
        public List<string> summaryFr { get; set; }
    }

    public class SummaryItem
    {
        public string no { get; set; }
        public string regulation { get; set; }
        public List<string> summaryList { get; set; }
    }

}
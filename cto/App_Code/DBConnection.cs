using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace cto
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class DBConnection
    {

        private string _lang;
        public string Lang
        {
            get { return this._lang; }
            set { this._lang = value; }
        }

        public DBConnection(string lang)
        {
            this._lang = lang;
        }

        private string ctoDBConnection
        {
            get { return ConfigurationManager.ConnectionStrings["CTO"].ToString(); }
        }

        public DataTable GetAllInspections(SearchItem item)
        {
            var dt = new DataTable();
            var whereClause = string.Empty;
            var commandText = string.Empty;

            if (!string.IsNullOrEmpty(item.establishmentName))
            {
                whereClause += " and estName LIKE '%" + item.establishmentName + "%'";
            }

            if (!string.IsNullOrEmpty(item.referenceNumber))
            {
                whereClause += " and refNumber='"+ item.referenceNumber + "'";
            }


            if (!string.IsNullOrEmpty(item.province))
            {
                whereClause += " and province =@province";
            }

            if (!string.IsNullOrEmpty(item.activity))
            {
                whereClause += " and activityEn LIKE '%" + item.activity + "%'";
            }

            if (!string.IsNullOrEmpty(item.rating))
            {
                whereClause += " and rating =@rating";
            }
            if (item.currentlyRegistered != null)
            {
                whereClause += " and curRegistered = @curRegistered";
            }
            if (!string.IsNullOrEmpty(item.registrationNumber))
            {
                whereClause += " and regNumber=@regNumber";
            }

            if (item.inspectionStartDateFrom != null)
            {
                whereClause += " and (insStartDate BETWEEN @insStartDateFrom AND @insStartDateTo)";
            }


            if (string.IsNullOrEmpty(whereClause))
            {
                commandText = "SELECT [insNumber],[refNumber],[estName],[province],[insStartDate],[rating],[curRegistered],[reportCard]"
                               + " FROM [dbo].[Inspections] ORDER BY insStartDate  DESC";
            }
            else
            {
                whereClause = whereClause.Remove(0, 4);
                commandText = "SELECT [insNumber],[refNumber],[estName],[province],[insStartDate],[rating],[curRegistered],[reportCard]"
                               + " FROM [dbo].[Inspections]"
                               + " WHERE" + whereClause
                               + " ORDER BY insStartDate  DESC;";
            }

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                if (!string.IsNullOrEmpty(item.referenceNumber))
                {
                    cmd.Parameters.AddWithValue("@refNumber", item.referenceNumber);
                }

                if (!string.IsNullOrEmpty(item.establishmentName))
                {
                    cmd.Parameters.AddWithValue("@estName", item.establishmentName);
                }
                if (!string.IsNullOrEmpty(item.province))
                {
                    cmd.Parameters.AddWithValue("@province", item.province);
                }
                if (!string.IsNullOrEmpty(item.rating))
                {
                    cmd.Parameters.AddWithValue("@rating", item.rating);
                }
                if (item.currentlyRegistered != null)
                {
                    cmd.Parameters.AddWithValue("@curRegistered", item.currentlyRegistered);
                }
                if (!string.IsNullOrEmpty(item.registrationNumber))
                {
                    cmd.Parameters.AddWithValue("@regNumber", item.registrationNumber);
                }
                if (item.inspectionStartDateFrom != null)
                {
                    cmd.Parameters.AddWithValue("@insStartDateFrom", item.inspectionStartDateFrom);

                    if (item.inspectionStartDateTo == null || item.inspectionStartDateTo == DateTime.MinValue || item.inspectionStartDateTo == DateTime.MaxValue)
                    {
                        cmd.Parameters.AddWithValue("@insStartDateTo", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@insStartDateTo", item.inspectionStartDateTo);
                    }
                }


                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllInspections()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }

        public List<InspectionItem> GetAllInspections()
        {
            var items = new List<InspectionItem>();
            string commandText = "SELECT * FROM [dbo].[Inspections]";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item  = new InspectionItem();
                                item.insepctionNumber = dr["insNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dr["insNumber"].ToString().Trim());
                                item.referenceNumber = dr["refNumber"] == DBNull.Value ? string.Empty : dr["refNumber"].ToString().Trim();
                                item.establishmentName = dr["estName"] == DBNull.Value ? string.Empty : dr["estName"].ToString().Trim();                           
                                item.registrationNumber = dr["regNumber"] == DBNull.Value ? string.Empty : dr["regNumber"].ToString().Trim();                              
                                item.street = dr["street"] == DBNull.Value ? string.Empty : dr["street"].ToString().Trim();
                                item.city  = dr["city"] == DBNull.Value ? string.Empty : dr["city"].ToString().Trim();
                                item.province = dr["province"] == DBNull.Value ? string.Empty : dr["province"].ToString().Trim();
                                item.country = dr["country"] == DBNull.Value ? string.Empty : dr["country"].ToString().Trim(); 
                                item.postalCode = dr["postalCode"] == DBNull.Value ? string.Empty : dr["postalCode"].ToString().Trim(); 
                                item.currentlyRegsitered = dr["curRegistered"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dr["curRegistered"]);
                                item.activityEn = dr["activityEn"] == DBNull.Value ? string.Empty: dr["activityEn"].ToString().Trim(); 
                                item.activityFr = dr["activityFr"] == DBNull.Value ? string.Empty : dr["activityFr"].ToString().Trim(); 
                                item.inspectionType = dr["insTypeEn"] == DBNull.Value ? string.Empty : dr["insTypeEn"].ToString().Trim(); ;
                                item.inspectionTypeFr = dr["insTypeFr"] == DBNull.Value ? string.Empty : dr["insTypeFr"].ToString().Trim(); ;
                                item.inspectionStartDate = dr["insStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["insStartDate"]);
                                item.inspectionEndDate = dr["insEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["insEndDate"]);
                                item.rating = dr["rating"] == DBNull.Value ? string.Empty : dr["rating"].ToString().Trim(); ;
                                item.reportCard = dr["reportCard"] == DBNull.Value ? false : Convert.ToBoolean(dr["reportCard"]);
                                item.createdDate = dr["createdDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["createdDate"]);
                                item.updatedDate = dr["updatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["updatedDate"]);

                                if (item.insepctionNumber > 0)
                                { items.Add(item); }                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllInspections()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<int> GetIRCs()
        {
            var insNumbers = new List<int>();
            string commandText = "SELECT  distinct(insNumber)  FROM [dbo].[ReportCardSummary]";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var insNumber= dr["insNumber"] == DBNull.Value ? 0 : (int)dr["insNumber"];
                                insNumbers.Add(insNumber);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIRCs()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return insNumbers;
        }

        public List<int> GetIIDs()
        {
            var insNumbers = new List<int>();
            string commandText = "SELECT  distinct(insNumber)  FROM [dbo].[InitialDeficiencies]";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var insNumber = dr["insNumber"] == DBNull.Value ? 0 : (int)dr["insNumber"];
                                insNumbers.Add(insNumber);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIIDs()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return insNumbers;
        }
        public List<InitialReportItem> GetAllInitialInspections()
        {
            var items = new List<InitialReportItem>();
            string commandText = "SELECT  *  FROM [dbo].[InitialDeficiencies]";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new InitialReportItem();
                                item.insepctionNumber = dr["insNumber"] == DBNull.Value ? 0 : (int)dr["insNumber"];
                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllInitialInspections()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<FullReportItem> GetAllFullReportInspections()
        {
            var items = new List<FullReportItem>();
            string commandText = "SELECT  *  FROM [dbo].[ReportCardSummary]";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new FullReportItem();
                                item.insepctionNumber = dr["insNumber"] == DBNull.Value ? 0 : (int)dr["insNumber"];
                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllFullReportInspections()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public DataTable GetInspectionByInspectionNumber(int inspectionNumber)
        {
            var dt = new DataTable();
            var whereClause = string.Empty;
            var commandText = string.Empty;
            if( Lang.Equals("en"))
            {
                commandText = "SELECT [insNumber],[refNumber],[estName],[regNumber],[street],[city],[province],[country], [postalCode],"
                          + " [curRegistered],[activityEn] as activity,[insTypeEn] as insType,[insStartDate],[insEndDate],[rating], [reportCard]";
            }
            else
            {
                commandText = "SELECT [insNumber],[refNumber],[estName],[regNumber],[street],[city],[province],[country],[postalCode],"
                          + " [curRegistered],[activityFr] as activity,[insTypeFr] as insType,[insStartDate],[insEndDate],[rating], [reportCard]";
            }
            commandText += " FROM [dbo].[Inspections]"
                          + " WHERE insNumber = @insNumber"
                          + " ORDER BY insStartDate  DESC;";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@insNumber", inspectionNumber);

                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetInspectionByInspectionNumber() : {0}", inspectionNumber);
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }

        public bool FindInspectionByInspectionNumber(int inspectionNumber)
        {
            var existInspection = true;
            var whereClause = string.Empty;
            var commandText = "SELECT  *  FROM [dbo].[Inspections] WHERE insNumber = @insNumber;";


            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@insNumber", inspectionNumber);

                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            existInspection = dr[0] == DBNull.Value ? false : true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetInspectionByInspectionNumber() : {0}", inspectionNumber);
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return existInspection;
        }

        public DataTable GetInspectionByRefNumber(int insNumber, string refNumber, string regNumber)
        {
            var dt = new DataTable();
            var commandText = string.Empty;
            if (Lang.Equals("en"))
            {
                commandText = "SELECT [insNumber],[refNumber],[estName],[regNumber],[street],[city],[province],[country],[postalCode],"
                                 + "[curRegistered],[activityEn] as activity,[insTypeEn] as insType,[insStartDate],[rating], [reportCard]";
            }
            else
            {
                commandText = "SELECT [insNumber],[refNumber],[estName],[regNumber],[street],[city],[province],[country],[postalCode],"
                               + "[curRegistered],[activityFr] as activity,[insTypeFr] as insType,[insStartDate],[rating], [reportCard]";
            }
            commandText +=  " FROM [dbo].[Inspections]"
                        + " WHERE refNumber = @refNumber and regNumber = @regNumber and  insNumber <> @insNumber"
                        + " ORDER BY insStartDate DESC;";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@refNumber", refNumber);
                cmd.Parameters.AddWithValue("@regNumber", regNumber);
                cmd.Parameters.AddWithValue("@insNumber", insNumber);

                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetInspectionByRefNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }

 
        public int InspectionsCreate(List<InspectionItem> items)
        {
            var errorMessages = string.Empty;
            var rowAffectedCount = 0;
            string commandText = "INSERT INTO[dbo].[Inspections]([insNumber],[refNumber],[estName],[regNumber],[street],[city],[province],[country],[postalCode],"
                            + "[curRegistered],[activityEn],[activityFr],[insTypeEn],[insTypeFr],[insStartDate],[insEndDate],[rating],[reportCard], [createdDate], [updatedDate]) "
                            + " VALUES(@insNumber, @refNumber, @estName, @regNumber, @street, @city, @province, @country, @postalCode,"
                            + " @curRegistered, @activityEn, @activityFr, @insTypeEn, @insTypeFr, @insStartDate, @insEndDate, @rating, @reportCard, @createdDate, @updatedDate);";

            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(commandText, con))
                        {
                            foreach (InspectionItem item in items)
                            {
                                try
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@insNumber", item.insepctionNumber); //0
                                    cmd.Parameters.AddWithValue("@refNumber", item.referenceNumber);//1
                                    cmd.Parameters.AddWithValue("@estName", item.establishmentName);//2
                     
                                    //LICENSE NUMBER
                                    if (string.IsNullOrWhiteSpace(item.registrationNumber))
                                    {
                                        cmd.Parameters.AddWithValue("@regNumber", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@regNumber", item.registrationNumber);
                                    }
                                    
                                    //STREET
                                    if (string.IsNullOrWhiteSpace(item.street))
                                    {
                                        cmd.Parameters.AddWithValue("@street", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@street", item.street);
                                    }
                                    //CITY
                                    if (string.IsNullOrWhiteSpace(item.city))
                                    {
                                        cmd.Parameters.AddWithValue("@city", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@city", item.city);
                                    }
                                    //.province
                                    if (string.IsNullOrWhiteSpace(item.province))
                                    {
                                        cmd.Parameters.AddWithValue("@province", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@province", item.province);
                                    }
                                  
                                    //country
                                    if (string.IsNullOrWhiteSpace(item.country))
                                    {
                                        cmd.Parameters.AddWithValue("@country", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@country", item.country);
                                    }

                                    //Postal Code
                                    if (string.IsNullOrWhiteSpace(item.postalCode))
                                    {
                                        cmd.Parameters.AddWithValue("@postalCode", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@postalCode", item.postalCode);
                                    }
                                    //CURRENTLY_LICENCED
                                    if (item.currentlyRegistered != null)
                                    {
                                        cmd.Parameters.AddWithValue("@curRegistered", item.currentlyRegistered);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@curRegistered", DBNull.Value);
                                    }
                                    //ACTIVITY_EN
                                    if (string.IsNullOrWhiteSpace(item.activityEn))
                                    {
                                        cmd.Parameters.AddWithValue("@activityEn", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@activityEn", item.activityEn);
                                    }
                                    //ACTIVITY_Fr
                                    if (string.IsNullOrWhiteSpace(item.activityFr))
                                    {
                                        cmd.Parameters.AddWithValue("@activityFr", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@activityFr", item.activityFr);
                                    }
                                    // INSP_TYPE_FR
                                    if (string.IsNullOrWhiteSpace(item.inspectionTypeFr))
                                    {
                                        cmd.Parameters.AddWithValue("@insTypeFr", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insTypeFr", item.inspectionTypeFr);
                                    }
                                    //// INSP_TYPE_EN
                                    if (string.IsNullOrWhiteSpace(item.inspectionType))
                                    {
                                        cmd.Parameters.AddWithValue("@insTypeEn", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insTypeEn", item.inspectionType);
                                    }
                                    //INSP_START_DATE
                                    if (item.inspectionStartDate != null)
                                    {
                                        cmd.Parameters.AddWithValue("@insStartDate", item.inspectionStartDate);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insStartDate", DBNull.Value);
                                    }
                                    //INSP_END_DATE
                                    if (item.inspectionEndDate != null)
                                    {
                                        cmd.Parameters.AddWithValue("@insEndDate", item.inspectionEndDate);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insEndDate", DBNull.Value);
                                    }
                                    //25.RATING
                                    if (string.IsNullOrWhiteSpace(item.rating))
                                    {
                                        cmd.Parameters.AddWithValue("@rating", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@rating", item.rating);
                                    }
                                    
                                    cmd.Parameters.AddWithValue("@reportCard", item.reportCard);
                                    cmd.Parameters.AddWithValue("@createdDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                    cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));

                                    cmd.Transaction = sqlTrans;
                                    rowAffectedCount += cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    errorMessages = string.Format("DbConnection.cs - InspectionsCreate() - Inspection Number:{0}  : Ref Number:{1}, ", item.insepctionNumber, item.referenceNumber);
                                    ExceptionHelper.LogException(ex, errorMessages);
                                    rowAffectedCount = 0;
                                    sqlTrans.Rollback();
                                    break;
                                }
                            }  
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = string.Format("DbConnection.cs - InspectionsCreate()");
                ExceptionHelper.LogException(ex, errorMessages);
            }
            return rowAffectedCount;
        }

        public KeyValuePair<int, string> GetInsRefNumberBylicNumber(string licNumber, string site, string rating, DateTime insStartDate)
        {
            var returnPair = new KeyValuePair<int, string>();
            var dt = new DataTable();
            var whereClause = string.Empty;
            var commandText = string.Empty;

            if (!string.IsNullOrEmpty(licNumber))
            {
                whereClause += " and licNumber = @licNumber";
            }
            if (insStartDate != null)
            {
                whereClause += " and insStartDate = @insStartDate";
            }
            if (!string.IsNullOrEmpty(site))
            {
                whereClause += " and site = @site";
            }
            if (!string.IsNullOrEmpty(rating))
            {
                if (rating.ToLower().Equals("c"))
                {
                    whereClause += " and (rating  = 'c'  or rating = 'ct') ";
                }
                else
                {
                    whereClause += " and rating  = @rating";
                }
            }

            whereClause = whereClause.Remove(0, 4);
            commandText = "SELECT [insNumber], [refNumber] FROM [dbo].[Inspections] "
                        + " WHERE" + whereClause
                        + " ORDER BY insStartDate  DESC;";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@licNumber", licNumber);
                cmd.Parameters.AddWithValue("@site", site);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@insStartDate", insStartDate);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                             returnPair = new KeyValuePair<int, string>(dr.GetInt32(0), dr.GetString(1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetInspectionNumberBylicNumber()_ License Number: {0}", licNumber);
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return returnPair;
        }

        public int ReportSummaryCreate(List<FullReportItem> items)
        {
            var errorMessages = string.Empty;
            var rowAffectedCount = 0;
            string commandText = "INSERT INTO [dbo].[ReportCardSummary]([insNumber],[refNumber],[orderNo],[subOrderNo],[regEn],[regFr],[summaryEn],[summaryFr],"
                               + "[insOutcomeEn],[insOutcomeFr],[measureTakenEn],[measureTakenFr], [createdDate], [updatedDate])"
                               + " VALUES(@insNumber,@refNumber, @orderNo, @subOrderNo, @regEn, @regFr, @summaryEn, @summaryFr, @insOutcomeEn, @insOutcomeFr, @measureTakenEn, @measureTakenFr, @createdDate, @updatedDate);";

            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                foreach (FullReportItem item in items)
                                {
                                    cmd.Parameters.Clear();
                                    if (item.insepctionNumber >  0 )
                                    {
                                        cmd.Parameters.AddWithValue("@insNumber", item.insepctionNumber);
                                    }

                                    if (!string.IsNullOrWhiteSpace(item.referenceNumber))
                                    {
                                        cmd.Parameters.AddWithValue("@refNumber", item.referenceNumber);
                                    }
                                    if (item.orderNo > 0)
                                    {
                                        cmd.Parameters.AddWithValue("@orderNo", item.orderNo);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@orderNo", 0);
                                    }
                                    if (item.subOrderNo > 0)
                                    {
                                        cmd.Parameters.AddWithValue("@subOrderNo", item.subOrderNo);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@subOrderNo", 0);
                                    }

                                    if (string.IsNullOrWhiteSpace(item.reg))
                                    {
                                        cmd.Parameters.AddWithValue("@regEn", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@regEn", item.reg);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.regFr))
                                    {
                                        cmd.Parameters.AddWithValue("@regFr", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@regFr", item.regFr);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.obsDesc))
                                    {
                                        cmd.Parameters.AddWithValue("@summaryEn", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@summaryEn", item.obsDesc);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.obsDescFr))
                                    {
                                        cmd.Parameters.AddWithValue("@summaryFr", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@summaryFr", item.obsDescFr);
                                    }

                                    if (string.IsNullOrWhiteSpace(item.insOutcome))
                                    {
                                        cmd.Parameters.AddWithValue("@insOutcomeEn", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insOutcomeEn", item.insOutcome);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.insOutcomeFr))
                                    {
                                        cmd.Parameters.AddWithValue("@insOutcomeFr", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@insOutcomeFr", item.insOutcomeFr);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.measureTaken))
                                    {
                                        cmd.Parameters.AddWithValue("@measureTakenEn", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@measureTakenEn", item.measureTaken);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.measureTakenFr))
                                    {
                                        cmd.Parameters.AddWithValue("@measureTakenFr", DBNull.Value);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@measureTakenFr", item.measureTakenFr);
                                    }
                                    cmd.Parameters.AddWithValue("@createdDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                    cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                    cmd.Transaction = sqlTrans;
                                    rowAffectedCount += cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = string.Format("DbConnection.cs - ReportSummaryCreate()");
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowAffectedCount = 0;
                            sqlTrans.Rollback();
                        }
                        finally
                        {
                            sqlTrans.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - ReportSummaryCreate()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            return rowAffectedCount;
        }

        //UpdateReportCard
        public int InspectionReportCardUpdate(List<int> insNumberList)
        {
            var errorMessages = string.Empty;
            var rowsAffected = 0;
            string commandText = "UPDATE  [dbo].[Inspections]"
                + " SET [reportCard] = 1, [updatedDate] = @updatedDate"
                + " WHERE insNumber =@insNumber";

            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                foreach (int insNumber in insNumberList)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@insNumber", insNumber);
                                    cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString())); 

                                    cmd.Transaction = sqlTrans;
                                    rowsAffected += cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - InspectionReportCardUpdate()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowsAffected = 0;
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - InspectionReportCardUpdate()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return rowsAffected;
        }
        public DataTable GetFullReportByInspectionNumber(int insNumber)
        {
            var dt = new DataTable();
            string commandText = "select a.insNumber , a.estName, a.refNumber, a.insStartDate, a.rating, a.reportCard, b.orderNo, b.subOrderNo, ";
            if (Lang == "en")
            {
                commandText += "a.insTypeEn as insType, b.regEn as reg, b.summaryEn as observation, b.insOutcomeEn as outcome, measureTakenEn as measure ";
            }
            else
            {
                commandText += "a.insTypeFr as insType, b.regFr as reg, b.summaryFr as observation, b.insOutcomeFr as outcome, measureTakenFr as measure ";
            }


            commandText += " from dbo.Inspections as a, ReportCardSummary as b "
                         + " where  b.insNumber = @insNumber and a.insNumber = b.insNumber"
                         + " order by b.orderNo, b.subOrderNo;";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@insNumber", insNumber);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorHandler = string.Format("DbConnection.cs - GetInspectionByInspectionNumber() - insNumber : {0}", insNumber);
                    ExceptionHelper.LogException(ex, errorHandler);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }


        public DataTable GetIIDReportByInspectionNumber(int insNumber)
        {
            var dt = new DataTable();
            string commandText = "select a.insNumber, a.estName, a.refNumber, a.insStartDate, a.insEndDate,  a.rating, a.reportCard,";
            if (Lang == "en")
            {
                commandText += "a.insTypeEn as insType, b.orderNo, b.subOrderNo, b.regEn as reg, b.summaryEn as observation";
            }
            else
            {
                commandText += "a.insTypeFr as insType, b.orderNo, b.subOrderNo, b.regFr as reg, b.summaryFr as observation";
            }


            commandText += " from dbo.Inspections as a, InitialDeficiencies as b "
                         + " where  b.insNumber = @insNumber and a.insNumber = b.insNumber"
                         + " order by b.orderNo, b.subOrderNo;";

            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                cmd.Parameters.AddWithValue("@insNumber", insNumber);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorHandler = string.Format("DbConnection.cs - GetIIDReportByInspectionNumber() - insNumber : {0}", insNumber);
                    ExceptionHelper.LogException(ex, errorHandler);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }

          public int InitialDeficienciesCreate(List<InitialReportItem> items)
        {
            var errorMessages = string.Empty;
            var rowAffectedCount = 0;
            string commandText = "INSERT INTO [dbo].[InitialDeficiencies]([insNumber],[refNumber],[orderNo],[subOrderNo],[regEn],[regFr],[summaryEn],[summaryFr], [createdDate], [updatedDate] )"
                               + " VALUES(@insNumber, @refNumber, @orderNo, @subOrderNo, @regEn, @regFr, @summaryEn, @summaryFr, @createdDate, @updatedDate);";
            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                foreach (InitialReportItem item in items)
                                {

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@insNumber", item.insepctionNumber);
                                    cmd.Parameters.AddWithValue("@refNumber", item.referenceNumber);
                                    if (item.orderNo > 0)
                                    {
                                        cmd.Parameters.AddWithValue("@orderNo", item.orderNo);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@orderNo", 0);
                                    }
                                    if (item.subOrderNo > 0)
                                    {
                                        cmd.Parameters.AddWithValue("@subOrderNo", item.subOrderNo);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@subOrderNo", 0);
                                    }

                                    if (string.IsNullOrWhiteSpace(item.regEn))
                                    {
                                        cmd.Parameters.AddWithValue("@regEn", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@regEn", item.regEn);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.regFr))
                                    {
                                        cmd.Parameters.AddWithValue("@regFr", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@regFr", item.regFr);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.obsDescEn))
                                    {
                                        cmd.Parameters.AddWithValue("@summaryEn", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@summaryEn", item.obsDescEn);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.obsDescFr))
                                    {
                                        cmd.Parameters.AddWithValue("@summaryFr", string.Empty);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@summaryFr", item.obsDescFr);
                                    }
                                    cmd.Parameters.AddWithValue("@createdDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                    cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                    cmd.Transaction = sqlTrans;
                                    rowAffectedCount += cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - InitialDeficienciesCreate()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                 }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - InitialDeficienciesCreate()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            return rowAffectedCount;
        }

        //public int InspectionEndDateUpdate(List<InitialReportItem> insNumberList)
        //{
        //    var errorMessages = string.Empty;
        //    var rowsAffected = 0;
        //    string commandText = "UPDATE  [dbo].[Inspections]"
        //        + " SET [insEndDate] = @insEndDate, [updatedDate] = @updatedDate, [reportCard] =1"
        //        + " WHERE insNumber =@insNumber";

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(ctoDBConnection))
        //        {
        //            con.Open();
        //            using (SqlTransaction sqlTrans = con.BeginTransaction())
        //            {
        //                try
        //                {
        //                    using (SqlCommand cmd = new SqlCommand(commandText, con))
        //                    {
        //                        foreach (InitialReportItem list in insNumberList)
        //                        {
        //                            cmd.Parameters.Clear();
        //                            cmd.Parameters.AddWithValue("@insNumber", list.insepctionNumber);
        //                            cmd.Parameters.AddWithValue("@insEndDate", list.insEndDate);
        //                            cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
        //                            cmd.Transaction = sqlTrans;
        //                            rowsAffected += cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                }
        //                catch (SqlException ex)
        //                {
        //                    errorMessages = "DbConnection.cs - InspectionEndDateUpdate()";
        //                    ExceptionHelper.LogException(ex, errorMessages);
        //                    rowsAffected = 0;
        //                    sqlTrans.Rollback();
        //                }
        //                sqlTrans.Commit();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessages = "DbConnection.cs - InspectionEndDateUpdate()";
        //        ExceptionHelper.LogException(ex, errorMessages);
        //    }
        //    finally
        //    {

        //    }
        //    return rowsAffected;
        //}


        public int InspectionsDelete()
        {
            var errorMessages = string.Empty;
            var rowsAffected = 0;
            string commandText = "DELETE FROM [dbo].[Inspections] WHERE drupalID > 0 ";
            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                cmd.Transaction = sqlTrans;
                                rowsAffected += cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - InspectionsDelete()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowsAffected = 0;
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - InspectionDelete()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return rowsAffected;
        }
        
        public int ReportCardSummaryDelete()
        {
            var errorMessages = string.Empty;
            var rowsAffected = 0;

            string commandText = "DELETE FROM [dbo].[ReportCardSummary] WHERE drupalID > 0  ;";
            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                cmd.Transaction = sqlTrans;
                                rowsAffected += cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - ReportCardSummaryDelete()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowsAffected = 0;
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - ReportCardSummaryDelete()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return rowsAffected;
        }

        public int InitialDeficienciesDelete()
        {
            var errorMessages = string.Empty;
            var rowsAffected = 0;

            string commandText = "DELETE FROM [dbo].[InitialDeficiencies] WHERE drupalID > 0 ;";
            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                cmd.Transaction = sqlTrans;
                                rowsAffected += cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - InitialDeficienciesDelete()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowsAffected = 0;
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - InitialDeficienciesDelete()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return rowsAffected;
        }

        public DataTable GetAllInspectionsforExcel()
        {
            var dt = new DataTable();
            var commandText = "SELECT [refNumber],[estName],[rating],[insStartDate],[insEndDate],[createdDate],[updatedDate] FROM [dbo].[Inspections] WHERE createdDate >= '2016-03-29'";
            using (SqlConnection con = new SqlConnection(ctoDBConnection))
            {
                SqlCommand cmd = new SqlCommand(commandText, con);
                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorHandler = string.Format("DbConnection.cs - GetAllInspectionsforExcel()");
                    ExceptionHelper.LogException(ex, errorHandler);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dt;
        }

        public int InspectionsUpdate(int insNumber, string rating)
        {
            var errorMessages = string.Empty;
            var rowsAffected = 0;
            string commandText = "UPDATE [dbo].[Inspections] SET rating = @rating, updatedDate=@updatedDate WHERE insNumber = @insNumber";
            try
            {
                using (SqlConnection con = new SqlConnection(ctoDBConnection))
                {
                    con.Open();
                    using (SqlTransaction sqlTrans = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(commandText, con))
                            {
                                cmd.Parameters.AddWithValue("@rating", rating);
                                cmd.Parameters.AddWithValue("@insNumber", insNumber);
                                cmd.Parameters.AddWithValue("@updatedDate", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                                cmd.Transaction = sqlTrans;
                                rowsAffected += cmd.ExecuteNonQuery();
                            }
                        }
                        catch (SqlException ex)
                        {
                            errorMessages = "DbConnection.cs - InspectionsUpdate()";
                            ExceptionHelper.LogException(ex, errorMessages);
                            rowsAffected = 0;
                            sqlTrans.Rollback();
                        }
                        sqlTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessages = "DbConnection.cs - InspectionsUpdate()";
                ExceptionHelper.LogException(ex, errorMessages);
            }
            return rowsAffected;
        }
    }
   
}

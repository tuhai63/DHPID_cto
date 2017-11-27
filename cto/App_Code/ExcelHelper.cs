using Excel;
using System.IO;
using System.Data;

namespace cto
{
    /// <summary>
    /// Summary description for XmlHelper
    /// </summary>
    public static class ExcelHelper
    {
        public static DataSet ReadClinicalExcelData(string excelFilePath)
        {

            DataSet resultDS = new DataSet();
            IExcelDataReader excelReader = null;
            using (FileStream stream = new FileStream(excelFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var fileExtension = Path.GetExtension(excelFilePath);

                switch (fileExtension.ToLower().Trim())
                {
                    case ".xls":
                        // 1.Reading from a binary Excel file('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        break;
                    case ".xlsx":
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        break;
                    default:
                        excelReader = null;
                        break;
                }
                //3. DataSet - Create column names from first row
                if( excelReader != null)
                {
                    excelReader.IsFirstRowAsColumnNames = true;

                    //4. DataSet - The result of each spreadsheet will be created in the result.Tables
                    resultDS = excelReader.AsDataSet();

                    ////5. Data Reader methods
                    //while (excelReader.Read())
                    //{
                    //    excelReader.GetInt32(0);
                    //}

                    excelReader.Close();
                }                
                return resultDS;
            }
        }

    }
}
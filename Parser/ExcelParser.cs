using System.Data;
using System.IO;
using Excel;
using log4net;

namespace Parser
{
    internal class ExcelParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ExcelParser _instance;
        public static ExcelParser Instance => _instance ?? (_instance = new ExcelParser());
        private ExcelParser() { }

        private const string BinaryExtension = ".xls";
        private const string OpenXmlExtension = ".xlsx";

        public void ParseExcel(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = null;
            switch (fileInfo.Extension)
            {
                case BinaryExtension:
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
                case OpenXmlExtension:
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    break;
            }

            if (excelReader == null)
            {
                return;
            }

            var result = excelReader.AsDataSet();

            var totalRows = 0;
            foreach (DataTable table in result.Tables)
            {
                totalRows += table.Rows.Count;
            }
            Log.Debug(totalRows);

            var rowBeingProcessed = 0;
            foreach (DataTable table in result.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    rowBeingProcessed++;
                    UpdateStatus(fileInfo.Name, rowBeingProcessed, rowBeingProcessed *100 / totalRows );
                    Log.Debug(string.Join(",", row.ItemArray));
                }
            }

            excelReader.Close();
        }

        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler OnUpdateStatus;

        private void UpdateStatus(string fileBeingProcessed, int recordNumberBeingProcessed, int percentsComplete)
        {
            if (OnUpdateStatus == null) return;

            var args = new ProgressEventArgs(fileBeingProcessed, recordNumberBeingProcessed, percentsComplete);
            OnUpdateStatus(this, args);
        }
    }
}

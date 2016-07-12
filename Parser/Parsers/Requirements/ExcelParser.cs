using System.Data;
using System.IO;
using Excel;
using Storage.Requirements.Model.Entity;

namespace Parser.Parsers.Requirements
{
    internal class ExcelParser : BaseFileParser
    {
        public override string AcceptableFileMask => "*.xls?";

        private const string BinaryExtension = ".xls";
        private const string OpenXmlExtension = ".xlsx";

        public override void ParseFile(string filePath)
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
                    UpdateStatus(fileInfo.Name, rowBeingProcessed, rowBeingProcessed * 100 / totalRows);
                    Log.Debug(string.Join(",", row.ItemArray));
                    AddRequirementToStorage(new Requirement());
                }
            }

            excelReader.Close();
        }
    }
}

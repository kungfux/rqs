using System;
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

        private Tuple<string, string> GetFileInfo(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                return new Tuple<string, string>(fileInfo.Name, fileInfo.Extension);
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot get {filePath} file info", ex);
                return null;
            }
        }

        public override void ParseFile(string filePath)
        {
            var fileInfo = GetFileInfo(filePath);
            string fileName = fileInfo.Item1;
            string fileExtension = fileInfo.Item2;

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = null;

                    if (fileExtension == BinaryExtension)
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        Log.Info("File is going to be parsed using BinaryReader");
                    }
                    else if (fileExtension == OpenXmlExtension)
                    {
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        Log.Info("File is going to be parsed using OpenXmlReader");
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

                    if (IsDebugEnabled)
                    {
                        Log.Debug($"Total rows: {totalRows}");
                    }

                    var rowBeingProcessed = 0;
                    foreach (DataTable table in result.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if (IsDebugEnabled)
                            {
                                Log.Debug($"Row {rowBeingProcessed} values: {string.Join(", ", row.ItemArray)}");
                            }

                            rowBeingProcessed++;
                            UpdateStatus(fileName, rowBeingProcessed, rowBeingProcessed*100/totalRows);
                            AddRequirementToStorage(new Requirement());
                        }
                    }

                    excelReader.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to parse file {filePath}", ex);
            }
        }
    }
}

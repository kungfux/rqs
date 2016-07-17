using System;
using System.Collections.Generic;
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
            Log.Info($"Parsing file {filePath}");

            var fileInfo = GetFileInfo(filePath);
            var fileName = fileInfo.Item1;
            var fileExtension = fileInfo.Item2;

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    var excelReader = GetReaderByExtension(fileExtension, stream);

                    if (excelReader == null)
                    {
                        return;
                    }

                    var result = excelReader.AsDataSet();
                    var totalRows = GetTotalRowsCount(result);

                    var rowBeingProcessed = 0;
                    foreach (DataTable table in result.Tables)
                    {
                        // TODO: Use templates instead of auto-column detections
                        var cFRID = 0;
                        var cFRTMSTask = 1;
                        var cFRText = 3;
                        var cFRObject = -1;
                        var cCCP = -1;
                        var cCreated = -1;
                        var cModified = -1;
                        var cStatus = -1;

                        foreach (DataRow row in table.Rows)
                        {
                            if (IsDebugEnabled)
                            {
                                Log.Debug($"Row {rowBeingProcessed} values: {string.Join(", ", row.ItemArray)}");
                            }

                            if (rowBeingProcessed == 0)
                            {
                                cFRID = GetColumnByName(row, new List<string>() {"id", "fr id", "nfr id"}, cFRID);
                                cFRTMSTask = GetColumnByName(row, new List<string>() { "fr tms task", "nfr tms task" }, cFRTMSTask);
                                cFRText = GetColumnByName(row, new List<string>() { "functional requirements", "non-functional requirements" }, cFRText);
                                cFRObject = GetColumnByName(row, new List<string>() { "object number" });
                                cCCP = GetColumnByName(row, new List<string>() { "ccp", "ccp level" });
                                cCreated = GetColumnByName(row, new List<string>() { "fr date", "nfr date" });
                                cModified = GetColumnByName(row, new List<string>() { "last modified on" });
                                cStatus = GetColumnByName(row, new List<string>() { "fr status", "nfr status" });
                            }

                            var req = new Requirement();
                            try
                            {
                                // TODO: Get all Requirement fields
                                req.Number = cFRID >= 0 ? row.ItemArray[cFRID].ToString() : "";
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Cannot parse row #{rowBeingProcessed}", ex);
                            }
                            finally
                            {
                                if (req.Number != null)
                                {
                                    if (IsDebugEnabled)
                                    {
                                        Log.Debug($"Requirement parsed: Number:  {req.Number}");
                                    }

                                    AddRequirementToStorage(new Requirement());
                                }
                            }

                            rowBeingProcessed++;
                            UpdateStatus(fileName, rowBeingProcessed, rowBeingProcessed * 100 / totalRows);
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

        private IExcelDataReader GetReaderByExtension(string fileExtension, Stream stream)
        {
            if (fileExtension == BinaryExtension)
            {
                Log.Info("File is going to be parsed using BinaryReader");
                return ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (fileExtension == OpenXmlExtension)
            {
                Log.Info("File is going to be parsed using OpenXmlReader");
                return ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            return null;
        }

        private int GetTotalRowsCount(DataSet dataset)
        {
            var totalRows = 0;
            foreach (DataTable table in dataset.Tables)
            {
                totalRows += table.Rows.Count;
            }

            if (IsDebugEnabled)
            {
                Log.Debug($"Total rows: {totalRows}");
            }

            return totalRows;
        }

        private int GetColumnByName(DataRow row, ICollection<string> names, int defaultRowNumber = -1)
        {
            for (var a = 0; a < row.ItemArray.Length; a++)
            {
                foreach (var name in names)
                {
                    if (row.ItemArray[a].ToString().ToLower() == name.ToLower())
                        return a;
                }
            }
            return defaultRowNumber;
        }
    }
}

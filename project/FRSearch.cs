/*   
 *  RQS
 *  Requirement Searching Utility
 *  Copyright (C) Fuks Alexander 2013
 *  
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *  
 *  Fuks Alexander, hereby disclaims all copyright
 *  interest in the program "RQS"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 10 May 2013.
 */

using System.IO;
using System.Collections.Generic;
using ExcelLibrary.SpreadSheet;
using System.Windows.Forms;
using System;

namespace RQS
{
    internal class FRSearch
    {
        public int XLSFilesCount
        {
            get { return XLSFiles.Length; }
        }

        private string[] XLSFiles = new string[0];

        public FRSearch()
        {
            LoadXLSFilesList();
        }

        public enum SearchBy
        {
            FR_ID = 0,
            FR_TMS_Task = 1,
            FR_TEXT = 2
        }

        private void LoadXLSFilesList()
        {
            DirectoryInfo XLSLocation = new DirectoryInfo(ClientParams.Parameters.XLSLocation);
            FileInfo[] XLSFilesInfo = XLSLocation.GetFiles("*.xls");
            int XLSFilesCount = XLSFilesInfo.Length;
            XLSFiles = new string[XLSFilesCount];
            for (int a = 0; a < XLSFilesCount; a++)
            {
                XLSFiles[a] = XLSFilesInfo[a].Name;
            }
        }

        public List<FR> Search(SearchBy searchBy, string[] values, bool limitResults)
        {
            List<FR> Result = new List<FR>();

            Workbook book;
            Worksheet sheet;
            Row row;
            FR FR;

            // change to lower to get more results
            for (int a = 0; a < values.Length; a++)
            {
                values[a] = values[a].ToLower();
            }

            foreach (string XLSFile in XLSFiles)
            {
                try
                {
                    book = Workbook.Load(ClientParams.Parameters.XLSLocation + "\\" + XLSFile);
                }
                catch (IOException)
                {
                    MessageBox.Show(XLSFile + " is locked by another process and will be skipped from search results!",
                        "RQS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                sheet = book.Worksheets[0];

                // Determine columns
                int cFRID = -1;
                int cFRTMSTask = -1;
                int cFRText = -1;
                int cCCP = -1;
                int cCreated = -1;
                int cModified = -1;

                if (sheet.Cells.FirstRowIndex >= 0)
                {
                    row = sheet.Cells.GetRow(0);

                    for (int a = row.FirstColIndex; a <= row.LastColIndex; a++)
                    {
                        if (row.GetCell(a).IsEmpty)
                        {
                            continue;
                        }
                        switch (row.GetCell(a).Value.ToString().ToLower())
                        {
                            case "fr id":
                                cFRID = a;
                                break;
                            case "fr tms task":
                                cFRTMSTask = a;
                                break;
                            case "functional requirements":
                                cFRText = a;
                                break;
                            case "ccp":
                                cCCP = a;
                                break;
                            case "fr date":
                                cCreated = a;
                                break;
                            case "last modified on":
                                cModified = a;
                                break;
                        }
                    }
                }

                for (int a = sheet.Cells.FirstRowIndex + 1; a <= sheet.Cells.LastRowIndex; a++)
                {
                    row = sheet.Cells.GetRow(a);

                    // Qualification
                    switch (searchBy)
                    {
                        case SearchBy.FR_ID: // If search criteria NOT equals to cell value
                            if (row.GetCell(cFRID).IsEmpty ||
                                (values.Length == 1 && !row.GetCell(cFRID).Value.ToString().ToLower().Equals(values[0])) ||
                                (values.Length > 1 && !MultiSearchORlogic(row.GetCell(cFRID).Value.ToString().ToLower(), values)))
                            {
                                continue;
                            }
                            break;
                        case SearchBy.FR_TMS_Task: // If search criteria is NOT present in cell value
                            if (row.GetCell(cFRTMSTask).IsEmpty ||
                                (values.Length == 1 && !row.GetCell(cFRTMSTask).Value.ToString().ToLower().Contains(values[0])) ||
                                (values.Length > 1 && !MultiSearchANDlogic(row.GetCell(cFRTMSTask).Value.ToString().ToLower(), values)))
                            {
                                continue;
                            }
                            break;
                        case SearchBy.FR_TEXT: // If search criteria is NOT present in cell value
                            if (row.GetCell(cFRText).IsEmpty ||
                                (values.Length == 1 && !row.GetCell(cFRText).Value.ToString().ToLower().Contains(values[0])) ||
                                (values.Length > 1 && !MultiSearchANDlogic(row.GetCell(cFRText).Value.ToString().ToLower(), values)))
                            {
                                continue;
                            }
                            break;
                    }

                    FR = new FR();
                    FR.FRSource = XLSFile;
                    FR.FRID = !row.GetCell(cFRID).IsEmpty ? row.GetCell(cFRID).Value.ToString() : "";
                    FR.FRTMSTask = !row.GetCell(cFRTMSTask).IsEmpty ? row.GetCell(cFRTMSTask).Value.ToString() : "";
                    FR.FRText = !row.GetCell(cFRText).IsEmpty ? row.GetCell(cFRText).Value.ToString() : "";
                    FR.CCP = !row.GetCell(cCCP).IsEmpty ? row.GetCell(cCCP).Value.ToString() : "";
                    FR.Created = !row.GetCell(cCreated).IsEmpty ? DateTime.FromOADate(Convert.ToInt32(row.GetCell(cCreated).Value)).ToShortDateString() : "";
                    FR.Modified = !row.GetCell(cModified).IsEmpty ? DateTime.FromOADate(Convert.ToInt32(row.GetCell(cModified).Value)).ToShortDateString() : "";
                    Result.Add(FR);
                    // Break if many results
                    if (limitResults && Result.Count >= ClientParams.Parameters.ResultsLimit)
                    {
                        break;
                    }
                }
                // Break if many results
                if (limitResults && Result.Count >= ClientParams.Parameters.ResultsLimit)
                {
                    break;
                }
            }
            return Result;
        }

        // Return true in case all values present in text
        public bool MultiSearchANDlogic(string text, string[] values)
        {
            bool result = true;
            for (int a = 0; a < values.Length; a++)
            {
                result = result & text.Contains(values[a]);
            }
            return result;
        }

        // Return true in case at leats one value present in text
        public bool MultiSearchORlogic(string text, string[] values)
        {
            for (int a = 0; a < values.Length; a++)
            {
                if (text.Equals(values[a]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

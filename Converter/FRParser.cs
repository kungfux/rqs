/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013-2015
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
 *  interest in the program "WebQA"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 06 November 2013.
 */

using System.IO;
using WebQA.Logic;
using ExcelLibrary.SpreadSheet;
using System;
using System.Data.SQLite;

namespace WebQA.Converter
{
    internal class FRParser
    {
        private string[] XLSFiles = new string[0];

        private void LoadXLSFilesList(string XLSFilesLocation)
        {
            DirectoryInfo XLSLocation = new DirectoryInfo(XLSFilesLocation);
            FileInfo[] XLSFilesInfo = XLSLocation.GetFiles("*.xls");
            int XLSFilesCount = XLSFilesInfo.Length;
            XLSFiles = new string[XLSFilesCount];
            for (int a = 0; a < XLSFilesCount; a++)
            {
                if (XLSFilesInfo[a].Name.StartsWith(".~"))
                {
                    continue;
                }
                XLSFiles[a] = XLSFilesInfo[a].Name;
            }
            Trace.Add(string.Format("{0} excel files are found", XLSFiles.Length), Trace.Color.Green);
        }

        public void SearchAndSave(string XLSFilesLocation)
        {
            // Load files before each search to
            // avoid message about locked files
            LoadXLSFilesList(XLSFilesLocation);
            if (XLSFiles.Length <= 0)
            {
                // Return only in case no .xls files
                return;
            }

            Workbook book;
            Worksheet sheet;
            Row row;
            FR FR;

            foreach (string XLSFile in XLSFiles)
            {
                if (XLSFile == null)
                {
                    continue;
                }

                try
                {
                    book = Workbook.Load(XLSFilesLocation + "\\" + XLSFile);
                }
                catch (IOException)
                {
                    Trace.Add(XLSFile + " is locked by another process and will be skipped!", Trace.Color.Yellow);
                    continue;
                }

                Trace.Add(string.Format("Processing file: {0}", XLSFile), Trace.Color.Green);

                sheet = book.Worksheets[0];

                Trace.Add(string.Format("{0} records to be processed", sheet.Cells.LastRowIndex), Trace.Color.Green);

                // Determine columns
                int cFRID = 0;        // 1st column by default
                int cFRTMSTask = 1;   // 2nd column by default
                int cFRText = 3;      // 4rd column by default
                int cFRObject = -1;
                int cCCP = -1;
                int cCreated = -1;
                int cModified = -1;
                int cStatus = -1;

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
                            // FR ID, NFR ID, ID
                            case "id":
                                cFRID = a;
                                break;
                            case "fr id":
                                cFRID = a;
                                break;
                            case "nfr id":
                                cFRID = a;
                                break;
                            // FR TMS Task, NFR TMS Task
                            case "fr tms task":
                                cFRTMSTask = a;
                                break;
                            case "nfr tms task":
                                cFRTMSTask = a;
                                break;
                            // Functional Requirements, Non-Functional Requirements
                            case "functional requirements":
                                cFRText = a;
                                break;
                            case "non-functional requirements":
                                cFRText = a;
                                break;
                            // Object Number
                            case "object number":
                                cFRObject = a;
                                break;
                            // CCP
                            case "ccp":
                                cCCP = a;
                                break;
                            case "ccp level":
                                cCCP = a;
                                break;
                            // FR Date, NFR Date
                            case "fr date":
                                cCreated = a;
                                break;
                            case "nfr date":
                                cCreated = a;
                                break;
                            // Last Modified On
                            case "last modified on":
                                cModified = a;
                                break;
                            // Status
                            case "fr status":
                                cStatus = a;
                                break;
                            case "nfr status":
                                cStatus = a;
                                break;
                        }
                    }
                }

                for (int a = sheet.Cells.FirstRowIndex + 1; a <= sheet.Cells.LastRowIndex; a++)
                {
                    row = sheet.Cells.GetRow(a);

                    FR = new FR();
                    try
                    {
                        FR.FRSource = XLSFile;
                        FR.FRID = !row.GetCell(cFRID).IsEmpty ? row.GetCell(cFRID).Value.ToString() : "";
                        FR.FRTMSTask = !row.GetCell(cFRTMSTask).IsEmpty ? row.GetCell(cFRTMSTask).Value.ToString() : "";
                        FR.FRObject = !row.GetCell(cFRObject).IsEmpty ? row.GetCell(cFRObject).Value.ToString() : "";
                        FR.FRText = !row.GetCell(cFRText).IsEmpty ? row.GetCell(cFRText).Value.ToString() : "";
                        FR.CCP = !row.GetCell(cCCP).IsEmpty ? row.GetCell(cCCP).Value.ToString() : "";
                        // .ToInt32() methods are commented due to date convertion issues
                        //FR.Created = !row.GetCell(cCreated).IsEmpty ? DateTime.FromOADate(Convert.ToInt32(row.GetCell(cCreated).Value)).ToShortDateString() : "";
                        //FR.Modified = !row.GetCell(cModified).IsEmpty ? DateTime.FromOADate(Convert.ToInt32(row.GetCell(cModified).Value)).ToShortDateString() : "";
                        FR.Created = !row.GetCell(cCreated).IsEmpty ? row.GetCell(cCreated).Value.ToString() : "";
                        FR.Modified = !row.GetCell(cModified).IsEmpty ? row.GetCell(cModified).Value.ToString() : "";
                        FR.Status = !row.GetCell(cStatus).IsEmpty ? row.GetCell(cStatus).Value.ToString() : "";
                    }
                    catch (Exception ex)
                    {
                        Trace.Add(string.Format("Cannot parse row #{0} because of {1}", a + 1, ex.Message), Trace.Color.Red);
                    }
                    finally
                    {
                        if (FR.FRID != null)
                        {
                            SQLiteIteractionLite.ChangeData("INSERT INTO Requirements (source, fr_id, fr_tms_task, fr_object, fr_text, ccp, created, modified, status) VALUES (@source, @fr_id, @fr_tms_task, @fr_object, @fr_text, @ccp, @created, @modified, @status)",
                                new SQLiteParameter("@source", FR.FRSource),
                                new SQLiteParameter("@fr_id", FR.FRID),
                                new SQLiteParameter("@fr_tms_task", FR.FRTMSTask),
                                new SQLiteParameter("@fr_object", FR.FRObject),
                                new SQLiteParameter("@fr_text", FR.FRText),
                                new SQLiteParameter("@ccp", FR.CCP),
                                new SQLiteParameter("@created", FR.Created),
                                new SQLiteParameter("@modified", FR.Modified),
                                new SQLiteParameter("@status", FR.Status));
                        }
                    }
                }

                Trace.Add(string.Format("Processed: {0} records", sheet.Cells.LastRowIndex), Trace.Color.Green);
            }
        }
    }
}
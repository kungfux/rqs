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

using WebQA.Logic;
using System.IO;
using System;

namespace WebQA.Converter
{
    internal class ReqConverter
    {
        private const string db = "webqa.sqlite";

        public int Convert(string[] args)
        {
            if (args.Length == 2)
            {
                string command = args[0];
                string path = args[1];

                if (command == null || !command.Equals("requirements"))
                {
                    Trace.Instance.Add(
                        string.Format("Unknown command '{0}' is specified", command), Trace.Color.Red);
                    return 2;
                }
                if (path == null || !Directory.Exists(path))
                {
                    Trace.Instance.Add(
                        string.Format("Path '{0}' not found", path), Trace.Color.Red);
                    return 2;
                }
                if (File.Exists(db))
                {
                    Trace.Instance.Add("Database already exist! Press any key to continue or Ctrl+C to abort", Trace.Color.Red);
                    Console.ReadKey();
                }

                // Create new database
                SQLiteIteractionLite.SetTrace(true);
                if (!SQLiteIteractionLite.TestConnection(
                    string.Format(
                    "Data Source={0};Version=3;FailIfMissing=False;UTF8Encoding=True;Foreign Keys=True;Journal Mode=Off;Locking Mode=EXCLUSIVE;", db),
                    true))
                {
                    Trace.Instance.Add("Unable to create database", Trace.Color.Red);
                    if (!string.IsNullOrEmpty(SQLiteIteractionLite.LastErrorMessage))
                    {
                        Trace.Instance.Add(SQLiteIteractionLite.LastErrorMessage, Trace.Color.Red);
                    }
                    return 2; // database not found, terminate
                }
                else
                {
                    SQLiteIteractionLite.ChangeData(
                        string.Concat(
                        "CREATE TABLE [REQUIREMENTS] (",
                        "[id]			INTEGER PRIMARY KEY AUTOINCREMENT,",
                        "[source]		VARCHAR(255) NOT NULL,",
                        "[fr_id]		VARCHAR(10) NOT NULL,",
                        "[fr_tms_task]	VARCHAR(10),",
                        "[fr_object]    VARCHAR(20),",
                        "[fr_text]		VARCHAR(255),",
                        "[ccp]			INTEGER(1),",
                        "[created]		VARCHAR(20),",
                        "[modified]		VARCHAR(20),",
                        "[status]		VARCHAR(15),",
                        "[boundary]		VARCHAR(5));"
                        ));

                    Trace.Instance.Add("Processing excel files...");
                    FRParser parser = new FRParser();
                    parser.SearchAndSave(path);
                    Trace.Instance.Add("Finished");

                    // Count all requirements
                    Int64 count = SQLiteIteractionLite.SelectCell<Int64>(
                        "SELECT COUNT(*) FROM REQUIREMENTS;");
                    Trace.Instance.Add(
                        string.Format("{0} requirements in the database", count));
                }

                return 0;
            }
            else
            {
                Trace.Instance.Add("Wrong arguments are specified", Trace.Color.Red);
                return 2;
            }
        }
    }
}

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

using System;
using WebQA.Logic;
using System.IO;

namespace WebQA.Converter
{
    class HospConverter
    {
        private const string db = "webqa.sqlite";

        public int Convert(string[] args)
        {
            if (args.Length == 2)
            {
                string command = args[0];
                string path = args[1];

                if (command == null || !command.Equals("hosparams"))
                {
                    Trace.Add(
                        string.Format("Unknown command '{0}' is specified", command), Trace.Color.Red);
                    return 2;
                }
                if (path == null || !File.Exists(path))
                {
                    Trace.Add(
                        string.Format("File '{0}' not found", path), Trace.Color.Red);
                    return 2;
                }
                if (File.Exists(db))
                {
                    Trace.Add("Database already exist! Are you sure want to continue?", Trace.Color.Red);
                    Console.ReadKey();
                }

                // Create new database
                SQLiteIteractionLite.SetTrace(true);
                if (!SQLiteIteractionLite.TestConnection(
                    string.Format(
                    "Data Source={0};Version=3;FailIfMissing=False;UTF8Encoding=True;Foreign Keys=True;Journal Mode=Off;Locking Mode=EXCLUSIVE;", db),
                    true))
                {
                    Trace.Add("Unable to create database", Trace.Color.Red);
                    return 2; // database not found, terminate
                }
                else
                {
                    SQLiteIteractionLite.ChangeData(
                        string.Concat(
                        "CREATE TABLE [HOSPARAMS] (",
                        "[id]			INTEGER PRIMARY KEY AUTOINCREMENT,",
                        "[source]		VARCHAR(255) NOT NULL,",
                        "[module]		VARCHAR(255),",
                        "[section]  	VARCHAR(255),",
                        "[mdesc]		VARCHAR(255),", // Main description
                        "[sdesc]		VARCHAR(255),",   // Short description
                        "[name] 		VARCHAR(255),",
                        "[modified]		VARCHAR(255),",
                        "[status]		VARCHAR(255));"
                        ));

                    Trace.Add("Processing hosparam file...", Trace.Color.Green);
                    HospParser parser = new HospParser(args[1]);
                    parser.Parse(args[1]);
                    Trace.Add("Finished", Trace.Color.Green);

                    // Count all requirements
                    Int64 count = SQLiteIteractionLite.SelectCell<Int64>(
                        "SELECT COUNT(*) FROM HOSPARAMS;");
                    Trace.Add(
                        string.Format("{0} hosparams in the database", count),
                        Trace.Color.Green);
                }

                return 0;
            }
            else
            {
                Trace.Add("Wrong arguments are specified", Trace.Color.Red);
                return 2;
            }
        }
    }
}

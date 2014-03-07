/*   
 *  WebQA
 *  WebQA Server
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
 *  interest in the program "WebQA"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 06 November 2013.
 */

using WebQA.Logic;
using System;
using System.Net;
using System.IO;

namespace WebQA
{
    class Program
    {
        public static IPEndPoint WebQAaddress;

        public static long REQS_IN_THE_DB = 0;

        private const string db = "webqa.sqlite";

        // The main entry point
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                // Display help
                Console.WriteLine(
                    string.Format(
                        "Usage:{0} - webqa [port]{0}{1}start as web server;{0} - webqa requirements [path]{0}{1}convert .xls requirements to database;{0}"+
                        " - webqa hosparams [path]{0}{1}convert hosparams from .txt to database.", 
                        Environment.NewLine,
                        "   "));
                return 0;
            }

            if (args.Length == 1)
            {
                // Start as web server
                int port = 8080;
                if (args.Length == 1 && !int.TryParse(args[0], out port))
                {
                    Trace.Add("Wrong argument is specified. Expecting port number.", Trace.Color.Red);
                }

                // Prepare dir for logs
                Directory.SetCurrentDirectory(".");
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }

                // Add trace about launching
                Trace.Add("WebQA is started", Trace.Color.Green);

                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                WebQAaddress = new IPEndPoint(ipAddress, port);

                Trace.Add(string.Format("WebQA address: {0}", WebQAaddress.ToString()), Trace.Color.Green);

                // Turn on sql traces and connect to db
                SQLiteIteractionLite.SetTrace(true);
                if (!SQLiteIteractionLite.TestConnection(
                    string.Format(
                    "Data Source={0};Version=3;FailIfMissing=True;UTF8Encoding=True;Foreign Keys=True;Read Only=True;", db),
                    true))
                {
                    Trace.Add("Database not found. WebQA is stopped", Trace.Color.Red);
                    return 2; // database not found, terminate
                }
                else
                {
                    // Count all requirements
                    REQS_IN_THE_DB = SQLiteIteractionLite.SelectCell<Int64>(
                        "SELECT COUNT(*) FROM REQUIREMENTS;");
                    Trace.Add(
                        string.Format("{0} requirements in the database", REQS_IN_THE_DB),
                        Trace.Color.Green);
                }

                // Prepare web server and start listening
                AsynchronousSocketListener l = new AsynchronousSocketListener();
                l.StartListening();

                // Add trace about finishing program
                Trace.Add("WebQA is stopped", Trace.Color.Green);

                return 0;
            }
            else
            {
                // Trying to start converters
                if (args[0].Equals("requirements"))
                {
                    Converter.ReqConverter converter = new Converter.ReqConverter();
                    converter.Convert(args);
                }

                if (args[0].Equals("hosparams"))
                {
                    Converter.HospConverter converter = new Converter.HospConverter();
                    converter.Convert(args);
                }
                
                return 2;
            }
        }
    }
}

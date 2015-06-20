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
using System;
using System.Net;

namespace WebQA
{
    class Program
    {

        private const string DB_FILE_NAME = "webqa.sqlite";
        private const string CONNECTION_STRING = @"Data Source={0};Version=3;FailIfMissing=True;UTF8Encoding=True;Foreign Keys=True;Read Only=True;";

        // The main entry point
        static int Main(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "/?":
                        Console.WriteLine(Resources.HELP);
                        Environment.Exit(0);
                        break;
                    case "--help":
                        Console.WriteLine(Resources.HELP);
                        Environment.Exit(0);
                        break;
                    case "requirements":
                        Converter.ReqConverter rc = new Converter.ReqConverter();
                        return rc.Convert(args);
                    case "hosparams":
                        Converter.HospConverter hc = new Converter.HospConverter();
                        return hc.Convert(args);
                    default:
                        int port;
                        if (int.TryParse(args[0], out port) == false)
                        {
                            Console.WriteLine("Unable to parse port number");
                            Environment.Exit(1);
                        }
                        ServerPort = port;
                        break;
                }
            }

            // Add trace about launching
            Trace.Instance.Add("WebQA is started");
            Trace.Instance.Add(string.Format("WebQA address: {0}", WebQAaddress.ToString()));

            // Turn on sql traces and connect to db
            SQLiteIteractionLite.SetTrace(true);
            if (!SQLiteIteractionLite.TestConnection(string.Format(CONNECTION_STRING, DB_FILE_NAME), true))
            {
                Trace.Instance.Add("Database not found. WebQA is stopped", Trace.Color.Red);
                Environment.Exit(2);
            }
            else
            {
                Trace.Instance.Add(
                    string.Format("{0} requirements in the database", ReqsCountInDB));
            }

            // Prepare web server and start listening
            (new AsynchronousSocketListener()).StartListening();

            Trace.Instance.Add("WebQA is stopped");

            return 0;
        }

        private static IPEndPoint _webQAaddress;
        public static IPEndPoint WebQAaddress
        {
            get
            {
                if (_webQAaddress == null)
                {
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    _webQAaddress = new IPEndPoint(ipAddress, ServerPort);
                }
                return _webQAaddress;
            }
        }

        private static int? _server_port;
        public static int ServerPort
        {
            get
            {
                return _server_port.HasValue ? _server_port.Value : 80;
            }
            set
            {
                _server_port = value;
            }
        }

        private static long? _reqsCount;
        public static long ReqsCountInDB
        {
            get
            {
                if (!_reqsCount.HasValue)
                {
                    _reqsCount = SQLiteIteractionLite.SelectCell<Int64>(
                   "SELECT COUNT(*) FROM REQUIREMENTS;");
                }
                return _reqsCount.Value;
            }
        }
    }
}

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
using System.IO;

namespace WebQA.Logic
{
    public class Trace : IDisposable
    {
        private static readonly Lazy<Trace> _instance = new Lazy<Trace>(() => new Trace());
        public static Trace Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public enum Color
        {
            Green,
            Yellow,
            Red
        }

        private Trace()
        {
            try
            {
                _logStreamWriter = new StreamWriter(LogFileName, true);
            }
            catch (Exception ex)
            {
                Add("Log file can not be created or accessed", Color.Red);
                Add("Logs will be written to console only!", Color.Red);
                Add("Details: " + ex, Color.Red);
            }
        }

        // Add record to log and console
        public void Add(string text, Color level = Color.Green)
        {
            switch (level)
            {
                case Color.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Color.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            string message = string.Concat(
                DateTime.Now.ToString(_timestampFormat),
                "   ",
                text);

            Console.WriteLine(message);

            if (_logStreamWriter != null)
            {
                try
                {
                    _logStreamWriter.WriteLine(message);
                    _logStreamWriter.Flush();
                }
                catch (IOException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        string.Concat(
                        DateTime.Now.ToString(_timestampFormat),
                        "WARNING",
                        Environment.NewLine,
                        e.ToString()));
                }
            }
        }

        public static string LogFileName
        {
            get
            {
                return "webqa.log";
            }
        }

        void IDisposable.Dispose()
        {
            if (_logStreamWriter != null)
            {
                _logStreamWriter.Close();
            }
        }

        private StreamWriter _logStreamWriter;
        private const string _timestampFormat = "dd.MM.yyyy HH:mm:ss:fff";
    }
}

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

using System;
using System.IO;

namespace WebQA.Logic
{
    internal static class Trace
    {
        public enum Color
        {
            Green,
            Yellow,
            Red
        }

        // Declare log file
        static StreamWriter streamWriter = new StreamWriter(
                string.Format("log\\log_{0}.log",
                DateTime.Now.ToString("dd.MM.yyyy")),
                true);

        // Add record to log and console
        public static void Add(string text, Color level)
        {
            // Set appropriate color for console
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

            // Prepare string for output
            string message = string.Concat(
                DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:fff"),
                "   ",
                text);

            // Output to console
            Console.WriteLine(message);

            // Output to file
            try
            {
                streamWriter.WriteLine(message);
                // Flush data
                streamWriter.Flush();
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    string.Concat(
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:fff"),
                    "WARNING",
                    Environment.NewLine,
                    e.ToString()));
            }
        }
    }
}

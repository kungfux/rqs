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
    class WebPageSource
    {
        public string LoadSource(string pSourceFileName)
        {
            StreamReader reader = null;

            try
            {
                if (pSourceFileName != null &&
                    !pSourceFileName.StartsWith(".") &&
                    !pSourceFileName.StartsWith(@"\") &&
                    !pSourceFileName.StartsWith(@"/"))
                {
                    reader = new StreamReader(string.Format(@"WebPages\{0}.html", pSourceFileName));
                    return reader.ReadToEnd();
                }
                else
                {
                    throw new Exception("Reading from external source is denied.");
                }
            }
            catch (FileNotFoundException)
            {
                Trace.Add(string.Format("Requested file was not found: {0}.html", pSourceFileName), Trace.Color.Red);
                return string.Format(
                    "<p>Requested file {0}.html was not found on server.<br>" + 
                    "Please notify server owner about that problem.<br>Thanks!</p>" + 
                    "<script>document.write('<a href=\"' + document.referrer + '\">Return Back</a>');</script>", 
                    pSourceFileName);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}

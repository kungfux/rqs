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

using System.Data.SQLite;
using System;
using System.Text;

namespace WebQA.Logic
{
    internal class Stat
    {
        public void Add(string ip, string request, int traffic)
        {
            SQLiteIteractionLite.ChangeData("INSERT INTO HISTORY (date, ip, request, traffic) VALUES (@date, @ip, @request, @traffic)",
                new SQLiteParameter("@date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:fff")),
                new SQLiteParameter("@ip", ip),
                new SQLiteParameter("@request", request),
                new SQLiteParameter("@traffic", traffic));
        }

        public StringBuilder GetHistory()
        {
            StringBuilder result = new StringBuilder();
            return result;
        }
    }
}

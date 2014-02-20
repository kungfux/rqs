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

using System;

namespace RQS.Logic
{
    internal class FR
    {
        public string FRSource;   // File name which contain this FR
        public string FRID;       // FR ID number (from file)
        public string FRTMSTask;  // FR TMS task number (from file)
        public string FRObject;   // FR Object Number (from file)
        public string FRText;     // FR text (from file)
        public string CCP;        // FR CCP (from file)
        public string Created;    // FR created date (from file)
        public string Modified;   // FR modified date (from file)
        public string Status;     // FR status (from file)
    }
}

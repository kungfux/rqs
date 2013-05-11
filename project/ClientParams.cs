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
using ItWorksTeam.IO;
using System.Windows.Forms;
using System.Drawing;

namespace RQS
{
    internal class ClientParams
    {
        private static ClientParams _clientParams;
        private static Object _lock = new Object();
        private Registry Registry = new Registry();
        private string RegPath = "Software\\ItWorksTeam\\RQS";

        #region Default parameters
        public string XLSLocation = Application.StartupPath;
        public Color ColoredLinesColor1 = Color.FromArgb(212, 208, 200);
        public Color ColoredLinesColor2 = Color.FromArgb(255, 255, 255);
        public int ResultsLimit = 100;
        #endregion

        public static ClientParams Parameters
        {
            get
            {
                if (_clientParams == null)
                {
                    lock (_lock)
                    {
                        _clientParams = new ClientParams();
                    }
                }
                return _clientParams;
            }
        }
    }
}

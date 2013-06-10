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

        public Color ColoredLinesColor1 = Color.Gainsboro;
        public Color ColoredLinesColor2 = Color.White;

        public int ResultsLimit = 100;
        public int WindowSizeWidth = 783;
        public int WindowSizeHeight = 527;
        public int WindowSizeState = (int)FormWindowState.Normal;

        public string[] SearchHistory;
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
                        _clientParams.LoadParams();
                    }
                }
                return _clientParams;
            }
        }

        // Save custom parameters
        // or remove which are set by default
        public void SaveParams()
        {
            // Window Size
            if (WindowSizeState != ClientParams.Parameters.WindowSizeState)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeState", ClientParams.Parameters.WindowSizeState);
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeState");
            }

            if (WindowSizeWidth != ClientParams.Parameters.WindowSizeWidth)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeWidth", ClientParams.Parameters.WindowSizeWidth);
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeWidth");
            }

            if (WindowSizeHeight != ClientParams.Parameters.WindowSizeHeight)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeHeight", ClientParams.Parameters.WindowSizeHeight);
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "WindowSizeHeight");
            }
            // end of Window Size

            // Search directory
            if (XLSLocation != ClientParams.Parameters.XLSLocation)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "XLSLocation", ClientParams.Parameters.XLSLocation);
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "XLSLocation");
            }
            // end of Search directory

            // Results limit
            if (ResultsLimit != ClientParams.Parameters.ResultsLimit)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ResultsLimit", ClientParams.Parameters.ResultsLimit);
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ResultsLimit");
            }
            // End of Results limit

            // Grid colors
            if (ColoredLinesColor1 != ClientParams.Parameters.ColoredLinesColor1)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ColoredLinesColor1", ClientParams.Parameters.ColoredLinesColor1.ToKnownColor().ToString());
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ColoredLinesColor1");
            }
            if (ColoredLinesColor2 != ClientParams.Parameters.ColoredLinesColor2)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ColoredLinesColor2", ClientParams.Parameters.ColoredLinesColor2.ToKnownColor().ToString());
            }
            else
            {
                Registry.DeleteKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                    RegPath, "ColoredLinesColor2");
            }
            // End of Grid colors

            // Search History
            if (ClientParams.Parameters.SearchHistory != null &&
                ClientParams.Parameters.SearchHistory.Length > 0)
            {
                string History = "";
                if (ClientParams.Parameters.SearchHistory.Length == 1)
                {
                    History = ClientParams.Parameters.SearchHistory[0];
                }
                else
                {
                    for (int a = 0; a < ClientParams.Parameters.SearchHistory.Length; a++)
                    {
                        if (ClientParams.Parameters.SearchHistory[a] != null &&
                            !ClientParams.Parameters.SearchHistory[a].Equals(""))
                        {
                            History += ClientParams.Parameters.SearchHistory[a];
                            // Limit saving to 30 records
                            if (a >= 29)
                            {
                                break;
                            }
                            // Do not add '|' at the end
                            if ((a + 1) == ClientParams.Parameters.SearchHistory.Length)
                            {
                                continue;
                            }
                            else
                            {
                                History += "|";
                            }
                        }
                    }
                }
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                            RegPath, "History", History);
            }
            // end of Search History
        }

        // Load custom parameters
        public void LoadParams()
        {
            // Window Size
            WindowSizeState =
                Registry.ReadKey<int>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "WindowSizeState", WindowSizeState);
            WindowSizeWidth =
                Registry.ReadKey<int>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "WindowSizeWidth", WindowSizeWidth);
            WindowSizeHeight =
                Registry.ReadKey<int>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "WindowSizeHeight", WindowSizeHeight);
            // end of Window Size

            // Search directory
            XLSLocation =
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "XLSLocation", XLSLocation);
            // end of Search directory

            // Results limit
            ResultsLimit =
                Registry.ReadKey<int>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "ResultsLimit", ResultsLimit);
            // End of Results limit

            // Grid colors
            ColoredLinesColor1 =
                Color.FromName(
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "ColoredLinesColor1", ColoredLinesColor1.ToKnownColor().ToString()));
            ColoredLinesColor2 =
                Color.FromName(
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "ColoredLinesColor2", ColoredLinesColor2.ToKnownColor().ToString()));
            // End of Grid colors

            // Search History
            string History =
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegPath, "History", null);
            if (History != null &&
                !History.Equals(""))
            {
                if (History.Contains("|"))
                {
                    SearchHistory = History.Split('|');
                }
                else
                {
                    SearchHistory = new string[1] { History };
                }
            }
            // end of Search History
        }
    }
}

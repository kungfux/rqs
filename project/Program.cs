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
using System.Threading;
using System.Windows.Forms;
using RQS.Logic;

namespace RQS
{
    public class Program
    {
        private const string AppMutexName = "RQS";

        [STAThread]
        static void Main(string[] args)
        {
            using (Mutex mutex = new Mutex(false, AppMutexName))
            {
                bool Running = !mutex.WaitOne(0, false);
                if (!Running || ClientParams.Parameters.SecondCopyAllowed)
                {
                    if (args != null &&
                        args.Length > 0)
                    {
                        if (args.Length == 2)
                        {
                            switch (args[0])
                            {
                                case "-sFR":
                                    ClientParams.Parameters.AutoSearchBy = FRSearch.SearchBy.FR_ID;
                                    break;
                                case "-sTMS":
                                    ClientParams.Parameters.AutoSearchBy = FRSearch.SearchBy.FR_TMS_Task;
                                    break;
                                case "-sText":
                                    ClientParams.Parameters.AutoSearchBy = FRSearch.SearchBy.FR_TEXT;
                                    break;
                            }
                            ClientParams.Parameters.AutoSearchArgument = args[1];
                            ClientParams.Parameters.AutoSearchAtStartUp = true;
                        }
                        else
                        {
                            string allArgs = "";
                            foreach(string s in args)
                            {
                                allArgs += s;
                            }
                            MessageBox.Show(
                                string.Format("Wrong arguments are passed to RQS: {1}{0}Expected arguments:{0}RQS.exe -[sFR|sTMS|sText] [criteria]",
                                Environment.NewLine, allArgs), "RQS",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new GUI.RQS());
                }
                else
                {
                    MessageBox.Show("RQS is already run.", "RQS", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}

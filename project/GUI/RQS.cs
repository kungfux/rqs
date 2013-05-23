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

using System.Windows.Forms;

namespace RQS.GUI
{
    public partial class RQS : Form
    {
        public RQS()
        {
            InitializeComponent();

            // Apply custom window size
            this.Width = ClientParams.Parameters.WindowSizeWidth;
            this.Height = ClientParams.Parameters.WindowSizeHeight;

            if ((FormWindowState)ClientParams.Parameters.WindowSizeState == FormWindowState.Normal ||
                (FormWindowState)ClientParams.Parameters.WindowSizeState == FormWindowState.Maximized)
            {
                this.WindowState = (FormWindowState)ClientParams.Parameters.WindowSizeState;
            }
        }

        private Search cSearch;
        private About cAbout;
        private Setup cSetup;

        private void DisplayControl(Control UserControl)
        {
            gRQS.Controls.Clear();
            if (UserControl.Tag != null)
            {
                gRQS.Text = UserControl.Tag.ToString();
            }
            gRQS.Controls.Add(UserControl);
            UserControl.Dock = DockStyle.Fill;
        }

        private void RQS_Load(object sender, System.EventArgs e)
        {
            tsSearch_Click(this, null);
        }

        private void tsSearch_Click(object sender, System.EventArgs e)
        {
            if (cSearch == null)
            {
                cSearch = new Search();
            }
            DisplayControl(cSearch);
        }

        private void tsAbout_Click(object sender, System.EventArgs e)
        {
            if (cAbout == null)
            {
                cAbout = new About();
            }
            DisplayControl(cAbout);
        }

        private void tsSetup_Click(object sender, System.EventArgs e)
        {
            if (cSetup == null)
            {
                cSetup = new Setup();
            }
            DisplayControl(cSetup);
        }

        private void RQS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void RQS_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save custom window size
            if (this.WindowState != FormWindowState.Minimized)
            {
                ClientParams.Parameters.WindowSizeState = (int)this.WindowState;

                if (this.WindowState != FormWindowState.Maximized)
                {
                    ClientParams.Parameters.WindowSizeWidth = this.Width;
                    ClientParams.Parameters.WindowSizeHeight = this.Height;
                }

                new ClientParams().SaveParams();
            }
        }
    }
}

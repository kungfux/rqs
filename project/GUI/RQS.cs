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
        }

        private Search cSearch = new Search();
        private About cAbout = new About();

        private void DisplayControl(Control UserControl)
        {
            gRQS.Controls.Clear();
            gRQS.Controls.Add(UserControl);
            UserControl.Dock = DockStyle.Fill;
        }

        private void RQS_Load(object sender, System.EventArgs e)
        {
            tsSearch_Click(this, null);
        }

        private void tsSearch_Click(object sender, System.EventArgs e)
        {
            DisplayControl(cSearch);
        }

        private void tsAbout_Click(object sender, System.EventArgs e)
        {
            DisplayControl(cAbout);
        }

        private void RQS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}

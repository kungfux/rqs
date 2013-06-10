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
using System.Drawing;
using System;

namespace RQS.GUI
{
    public partial class Setup : UserControl
    {
        public Setup()
        {
            InitializeComponent();

            string[] KnownColors = Enum.GetNames(typeof(KnownColor));
            cmbColor1.Items.AddRange(KnownColors);
            cmbColor2.Items.AddRange(KnownColors);
            cmbColor1.SelectedIndex = 0;
            cmbColor2.SelectedIndex = 0;

            cmbColor1.DrawItem += new DrawItemEventHandler(cmbColor_DrawItem);
            cmbColor2.DrawItem += new DrawItemEventHandler(cmbColor_DrawItem);
        }

        private void Setup_Load(object sender, System.EventArgs e)
        {
            // Show current parameters to user
            UpdateGUIValues();
        }

        private void UpdateGUIValues()
        {
            lLocation.Text = string.Format("Current location: {0}",
                ClientParams.Parameters.XLSLocation);
            nLimit.Value = ClientParams.Parameters.ResultsLimit;
            cmbColor1.Text = ClientParams.Parameters.ColoredLinesColor1.ToKnownColor().ToString();
            cmbColor2.Text = ClientParams.Parameters.ColoredLinesColor2.ToKnownColor().ToString();
        }

        void cmbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox combo = (ComboBox)sender;

                e.DrawBackground();

                Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                e.Bounds.Height, e.Bounds.Height - 4);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromName(combo.Items[e.Index].ToString())), rectangle);
                Font myFont = new Font("Tahome", 8, FontStyle.Regular);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), myFont, System.Drawing.Brushes.Black,
                    new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                // Draw the focus rectangle if the mouse hovers over an item.
                e.DrawFocusRectangle();
            }
        }

        private void linkLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog folderLookup = new FolderBrowserDialog();
            folderLookup.Description = "Select folder with .xls files that will be used for searching requirements";
            folderLookup.SelectedPath = ClientParams.Parameters.XLSLocation;
            folderLookup.ShowNewFolderButton = false;
            folderLookup.ShowDialog();
            if (folderLookup.SelectedPath != ClientParams.Parameters.XLSLocation)
            {
                ClientParams.Parameters.XLSLocation = folderLookup.SelectedPath;
                UpdateGUIValues();
            }
        }

        private void nLimit_ValueChanged(object sender, EventArgs e)
        {
            if (!nLimit.Focused)
            {
                return;
            }
            ClientParams.Parameters.ResultsLimit = (int)nLimit.Value;
        }

        private void cmbColor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbColor1.Focused)
            {
                return;
            }
            ClientParams.Parameters.ColoredLinesColor1 = Color.FromName(cmbColor1.Text);
        }

        private void cmbColor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbColor2.Focused)
            {
                return;
            }
            ClientParams.Parameters.ColoredLinesColor2 = Color.FromName(cmbColor2.Text);
        }
    }
}

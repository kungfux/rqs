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
using System.Collections.Generic;
using System;
using RQS.Logic;
using System.Diagnostics;

namespace RQS.GUI
{
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
        }

        private FRSearch FRSearch = new FRSearch();
        private SmartDataGridView DataGridView = new SmartDataGridView("SearchResults", 9);
        private int[] LastMouseDownLocation = new int[] { 0, 0 };
        private SmartDataGridViewColumnSorter ColumnSorter;
        private bool HandledKeyDown = false;

        private void Search_Load(object sender, System.EventArgs e)
        {
            comboSearchBy.SelectedIndex = 0;

            DataGridView.Dock = DockStyle.Fill;

            DataGridView.Columns.Add("", "#");
            DataGridView.Columns.Add("", "Source");
            DataGridView.Columns.Add("", "FR ID");
            DataGridView.Columns.Add("", "TMS task");
            DataGridView.Columns.Add("", "FR text");
            DataGridView.Columns.Add("", "CCP");
            DataGridView.Columns.Add("", "Created");
            DataGridView.HideColumnByDefault(6);
            DataGridView.Columns.Add("", "Last modified");
            DataGridView.HideColumnByDefault(7);

            DataGridViewCheckBoxCell checkboxCell = new DataGridViewCheckBoxCell();
            checkboxCell.Value = false;
            DataGridViewColumn IsChangedColumn = new DataGridViewColumn(checkboxCell);
            IsChangedColumn.HeaderText = "Is changed?";
            IsChangedColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            DataGridView.Columns.Add(IsChangedColumn);
            DataGridView.HideColumnByDefault(8);

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            DataGridView.MouseDown += new MouseEventHandler(DataGridView_MouseDown);

            ColumnSorter = new SmartDataGridViewColumnSorter(DataGridView);

            DataGridView.MultiSelect = true;
            //DataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DataGridView.ContextMenuStrip = contextMenuStrip1;

            tableLayoutPanel1.Controls.Add(DataGridView, 0, 1);

            // Restore search history
            if (ClientParams.Parameters.SearchHistory != null &&
                ClientParams.Parameters.SearchHistory.Length > 0)
            {
                comboSearchText.Items.AddRange(ClientParams.Parameters.SearchHistory);
            }
        }

        void DataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                LastMouseDownLocation[0] = DataGridView.HitTest(e.X, e.Y).RowIndex;
                LastMouseDownLocation[1] = DataGridView.HitTest(e.X, e.Y).ColumnIndex;
            }
        }

        private void bSearch_Click(object sender, System.EventArgs e)
        {
            // Check if user forgot specify keyword
            if (comboSearchText.Text.Length <= 0)
            {
                MessageBox.Show("Specify search keyword(s) first!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboSearchText.Focus();
                return;
            }
            // perform searching
            SetEnabledToSearchControls(false);
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            // Clear previous results
            DataGridView.Rows.Clear();
            // Save search criteria to history
            AddRecordToHistory();
            // Search
            string[] criteria = new string[] { comboSearchText.Text }; ;
            if (comboSearchText.Text.Contains(";"))
            {
                criteria = comboSearchText.Text.Split(';');
            }
            else
            {
                if (comboSearchText.Text.Contains(","))
                {
                    criteria = comboSearchText.Text.Split(',');
                }
            }
            // Remove trailing white spaces
            for (int a=0; a<criteria.Length;a++)
            {
                criteria[a] = criteria[a].Trim();
            }

            List<FR> FRs = FRSearch.Search((FRSearch.SearchBy)comboSearchBy.SelectedIndex, 
                criteria, checkBox1.Checked);
            // If FRs is null then no xls files are found
            // and reported by search engine
            if (FRs != null)
            {
                // If nothing is found
                if (FRs.Count <= 0)
                {
                    MessageBox.Show("Nothing is found!", "RQS",
                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Display results
                    for (int a = 0; a < FRs.Count; a++)
                    {
                        DataGridView.Rows.Add(
                            (a + 1).ToString(),
                            FRs[a].FRSource,
                            FRs[a].FRID,
                            FRs[a].FRTMSTask,
                            FRs[a].FRText,
                            FRs[a].CCP,
                            FRs[a].Created,
                            FRs[a].Modified,
                            FRs[a].Created.Length > 0 && FRs[a].Modified.Length > 0
                                ? !FRs[a].Created.Equals(FRs[a].Modified) : false);
                    }
                }
            }

            this.Cursor = Cursors.Arrow;
            SetEnabledToSearchControls(true);
        }

        private void AddRecordToHistory()
        {
            for (int a = 0; a < comboSearchText.Items.Count; a++)
            {
                if (comboSearchText.Items[a].ToString() == comboSearchText.Text)
                {
                    return;
                }
            }
            comboSearchText.Items.Insert(0, comboSearchText.Text);
            ClientParams.Parameters.SearchHistory = new string[comboSearchText.Items.Count];
            for (int a = 0; a < comboSearchText.Items.Count; a++)
            {
                ClientParams.Parameters.SearchHistory[a] = comboSearchText.Items[a].ToString();
            }
        }

        private void comboSearchBy_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    bSearch_Click(this, null);
                    break;
                case Keys.Up:
                    // Change 'Search by' by Up
                    if (!comboSearchText.DroppedDown)
                    {
                        if (comboSearchBy.SelectedIndex > 0)
                        {
                            comboSearchBy.SelectedIndex--;
                        }
                        HandledKeyDown = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.Down:
                    // Change 'Search by' by Down
                    if (!comboSearchText.DroppedDown)
                    {
                        if (comboSearchBy.SelectedIndex < comboSearchBy.Items.Count - 1)
                        {
                            comboSearchBy.SelectedIndex++;
                        }
                        HandledKeyDown = true;
                        e.Handled = true;
                    }
                    break;
                case Keys.Space:
                    // Open/close search history by Ctrl+Space
                    if (e.Modifiers == Keys.Control)
                    {
                        comboSearchText.DroppedDown = !comboSearchText.DroppedDown; ;
                        HandledKeyDown = true;
                    }
                    break;
            }
        }

        private void comboSearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = HandledKeyDown;
            if (HandledKeyDown)
            {
                HandledKeyDown = !HandledKeyDown;
            }
        }

        private void contextCopyCell_Click(object sender, System.EventArgs e)
        {
            if (LastMouseDownLocation[0] >= 0 &&
                LastMouseDownLocation[1] >= 0 &&
                DataGridView.Rows.Count >= LastMouseDownLocation[0] &&
                DataGridView.Columns.Count >= LastMouseDownLocation[1])
            {
                Clipboard.SetText(DataGridView.Rows[LastMouseDownLocation[0]].
                    Cells[LastMouseDownLocation[1]].Value.ToString());
            }
        }

        void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Clipboard.SetText(DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
        }

        private void contextCopyRows_Click(object sender, System.EventArgs e)
        {
            string text = "";
            foreach (DataGridViewRow row in DataGridView.SelectedRows)
            {
                for (int a = 0; a < DataGridView.Columns.Count; a++)
                {
                    if (!DataGridView.Columns[a].Visible)
                    {
                        continue;
                    }
                    for (int b = 0; b < DataGridView.Columns.Count; b++)
                    {
                        if (DataGridView.Columns[b].DisplayIndex == a)
                        {
                            text += row.Cells[b].Value.ToString() + ";";
                        }
                    }
                }
                text += Environment.NewLine;
            }
            Clipboard.SetText(text);
        }

        private void contextCustomize_Click(object sender, EventArgs e)
        {
            DataGridView.ShowSetup(this.ParentForm);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextCopyCell.Enabled = DataGridView.SelectedRows.Count > 0;
            contextCopyRows.Enabled = DataGridView.SelectedRows.Count > 0;
            contextOpenSourceFile.Enabled = DataGridView.SelectedRows.Count > 0;
        }

        private void contextOpenSourceFile_Click(object sender, EventArgs e)
        {
            if (LastMouseDownLocation[0] >= 0 &&
                LastMouseDownLocation[1] >= 0 &&
                DataGridView.Rows.Count >= LastMouseDownLocation[0] &&
                DataGridView.Columns.Count >= LastMouseDownLocation[1] &&
                DataGridView.Rows[LastMouseDownLocation[0]].Cells[1].Value != null)
            {
                Process.Start(ClientParams.Parameters.XLSLocation + "\\" +
                    DataGridView.Rows[LastMouseDownLocation[0]].Cells[1].Value.ToString());
            }
        }

        private void SetEnabledToSearchControls(bool Enabled)
        {
            comboSearchBy.Enabled = Enabled;
            comboSearchText.Enabled = Enabled;
            bSearch.Enabled = Enabled;
            checkBox1.Enabled = Enabled;
        }

        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationRemoveDuplication();
        }

        // Compare and remove duplication in search results
        // Compare is performed by FR ID and FR TEXT
        private void OperationRemoveDuplication()
        {
            if (DataGridView.Rows.Count > 0)
            {
                int DeletedRowsCount = 0;
                for (int a = 0; a < DataGridView.Rows.Count; a++)
                {
                    if (DataGridView.Rows.Count >= a)
                    {
                        for (int b = 0; b < DataGridView.Rows.Count; b++)
                        {
                            if (a == b)
                            {
                                continue;
                            }
                            if (DataGridView.Rows[a].Cells[2].Value.Equals(DataGridView.Rows[b].Cells[2].Value) ||
                                DataGridView.Rows[a].Cells[4].Value.Equals(DataGridView.Rows[b].Cells[4].Value))
                            {
                                DataGridView.Rows.RemoveAt(b);
                                DeletedRowsCount++;
                            }
                        }
                    }
                }
                MessageBox.Show(string.Format("{0} rows were removed because of duplication.", DeletedRowsCount), "RQS",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No search results to process!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

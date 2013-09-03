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
using System.Diagnostics;
using System.Drawing;
using RQS.SmartComponents;
using RQS.Logic;

namespace RQS.GUI
{
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
        }

        public int SearchResultsCount 
        { 
            get 
            { 
                return ClientParams.Parameters.AutofilterEnabled ? 
                    DataGridView.Rows.Count -1 : 
                    DataGridView.Rows.Count; 
            } 
        }

        public event SearchInProgress backgroundWorkInProgress;
        public delegate void SearchInProgress();
        public event SearchComplete backgroundWorkComplete;
        public delegate void SearchComplete();
        public event SearchResultsAdded searchResultsAdded;
        public delegate void SearchResultsAdded();

        private FRSearch FRSearch = new FRSearch();
        private SmartDataGridView DataGridView = new SmartDataGridView("SearchResults", 10);
        private int[] LastMouseDownLocation = new int[] { 0, 0 };
        private SmartDataGridViewColumnSorter ColumnSorter;
        private bool HandledKeyDown = false;
        private bool RequestCancel = false;
        private string AutofilterLastKey = null;

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

            DataGridView.Columns.Add("", "Status");
            DataGridView.HideColumnByDefault(9);

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            DataGridView.MouseDown += new MouseEventHandler(DataGridView_MouseDown);
            DataGridView.EditingControlShowing += 
                new DataGridViewEditingControlShowingEventHandler(DataGridView_EditingControlShowing);

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
            // If cancel search is requested
            if (RequestCancel)
            {
                FRSearch.RequestCancel = true;
                backgroundWorker1.CancelAsync();
                DisableInputControls(true);
                this.Cursor = Cursors.Arrow;
                return;
            }
            // If search is requested
            // Check if user forgot specify keyword
            if (comboSearchText.Text.Length <= 0)
            {
                MessageBox.Show("Specify search keyword(s) first!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboSearchText.Focus();
                return;
            }
            // Clear previous results
            DataGridView.Rows.Clear();
            // Save search criteria to history
            AddRecordToHistory();
            // Prepare search keywords
            string[] criteria = new string[] { comboSearchText.Text };
            // Detect several keywords splitted by ';' or ',' or '-'
            // Priorities for splitting will be the following: ',;-'
            if (comboSearchText.Text.Contains(";"))
            {
                criteria = comboSearchText.Text.Split(';');
            }
            if (comboSearchText.Text.Contains(","))
            {
                criteria = comboSearchText.Text.Split(',');
            }
            if ((FRSearch.SearchBy)comboSearchBy.SelectedIndex == FRSearch.SearchBy.FR_ID)
            {
                // Navigate over initial search array
                for (int a = 0; a < criteria.Length; a++)
                {
                    if (criteria[a].Contains("-"))
                    {
                        // Explode the diapason
                        string[] criterias = ExplodeCriterias(criteria[a]);
                        if (criterias != null)
                        {
                            string[] oldArray = criteria;
                            // -1 because one element of the initial array should be
                            //  deleted and replaced by exploded array
                            criteria = new string[criteria.Length + criterias.Length - 1];
                            // Navigate over new search array
                            for (int b = 0; b < criteria.Length; b++)
                            {
                                switch (b.CompareTo(a))
                                {
                                    // if b < a
                                    case -1:
                                        criteria[b] = oldArray[b];
                                        continue;
                                    // if b == a
                                    case 0:
                                        // Navigate over exploded array
                                        for (int c = 0; c < criterias.Length; c++)
                                        {
                                            criteria[b + c] = criterias[c];
                                        }
                                        // -1 because 'for' statement will do ++
                                        b = b + criterias.Length - 1;
                                        continue;
                                    // if b > a
                                    case 1:
                                        criteria[b] = oldArray[a + 1];
                                        a++;
                                        continue;
                                }
                            }
                            a = 0;
                        }
                    }
                }
            }
            // Remove white spaces
            for (int a=0; a<criteria.Length;a++)
            {
                criteria[a] = criteria[a].Trim();
            }
            // Search
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("Please wait until previous searching will be canceled.", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // All OK, disable contols and perform searching
                DisableInputControls(false);
                this.Cursor = Cursors.WaitCursor;

                if (backgroundWorkInProgress != null)
                {
                    backgroundWorkInProgress();
                }
                SearchOptions options = new SearchOptions();
                options.SearchCriteria = criteria;
                options.SearchBy = (FRSearch.SearchBy)comboSearchBy.SelectedIndex;
                options.LimitResults = checkBox1.Checked;
                backgroundWorker1.RunWorkerAsync(options);
            }
        }

        // Perform searching in the second thread
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SearchOptions options = (SearchOptions)e.Argument;
            // Search
            List<FR> FRs = FRSearch.Search(options.SearchBy,
                options.SearchCriteria, options.LimitResults);
            // If user cancel operation
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
            // If FRs is null then no xls files are found
            // and reported by search engine
            if (FRs != null)
            {
                e.Result = FRs;
                return;
            }
            e.Result = null;
        }

        // Display search results
        // after second thread is finished
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // Report about operation is completed
            if (backgroundWorkComplete != null)
            {
                backgroundWorkComplete();
            }
            // Do not display results if operation is canceled
            if (e.Cancelled)
            {
                return;
            }
            if (e.Result == null)
            {
                // If no .xls files to search in
                // null is reported by search engine
                MessageBox.Show("No .xls files found!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<FR> FRs = (List<FR>)e.Result;
                if (FRs.Count > 0)
                {
                    //Display results
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
                                ? !FRs[a].Created.Equals(FRs[a].Modified) : false,
                            FRs[a].Status);
                    }

                    if (ClientParams.Parameters.AutofilterEnabled)
                    {
                        // Add autofilter
                        AddAutoFilter();
                    }
                }
                else
                {
                    // If nothing is found
                    MessageBox.Show("Nothing is found!", "RQS",
                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Report about search results are displayed
            if (searchResultsAdded != null)
            {
                searchResultsAdded();
            }

            DisableInputControls(true);
            this.Cursor = Cursors.Arrow;
            comboSearchText.Focus();
        }

        // Converts keywords like FR001-005
        // to array with FR001, FR002, FR003, FR004, FR005
        private string[] ExplodeCriterias(string str)
        {
            // Check argument
            if (str == null || !str.Contains("-"))
            {
                return null;
            }
            // Get 1st and 2nd numbers
            string[] Input = str.Split('-');
            if (Input.Length != 2)
            {
                return null;
            }
            // Leave only numbers in Input string
            int _temp;
            string prefix = ""; // will store FR or NFR
            for (int a = 0; a < Input.Length; a++)
            {
                for (int b = 0; b < Input[a].Length; b++)
                {
                    if (Input[a].Length > b &&
                        !int.TryParse(Input[a].Substring(b, 1), out _temp))
                    {
                        if (a == 0)
                        {
                            // Store prefix only from 1st word
                            prefix += Input[a].Substring(b, 1);
                        }
                        Input[a] = Input[a].Remove(b, 1);
                        b--;
                    }
                }
            }
            // Get numbers
            int NumberSize = Input[0].Length;
            int Number1;
            int Number2;
            if (!int.TryParse(Input[0], out Number1) ||
                !int.TryParse(Input[1], out Number2))
            {
                return null;
            }
            // Check numbers
            if (Input[0].Length > Input[1].Length ||
                Number1 > Number2)
            {
                return null;
            }
            // Prevent working with big diapason
            int MaxDiapason = 5000;
            if (Number2 - Number1 > MaxDiapason)
            {
                MessageBox.Show(string.Format("Diapason over {0} is prohibited and will be threated as a word!", MaxDiapason), 
                    "RQS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            // Prepare prefix
            prefix = prefix.Trim().ToLower(); // will store 'fr' or 'nfr'
            // Generate diapason
            string[] result = new string[Number2-Number1+1];
            for (int a = 0; a <= Number2 - Number1; a++)
            {
                result[a] = (Number1 + a).ToString("D" + NumberSize.ToString());
                result[a] = prefix + result[a];
            }
            return result;
        }

        private void AddAutoFilter()
        {
            DataGridViewRow filterRow = new DataGridViewRow();
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());
            filterRow.Cells.Add(new DataGridViewComboBoxCell());

            filterRow.Frozen = true;

            for (int column = 0; column < DataGridView.Columns.Count; column++)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)filterRow.Cells[column];
                cell.Items.Add("");

                for (int row = 0; row < DataGridView.Rows.Count; row++)
                {
                    if (cell.Items.IndexOf(DataGridView.Rows[row].Cells[column].Value.ToString()) < 0)
                    {
                        cell.Items.Add(DataGridView.Rows[row].Cells[column].Value.ToString());
                    }
                }
            }

            DataGridView.Rows.Insert(0, filterRow);
            DataGridView.Rows[0].ReadOnly = false;
        }

        void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox autofilter = e.Control as ComboBox;
            if (autofilter != null)
            {
                autofilter.SelectedIndexChanged -= new EventHandler(autofilter_SelectedIndexChanged);
                autofilter.SelectedIndexChanged += new EventHandler(autofilter_SelectedIndexChanged);
            }
        }

        void autofilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string value = cb.Text;
            if (value != null)
            {
                DoFilter(DataGridView.CurrentCell.ColumnIndex, value);
            }
        }

        private void DoFilter(int columnIndex, string value)
        {
            // We have a problem here with grid refreshing, so there is a fix.
            // This fix is peace of shit! It works for single column only. Probably,
            //   some users will not find catch in it.
            // TODO: Remove this shit!
            if (value == AutofilterLastKey)
            {
                // Sometimes the best way do nothing! :)
                return;
            }
            else
            {
                AutofilterLastKey = value;
            }

            // First of all display all rows
            for (int row = 0; row < DataGridView.Rows.Count; row++)
            {
                DataGridView.Rows[row].Visible = true;
            }

            // Value that autofilter should left in grid
            string valuetofilter = null;

            for (int column = 0; column < DataGridView.Columns.Count; column++)
            {
                // Determine value selected in filter
                // If value was just selected than take it from param
                //   if not - take it from grid
                if (column == columnIndex)
                {
                    valuetofilter = value;
                }
                else
                {
                    valuetofilter = DataGridView.Rows[0].Cells[column].Value != null ?
                        DataGridView.Rows[0].Cells[column].Value.ToString() : null;
                }

                if (valuetofilter == null || valuetofilter.Equals(""))
                {
                    // Nothing to do so
                    continue;
                }

                // Go over all rows in grid
                for (int row = 1; row < DataGridView.Rows.Count; row++)
                {
                    if (!DataGridView.Rows[row].Cells[column].Value.ToString().Equals(valuetofilter))
                    {
                        DataGridView.Rows[row].Visible = false;
                    }
                }
            }
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
            if (HandledKeyDown)
            {
                e.Handled = true;
                HandledKeyDown = false;
            }
        }

        private void contextCopyCell_Click(object sender, System.EventArgs e)
        {
            if (LastMouseDownLocation[0] >= 0 &&
                LastMouseDownLocation[1] >= 0 &&
                DataGridView.Rows.Count >= LastMouseDownLocation[0] &&
                DataGridView.Columns.Count >= LastMouseDownLocation[1])
            {
                if (DataGridView.Rows[LastMouseDownLocation[0]].
                    Cells[LastMouseDownLocation[1]].Value.ToString() != "")
                {
                    Clipboard.SetText(DataGridView.Rows[LastMouseDownLocation[0]].
                        Cells[LastMouseDownLocation[1]].Value.ToString());
                }
            }
        }

        void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null &&
                    DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    Clipboard.SetText(DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
            }
        }

        private void contextCopyRows_Click(object sender, System.EventArgs e)
        {
            string text = "";
            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                if (!row.Selected)
                {
                    continue;
                }
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
                            text += row.Cells[b].Value.ToString() + "   ";
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
            contextCopyCell.Enabled = DataGridView.SelectedRows.Count > 0 && !DataGridView.SelectedRows.Contains(DataGridView.Rows[0]);
            contextCopyRows.Enabled = DataGridView.SelectedRows.Count > 0 && !DataGridView.SelectedRows.Contains(DataGridView.Rows[0]);
            contextOpenSourceFile.Enabled = DataGridView.SelectedRows.Count > 0 && !DataGridView.SelectedRows.Contains(DataGridView.Rows[0]);
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

        private void DisableInputControls(bool Enabled)
        {
            // Enable/disable input controls
            comboSearchBy.Enabled = Enabled;
            comboSearchText.Enabled = Enabled;
            checkBox1.Enabled = Enabled;
            // Rename search button
            if (Enabled)
            {
                bSearch.Text = "Search";
                RequestCancel = false;
            }
            else
            {
                bSearch.Text = "Cancel";
                RequestCancel = true;
            }
        }

        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationRemoveDuplication();
        }

        // Compare and remove duplication in search results
        // Compare is performed by FR ID and FR TEXT
        private void OperationRemoveDuplication()
        {
            int firstrow = ClientParams.Parameters.AutofilterEnabled ? 1 : 0;
            if (DataGridView.Rows.Count > 0)
            {
                int DeletedRowsCount = 0;
                for (int a = firstrow; a < DataGridView.Rows.Count; a++)
                {
                    if (DataGridView.Rows.Count >= a)
                    {
                        for (int b = firstrow; b < DataGridView.Rows.Count; b++)
                        {
                            if (a == b)
                            {
                                continue;
                            }
                            if (DataGridView.Rows[a].Cells[2].Value.Equals(DataGridView.Rows[b].Cells[2].Value) &&
                                DataGridView.Rows[a].Cells[4].Value.Equals(DataGridView.Rows[b].Cells[4].Value))
                            {
                                DataGridView.Rows.RemoveAt(b);
                                DeletedRowsCount++;
                            }
                        }
                    }
                }

                if (DeletedRowsCount > 0)
                {
                    MessageBox.Show(string.Format("{0} rows were removed because of duplication.", DeletedRowsCount), "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No duplication found!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No search results to process!", "RQS",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void removeSelectedLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationRemoveSelected();
        }

        // Remove selected rows from DataGridView
        private void OperationRemoveSelected()
        {
            for (int a = 1; a < DataGridView.Rows.Count; a++)
            {
                if (DataGridView.Rows[a].Selected)
                {
                    DataGridView.Rows.RemoveAt(a);
                    if (a != DataGridView.Rows.Count - 1)
                    {
                        a--;
                    }
                }
            }
        }
    }
}

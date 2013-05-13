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
using System.Windows.Forms;
using System.Drawing;
using ItWorksTeam.IO;

namespace RQS.Logic
{
    // SmartDataGridView class implement SmartDataGridView control default settings set, saving/restoring
    // columns width from box.
    // NOTE:
    // 1. each copy of SmartDataGridView should have unique name "SmartDataGridViewUniqueName" which will be
    //    used as key name in registry;
    // 2. Please specify parent control for you copy of SmartDataGridView as last step (before all headers
    //    will be added to avoid issues.
    internal class SmartDataGridView : DataGridView
    {
        // This name is used to save and load data
        // from registry by using unique key name
        public string SmartDataGridViewUniqueName = "";

        private Registry Registry = new Registry();
        private string RegistryPath = "Software\\ItWorksTeam\\RQS\\Grids";

        private int ColumnsExpectedCount = 0;
        private bool[] ColumnsVisibility = null;
        private int[] ColumnsOrder = null;
        private int[] ColumnsWidth = null;
        private int RowsHeight = 22;
        private bool ColumnsVisibilityWasLoaded = false;

        private SmartDataGridViewSetupColumns setup;

        // Constructor
        public SmartDataGridView(string UniqueName, int ExpectedColumnsCount)
        {
            // Set needed parameters
            this.MultiSelect = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = true;
            this.AllowUserToResizeColumns = true;
            this.AllowUserToResizeRows = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ReadOnly = true;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowHeadersVisible = false;
            this.EditMode = DataGridViewEditMode.EditOnEnter;

            // Save specified unique name
            SmartDataGridViewUniqueName = UniqueName;
            // Save expected columns count
            ColumnsExpectedCount = ExpectedColumnsCount;

            ReadColumnsVisibility();
            ReadColumnsWidth();
            ReadColumnsOrder();
            ReadRowsHeight();

            // Add events
            this.ColumnAdded += new DataGridViewColumnEventHandler(SmartDataGridView_ColumnAdded);
            this.RowsAdded += new DataGridViewRowsAddedEventHandler(SmartDataGridView_RowsAdded);
            this.ColumnWidthChanged += new DataGridViewColumnEventHandler(SmartDataGridView_ColumnWidthChanged);
            this.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(SmartDataGridView_ColumnDisplayIndexChanged);
            this.CellFormatting += new DataGridViewCellFormattingEventHandler(SmartDataGridView_CellFormatting);
            this.RowHeightChanged += new DataGridViewRowEventHandler(SmartDataGridView_RowHeightChanged);
            this.HandleDestroyed += new EventHandler(SmartDataGridView_HandleDestroyed);
        }

        void SmartDataGridView_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            RowsHeight = e.Row.Height;
            foreach (DataGridViewRow row in this.Rows)
            {
                row.Height = e.Row.Height;
            }
        }

        void SmartDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
            {
                e.CellStyle.BackColor = ClientParams.Parameters.ColoredLinesColor2;
            }
            else
            {
                e.CellStyle.BackColor = ClientParams.Parameters.ColoredLinesColor1;
            }
        }

        #region Registry
        // Read columns visibility
        private void ReadColumnsVisibility()
        {
            bool allOK = true;
            string ColumnsVisibilityValue =
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Visibility"), null);
            if (ColumnsVisibilityValue != null &&
                ColumnsVisibilityValue != "" &&
                ColumnsVisibilityValue.Length == ColumnsExpectedCount)
            {
                ColumnsVisibility = new bool[ColumnsExpectedCount];
                int columnVisibility = 0;
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    if (int.TryParse(ColumnsVisibilityValue.Substring(a, 1), out columnVisibility))
                    {
                        ColumnsVisibility[a] = (columnVisibility == 1);
                    }
                    else
                    {
                        allOK = false;
                    }
                }
            }
            else
            {
                allOK = false;
            }
            if (!allOK)
            {
                ColumnsVisibility = new bool[ColumnsExpectedCount];
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    ColumnsVisibility[a] = true;
                }
            }
            ColumnsVisibilityWasLoaded = allOK;
        }

        // Read columns width
        private void ReadColumnsWidth()
        {
            bool allOK = true;
            string ColumnsWidthValue =
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Width"), null);
            if (ColumnsWidthValue != null &&
                ColumnsWidthValue != "" &&
                ColumnsWidthValue.Length % 3 == 0 &&
                ColumnsWidthValue.Length / 3 == ColumnsExpectedCount)
            {
                ColumnsWidth = new int[ColumnsExpectedCount];
                int columnWidth = 0;
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    if (int.TryParse(ColumnsWidthValue.Substring(a * 3, 3), out columnWidth))
                    {
                        ColumnsWidth[a] = columnWidth;
                    }
                    else
                    {
                        allOK = false;
                    }
                }
            }
            else
            {
                allOK = false;
            }
            if (!allOK)
            {
                ColumnsWidth = new int[ColumnsExpectedCount];
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    ColumnsWidth[a] = 200;
                }
            }
        }

        // Read columns order
        private void ReadColumnsOrder()
        {
            bool allOK = true;
            string ColumnsOrderValue =
                Registry.ReadKey<string>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Order"), null);
            if (ColumnsOrderValue != null &&
                ColumnsOrderValue != "" &&
                ColumnsOrderValue.Length % 2 == 0 &&
                ColumnsOrderValue.Length / 2 == ColumnsExpectedCount)
            {
                ColumnsOrder = new int[ColumnsExpectedCount];
                int columnOrder = 0;
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    if (int.TryParse(ColumnsOrderValue.Substring(a * 2, 2), out columnOrder))
                    {
                        ColumnsOrder[a] = columnOrder;
                    }
                    else
                    {
                        allOK = false;
                    }
                }
            }
            else
            {
                allOK = false;
            }
            if (!allOK)
            {
                ColumnsOrder = new int[ColumnsExpectedCount];
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    ColumnsOrder[a] = a;
                }
            }
        }

        // Read rows height
        private void ReadRowsHeight()
        {
            RowsHeight =
                Registry.ReadKey<int>(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "RowsHeight"), 22);
        }

        // Save columns width
        private void SaveColumnsVisibility()
        {
            string KeyValue = "";
            foreach (bool value in ColumnsVisibility)
            {
                if (value)
                {
                    KeyValue = string.Concat(KeyValue, "1");
                }
                else
                {
                    KeyValue = string.Concat(KeyValue, "0");
                }
            }
            Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Visibility"), KeyValue);
        }

        // Save columns width
        private void SaveColumnsWidth()
        {
            string KeyValue = "";
            foreach (int value in ColumnsWidth)
            {
                KeyValue = string.Concat(KeyValue, value.ToString("D3"));
            }
            Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Width"), KeyValue);
        }

        // Save columns width
        private void SaveColumnsOrder()
        {
            string KeyValue = "";
            foreach (int value in ColumnsOrder)
            {
                KeyValue = string.Concat(KeyValue, value.ToString("D2"));
            }
            Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "Order"), KeyValue);
        }

        // Save rows height
        private void SaveRowsHeight()
        {
            if (this.Rows.Count > 0 &&
                this.Rows[0].Height > 0)
            {
                Registry.SaveKey(Registry.BaseKeys.HKEY_CURRENT_USER,
                RegistryPath, string.Concat(SmartDataGridViewUniqueName, "RowsHeight"), this.Rows[0].Height);
            }
        }
        #endregion

        #region Runtime
        // apply width to all columns
        private void SetColumnsWidth()
        {
            for (int a = 0; a < ColumnsExpectedCount; a++)
            {
                this.Columns[a].Width = ColumnsWidth[a];
            }
        }

        // apply visibility for all columns
        private void SetColumnsVisibility()
        {
            for (int a = 0; a < ColumnsExpectedCount; a++)
            {
                this.Columns[a].Visible = ColumnsVisibility[a];
            }
        }

        // apply columns order for all columns
        private void SetColumnsOrder()
        {
            this.ColumnDisplayIndexChanged -=new DataGridViewColumnEventHandler(SmartDataGridView_ColumnDisplayIndexChanged);
            for (int a = 0; a < ColumnsExpectedCount; a++)
            {
                this.Columns[a].DisplayIndex = ColumnsOrder[a];
            }
            this.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(SmartDataGridView_ColumnDisplayIndexChanged);
        }

        // public method to mark column as not visible by default
        // in case registry does not override this
        public void HideColumnByDefault(int ColumnIndex)
        {
            if (!ColumnsVisibilityWasLoaded && this.Columns.Count > ColumnIndex)
            {
                ColumnsVisibility[ColumnIndex] = false;
            }
        }
        #endregion

        #region Events
        // when new column is added
        void SmartDataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // set default parameters
            e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            e.Column.Resizable = DataGridViewTriState.True;
            // if last column is added perform applying settings
            if (e.Column.Index == (ColumnsExpectedCount - 1))
            {
                SetColumnsWidth();
                SetColumnsOrder();
                SetColumnsVisibility();
            }
            // set word wrap
            e.Column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        // remember new width of column when user change it
        void SmartDataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.Columns.Count == ColumnsExpectedCount)
            {
                ColumnsWidth[e.Column.Index] = e.Column.Width;
            }
        }

        // remember new columns order when user change it
        void SmartDataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (this.Columns.Count == ColumnsExpectedCount)
            {
                for (int a = 0; a < ColumnsExpectedCount; a++)
                {
                    ColumnsOrder[a] = this.Columns[a].DisplayIndex;
                }
            }
        }

        // save all changes when control is beign destroyed
        void SmartDataGridView_HandleDestroyed(object sender, EventArgs e)
        {
            SaveColumnsWidth();
            SaveColumnsOrder();
            SaveColumnsVisibility();
            SaveRowsHeight();
        }

        // when new row is added
        void SmartDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int a = 0; a < e.RowCount; a++)
            {
                this.Rows[a + e.RowIndex].Height = RowsHeight;
            }
        }
        #endregion

        #region Menu
        // user wants to see setup
        public void ShowSetup()
        {
            ShowSetup((Form)this.Parent);
        }

        public void ShowSetup(Form MdiParent)
        {
            setup = new SmartDataGridViewSetupColumns(
                MdiParent,
                this);
            setup.ColumnsVisibilityChanged += new SmartDataGridViewSetupColumns.VisibilityChanged(setup_ColumnsVisibilityChanged);
            setup.ColumnsOrderChanged += new SmartDataGridViewSetupColumns.OrderChanged(setup_ColumnsOrderChanged);
            setup.ShowDialog();
        }

        void setup_ColumnsOrderChanged()
        {
            for (int a = 0; a < this.Columns.Count; a++)
            {
                ColumnsOrder[a] = this.Columns[a].DisplayIndex;
            }
            SetColumnsOrder();
        }

        void setup_ColumnsVisibilityChanged()
        {
            for (int a = 0; a < this.Columns.Count; a++)
            {
                ColumnsVisibility[a] = this.Columns[a].Visible;
            }
            SetColumnsVisibility();
        }
        #endregion
    }
}

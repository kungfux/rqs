/*   
 ****************************************************************
 * This document contains unpublished, confidential and
 * proprietary information of IT WORKS Team. No disclosure or use
 * of any portion of the contents of these materials may be made
 * without the express written consent of IT WORKS Team's leader.
 ****************************************************************
 */

/*
 * This file is a part of "Bestfish .NET" project
 * Copyright (C) 2012-2013 IT WORKS Team
 */

using System;
using System.Windows.Forms;
using System.Drawing;

namespace RQS
{
    internal class SmartDataGridViewSetupColumns : Form
    {
        private CheckedListBox columns = new CheckedListBox();
        private SmartDataGridView grid;

        public SmartDataGridViewSetupColumns(
            Form MdiParent, 
            SmartDataGridView grid)
        {
            this.grid = grid;

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(MdiParent.Left + MdiParent.Width / 2 - this.Width / 2,
                MdiParent.Top + MdiParent.Height / 2 - this.Height / 2);
            this.Text = "Customize...";

            columns.Dock = DockStyle.Fill;
            RefreshList();
            columns.ItemCheck += new ItemCheckEventHandler(columns_ItemCheck);

            GroupBox box = new GroupBox();
            box.Text = "";
            box.Dock = DockStyle.Fill;
            box.Controls.Add(columns);

            Button OK = new Button();
            OK.Text = "Close";
            OK.TabIndex = 2;
            OK.Click += new EventHandler(OK_Click);

            Button Up = new Button();
            Up.Text = "Up";
            Up.TabIndex = 0;
            Up.Click += new EventHandler(Up_Click);

            Button Down = new Button();
            Down.Text = "Down";
            Down.TabIndex = 1;
            Down.Click += new EventHandler(Down_Click);

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.ColumnCount = 2;
            panel.RowCount = 2;
            panel.ColumnStyles.Clear();
            panel.RowStyles.Clear();
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Panel buttons = new Panel();
            buttons.Dock = DockStyle.Fill;
            buttons.Controls.Add(Down);
            buttons.Controls.Add(Up);
            buttons.Controls.Add(OK);
            Up.Dock = DockStyle.Top;
            Down.Dock = DockStyle.Top;
            OK.Dock = DockStyle.Bottom;

            panel.Controls.Add(box, 0, 0);
            panel.Controls.Add(buttons, 1, 0);

            this.Controls.Add(panel);

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(SmartDataGridViewSetupColumns_KeyDown);
        }

        void SmartDataGridViewSetupColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void RefreshList()
        {
            string selectedColumn = null;
            if (columns.SelectedIndex != -1)
            {
                selectedColumn = columns.SelectedItem.ToString();
            }
            columns.Items.Clear();
            for (int a = 0; a < grid.Columns.Count; a++)
            {
                int column = getColumnByDisplayIndex(a);
                columns.Items.Add(grid.Columns[column].HeaderText, grid.Columns[column].Visible);
            }
            if (selectedColumn != null)
            {
                columns.SelectedItem = selectedColumn;
            }
        }

        private int getColumnByDisplayIndex(int DisplayIndex)
        {
            for (int a = 0; a < grid.Columns.Count; a++)
            {
                if (grid.Columns[a].DisplayIndex == DisplayIndex)
                {
                    return a;
                }
            }
            return -1;
        }

        void Down_Click(object sender, EventArgs e)
        {
            if (columns.SelectedIndex == -1 || columns.SelectedIndex  >= columns.Items.Count - 1)
                return;

            columns.ItemCheck -= columns_ItemCheck;

            grid.Columns[getColumnIndex(columns.Items[columns.SelectedIndex].ToString())].DisplayIndex += 1;
            RefreshList();
            
            columns.ItemCheck +=new ItemCheckEventHandler(columns_ItemCheck);

            if (ColumnsOrderChanged != null)
            {
                ColumnsOrderChanged();
            }
        }

        void Up_Click(object sender, EventArgs e)
        {
            if (columns.SelectedIndex <= 0)
                return;

            columns.ItemCheck -= columns_ItemCheck;

            grid.Columns[getColumnIndex(columns.Items[columns.SelectedIndex].ToString())].DisplayIndex -= 1;
            RefreshList();

            columns.ItemCheck += new ItemCheckEventHandler(columns_ItemCheck);

            if (ColumnsOrderChanged != null)
            {
                ColumnsOrderChanged();
            }
        }

        void columns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            grid.Columns[getColumnIndex(columns.Items[e.Index].ToString())].Visible = e.NewValue == CheckState.Checked;
            if (ColumnsVisibilityChanged != null)
            {
                ColumnsVisibilityChanged();
            }
        }

        private int getColumnIndex(string Header)
        {
            for (int a = 0; a < grid.Columns.Count; a++)
            {
                if (grid.Columns[a].HeaderText == Header)
                    return a;
            }
            return -1;
        }

        public event VisibilityChanged ColumnsVisibilityChanged;
        public delegate void VisibilityChanged();
        public event OrderChanged ColumnsOrderChanged;
        public delegate void OrderChanged();

        void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

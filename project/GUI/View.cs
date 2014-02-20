using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RQS.Logic;

namespace RQS.GUI
{
    public partial class View : Form
    {
        private DataGridView grid;
        int row;
        string[] searchcriteria;

        public View(DataGridView pgrid, int prow, string[] psearchcriteria)
        {
            InitializeComponent();

            grid = pgrid;
            row = prow;
            searchcriteria = psearchcriteria;

            ShowFR();
        }

        private void ShowFR()
        {
            // Limit row index by 0 or 1 in case autofilter is enabled
            int filter = ClientParams.Parameters.AutofilterEnabled ? 1 : 0;
            int rowscount = 0;
            for (int a = filter; a < grid.Rows.Count; a++)
            {
                if (grid.Rows[a].Visible)
                {
                    rowscount++;
                }
            }

            if (row < 0 + filter)
            {
                row = 0 + filter;
            }
            if (row > rowscount - filter)
            {
                row = rowscount - filter;
            }
            // Display selected row only if it is visible
            if (grid.Rows[row].Visible)
            {
                txtFRID.Text = grid.Rows[row].Cells[2].Value.ToString();
                txtTMSTask.Text = grid.Rows[row].Cells[3].Value.ToString();
                richTextBox1.Text = grid.Rows[row].Cells[4].Value.ToString();
            }
            else
            {
                row++;
                ShowFR();
            }
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Close by Esc
                case Keys.Escape:
                    this.Close();
                    break;
                // Move between requirements
                case Keys.Left:
                    row--;
                    ShowFR();
                    break;
                case Keys.Up:
                    row--;
                    ShowFR();
                    break;
                case Keys.Right:
                    row++;
                    ShowFR();
                    break;
                case Keys.Down:
                    row++;
                    ShowFR();
                    break;
                case Keys.Home:
                    row = 0;
                    ShowFR();
                    break;
                case Keys.End:
                    row = grid.Rows.Count;
                    ShowFR();
                    break;
                case Keys.Delete:
                    grid.Rows[row].Visible = false;
                    ShowFR();
                    break;
            }
        }
    }
}

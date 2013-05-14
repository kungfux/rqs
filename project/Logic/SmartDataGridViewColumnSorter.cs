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

using System.Collections;
using System.Windows.Forms;
using System;

namespace RQS.Logic
{
    internal class SmartDataGridViewColumnSorter
    {
        public SmartDataGridViewColumnSorter(DataGridView DataGridView)
        {
            DataGridView.SortCompare += new DataGridViewSortCompareEventHandler(DataGridView_SortCompare);
        }

        void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            switch (e.Column.Index)
            {
                case 0:
                    e.SortResult = CompareInt32(e.CellValue1, e.CellValue2);
                    e.Handled = true;
                    break;
                case 6:
                    e.SortResult = CompareDateTime(e.CellValue1, e.CellValue2);
                    e.Handled = true;
                    break;
                case 7:
                    e.SortResult = CompareDateTime(e.CellValue1, e.CellValue2);
                    e.Handled = true;
                    break;
            }
        }

        private int CompareDateTime(object value1, object value2)
        {
            DateTime a;
            DateTime b;

            if (DateTime.TryParse(value1.ToString(), out a) &&
                DateTime.TryParse(value2.ToString(), out b))
            {
                return a.CompareTo(b);
            }
            else
            {
                if (value1.Equals("") & value2.Equals(""))
                {
                    return 0;
                }
                if (value1.Equals("") & !value2.Equals(""))
                {
                    return -1;
                }
                if (!value1.Equals("") & value2.Equals(""))
                {
                    return 1;
                }
                return 0;
            }
        }

        private int CompareInt32(object value1, object value2)
        {
            int a;
            int b;

            if (Int32.TryParse(value1.ToString(), out a) && 
                Int32.TryParse(value2.ToString(), out b))
            {
                return a.CompareTo(b);
            }
            else
            {
                return 0;
            }
        }
    }
}
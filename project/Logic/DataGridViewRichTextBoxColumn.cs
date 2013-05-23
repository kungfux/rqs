/*
 * Original source code is taken from http://www.codeproject.com/Articles/31823/RichTextBox-Cell-in-a-DataGridView
 * Original class is published under CPOL license.
 */

using System.Windows.Forms;
using System;

namespace RQS.Logic
{
    public class DataGridViewRichTextBoxColumn : DataGridViewColumn
    {
        public DataGridViewRichTextBoxColumn()
            : base(new DataGridViewRichTextBoxCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (!(value is DataGridViewRichTextBoxCell))
                    throw new InvalidCastException("CellTemplate must be a DataGridViewRichTextBoxCell");

                base.CellTemplate = value;
            }
        }
    }
}

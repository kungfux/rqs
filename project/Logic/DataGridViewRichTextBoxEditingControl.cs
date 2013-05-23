/*
 * Original source code is taken from http://www.codeproject.com/Articles/31823/RichTextBox-Cell-in-a-DataGridView
 * Original class is published under CPOL license.
 */

using System;
using System.Windows.Forms;
using System.Drawing;

namespace RQS.Logic
{
    internal class DataGridViewRichTextBoxEditingControl : RichTextBox, IDataGridViewEditingControl
    {
        private DataGridView _dataGridView;
        private int _rowIndex;
        private bool _valueChanged;

        public DataGridViewRichTextBoxEditingControl()
        {
            this.BorderStyle = BorderStyle.None;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _valueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys = keyData & Keys.KeyCode;
            if (keys == Keys.Return)
            {
                return this.Multiline;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    // Control + B = Bold
                    case Keys.B:
                        if (this.SelectionFont.Bold)
                        {
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Bold & this.Font.Style);
                        }
                        else
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold | this.Font.Style);
                        break;
                    // Control + U = Underline
                    case Keys.U:
                        if (this.SelectionFont.Underline)
                        {
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Underline & this.Font.Style);
                        }
                        else
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Underline | this.Font.Style);
                        break;
                    // Control + I = Italic
                    // Conflicts with the default shortcut
                    //case Keys.I:
                    //    if (this.SelectionFont.Italic)
                    //    {
                    //        this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Italic & this.Font.Style);
                    //    }
                    //    else
                    //        this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Italic | this.Font.Style);
                    //    break;
                    default:
                        break;
                }
            }
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _dataGridView;
            }
            set
            {
                _dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Rtf;
            }
            set
            {
                if (value is string)
                    this.Text = value as string;
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Return:
                    if ((((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.Shift) && this.Multiline))
                    {
                        return true;
                    }
                    break;
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }

            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return this.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Rtf;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion
    }
}

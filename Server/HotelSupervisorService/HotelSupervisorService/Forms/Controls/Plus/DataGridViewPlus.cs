using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class DataGridViewPlus : DataGridView
    {
        public DataGridViewPlus()
        {
            InitializeComponent();
        }

        public List<int> numberColumns = new List<int>();
        public event EventHandler AddNewRow;
        public event EventHandler ValueChanged;
        private Hashtable htNumberColumnsRightValueBackUp = new Hashtable();
        private Hashtable htCellTextBackUp = new Hashtable();

        public List<int> NumberColumns
        {
            get
            {
                return numberColumns;
            }
            set
            {
                numberColumns = value;
                foreach (int i in numberColumns)
                {
                    if (!htNumberColumnsRightValueBackUp.Contains(i))
                    {
                        htNumberColumnsRightValueBackUp.Add(i, string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 选中某行某列后的第一个字符型单元格。
        /// </summary>
        /// <param name="rowIndex">行号。</param>
        /// <param name="columnIndex">列号。</param>
        /// <returns>字符型单元格。</returns>
        private DataGridViewCell FindNextTextBoxCell(int rowIndex,int columnIndex)
        {
            if (columnIndex >= Columns.Count - 1)
            {
                return null;
            }
            for (int i = columnIndex + 1; i < Columns.Count; i++)
            {
                DataGridViewCell cell = Rows[rowIndex].Cells[i];
                Type type = cell.GetType();
                if (type.FullName == "System.Windows.Forms.DataGridViewTextBoxCell")
                {
                    return cell;
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新一行，并选中新的一行。
        /// </summary>
        public void AddRow()
        {
            Rows.Add(1);
            if (Columns.Count == 0)
            {
                return;
            }
            DataGridViewCell cell = Rows[Rows.Count-1].Cells[0];
            CurrentCell = cell;
            cell = FindNextTextBoxCell(Rows.Count - 1, -1);
            if (cell != null)
            {
                CurrentCell = cell;
            }
        }

        /// <summary>
        /// 选中某行第一个字符型单元格。
        /// </summary>
        /// <param name="rowIndex">行号。</param>
        public void SelectNextTextBoxCell(int rowIndex)
        {
            DataGridViewCell cell = FindNextTextBoxCell(rowIndex, -1);
            if (cell != null)
            {
                CurrentCell = cell;
            }
        }

        /// <summary>
        /// 检查指定列的值是否为空。
        /// </summary>
        /// <param name="columnList">指定的列。</param>
        /// <param name="htExcept">当某列满足某值时，忽略检查。</param>
        /// <returns>1为指定的列均不为空；-1为指定的列存在为空的列。</returns>
        public int CheckNullValue(ArrayList columnList, Hashtable htExcept)
        {
            if (htExcept != null)
            {
                int exceptColumn = 0;
                object exceptValue = null;
                foreach (DictionaryEntry de in htExcept)
                {
                    exceptColumn = Convert.ToInt32(de.Key);
                    exceptValue = de.Value;
                }
                for (int row = 0; row < Rows.Count; row++)
                {
                    DataGridViewCell cell = Rows[row].Cells[exceptColumn];
                    if (cell.Value.Equals(exceptValue))
                    {
                        continue;
                    }
                    foreach (int column in columnList)
                    {
                        cell = Rows[row].Cells[column];
                        if (cell.Value == null || cell.Value.ToString().Trim() == string.Empty)
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, Columns[cell.ColumnIndex].HeaderText + "不能为空。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                    }
                }
            }
            else
            {
                for (int row = 0; row < Rows.Count; row++)
                {
                    DataGridViewCell cell;
                    foreach (int column in columnList)
                    {
                        cell = Rows[row].Cells[column];
                        if (cell.Value == null || cell.Value.ToString().Trim() == string.Empty)
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, Columns[cell.ColumnIndex].HeaderText + "不能为空。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 检查指定列的值是否为唯一。
        /// </summary>
        /// <param name="columnList">指定的列。</param>
        /// <param name="htExcept">当某列满足某值时，忽略检查。</param>
        /// <returns>1为指定的列不存在唯一冲突；-1为指定的列存在唯一冲突。</returns>
        public int CheckUnique(ArrayList columnList, Hashtable htExcept)
        {
            if (htExcept != null)
            {
                int exceptColumn = 0;
                object exceptValue = null;
                foreach (DictionaryEntry de in htExcept)
                {
                    exceptColumn = Convert.ToInt32(de.Key);
                    exceptValue = de.Value;
                }
                foreach (int column in columnList)
                {
                    ArrayList checkList = new ArrayList();
                    for (int row = 0; row < Rows.Count; row++)
                    {
                        DataGridViewCell cell = Rows[row].Cells[exceptColumn];
                        if (cell.Value.Equals(exceptValue))
                        {
                            continue;
                        }
                        cell = Rows[row].Cells[column];
                        if (checkList.Contains(cell.Value))
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, "已存在" + Columns[cell.ColumnIndex].HeaderText + "为" + cell.Value.ToString() + "的数据。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                        else
                        {
                            checkList.Add(cell.Value);
                        }
                    }
                }
            }
            else
            {
                foreach (int column in columnList)
                {
                    ArrayList checkList = new ArrayList();
                    for (int row = 0; row < Rows.Count; row++)
                    {
                        DataGridViewCell cell = Rows[row].Cells[column];
                        if (checkList.Contains(cell.Value))
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, "已存在" + Columns[cell.ColumnIndex].HeaderText + "为" + cell.Value.ToString() + "的数据。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                        else
                        {
                            checkList.Add(cell.Value);
                        }
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 测试指定列的单元格长度是否满足条件。
        /// </summary>
        /// <param name="htLengthCompare">对比用的哈希表。</param>
        /// <param name="htExcept">当某列满足某值时，忽略检查。</param>
        /// <returns>1为所有指定的单元格均满足条件；-1为存在不满足条件的单元格。</returns>
        public int CheckColumnLength(Hashtable htLengthCompare, Hashtable htExcept)
        {
            if (htExcept != null)
            {
                int exceptColumn = 0;
                object exceptValue = null;
                foreach (DictionaryEntry de in htExcept)
                {
                    exceptColumn = Convert.ToInt32(de.Key);
                    exceptValue = de.Value;
                }
                foreach (DictionaryEntry de in htLengthCompare)
                {
                    int columnIndex = Convert.ToInt32(de.Key);
                    ArrayList lengthList = de.Value as ArrayList;
                    #region 获取错误提示字串。
                    string err = string.Empty;
                    foreach (int length in lengthList)
                    {
                        err += length.ToString() + "位或";
                    }
                    //去除最后一个“或”字。
                    if (err != string.Empty)
                    {
                        err = err.Substring(0, err.Length - 1);
                    }
                    #endregion
                    DataGridViewCell cell;
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        cell = Rows[i].Cells[exceptColumn];
                        if (cell.Value.Equals(exceptValue))
                        {
                            continue;
                        }
                        cell = Rows[i].Cells[columnIndex];
                        if (cell.Value == null)
                        {
                            cell.Value = string.Empty;
                        }
                        string str = cell.Value.ToString().Trim();
                        if (!lengthList.Contains(str.Length))
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, Columns[cell.ColumnIndex].HeaderText + "的数据长度应为" + err + "。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                    }
                }
            }
            else
            {
                foreach (DictionaryEntry de in htLengthCompare)
                {
                    int columnIndex = Convert.ToInt32(de.Key);
                    ArrayList lengthList = de.Value as ArrayList;
                    #region 获取错误提示字串。
                    string err = string.Empty;
                    foreach (int length in lengthList)
                    {
                        err += length.ToString() + "位或";
                    }
                    //去除最后一个“或”字。
                    if (err != string.Empty)
                    {
                        err = err.Substring(0, err.Length - 1);
                    }
                    #endregion
                    DataGridViewCell cell;
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        cell = Rows[i].Cells[columnIndex];
                        if (cell.Value == null)
                        {
                            cell.Value = string.Empty;
                        }
                        string str = cell.Value.ToString().Trim();
                        if (!lengthList.Contains(str.Length))
                        {
                            Point location = GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false).Location;
                            location.Offset(0, Rows[cell.RowIndex].Height);
                            WindowManager.ShowToolTip(Parent, Columns[cell.ColumnIndex].HeaderText + "的数据长度应为" + err + "。", 3000, location);
                            CurrentCell = cell;
                            BeginEdit(false);
                            return -1;
                        }
                    }
                }
            }
            return 1;
        }

        private bool IsNumberic(string str)
        {
            Regex rex = new Regex(@"^\d+$");
            if (rex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            base.OnCellClick(e);
            //使点击Cell时，也可使DataGridView中的CheckBox列得到改变。
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            Type type = Rows[e.RowIndex].Cells[e.ColumnIndex].GetType();
            if (type.FullName == "System.Windows.Forms.DataGridViewCheckBoxCell")
             {
                DataGridViewCell cell = Rows[e.RowIndex].Cells[e.ColumnIndex];
                int contentX = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).X+cell.ContentBounds.X;
                int contentY = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Y+cell.ContentBounds.Y;
                Point contentScreen = PointToScreen(new Point(contentX, contentY));
                Rectangle contentRectangle = new Rectangle(contentScreen.X, contentScreen.Y, cell.ContentBounds.Width, cell.ContentBounds.Height);
                if (contentRectangle.Contains(Cursor.Position))
                {
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, e);
                    }
                    return;
                }
                else
                {
                    if (cell.Value == null)
                    {
                        cell.Value = false;
                    }
                    cell.Value=!((bool)cell.Value);
                    CurrentCell = null;
                    CurrentCell = cell;
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, e);
                    }
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (CurrentCell == null)
                {
                    return base.ProcessDialogKey(keyData); ;
                }
                DataGridViewCell cell = FindNextTextBoxCell(CurrentCell.RowIndex, CurrentCell.ColumnIndex);
                if (cell != null)
                {
                    CurrentCell = cell;
                    return true;
                }
                else
                {
                    if (CurrentCell.RowIndex < Rows.Count - 1)
                    {
                        cell = FindNextTextBoxCell(CurrentCell.RowIndex + 1, -1);
                        if (cell != null)
                        {
                            CurrentCell = cell;
                            return true;
                        }
                    }
                    else
                    {
                        if (AddNewRow != null)
                        {
                            AddNewRow(this, null);
                        }
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            int currentCellHashCode = CurrentCell.GetHashCode();
            if (!htCellTextBackUp.Contains(currentCellHashCode))
            {
                htCellTextBackUp.Add(currentCellHashCode, CurrentCell.Value);
            }
            base.OnCellBeginEdit(e);
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl CellEdit = null;
            CellEdit = e.Control as DataGridViewTextBoxEditingControl;
            CellEdit.TextChanged -= CellEdit_NumberTextChanged;
            CellEdit.TextChanged += CellEdit_NumberTextChanged;
            CellEdit.TextChanged -= CellEdit_TextChanged;
            CellEdit.TextChanged += CellEdit_TextChanged;
            base.OnEditingControlShowing(e);
        }

        void CellEdit_TextChanged(object sender, EventArgs e)
        {
            if (!numberColumns.Contains(CurrentCell.ColumnIndex))
            {
                DataGridViewTextBoxEditingControl editControl = sender as DataGridViewTextBoxEditingControl;
                int currentCellHashCode = CurrentCell.GetHashCode();
                if (htCellTextBackUp[currentCellHashCode].ToString() != editControl.Text)
                {
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, new DataGridViewCellEventArgs(CurrentCell.ColumnIndex, CurrentCell.RowIndex));
                    }
                }
                htCellTextBackUp[currentCellHashCode] = editControl.Text;
            }
        }

        void CellEdit_NumberTextChanged(object sender, EventArgs e)
        {
            if (numberColumns.Contains(CurrentCell.ColumnIndex))
            {
                DataGridViewTextBoxEditingControl editControl = sender as DataGridViewTextBoxEditingControl;
                int currentCellHashCode = CurrentCell.GetHashCode();
                if (editControl.Text.Length != 0)
                {
                    if (IsNumberic(editControl.Text))
                    {
                        htNumberColumnsRightValueBackUp[CurrentCell.ColumnIndex] = editControl.Text;
                        if (htCellTextBackUp[currentCellHashCode].ToString() != editControl.Text)
                        {
                            if (ValueChanged != null)
                            {
                                ValueChanged(this, new DataGridViewCellEventArgs(CurrentCell.ColumnIndex, CurrentCell.RowIndex));
                            }
                        }
                    }
                    else
                    {
                        if (editControl.Text.Length == 1)
                        {
                            editControl.Text = string.Empty;
                        }
                        else
                        {
                            editControl.Text = htNumberColumnsRightValueBackUp[CurrentCell.ColumnIndex].ToString();
                            editControl.Select(editControl.Text.Length, 0);
                        }
                    }
                }
                else
                {
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, new DataGridViewCellEventArgs(CurrentCell.ColumnIndex, CurrentCell.RowIndex));
                    }
                }
                htCellTextBackUp[currentCellHashCode] = editControl.Text;
            }
        }
    }
}

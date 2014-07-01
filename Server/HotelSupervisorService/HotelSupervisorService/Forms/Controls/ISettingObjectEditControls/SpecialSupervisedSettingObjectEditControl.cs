using System;
using System.Collections;
using System.Windows.Forms;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls.ToolStripButtons;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects;
using HotelSupervisorService.Objects.Setting;

namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    public partial class SpecialSupervisedSettingObjectEditControl : BaseSettingObjectEditControl, ISettingObjectEditControl
    {
        public SpecialSupervisedSettingObjectEditControl()
        {
            InitializeComponent();
        }

        private SpecialSupervisedSetting specialSupervisedSetting;
        private Hashtable htNeedUpdate = new Hashtable();

        private void AddItem(SpecialSupervised item)
        {
            gvMain.Rows.Add(1);
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[0].Value = item.CardNumber;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[1].Value = item.Name;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[2].Value = item.FuzzyCompare;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[3].Value = item.NeedSMSAlert;
            gvMain.Rows[gvMain.Rows.Count - 1].Tag = item;
        }

        private void AddNewItem()
        {
            gvMain.Rows.Add(1);
            SpecialSupervised item = new SpecialSupervised();
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[0].Value = item.CardNumber;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[1].Value = item.Name;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[2].Value = item.FuzzyCompare;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[3].Value = item.NeedSMSAlert;
            gvMain.SelectNextTextBoxCell(gvMain.Rows.Count - 1);
            gvMain.BeginEdit(false);
            NeedSave = true;
        }

        private int Save(ref string message)
        {
            gvMain.EndEdit();
            #region 检测是否有为空的单元格。
            ArrayList columnList = new ArrayList();
            columnList.Add(0);
            columnList.Add(1);
            int i = gvMain.CheckNullValue(columnList,null);
            if (i < 0)
            {
                return -1;
            }
            #endregion
            #region 检测是否有不满足长度的单元格。
            Hashtable htLengthCompare = new Hashtable();
            Hashtable htExcept = new Hashtable();
            ArrayList lengthList = new ArrayList();
            lengthList.Add(15);
            lengthList.Add(18);
            htLengthCompare.Add(0, lengthList);
            htExcept.Add(2, true);
            i = gvMain.CheckColumnLength(htLengthCompare, htExcept);
            if (i < 0)
            {
                return -1;
            }
            #endregion
            #region 检测是否有不唯一的单元格。
            columnList.Clear();
            columnList.Add(0);
            i = gvMain.CheckUnique(columnList,null);
            if (i < 0)
            {
                return -1;
            }
            #endregion
            #region 暂存存在冲突的行，在更新结束后单独处理。例如存在2行数据A、B，若A、B的身份证号互换，则会引起更新冲突。
            ArrayList alNeedSingleProcess = new ArrayList();
            SpecialSupervised item;
            foreach (DictionaryEntry de in htNeedUpdate)
            {
                item = de.Key as SpecialSupervised;
                foreach (DictionaryEntry de1 in htNeedUpdate)
                {
                    if (de.Key != de1.Key && item.CardNumber == de1.Value.ToString())
                    {
                        item.CardNumber += "temp";
                        alNeedSingleProcess.Add(item);
                    }
                }
            }
            #endregion
            foreach (DictionaryEntry de in htNeedUpdate)
            {
                i = DataBaseManager.GlobalDataBaseManager.UpdateSpecialSupervisedByCardNumber(de.Key as SpecialSupervised, de.Value.ToString());
                if (i < 0)
                {
                    message = "保存失败。";
                    return -1;
                }
            }
            #region 处理暂存的行。
            foreach (SpecialSupervised itemTmp in alNeedSingleProcess)
            {
                itemTmp.CardNumber = itemTmp.CardNumber.Replace("temp",string.Empty);
                i = DataBaseManager.GlobalDataBaseManager.UpdateSpecialSupervisedByCardNumber(itemTmp, itemTmp.CardNumber+"temp");
                if (i < 0)
                {
                    message = "保存失败。";
                    return -1;
                }
            }
            #endregion
            for (int rowIndex = 0; rowIndex < gvMain.Rows.Count; rowIndex++)
            {
                DataGridViewRow row = gvMain.Rows[rowIndex];
                if (row.Tag == null)
                {
                    item = GetItemFromRow(row.Index);
                    i = DataBaseManager.GlobalDataBaseManager.InsertSpecialSupervised(item);
                    if (i < 0)
                    {
                        message = "保存失败。";
                        return -1;
                    }
                    else
                    {
                        row.Tag = item;
                    }
                }
            }
            NeedSave = false;
            htNeedUpdate.Clear();
            gvMain.EndEdit();
            message = "保存成功。";
            return 1;
        }

        private void Delete()
        {
            try
            {
                if (gvMain.CurrentRow == null)
                {
                    return;
                }
                DataGridViewRow row = gvMain.CurrentRow;
                SpecialSupervised item = row.Tag as SpecialSupervised;
                if (item == null)
                {
                    gvMain.Rows.Remove(row);
                }
                else
                {
                    DialogResult dr = MessageBox.Show("确实要删除姓名为“" + item.Name + "”的记录吗？", "删除记录", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        int i = DataBaseManager.GlobalDataBaseManager.DeleteSpecialSupervisedByCardNumber(item.CardNumber);
                        if (i < 0)
                        {
                            MessageBox.Show("删除失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (htNeedUpdate.Contains(item))
                            {
                                htNeedUpdate.Remove(item);
                            }
                            gvMain.Rows.Remove(row);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("94。", e);
            }
        }

        private void Add()
        {
            AddNewItem();
        }

        private SpecialSupervised GetItemFromRow(int rowIndex)
        {
            SpecialSupervised item = new SpecialSupervised();
            item.CardNumber = gvMain.Rows[rowIndex].Cells[0].Value.ToString();
            item.Name = gvMain.Rows[rowIndex].Cells[1].Value.ToString();
            item.FuzzyCompare = (bool)gvMain.Rows[rowIndex].Cells[2].Value;
            item.NeedSMSAlert = (bool)gvMain.Rows[rowIndex].Cells[3].Value;
            return item;
        }

        protected override void InitToolStripButtons()
        {
            toolStripButtons=new ToolStripButton[2];
            toolStripButtons[0] = new ToolStripButtonNewOne();
            toolStripButtons[0].Click += new EventHandler(toolStripButton_Click);
            toolStripButtons[1] = new ToolStripButtonDeleteOne();
            toolStripButtons[1].Click+=new EventHandler(toolStripButton_Click);
            base.InitToolStripButtons();
        }

        void toolStripButton_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ToolStripButtonNewOne))
            {
                Add();
            }
            else if (sender.GetType() == typeof(ToolStripButtonDeleteOne))
            {
                Delete();
            }
        }

        private void gvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!ControlLoaded)
            {
                return;
            }
            NeedSave = true;
            DataGridViewRow row = gvMain.Rows[e.RowIndex];
            if (row.Tag != null)
            {
                SpecialSupervised item=row.Tag as SpecialSupervised;
                if (!htNeedUpdate.Contains(item))
                {
                    htNeedUpdate.Add(item, item.CardNumber);
                }
                DataGridViewCell cell = gvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            if (cell.Value == null)
                            {
                                cell.Value = string.Empty;
                            }
                            item.CardNumber = cell.Value.ToString();
                            break;
                        }
                    case 1:
                        {
                            if (cell.Value == null)
                            {
                                cell.Value = string.Empty;
                            }
                            item.Name = cell.Value.ToString();
                            break;
                        }
                    case 2:
                        {
                            item.FuzzyCompare = (bool)cell.Value;
                            break;
                        }
                    case 3:
                        {
                            item.NeedSMSAlert = (bool)cell.Value;
                            break;
                        }
                }
            }
        }

        private void gvMain_AddNewRow(object sender, EventArgs e)
        {
            Add();
        }

        private void gvMain_ValueChanged(object sender, EventArgs e)
        {
            gvMain_CellValueChanged(sender, e as DataGridViewCellEventArgs);
        }

        protected override int OnSave(ref string message)
        {
            try
            {
                return Save(ref message);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("56。", e);
            }
        }

        #region ISettingObjectEditControl 成员

        public ISettingObject ISettingObject
        {
            get
            {
                return specialSupervisedSetting;
            }
            set
            {
                specialSupervisedSetting = value as SpecialSupervisedSetting;
                try
                {
                    InitSettingObject(specialSupervisedSetting);
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ExceptionPlus))
                    {
                        throw;
                    }
                    throw new ExceptionPlus("55。", e);
                }
            }
        }

        public void InitSettingObject(ISettingObject iSettingObject)
        {
            SpecialSupervisedSetting specialSupervisedSetting =  iSettingObject as SpecialSupervisedSetting;
            gvMain.SuspendLayout();
            foreach (SpecialSupervised item in specialSupervisedSetting.SpecialSupervisedList)
            {
                AddItem(item);
            }
            gvMain.ResumeLayout();
        }

        #endregion
    }
}

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
    public partial class HotelSettingObjectEditControl : BaseSettingObjectEditControl, ISettingObjectEditControl
    {
        public HotelSettingObjectEditControl()
        {
            InitializeComponent();
        }

        private HotelSetting hotelSetting;
        private Hashtable htNeedUpdate = new Hashtable();

        private void AddItem(Hotel item)
        {
            gvMain.Rows.Add(1);
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[0].Value = item.Name;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[1].Value = item.Location;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[2].Value = item.HotelTel;
            gvMain.Rows[gvMain.Rows.Count - 1].Tag = item;
        }

        private void AddNewItem()
        {
            gvMain.Rows.Add(1);
            Hotel item = new Hotel();
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[0].Value = item.Name;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[1].Value = item.Location;
            gvMain.Rows[gvMain.Rows.Count - 1].Cells[2].Value = item.HotelTel;
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
            columnList.Add(2);
            int i = gvMain.CheckNullValue(columnList, null);
            if (i < 0)
            {
                return -1;
            }
            #endregion
            #region 检测是否有不满足长度的单元格。
            Hashtable htLengthCompare = new Hashtable();
            ArrayList lengthList = new ArrayList();
            lengthList.Add(7);
            lengthList.Add(8);
            lengthList.Add(11);
            lengthList.Add(12);
            htLengthCompare.Add(2, lengthList);
            i = gvMain.CheckColumnLength(htLengthCompare, null);
            if (i < 0)
            {
                return -1;
            }
            #endregion

            Hotel item;
            foreach (DictionaryEntry de in htNeedUpdate)
            {
                i = DataBaseManager.GlobalDataBaseManager.UpdateHotelInfoByHotelCode(de.Key as Hotel, de.Value.ToString());
                if (i < 0)
                {
                    message = "保存失败。";
                    return -1;
                }
            }

            for (int rowIndex = 0; rowIndex < gvMain.Rows.Count; rowIndex++)
            {
                DataGridViewRow row = gvMain.Rows[rowIndex];
                if (row.Tag == null)
                {
                    item = GetItemFromRow(row.Index);
                    i = DataBaseManager.GlobalDataBaseManager.InsertHotelInfo(item);
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
                    DialogResult dr = MessageBox.Show("确实要删除名称为“" + item.Name + "”的记录吗？", "删除记录", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
                throw new ExceptionPlus("97。", e);
            }
        }

        private void Add()
        {
            AddNewItem();
        }

        private Hotel GetItemFromRow(int rowIndex)
        {
            Hotel item = new Hotel();
            item.Name = gvMain.Rows[rowIndex].Cells[0].Value.ToString();
            item.Location = gvMain.Rows[rowIndex].Cells[1].Value.ToString();
            item.HotelTel = gvMain.Rows[rowIndex].Cells[2].Value.ToString();
            return item;
        }

        protected override void InitToolStripButtons()
        {
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
                Hotel item = row.Tag as Hotel;
                if (!htNeedUpdate.Contains(item))
                {
                    htNeedUpdate.Add(item, item.Code);
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
                            item.Name = cell.Value.ToString();
                            break;
                        }
                    case 1:
                        {
                            if (cell.Value == null)
                            {
                                cell.Value = string.Empty;
                            }
                            item.Location = cell.Value.ToString();
                            break;
                        }
                    case 2:
                        {
                            if (cell.Value == null)
                            {
                                cell.Value = string.Empty;
                            }
                            item.HotelTel = cell.Value.ToString();
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
                throw new ExceptionPlus("96。", e);
            }
        }

        #region ISettingObjectEditControl 成员

        public ISettingObject ISettingObject
        {
            get
            {
                return hotelSetting;
            }
            set
            {
                hotelSetting = value as HotelSetting;
                try
                {
                    InitSettingObject(hotelSetting);
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ExceptionPlus))
                    {
                        throw;
                    }
                    throw new ExceptionPlus("95。", e);
                }
            }
        }

        public void InitSettingObject(ISettingObject iSettingObject)
        {
            HotelSetting hotelSetting = iSettingObject as HotelSetting;
            gvMain.SuspendLayout();
            foreach (Hotel item in hotelSetting.HotelList)
            {
                AddItem(item);
            }
            gvMain.ResumeLayout();
        }

        #endregion
    }
}

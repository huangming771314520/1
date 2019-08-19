using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zephyr.WinForm.Content
{
    public partial class PrintBarcodeForm : Form
    {
        public SkinSharp.SkinH_Net skinh;

        public PrintBarcodeForm()
        {
            skinh = new SkinSharp.SkinH_Net();
            skinh.AttachEx(@"grape.she", string.Empty);

            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }

        private void PrintBarcodeForm_Load(object sender, EventArgs e)
        {
            LoadTableData(string.Empty, string.Empty);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtPartCode.Text = string.Empty;
            txtPartType.Text = string.Empty;
            txtPartCode.Focus();

            LoadTableData(string.Empty, string.Empty);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strPartCode = txtPartCode.Text.Trim();
            string strPartType = txtPartType.Text.Trim();

            LoadTableData(strPartCode, strPartType);
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)13))
            {
                string strPartCode = txtPartCode.Text.Trim();
                string strPartType = txtPartType.Text.Trim();

                LoadTableData(strPartCode, strPartType);
            }
        }

        private void LoadTableData(string partCode, string partType)
        {
            string url = string.Format(@"http://" + Program.IP + "/api/Mms/WinFormClient/GetPartByPCodeAndPType?partCode={0}&partType={1}", partCode, partType);

            var json = Helpers.HttpHelper.GetJSON(url);
            var result = JsonConvert.DeserializeObject<Models.PartInfoModel>(json);

            var data = result.data.GetSysParts.Select(item =>
            {
                return new
                {
                    //ID
                    ID = item.ID,
                    //零件编码
                    PartCode = item.PartCode,
                    //零件名称
                    PartName = item.PartName,
                    //零件类型
                    TypeName = item.TypeName,
                    //型号
                    Model = item.Model,
                    //规格
                    Spec = item.Spec,
                    //重量
                    Weight = item.Weight,
                    //安全库存
                    SafetyStock = item.SafetyStock,
                    //最高库存
                    MaxStock = item.MaxStock,
                    //最低库存
                    MinStock = item.MinStock,
                    //计量单位
                    QuantityUnit = item.QuantityUnit,
                    //图号
                    FigureCode = item.FigureCode,
                    //质检批号
                    QualityCode = item.QualityCode,
                    //对应条码
                    CorrespondingBarcode = item.CorrespondingBarcode
                };
            }).ToList();

            dgvPartDataShow.DataSource = data;

            lvwPrintInfo.Items.Clear();
        }

        private void dgvPartDataShow_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPartDataShow.SelectedRows.Count > 0)
            {
                int selectRowIndex = dgvPartDataShow.CurrentRow.Index;
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvPartDataShow.Rows[selectRowIndex].Cells["chkSelect"];
                bool status = Convert.ToBoolean(checkCell.Value);
                checkCell.Value = !status;

                DataGridViewRow row = dgvPartDataShow.Rows[selectRowIndex];
                //选中
                if (!status)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 255, 230);
                    string barCode = string.Empty;

                    if (row.Cells["CorrespondingBarcode"].Value == null || string.IsNullOrEmpty(row.Cells["CorrespondingBarcode"].Value.ToString()))
                    {
                        //获取零件编码
                        string partCode = row.Cells["PartCode"].Value.ToString();

                        string url = string.Format(@"http://" + Program.IP + "/api/Mms/WinFormClient/GetUpdatePartBarCodeByPCode?partCode={0}", partCode);
                        var json = Helpers.HttpHelper.GetJSON(url);

                        var result = JsonConvert.DeserializeObject<Models.PartInfoModel>(json);
                        if (result.result)
                        {
                            barCode = result.data.GetBarCode;
                            //row.Cells["CorrespondingBarcode"].Value = result.data.GetBarCode;

                        }
                    }
                    else
                    {
                        barCode = row.Cells["CorrespondingBarcode"].Value.ToString();
                    }

                    ListViewItem lvi = new ListViewItem((lvwPrintInfo.Items.Count + 1).ToString());
                    lvi.SubItems.Add(row.Cells["PartCode"].Value.ToString());
                    lvi.SubItems.Add(row.Cells["PartName"].Value == null ? string.Empty : row.Cells["PartName"].Value.ToString());
                    lvi.SubItems.Add(barCode);
                    lvi.SubItems.Add("0");
                    lvi.Tag = row;
                    lvwPrintInfo.Items.Add(lvi);
                }
                //不选中
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;

                    for (int i = 0; i < lvwPrintInfo.Items.Count; i++)
                    {
                        DataGridViewRow tempRow = lvwPrintInfo.Items[i].Tag as DataGridViewRow;
                        if (row.Cells["PartCode"].Value == tempRow.Cells["PartCode"].Value)
                        {
                            lvwPrintInfo.Items.Remove(lvwPrintInfo.Items[i]);
                            break;
                        }
                    }

                }

                for (int i = 0; i < lvwPrintInfo.Items.Count; i++)
                {
                    lvwPrintInfo.Items[i].SubItems[0].Text = (i + 1).ToString();
                }

            }
        }

        private bool isExecute = false;
        private void lvwPrintInfo_Click(object sender, EventArgs e)
        {
            if (lvwPrintInfo.SelectedItems.Count == 0)
            {
                return;
            }

            isExecute = true;
            DataGridViewRow row = lvwPrintInfo.SelectedItems[0].Tag as DataGridViewRow;

            lblPartCode.Text = row.Cells["PartCode"].Value.ToString();
            lblPartName.Text = row.Cells["PartName"].Value == null ? string.Empty : row.Cells["PartName"].Value.ToString();
            lblTypeName.Text = row.Cells["TypeName"].Value == null ? string.Empty : row.Cells["TypeName"].Value.ToString();
            lblModel.Text = row.Cells["Model"].Value.ToString();
            lblCorrespondingBarcode.Text = lvwPrintInfo.SelectedItems[0].SubItems[3].Text;

            nudPrintNum.Value = Convert.ToInt32(lvwPrintInfo.SelectedItems[0].SubItems[4].Text);

            nudPrintNum.Tag = lvwPrintInfo.SelectedIndices[0];

            isExecute = false;
        }

        private void nudPrintNum_ValueChanged(object sender, EventArgs e)
        {
            if (isExecute)
            {
                return;
            }

            lvwPrintInfo.Items[Convert.ToInt32(nudPrintNum.Tag)].SubItems[4].Text = nudPrintNum.Value.ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

    }
}

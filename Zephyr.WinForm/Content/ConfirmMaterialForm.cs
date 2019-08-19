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
    public partial class ConfirmMaterialForm : Form
    {
        /// <summary>
        /// 该计划的ID
        /// </summary>
        private int ID { get; set; }

        private Models.ProducePlanInfoModel.DataModel producePlanInfoModel { get; set; }
        private Models.MaterialDetailModel materialDetailModel { get; set; }

        public ConfirmMaterialForm(int _id, Models.ProducePlanInfoModel.DataModel _producePlanInfoModel, Models.MaterialDetailModel _materialDetailModel)
        {
            ID = _id;
            producePlanInfoModel = _producePlanInfoModel;
            materialDetailModel = _materialDetailModel;
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
            WindowState = FormWindowState.Maximized;
        }

        public ConfirmMaterialForm(int _id)
        {
            ID = _id;
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
            WindowState = FormWindowState.Maximized;
        }

        private void ConfirmMaterialForm_Load(object sender, EventArgs e)
        {
            producePlanInfoModel = Program.ProducePlanInfo.data.SingleOrDefault(item => item.GetProducePlanInfo.ID.Equals(ID));
            Program.ProducePlanInfoModel = producePlanInfoModel;

            string json = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetMaterialDetailByPartCode?partCode=" + producePlanInfoModel.GetProducePlanInfo.PartCode);
            materialDetailModel = JsonConvert.DeserializeObject<Models.MaterialDetailModel>(json);
            Program.MaterialDetailModel = materialDetailModel;

            //本体
            var oneself = materialDetailModel.data.FirstOrDefault(item => item.GetPart.PartCode.Equals(producePlanInfoModel.GetProducePlanInfo.PartCode));
            Program.MaterialDetail = oneself;
            //子集
            var son = materialDetailModel.data.Where(item => !item.GetPart.PartCode.Equals(producePlanInfoModel.GetProducePlanInfo.PartCode)).ToList();

            lblContractCode.Text = producePlanInfoModel.GetProducePlanInfo.ContractCode;
            lblProductName.Text = producePlanInfoModel.GetProjectDetail.ProductName;
            lblProductType.Text = producePlanInfoModel.GetCode.Text;
            lblModel.Text = oneself.GetPart.Model;
            lblSpecifications.Text = producePlanInfoModel.GetProjectDetail.Specifications;
            lblPartCode.Text = oneself.GetPart.PartCode;
            lblProcessCode.Text = producePlanInfoModel.GetProductProcessRoute.ProcessCode;
            lblWorkshopName.Text = producePlanInfoModel.GetProductProcessRoute.WorkshopName;
            lblEquipmentName.Text = producePlanInfoModel.GetProductProcessRoute.EquipmentName;
            lblWorkGroupName.Text = producePlanInfoModel.GetProductProcessRoute.WorkGroupName;
            lblQuantity.Text = oneself.GetMaterialDetail.PartQuantity.ToString();
            lblManHour.Text = producePlanInfoModel.GetProductProcessRoute.ManHour + producePlanInfoModel.GetProductProcessRoute.Unit == 1 ? "分" : "秒";
            lblPlanedFinishTime.Text = producePlanInfoModel.GetProducePlanInfo.PlanedFinishTime.ToString();
            lblPlanedStartTime.Text = producePlanInfoModel.GetProducePlanInfo.PlanedStartTime.ToString();

            if (son.Count > 0)
            {
                for (int i = 0; i < son.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem((i + 1).ToString());
                    lvi.SubItems.Add(son[i].GetMaterialDetail.PartName);
                    lvi.SubItems.Add(son[i].GetMaterialDetail.PartCode);
                    lvi.SubItems.Add(son[i].GetMaterialDetail.PartQuantity.ToString());
                    lvwProductMaterialDetail.Items.Add(lvi);

                    Panel panel = new Panel();
                    Label label1 = new Label();
                    Label label2 = new Label();
                    Label label3 = new Label();
                    Label label4 = new Label();
                    Label label5 = new Label();
                    Label label6 = new Label();
                    TextBox textBox = new TextBox();

                    panel.Size = new Size(450, 152);
                    panel.Location = new Point(0, 0 + 153 * i);
                    panel.BackColor = i % 2 == 0 ? Color.FromArgb(230, 230, 230) : Color.FromArgb(236, 236, 236);
                    panel.Margin = new Padding(0, 0, 0, 0);
                    panel.Tag = son[i];

                    label1.Size = new Size(75, 15);
                    label1.Location = new Point(27, 19);
                    label1.Text = "零件编码:";
                    label1.BackColor = Color.Transparent;
                    panel.Controls.Add(label1);
                    label2.Size = new Size(300, 15);
                    label2.Location = new Point(128, 18);
                    label2.Text = son[i].GetMaterialDetail.PartCode;
                    label2.BackColor = Color.Transparent;
                    panel.Controls.Add(label2);

                    label3.Size = new Size(75, 15);
                    label3.Location = new Point(27, 49);
                    label3.Text = "零件名称:";
                    label3.BackColor = Color.Transparent;
                    panel.Controls.Add(label3);
                    label4.Size = new Size(300, 15);
                    label4.Location = new Point(128, 48);
                    label4.Text = son[i].GetMaterialDetail.PartName;
                    label4.BackColor = Color.Transparent;
                    panel.Controls.Add(label4);

                    label5.Size = new Size(75, 15);
                    label5.Location = new Point(27, 79);
                    label5.Text = "录入数量:";
                    label5.BackColor = Color.Transparent;
                    panel.Controls.Add(label5);
                    label6.Size = new Size(300, 15);
                    label6.Location = new Point(128, 78);
                    label6.Text = string.Format("{0}/{1}", 0, son[i].GetMaterialDetail.PartQuantity);
                    label6.BackColor = Color.Transparent;
                    panel.Controls.Add(label6);

                    textBox.Location = new Point(27, 109);
                    textBox.Size = new Size(400, 25);
                    textBox.Tag = 0;
                    textBox.KeyPress += new KeyPressEventHandler(txtPartCode_KeyPress);
                    panel.Controls.Add(textBox);

                    pnlVerifyProductMaterial.Controls.Add(panel);
                }
            }

        }

        private void txtPartCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)13))
            {
                TextBox textBox = sender as TextBox;

                if (string.IsNullOrEmpty(textBox.Text))
                {
                    return;
                }

                Panel panel = (Panel)textBox.Parent;
                int n = Convert.ToInt32(textBox.Tag);

                Models.MaterialDetailModel.DataModel model = panel.Tag as Models.MaterialDetailModel.DataModel;

                if (n.Equals(model.GetMaterialDetail.PartQuantity))
                {
                    if (MessageBox.Show("已录入正确的数量，是否继续录入？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                n++;
                textBox.Tag = n;
                panel.Controls[5].Text = string.Format("{0}/{1}", n, model.GetMaterialDetail.PartQuantity);

                if (n.Equals(model.GetMaterialDetail.PartQuantity))
                {
                    panel.BackColor = Color.FromArgb(220, 255, 220);
                }
                if (n > model.GetMaterialDetail.PartQuantity)
                {
                    panel.BackColor = Color.FromArgb(255, 220, 220);
                }

                bool verify = false;
                foreach (var item in pnlVerifyProductMaterial.Controls)
                {
                    if (item is Panel)
                    {
                        Panel pnl = item as Panel;
                        foreach (var control in pnl.Controls)
                        {
                            if ((pnl.Tag as Models.MaterialDetailModel.DataModel).GetMaterialDetail.PartQuantity.Equals(Convert.ToInt32(((TextBox)pnl.Controls[6]).Tag)))
                            {
                                verify = true;
                            }
                            else
                            {
                                verify = false;
                                return;
                            }
                        }
                    }
                }

                if (verify)
                {
                    DialogResult dr = MessageBox.Show("验证通过！是否立即开始？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        //录入计划实际开始时间
                        string json = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetPlanActualStartTime?ppdID=" + ID + "&dTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        var data = JsonConvert.DeserializeObject<dynamic>(json);

                        if (Convert.ToBoolean(data["result"]))
                        {
                            //物料出库
                            OutputStorage();
                        }

                        DrawingsShowForm form = new DrawingsShowForm(ID);
                        DialogResult drB = form.ShowDialog();
                        if (drB == DialogResult.OK)
                        {
                            string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                            System.Diagnostics.Process.Start(str, Program.WorkGroupInfo.data.GetWorkGroupInfo.TeamCode);
                            Application.Exit();
                        }

                    }
                }

            }
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定跳过验证吗？\n点击『确定』将跳过验证并立即开始生产！\n点击『取消』即可返回验证！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //录入计划实际开始时间
                string json = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetPlanActualStartTime?ppdID=" + ID + "&dTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                if (Convert.ToBoolean(data["result"]))
                {
                    //物料出库
                    OutputStorage();
                }

                DrawingsShowForm form = new DrawingsShowForm(ID);
                DialogResult drB = form.ShowDialog();
                if (drB == DialogResult.OK)
                {
                    string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    System.Diagnostics.Process.Start(str, Program.WorkGroupInfo.data.GetWorkGroupInfo.TeamCode);
                    Application.Exit();
                }
            }
        }


        /// <summary>
        /// 物料入库
        /// </summary>
        public void InputStorage()
        {
            Models.MaterialDetailModel.DataModel model = materialDetailModel.data.SingleOrDefault(item => item.GetMaterialDetail.PartCode.Equals(producePlanInfoModel.GetProducePlanInfo.PartCode));

            //string json = Helpers.HttpHelper.PostJSON("http://localhost:26888/api/Mms/WinFormClient/PostMaterialInputStorage", new
            string json = Helpers.HttpHelper.PostJSON("http://" + Program.IP + "/api/Mms/WinFormClient/PostMaterialInputStorage", new
            {
                mainData = new
                {
                    ContractCode = producePlanInfoModel.GetProducePlanInfo.ContractCode,
                    ProjectName = producePlanInfoModel.GetProject.ProjectName,
                    WarehouseCode = Program.WorkGroupInfo.data.GetWarehouse.WarehouseCode,
                    WarehouseName = Program.WorkGroupInfo.data.GetWarehouse.WarehouseName
                },
                detailData = new
                {
                    InventoryCode = model.GetPart.InventoryCode,
                    InventoryName = model.GetPart.InventoryName,
                    Specs = model.GetPart.Model,
                    Unit = model.GetPart.QuantityUnit,
                    MateNum = producePlanInfoModel.GetProducePlanInfo.Quantity,
                    ConfirmNum = producePlanInfoModel.GetProducePlanInfo.Quantity
                }
            });
        }

        /// <summary>
        /// 物料出库
        /// </summary>
        private void OutputStorage()
        {
            List<Models.MaterialDetailModel.DataModel> models = materialDetailModel.data.Where(item => item.GetMaterialDetail.ParentCode.Equals(producePlanInfoModel.GetProducePlanInfo.PartCode)).ToList();

            List<object> list = new List<object>();

            foreach (var item in models)
            {
                if (item.GetPart == null)
                {
                    continue;
                }

                list.Add(new
                {
                    InventoryCode = item.GetPart.InventoryCode,
                    InventoryName = item.GetPart.InventoryName,
                    Specs = item.GetPart.Model,
                    Unit = item.GetPart.QuantityUnit,
                    MateNum = item.GetMaterialDetail.PartQuantity,
                    ConfirmNum = item.GetMaterialDetail.PartQuantity
                });
            }

            //string json = Helpers.HttpHelper.PostJSON("http://localhost:26888/api/Mms/WinFormClient/PostMaterialOutputStorage", new
            string json = Helpers.HttpHelper.PostJSON("http://" + Program.IP + "/api/Mms/WinFormClient/PostMaterialOutputStorage", new
            {
                mainData = new
                {
                    ContractCode = producePlanInfoModel.GetProducePlanInfo.ContractCode,
                    ProjectName = producePlanInfoModel.GetProject.ProjectName,
                    WarehouseCode = Program.WorkGroupInfo.data.GetWarehouse.WarehouseCode,
                    WarehouseName = Program.WorkGroupInfo.data.GetWarehouse.WarehouseName
                },
                detailData = list
            });
        }


    }
}

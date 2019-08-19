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
    public partial class ProducePlanForm : Form
    {
        public ProducePlanForm()
        {
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
            WindowState = FormWindowState.Maximized;
        }

        private void ProducePlanForm_Load(object sender, EventArgs e)
        {
            List<dynamic> data = new List<dynamic>();
            Program.ProducePlanInfo.data.ForEach(item =>
            {
                data.Add(new
                {
                    ID = item.GetProducePlanInfo.ID,//ID
                    ContractCode = item.GetProducePlanInfo.ContractCode,//合同编号
                    ProjectName = item.GetProject.ProjectName,//项目名称
                    PartCode = item.GetProducePlanInfo.PartCode,//零件编码
                    //ProcessCode = item.GetProducePlanInfo.ProcessCode,//工序编码
                    ProcessName = item.GetProductProcessRoute.ProcessName,//工序名称
                    ProcessDesc = item.GetProductProcessRoute.ProcessDesc,//工序说明
                    WorkshopName = item.GetProducePlanInfo.WorkshopName,//车间名称
                    EquipmentID = item.GetProducePlanInfo.EquipmentID,//设备ID/编码
                    EquipmentName = item.GetProducePlanInfo.EquipmentName,//设备名称
                    WorkGroupName = item.GetProducePlanInfo.WorkGroupName,//班组名称
                    Quantity = item.GetProducePlanInfo.Quantity,//生产数量
                    ManHour = item.GetProducePlanInfo.ManHour + item.GetProducePlanInfo.Unit == 1 ? "分" : "秒",//工时定额
                    PlanedStartTime = item.GetProducePlanInfo.PlanedStartTime,//计划开始时间
                    PlanedFinishTime = item.GetProducePlanInfo.PlanedFinishTime,//计划结束时间
                    ProductName = item.GetProjectDetail.ProductName,//产品名称
                    ProductType = item.GetProjectDetail.ProductType,//产品类型
                    Model = item.GetProjectDetail.Model,//型号
                    Specifications = item.GetProjectDetail.Specifications,//规格
                });
            });

            dgvProducePlanInfoShow.DataSource = data;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (dgvProducePlanInfoShow.SelectedRows.Count > 0)
                {
                    int ID = Convert.ToInt32(dgvProducePlanInfoShow.SelectedRows[0].Cells["ID"].Value);
                    string EquipmentID = dgvProducePlanInfoShow.SelectedRows[0].Cells["EquipmentID"].Value.ToString();

                    //获取设备保养计划
                    string json = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetEquipmentMaintenancePlanByECode?equipmentCode=" + EquipmentID);
                    var result = JsonConvert.DeserializeObject<Models.EquipmentMaintenancePlanModel>(json);

                    DateTime dTime = DateTime.Now;
                    var data = result.data.GetEMaintenancePlan.Where(item =>
                    {
                        bool a = dTime > item.PlanedStartTime;
                        bool b = dTime < item.PlanedFinishTime;
                        return a && b;
                    }).ToList();
                    if (data.Count > 0)
                    {
                        var eMaintenancePlan = data.OrderBy(item => item.PlanedStartTime).ToList()[0];
                        Form form = new EquipmentMaintenanceForm(eMaintenancePlan);
                        DialogResult drA = form.ShowDialog();
                        if (drA == DialogResult.OK)
                        {
                            //string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                            //System.Diagnostics.Process.Start(str, Program.WorkGroupIDTemp);
                            //Application.Exit();
                        }
                        else
                        {
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }

                    Form loginForm = new LoginForm(LoginForm.ActivateTypeEnum.VerifyIdentity);
                    loginForm.ShowDialog();
                    if (loginForm.DialogResult == DialogResult.OK)
                    {
                        ConfirmMaterialForm form = new ConfirmMaterialForm(ID);
                        form.ShowDialog();
                    }
                    else
                    {
                        string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        System.Diagnostics.Process.Start(str, Program.WorkGroupIDTemp);
                        Application.Exit();
                    }
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}

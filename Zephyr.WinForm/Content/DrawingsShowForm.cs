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
    public partial class DrawingsShowForm : Form
    {
        /// <summary>
        /// 该计划的ID
        /// </summary>
        private int ID { get; set; }

        public DrawingsShowForm()
        {
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }

        public DrawingsShowForm(int _id)
        {
            ID = _id;
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }


        private void DrawingsShowForm_Load(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否确认完成生产？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                //录入计划实际结束时间
                string jsonA = Helpers.HttpHelper.GetJSON("http://" + Program.IP + "/api/Mms/WinFormClient/GetPlanActualEndTime?ppdID=" + ID + "&dTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var dataA = JsonConvert.DeserializeObject<dynamic>(jsonA);
                if (Convert.ToBoolean(dataA["result"]))
                {
                    //获取工艺路线，判断是否需要报检
                    var IsInspectionReport = Program.ProducePlanInfoModel.GetProductProcessRoute.IsInspectionReport ?? 0;
                    //不需要报检
                    if (IsInspectionReport.Equals(0))
                    {
                        //物料入库
                        new ConfirmMaterialForm(Program.ProducePlanInfoModel.GetProducePlanInfo.ID, Program.ProducePlanInfoModel, Program.MaterialDetailModel).InputStorage();
                        DialogResult = DialogResult.OK;
                    }
                    //需要报检
                    else
                    {
                        DateTime dTime = DateTime.Now;
                        string jsonB = Helpers.HttpHelper.PostJSON("http://" + Program.IP + "/api/Mms/WinFormClient/PostCheckRequest", new
                        {
                            ContractCode = Program.ProducePlanInfoModel.GetProducePlanInfo.ContractCode,
                            ProjectName = Program.ProducePlanInfoModel.GetProductProcessRoute.ProjectName,
                            ProductName = Program.ProducePlanInfoModel.GetProjectDetail.ProductName,
                            ProductFigureNumber = Program.ProducePlanInfoModel.GetProjectDetail.FigureNumber,
                            PartCode = Program.ProducePlanInfoModel.GetProducePlanInfo.PartCode,
                            PartName = Program.MaterialDetail.GetPart.PartName,
                            partFigure = Program.ProducePlanInfoModel.GetProductProcessRoute.FigureCode,
                            MaterialCode = Program.MaterialDetail.GetMaterialDetail.MaterialCode,
                            CheckQuantity = Program.ProducePlanInfoModel.GetProducePlanInfo.Quantity,
                            BillState = 2,
                            IsEnable = 1,
                            CreatePerson = Program.WorkGroupInfo.data.GetWorkGroupInfo.TeamName,
                            CreateTime = dTime,
                            ModifyPerson = Program.WorkGroupInfo.data.GetWorkGroupInfo.TeamName,
                            ModifyTime = dTime,
                            ProductPlanCode = Program.ProducePlanInfoModel.GetProducePlanInfo.ProductPlanCode
                        });

                        var dataB = JsonConvert.DeserializeObject<dynamic>(jsonB);
                        if (Convert.ToBoolean(dataB["result"]))
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("报检出现异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("录入生产结束数据异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

    }
}

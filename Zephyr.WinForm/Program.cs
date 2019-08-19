using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zephyr.WinForm
{
    static class Program
    {
        /// <summary>
        /// 登录班组详情
        /// </summary>
        public static Models.WorkGroupInfoModel WorkGroupInfo { get; set; }

        /// <summary>
        /// 生产计划列表
        /// </summary>
        public static Models.ProducePlanInfoModel ProducePlanInfo { get; set; }

        /// <summary>
        /// 已选择的生产计划
        /// </summary>
        public static Models.ProducePlanInfoModel.DataModel ProducePlanInfoModel { get; set; }

        /// <summary>
        /// 该生产计划所用的物料详情
        /// </summary>
        public static Models.MaterialDetailModel MaterialDetailModel { get; set; }

        /// <summary>
        /// 该计划生产的零件详情
        /// </summary>
        public static Models.MaterialDetailModel.DataModel MaterialDetail { get; set; }

        /// <summary>
        /// 验证身份失败的班组ID
        /// </summary>
        public static string WorkGroupIDTemp { get; set; }

        /// <summary>
        /// API地址
        /// </summary>
        public static string IP { get; set; }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IP = ConfigurationManager.AppSettings["API"].ToString();
            Form formLogin = new Form();

            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                //新身份登录
                WorkGroupIDTemp = args[0];
                formLogin = new Content.LoginForm(Content.LoginForm.ActivateTypeEnum.NewIdentity);
            }
            else
            {
                //运行程序登录
                formLogin = new Content.LoginForm(Content.LoginForm.ActivateTypeEnum.Normal);
            }

            formLogin.ShowDialog();

            if (formLogin.DialogResult == DialogResult.OK)
            {
                //登录成功的班组编号
                string teamCode = WorkGroupInfo.data.GetWorkGroupInfo.TeamCode;
                //获取任务计划
                string json = Helpers.HttpHelper.GetJSON("http://" + IP + "/api/Mms/WinFormClient/GetProducePlanInfoByTCode?teamCode=" + teamCode);
                ProducePlanInfo = JsonConvert.DeserializeObject<Models.ProducePlanInfoModel>(json);

                //如果有正在进行的计划，则直接进入下一个页面
                //查找正在进行的任务计划
                var planList = ProducePlanInfo.data.Where(item => item.GetProducePlanInfo.ActualStartTime != null && item.GetProducePlanInfo.ActualFinishTime == null).ToList();
                if (planList.Count > 0)
                {
                    //如果刚好有一个计划正在进行中,则直接进入工艺图纸界面
                    if (planList.Count.Equals(1))
                    {
                        //获取计划的ID
                        int planID = planList[0].GetProducePlanInfo.ID;
                        //获取该计划的详情
                        ProducePlanInfoModel = ProducePlanInfo.data.SingleOrDefault(item => item.GetProducePlanInfo.ID.Equals(planID));
                        //该计划生产的物料详情
                        string jsonB = Helpers.HttpHelper.GetJSON("http://" + IP + "/api/Mms/WinFormClient/GetMaterialDetailByPartCode?partCode=" + ProducePlanInfoModel.GetProducePlanInfo.PartCode);
                        MaterialDetailModel = JsonConvert.DeserializeObject<Models.MaterialDetailModel>(jsonB);

                        MaterialDetail = MaterialDetailModel.data.FirstOrDefault(item => item.GetPart.PartCode.Equals(ProducePlanInfoModel.GetProducePlanInfo.PartCode));

                        //直接进入工艺图纸界面
                        Form formDrawingsShow = new Content.DrawingsShowForm(planID);
                        formDrawingsShow.ShowDialog();
                        //如果生产完毕，重新选择计划
                        if (formDrawingsShow.DialogResult == DialogResult.OK)
                        {
                            string str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                            System.Diagnostics.Process.Start(str, WorkGroupInfo.data.GetWorkGroupInfo.TeamCode);
                            Application.Exit();
                        }
                    }
                    else
                    {
                        MessageBox.Show("已经开始进行多个计划任务！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    Application.Run(new Content.ProducePlanForm());
                }
            }

        }
    }
}

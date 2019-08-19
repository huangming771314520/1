using System.Collections.Generic;

namespace Model
{
    public class StoreInfoModel
    {
        private static readonly string localAddressIP = GetAddressIP();

        /// <summary>
        /// 用户登录或得到的令牌
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public static string UserCode { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 用户条码
        /// </summary>
        public static string UserBarCode { get; set; }

        /// <summary>
        /// 本机IP地址
        /// </summary>
        public static string LocalAddressIP { get { return localAddressIP; } }

        /// <summary>
        /// 用户登录所在班组信息
        /// </summary>
        public static WorkGroupInfoModel.DataModel WorkGroupInfo { get; set; }

        /// <summary>
        /// 所有任务计划
        /// </summary>
        public static List<ProducePlanInfoModel.DataModel> ProducePlanInfos { get; set; }

        /// <summary>
        /// 当前选择生产计划
        /// </summary>
        public static ProducePlanInfoModel.DataModel ProducePlanInfo { get; set; }

        /// <summary>
        /// 当前生产计划物料-零件及子集
        /// </summary>
        public static List<MaterialInfoModel.DataModel> MaterialInfos { get; set; }

        /// <summary>
        /// 该计划生产的零件详情
        /// </summary>
        public static MaterialInfoModel.DataModel MaterialInfo { get; set; }

        /// <summary>
        /// 改计划生产的零件子集详情
        /// </summary>
        public static List<MaterialInfoModel.DataModel> MaterialChildInfos { get; set; }

        /// <summary>
        /// 获取班组负责所有设备的保养计划
        /// </summary>
        public static List<EquipmentInfoModel.DataModel> EquipmentInfos { get; set; }

        /// <summary>
        /// 某台设备的保养计划
        /// </summary>
        public static EquipmentInfoModel.DataModel EquipmentInfo { get; set; }


        private static string GetAddressIP()
        {
            string AddressIP = string.Empty;
            foreach (System.Net.IPAddress _IPAddress in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

    }
}

using MesClient.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace MesClient
{
    public class StoreInfo
    {
        /// <summary>
        /// 登录令牌
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// 用户条码
        /// </summary>
        public static string BarCode { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public static string UserCode { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 当前登录用户其他信息
        /// </summary>
        public static UserDetailInfoEntity.ExtraInfoModel UserExtraInfo { get; set; }

        /// <summary>
        /// 所有计划&工票
        /// </summary>
        public static List<WorkingTicketEntity> WorkingTickets { get; set; }

        /// <summary>
        /// 当前选择的计划&工票
        /// </summary>
        public static WorkingTicketEntity CurrentWorkingTicket { get; set; }

        /// <summary>
        /// 当前选择的计划&工票 所用图纸及文件
        /// </summary>
        public static List<FileInfoEntity> CurrentPlanFiles { get; set; } = new List<FileInfoEntity>();

        /// <summary>
        /// 当前计划加工的零件本体
        /// </summary>
        public static MaterialInfoEntity.OneSelfModel MaterialOneSelf { get; set; }

        /// <summary>
        /// 当前计划加工的零件子集
        /// </summary>
        public static MaterialInfoEntity.ChildModel MaterialChild { get; set; }

        /// <summary>
        /// 下料计划 物料信息
        /// </summary>
        public static BlankingMaterialInfoEntity BlankingMaterial { get; set; }
    }
}

using MesClient.ViewModels;
using MesClient.Views;
using System;
using System.Windows;

namespace MesClient.API
{
    public class WindowHelper
    {
        /// <summary>
        /// 主窗体-视图模型
        /// </summary>
        public static MainViewModel MainViewModel;

        /// <summary>
        /// 跳转至登录页面
        /// </summary>
        public static Action ShowPageLogin = () => MainViewModel.CurrentPage = new PageLogin();

        /// <summary>
        /// 跳转至生产计划磁贴列表界面
        /// </summary>
        public static Action ShowPagePlanCard = () => MainViewModel.CurrentPage = new PagePlanCard();

        /// <summary>
        /// 跳转至计划执行人员记录页面
        /// </summary>
        public static Action ShowPageOperatorRecord = () => MainViewModel.CurrentPage = new PageOperatorRecord();

        /// <summary>
        /// 跳转至执行生产过程界面
        /// </summary>
        public static Action ShowPagePlanExecute = () => MainViewModel.CurrentPage = new PagePlanExecute();

        ///// <summary>
        ///// 跳转至下料记录页面
        ///// </summary>
        //public static Action ShowPageBlankingRecord = () => CurrentWindow.CurrentPage = new PageBlankingRecord();

        ///// <summary>
        ///// 跳转至保养计划磁贴列表界面
        ///// </summary>
        //public static Action ShowPageMaintainPlan = () => CurrentWindow.CurrentPage = new PageMaintainPlan();

        /// <summary>
        /// 是否打开扫描物料弹窗
        /// </summary>
        public static bool IsOpenMaterialScanWindow = false;

        /// <summary>
        /// 扫描物料弹窗
        /// </summary>
        public static Window MaterialScanWindow;

        /// <summary>
        /// 车间类型
        /// </summary>
        public static EnumManager.WorkShopTypeEnum WorkShopType = EnumManager.WorkShopTypeEnum.OtherShop;
    }
}

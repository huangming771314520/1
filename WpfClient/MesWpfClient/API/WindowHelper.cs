using MesWpfClient.ViewModels;
using MesWpfClient.Views;
using System;
using System.Windows;

namespace MesWpfClient.API
{
    public class WindowHelper
    {
        public static MainViewModel CurrentWindow;

        /// <summary>
        /// 跳转至登录页面
        /// </summary>
        public static Action ShowPageLogin = () => CurrentWindow.CurrentPage = new PageLogin();

        /// <summary>
        /// 跳转至生产计划磁贴列表界面
        /// </summary>
        public static Action ShowPageProducePlan = () => CurrentWindow.CurrentPage = new PageProducePlan();

        /// <summary>
        /// 跳转至计划执行人员记录页面
        /// </summary>
        public static Action ShowOperatorStatistical = () => CurrentWindow.CurrentPage = new PageOperatorStatistical();

        /// <summary>
        /// 跳转至执行生产过程界面
        /// </summary>
        public static Action ShowPageProduceProce = () => CurrentWindow.CurrentPage = new PageProduceProce();

        /// <summary>
        /// 跳转至下料记录页面
        /// </summary>
        public static Action ShowPageBlankingRecord = () => CurrentWindow.CurrentPage = new PageBlankingRecord();

        /// <summary>
        /// 跳转至保养计划磁贴列表界面
        /// </summary>
        public static Action ShowPageMaintainPlan = () => CurrentWindow.CurrentPage = new PageMaintainPlan();

        /// <summary>
        /// 是否打开扫描物料弹窗
        /// </summary>
        public static bool IsOpenMaterialScanWindow = false;

        /// <summary>
        /// 扫描物料弹窗
        /// </summary>
        public static Window MaterialScanWindow;

    }
}

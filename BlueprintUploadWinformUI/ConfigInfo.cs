using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueprintUploadWinformUI
{
    public class ConfigInfo
    {
        /// <summary>
        /// Web端API地址
        /// </summary>
        public static string API { get; set; }

        /// <summary>
        /// SignalR服务地址
        /// </summary>
        public static string SignalRURI { get; set; }

        /// <summary>
        /// FTP站点
        /// </summary>
        public static string FtpSite { get; set; }

        /// <summary>
        /// FTP用户名
        /// </summary>
        public static string FtpUid { get; set; }

        /// <summary>
        /// FTP密码
        /// </summary>
        public static string FtpPwd { get; set; }

        /// <summary>
        /// 本地唯一值（每个客户端 此值唯一）
        /// </summary>
        public static string LocalKey { get; set; }

        /// <summary>
        /// 图纸文件 存放的文件夹
        /// </summary>
        public static string FtpSiteDicPath { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update.Model
{
    public class RootLocal
    {
        /// <summary>
        /// 当前版本
        /// </summary>
        public static Version CurrentVersion { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public static string Workshop { get; set; }

        /// <summary>
        /// 服务器URL
        /// </summary>
        public static string ServerUrl { get; set; }

        /// <summary>
        /// 程序名
        /// </summary>
        public static string AppName { get; set; }

        /// <summary>
        /// 更新-XML文件名
        /// </summary>
        public static string XmlName { get; set; }

        /// <summary>
        /// 更新-Zip文件名
        /// </summary>
        public static string ZipName { get; set; }

    }
}

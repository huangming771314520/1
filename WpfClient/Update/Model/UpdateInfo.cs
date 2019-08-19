using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update.Model
{
    public class UpdateInfo
    {
        /// <summary>
        /// 程序名称
        /// </summary>
        public static string AppName { get; set; }

        /// <summary>
        /// 程序版本
        /// </summary>
        public static Version AppVersion { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public static DateTime UpdateTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public static string Describe { get; set; }

        /// <summary>
        /// 更新类型 All/Add
        /// </summary>
        public static string UpdateType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient
{
    public class EnumManager
    {
        /// <summary>
        /// 资源地址类型枚举
        /// </summary>
        public enum WebAddressTypeEnum
        {
            Other = 0,

            /// <summary>
            /// 资源地址类型为 http://xxx.com/xxx.xxx
            /// </summary>
            HTTP = 1,

            /// <summary>
            /// 资源地址类型为 ftp://xxx.com/xxx.xxx
            /// </summary>
            FTP = 2,

            /// <summary>
            /// 本地资源
            /// </summary>
            Local = 3
        }

        /// <summary>
        /// 生产日志类型
        /// </summary>
        public enum ProduceLogTypeEnum
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start = 0,

            /// <summary>
            /// 暂停
            /// </summary>
            Pause = 1,

            /// <summary>
            /// 结束
            /// </summary>
            End = 2
        }

        /// <summary>
        /// 生产计划状态类型
        /// </summary>
        public enum ProducePlanStateEnum
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start = 0,

            /// <summary>
            /// 结束
            /// </summary>
            End = 1
        }

        /// <summary>
        /// 生产过程状态
        /// </summary>
        public enum ProduceProceStateEnum
        {
            /// <summary>
            /// 完工
            /// </summary>
            CompleteTask = 0,

            /// <summary>
            /// 暂停
            /// </summary>
            PauseTask = 1,

            /// <summary>
            /// 取消
            /// </summary>
            Cancel = 2
        }

        /// <summary>
        /// 暂停原因
        /// </summary>
        public enum PauseResonEnum
        {
            /// <summary>
            /// 其他
            /// </summary>
            Other = 0,
            /// <summary>
            /// 休息或下班
            /// </summary>
            RestOrAfterWork = 1,
            /// <summary>
            /// 物料问题暂停
            /// </summary>
            MaterialProblem = 2,
            /// <summary>
            /// 质量问题暂停
            /// </summary>
            QualityProblem = 3
        }

        /// <summary>
        /// 车间类型
        /// </summary>
        public enum WorkShopTypeEnum
        {
            OtherShop = 0,

            /// <summary>
            /// 生产车间
            /// </summary>
            ProduceShop = 1,

            /// <summary>
            /// 下料车间
            /// </summary>
            BlankingShop = 2
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintBarCodeService.Entity
{
    /// <summary>
    /// 打印类型枚举
    /// </summary>
    public enum PrintTypeEnum
    {
        /// <summary>
        /// 人员信息打印
        /// </summary>
        PersonPrint = 1,

        /// <summary>
        /// 零件信息打印
        /// </summary>
        PartPrint = 2
    }

    public class PrintInfoEntity
    {
        public PrintTypeEnum PrintType { get; set; }

        public object Data { get; set; }
    }


    /// <summary>
    /// 人员条码打印信息实体类
    /// </summary>
    public class PersonPrintInfoEntity
    {

        ///// <summary>
        ///// 打码机名称
        ///// </summary>
        //public string PrintMachineName { get; set; }

        /// <summary>
        /// 人名
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 人编码
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// 部门名
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

    }

    /// <summary>
    /// 零件条码打印信息实体类
    /// </summary>
    public class PartPrintInfoEntity
    {
        ///// <summary>
        ///// 打码机名称
        ///// </summary>
        //public string PrintMachineName { get; set; }

        /// <summary>
        /// 合同
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 零件图号
        /// </summary>
        public string PartFigureNo { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

    }
}

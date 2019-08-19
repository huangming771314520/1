using System.Collections.Generic;

namespace ClientFilesUpLoad.Models
{
    public class DrawTaskInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            /// <summary>
            /// 主键ID
            /// </summary>
            public long ID { get; set; }

            /// <summary>
            /// 申请单号
            /// </summary>
            public string RequestCode { get; set; }

            /// <summary>
            /// 合同编号
            /// </summary>
            public string ContractCode { get; set; }

            /// <summary>
            /// 产品ID
            /// </summary>
            //public int? ProductID { get; set; }

            /// <summary>
            /// 产品名称
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 根零件编号
            /// </summary>
            //public string RootPartCode { get; set; }

            /// <summary>
            /// 子零件编号
            /// </summary>
            //public string PartCode { get; set; }

            /// <summary>
            /// 零件名称
            /// </summary>
            public string PartName { get; set; }

            /// <summary>
            /// 子零件图号
            /// </summary>
            public string FigureCode { get; set; }

            /// <summary>
            /// 申请原因
            /// </summary>
            public string ApplicationReason { get; set; }

            /// <summary>
            /// 申请状态
            /// </summary>
            public int? RequestStatus { get; set; }

            /// <summary>
            /// 状态名称
            /// </summary>
            public string RequestStatusName { get; set; }

            /// <summary>
            /// 创建人
            /// </summary>
            //public string CreatePerson { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            //public DateTime? CreateTime { get; set; }

            /// <summary>
            /// 是否删除
            /// </summary>
            //public int? IsEnable { get; set; }


            public string FileName { get; set; }

            public string FileAddress { get; set; }

            public string DocName { get; set; }
        }
    }
}

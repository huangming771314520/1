using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessBlueprintUpload.Models
{
    public class PRouteBlueprintParamsModel
    {
        public enum TypeEnum
        {
            /// <summary>
            /// 其他
            /// </summary>
            Other = 0,
            /// <summary>
            /// 新增
            /// </summary>
            Add = 1,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit = 2
        }

        public TypeEnum Type { get; set; }

        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

    }
}

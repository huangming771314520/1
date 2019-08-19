using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueprintUpload.Entity
{
    /// <summary>
    /// 工艺图纸上传 数据信息 实体类
    /// </summary>
    public class ProcessBlueprintEntity
    {
        public int BomID { get; set; }

        public string PartFigureCode { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueprintUpload.Entity
{
    /// <summary>
    /// 上传类型枚举
    /// </summary>
    public enum UploadTypeEnum
    {
        /// <summary>
        /// 工艺图纸上传
        /// </summary>
        ProcessBlueprint = 1,
        /// <summary>
        /// 工艺路线编制上传图纸
        /// </summary>
        PRouteBlueprint = 2
    }

    public class UploadInfoEntity
    {
        public UploadTypeEnum UploadType { get; set; }

        public object Data { get; set; }
    }

}

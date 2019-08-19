using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueprintUploadWinformUI.Models
{
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

    public class ResultModel
    {
        public bool Result { get; set; }

        public object Data { get; set; }

        public string Msg { get; set; }

    }

    public class ResultModel<T>
    {
        public bool Result { get; set; }

        public DataModel<T> Data { get; set; }

        public string Msg { get; set; }

    }

    public class DataModel<T>
    {
        public UploadTypeEnum UploadType { get; set; }

        public T ParamsModel { get; set; }

    }


}

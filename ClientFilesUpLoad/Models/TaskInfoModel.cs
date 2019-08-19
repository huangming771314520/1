using System;
using System.Collections.Generic;

namespace ClientFilesUpLoad.Models
{
    public class TaskInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public long Num { get; set; }

            public long TaskID { get; set; }

            public string ContractCode { get; set; }

            public string ProductName { get; set; }

            public string Model { get; set; }

            public string Specifications { get; set; }

            public string TaskTypeName { get; set; }

            public DateTime? TaskFinishDate { get; set; }

            public DateTime? ActualFinishTime { get; set; }

            public int? ProjectID { get; set; }

            public string ProjectName { get; set; }

            public int? ProjectDetailID { get; set; }

            public int? DesignType { get; set; }

            public string DesignTypeName { get; set; }

            public string BillCode { get; set; }

            public string FileName { get; set; }

            public string FileAddress { get; set; }

            public string DocName { get; set; }

            public string DesignDepartmentType { get; set; }

            public string DesignDepartmentName { get; set; }
        }

    }
}

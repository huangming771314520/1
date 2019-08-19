using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Model
{
    public class WorkGroupInfoModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public DataModel Data { get; set; }

        public class DataModel
        {
            public string GetToken { get; set; }

            public SYS_WorkGroupDetail GetWorkGroupDetail { get; set; }

            public SYS_WorkGroup GetWorkGroup { get; set; }

            public SYS_BN_Warehouse GetWarehouse { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BlankingRecordLeftModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public long ID { get; set; }

            public string ContractCode { get; set; }

            public string FigureCode { get; set; }

            public string PartName { get; set; }

            public string BlankingSize { get; set; }

            public string BiankingSize { get; set; }
        }
    }


    public class BlankingRecordRightModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public long ID { get; set; }

            public string BiankingSize { get; set; }
        }
    }
}

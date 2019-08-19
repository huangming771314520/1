using System.Collections.Generic;

namespace ClientFilesUpLoad.Models
{
    public class DesignTaskResultModel
    {
        public bool Result { get; set; }

        public string Msg { get; set; }

        public List<DataModel> Data { get; set; }

        public class DataModel
        {
            public string Value { get; set; }

            public string Text { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueprintUpload.Entity
{
    public class ResultModel
    {
        public bool Result { get; set; }

        public object Data { get; set; }

        public string Msg { get; set; }
    }

    public class ResultModel<T>
    {
        public bool Result { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }
    }

}

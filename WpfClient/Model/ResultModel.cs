using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ResultModel
    {
        public bool Result { get; set; }

        public object Data { get; set; }

        public string Msg { get; set; }
    }

    public class ResultModel<T> where T : class
    {
        public bool Result { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }
    }
}

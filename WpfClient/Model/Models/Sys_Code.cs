using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Sys_Code
    {
        public string Code { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        public string ParentCode { get; set; }

        public string Seq { get; set; }

        public bool? IsEnable { get; set; }

        public bool? IsDefault { get; set; }

        public string Description { get; set; }

        public string CodeTypeName { get; set; }

        public string CodeType { get; set; }

        public string CreatePerson { get; set; }

        public string CreateDate { get; set; }

        public string UpdatePerson { get; set; }

        public string UpdateDate { get; set; }
    }
}

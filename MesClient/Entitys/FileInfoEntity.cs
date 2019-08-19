using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Entitys
{
    public class FileInfoEntity
    {
        public string DocName { get; set; }

        public string FileName { get; set; }

        public string FileAddress { get; set; }

        public EnumManager.WebAddressTypeEnum WebAddressType { get; set; }
    }

    public class FileLineInfoEntity : FileInfoEntity
    {
        public int LineNum { get; set; }
    }

}

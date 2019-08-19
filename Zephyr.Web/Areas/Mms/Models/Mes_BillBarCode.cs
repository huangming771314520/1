using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class Mes_BillBarCodeService : ServiceBase<Mes_BillBarCode>
    {
       
    }

    public class Mes_BillBarCode : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int ID { get; set; }
        public string BillType { get; set; }
        public string BillCode { get; set; }
        public string BillBarcode { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

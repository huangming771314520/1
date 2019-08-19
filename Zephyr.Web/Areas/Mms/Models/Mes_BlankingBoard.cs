using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class Mes_BlankingBoardService : ServiceBase<Mes_BlankingBoard>
    {
       
    }

    public class Mes_BlankingBoard : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public string PartCode { get; set; }
        public string BoardInventoryCode { get; set; }
        public string BoardInventoryName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string FileAddress { get; set; }
        public string FileName { get; set; }
        public string DocName { get; set; }

        public string ProgramCode { get; set; }
    }
}

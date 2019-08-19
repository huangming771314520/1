using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_MaterialBatchService : ServiceBase<SYS_MaterialBatch>
    {
        public string CreateMateBarCode(long StartNum)
        {
            //700000000000
            long MateBarCode = 0;
            using (var db = Db.Context("Mms"))
            {
                string sql = $"SELECT ISNULL(MAX(CONVERT(BIGINT,MateBarCode)),{StartNum})+1 AS MateBarCode FROM SYS_MaterialBatch WHERE MateBarCode IS NOT NULL AND LTRIM(RTRIM(MateBarCode)) <> ''";
                if(StartNum== 700000000000)
                {
                    sql += " AND LEFT(MateBarCode,1)='7'";
                }
                MateBarCode = db.Sql(sql).QuerySingle<long>();
            }
            return MateBarCode.ToString().PadLeft(12, '0');
        }
    }

    public class SYS_MaterialBatch : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string MateBarCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string PartCode { get; set; }
        public string PartFigureCode { get; set; }
        public string PartName { get; set; }
        public string ContractCode { get; set; }
        public int ProductID { get; set; }
        public string MaterialCode { get; set; }
        public string InnerFactoryBatch { get; set; }
        public string OuterFactoryBatch { get; set; }
    }
}

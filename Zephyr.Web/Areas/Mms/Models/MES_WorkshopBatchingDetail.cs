using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Zephyr.Core;
using Zephyr.Utils.ExcelLibrary.BinaryFileFormat;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_WorkshopBatchingDetailService : ServiceBase<MES_WorkshopBatchingDetail>
    {
        public dynamic GetData(string figureNo, string parentPartCode)
        {
            try
            {
                if (string.IsNullOrEmpty(figureNo))
                {
                    throw new Exception(@"参数异常！");
                }
                string sqlPBomModel = string.Format(@"SELECT * FROM dbo.PRS_Process_BOM tbA WHERE tbA.IsEnable = 1 AND tbA.PartFigureCode = '{0}' AND tbA.ParentCode = '{1}'", figureNo, parentPartCode ?? "");
                PRS_Process_BOM pBomModel = db.Sql(sqlPBomModel).QuerySingle<PRS_Process_BOM>();

                if (pBomModel != null && (!string.IsNullOrEmpty(pBomModel.New_InventoryCode)))
                {
                    string sqlPartModel = string.Format(@"SELECT * FROM dbo.SYS_Part WHERE IsEnable = 1 AND InventoryCode = '{0}'", pBomModel.New_InventoryCode);
                    SYS_Part partModel = db.Sql(sqlPartModel).QuerySingle<SYS_Part>();
                    if (partModel == null)
                    {
                        throw new Exception(sqlPartModel + "  查询结果为空！");
                    }

                    object data = new {
                        PartCode = pBomModel.PartCode,
                        ParentCode = parentPartCode,
                        PartName = pBomModel.PartName,
                        PartFigureNumber = pBomModel.PartFigureCode,
                        MaterialCode = pBomModel.MaterialCode,
                        PartQuantity= pBomModel.PartQuantity,
                        BatchingCode="",
                        IsCrux = pBomModel.IsCrux
                    };

                    return new ResultModel()
                    {
                        Result = true,
                        Data = data
                    };
                }
                else
                {
                    throw new Exception(sqlPBomModel + "  查询 结果为空！");
                }
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }
    }

    public class MES_WorkshopBatchingDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string PartCode { get; set; }
        public string PartFigureNumber { get; set; }
        public string PartName { get; set; }
        public string MaterialCode { get; set; }
        public int? PartQuantity { get; set; }
        public int? Quantity { get; set; }
        public int? RequirementNum { get; set; }
        public int? BatchingNum { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string OutDeptCode { get; set; }
        public string BatchingCode { get; set; }
        public string ParentCode { get; set; }
        public int? IsCrux { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BarCreateMateService : ServiceBase<PRS_BarCreateMate>
    {
        public List<dynamic> GetBomList(string BomList = "")
        {
            var PRS_BarCreateMateList = new List<dynamic>();
            List<PRS_Process_BOM_Blanking> barDataList = new PRS_Process_BOM_BlankingService().GetModelList(ParamQuery.Instance().AndWhere("ID", BomList, Cp.In));

            //.AndWhere(Cp.In("ID", BomList)))
            if (!string.IsNullOrWhiteSpace(BomList))
            {
                #region 原SQL
                /*
select a.ContractCode,a.New_InventoryCode InventoryCode,b.InventoryName,b.Model,b.Spec as Specs,b.FigureCode,sum(a.SetMateNum) TotalNum,0 RealNum,sum(a.SetMateNum) NeedNum
from PRS_Process_BOM a
left join SYS_Part b on a.New_InventoryCode=b.InventoryCode
where a.MateType=2 and a.ID in({0}) AND ContractCode NOT IN (SELECT ContractCode FROM PRS_BarCreateMate WHERE a.IsEnable=1)
group by a.ContractCode,a.New_InventoryCode,b.InventoryName,b.Model,b.Spec,b.FigureCode
                 */
                #endregion

                var query = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings defaultOrderBy='temp.InventoryCode'>
        <select>*</select>
        <from>(
SELECT a.ContractCode,
       a.New_InventoryCode InventoryCode,
       b.InventoryName,
       b.Model,
       b.Spec AS Specs,
       b.FigureCode,
       SUM(a.SetMateNum) TotalNum,
       0 RealNum,
       SUM(a.SetMateNum) NeedNum
FROM dbo.PRS_Process_BOM_Blanking a
    LEFT JOIN SYS_Part b
        ON a.New_InventoryCode = b.InventoryCode
WHERE a.MateType IN (2,3)
      AND a.ID IN ( {0} )
      AND NOT EXISTS
(
    SELECT 1
    FROM PRS_BarCreateMate
    WHERE a.IsEnable = 1
          AND ContractCode = a.ContractCode
          AND InventoryCode = a.New_InventoryCode
)
GROUP BY a.ContractCode,
         a.New_InventoryCode,
         b.InventoryName,
         b.Model,
         b.Spec,
         b.FigureCode
) temp</from>
    </settings>", BomList);
                var service = new PRS_BarCreateMateService();
                PRS_BarCreateMateList = service.GetDynamicList(query.ToParamQuery()).ToList<dynamic>();
            }
            return PRS_BarCreateMateList;
        }

        public List<PRS_Process_BOM> GetIsSum(string BomList = "")
        {
            var PRS_BarCreateMateList = new List<dynamic>();
            List<PRS_Process_BOM> barDataList = new PRS_Process_BOMService().GetModelList(ParamQuery.Instance().AndWhere("ID", BomList, Cp.In));
            return barDataList;
        }

        public List<PRS_Process_BOM_Blanking> GetIsSum2(string BomList = "")
        {
            var PRS_BarCreateMateList = new List<dynamic>();
            List<PRS_Process_BOM_Blanking> barDataList = new PRS_Process_BOM_BlankingService().GetModelList(ParamQuery.Instance().AndWhere("ID", BomList, Cp.In));
            return barDataList;
        }
    }

    public class PRS_BarCreateMate : ModelBase
    {
        [PrimaryKey]
        [Identity]
        public long ID { get; set; }
        public string ContractCode { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string Model { get; set; }
        public string Specs { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public double? TotalNum { get; set; }
        public double? RealNum { get; set; }
        public double? NeedNum { get; set; }
        public int? ApproveState { get; set; }
        public string InventoryNum { get; set; }
        public string Remark { get; set; }
        public string Unit { get; set; }

    }
}

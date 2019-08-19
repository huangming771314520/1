using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_MaterialPickDetailService : ServiceBase<MES_MaterialPickDetail>
    {
        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;

            var rowsAffected = 0;
            string sql = String.Format(@"SELECT t1.*,
       t2.InventoryCode,
       t2.InventoryName,
       t2.RequiredQuantity,
       t2.Model,
       t2.Unit,
       t2.Material
      
FROM dbo.MES_MaterialPickMain t1
    INNER JOIN dbo.MES_MaterialPickDetail t2
        ON t1.ID = t2.MainID
           AND t1.IsEnable = 1
           AND t2.IsEnable = 1
 where t1.BillCode='{0}'", BillCode);
            string insertSql = "";
            dynamic MaterialPick = db.Sql(sql).QueryMany<dynamic>();
            if (MaterialPick.Count == 0)
            {
                msg = "请保存数据后审核";
                return 0;
            }
            if (MaterialPick[0].BillState == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                using (var trans = db.UseTransaction(true))
                {
                    sql = string.Format(@"update MES_MaterialPickMain set BillState=2 where BillCode='{0}'", BillCode);
                    rowsAffected = db.Sql(sql).Execute();
                    var dno = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "LLCK", "", "");
                    insertSql = string.Format(@" INSERT dbo.WMS_BN_BillMain
	(
	    ID,
	    BillCode,
	    BillType,
	    ContractCode,
	    ProjectName,
	    DepartmentID,
	    DepartmentName,
	    WarehouseCode,
	    WarehouseName,
	    ApproveState,
	    Remark,
	    CreatePerson,
	    CreateTime,
	    ModifyPerson,
	    ModifyTime,
	    IsEnable,
        PickPerson,
        PickPersonCode
	)
	VALUES
	(   (select MAX(id)+1 from WMS_BN_BillMain),         -- ID - int                                         ID,
	    '{0}',        -- BillCode - varchar(50)                                                             BillCode,
	    ( SELECT Value FROM HBHC_Sys.dbo.sys_code WHERE Text='领料出库' ),                                  --BillType,
	   '{1}','{2}','{3}','{4}','{5}','{6}',1,'','{7}','{8}','{9}','{10}',1,'{11}','{12}'           
	    )", dno, MaterialPick[0].ContractCode, MaterialPick[0].ProjectName, MaterialPick[0].DepartmentID, MaterialPick[0].DepartmentName,
          MaterialPick[0].WarehouseCode, MaterialPick[0].WarehouseName, MmsHelper.GetUserName(), DateTime.Now, MmsHelper.GetUserName(), DateTime.Now, MmsHelper.GetUserName(), MmsHelper.GetUserCode());
                    foreach (var item in MaterialPick)
                    {
                        insertSql += string.Format(@"INSERT INTO dbo.WMS_BN_BillDetail
(
    BillCode,
    InventoryCode,
    InventoryName,
    Specs,
    Unit,
    MateNum,
    ConfirmNum,
    PBillCode,
    Remark,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    IsEnable
)
VALUES
(   '{0}',        -- BillCode - varchar(50)
    N'{1}',       -- InventoryCode - nvarchar(50)
    N'{2}',       -- InventoryName - nvarchar(50)
    '{3}',        -- Specs - varchar(50)
    N'{4}',       -- Unit - nvarchar(10)
    '{5}',       -- MateNum - float
    '{6}',       -- ConfirmNum - float
    '{7}',        -- PBillCode - varchar(50)
    N'',       -- Remark - nvarchar(500)
    N'{8}',       -- CreatePerson - nvarchar(50)
    GETDATE(), -- CreateTime - datetime
    N'{9}',       -- ModifyPerson - nvarchar(50)
    GETDATE(), -- ModifyTime - datetime
    1          -- IsEnable - int
    )", dno, item.InventoryCode, item.InventoryName, item.Model, item.Unit, item.RequiredQuantity,
           item.RequiredQuantity, item.BillCode, MmsHelper.GetUserName(), MmsHelper.GetUserName());
                    }

                    var res = db.Sql(insertSql).Execute();
                    if (rowsAffected > 0 && res == MaterialPick.Count + 1)
                    {
                        msg = "检验记录审核成功";
                        trans.Commit();
                        return rowsAffected;
                    }
                    else
                    {
                        trans.Rollback();
                        msg = "检验记录审核失败";
                        return 0;
                    }
                }

            }
        }
    }

    public class MES_MaterialPickDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int MainID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }

        /// <summary>
        /// 型号规格
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; }
        public int RequiredQuantity { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

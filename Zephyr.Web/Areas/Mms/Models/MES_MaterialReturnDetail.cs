using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_MaterialReturnDetailService : ServiceBase<MES_MaterialReturnDetail>
    {
        public List<dynamic> GetMaterialReturnDetail(string ID)
        {
            string sql = string.Format(@"select *,RequiredQuantity as ReturnQuantity from MES_MaterialPickDetail where MainID='{0}'", ID);
            return db.Sql(sql).QueryMany<dynamic>();
        }

        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;

            var rowsAffected = 0;
            string sql = String.Format(@"SELECT t1.*,
       t2.InventoryCode,
       t2.InventoryName,
       t2.ReturnQuantity,
       t2.Model,
       t2.Unit,
       t2.Material
FROM dbo.MES_MaterialReturnMain t1
    INNER JOIN dbo.MES_MaterialReturnDetail t2
        ON t1.ID = t2.MainID
           AND t1.IsEnable = 1
           AND t2.IsEnable = 1
 where t1.BillCode='{0}'", BillCode);
            string insertSql = "";
            dynamic MaterialPick = db.Sql(sql).QueryMany<dynamic>();
            if (MaterialPick.Count == 0)
            {
                msg = "请保存数据后审核";
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
                    sql = string.Format(@"update MES_MaterialReturnMain set BillState=2 where BillCode='{0}'", BillCode);
                    rowsAffected = db.Sql(sql).Execute();
                    var dno = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "TLRK", "", "");
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
	    IsEnable
	)
	VALUES
	(   (select MAX(id)+1 from WMS_BN_BillMain),         -- ID - int                                         ID,
	    '{0}',        -- BillCode - varchar(50)                                                             BillCode,
	    ( SELECT Value FROM HBHC_Sys.dbo.sys_code WHERE Text='退料入库' ),                                  --BillType,
	   '{1}','{2}','{3}','{4}','{5}','{6}',1,'','{7}','{8}','{9}','{10}',1           
	    )", dno, MaterialPick[0].ContractCode, MaterialPick[0].ProjectName, MaterialPick[0].DepartmentID, MaterialPick[0].DepartmentName,
          MaterialPick[0].WarehouseCode, MaterialPick[0].WarehouseName, MmsHelper.GetUserName(), DateTime.Now, MmsHelper.GetUserName(), DateTime.Now);
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
    )", dno, item.InventoryCode, item.InventoryName, item.Model, item.Unit, item.ReturnQuantity,
           item.ReturnQuantity, item.BillCode, MmsHelper.GetUserName(), MmsHelper.GetUserName());
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


        public dynamic NewAuditBillCode(string billCode)
        {
            try
            {
                if (string.IsNullOrEmpty(billCode))
                {
                    throw new Exception(@"参数错误！");
                }

                string sqlA = string.Format(@"SELECT * FROM dbo.MES_MaterialReturnMain WHERE IsEnable = 1 AND BillCode = '{0}';", billCode);
                List<MES_MaterialReturnMain> listA = db.Sql(sqlA).QueryMany<MES_MaterialReturnMain>();

                DateTime newDT = DateTime.Now;

                if (listA.Count <= 0)
                {
                    throw new Exception(@"请保存数据后审核！");
                }

                if (listA[0].BillState != null && listA[0].BillState.Equals(2))
                {
                    throw new Exception(@"单据已审核！");
                }

                int billType = 0;
                using (var dbA = Db.Context("Sys"))
                {
                    var sysCodeModel = dbA.Sql(@"SELECT * FROM HBHC_Sys.dbo.sys_code WHERE Text='退料入库';").QuerySingle<sys_code>();
                    if (sysCodeModel == null)
                    {
                        throw new Exception(@"字典表查不到【退料入库】数据信息！");
                    }
                    else
                    {
                        billType = Convert.ToInt32(sysCodeModel.Value);
                    }
                }

                using (var dbB = Db.Context("Mms"))
                {
                    try
                    {
                        dbB.UseTransaction(true);

                        string sqlB = string.Format(@"UPDATE dbo.MES_MaterialReturnMain SET BillState = 2 WHERE IsEnable = 1 AND BillCode = '{0}';", billCode);
                        int tempA = dbB.Sql(sqlB).Execute();

                        var dno = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "TLRK", "", "");

                        string sqlC = WinFormClientService.GetInsertSQL(new WMS_BN_BillMain()
                        {
                            ID = dbB.Sql("SELECT ISNULL(MAX(ID),1)+1 FROM dbo.WMS_BN_BillMain;").QuerySingle<int>(),
                            BillCode = dno,
                            BillType = billType,
                            ContractCode = listA[0].ContractCode,
                            ProjectName = listA[0].ProjectName,
                            DepartmentID = listA[0].DepartmentID == null ? null : listA[0].DepartmentID.ToString(),
                            DepartmentName = listA[0].DepartmentName,
                            WarehouseCode = listA[0].WarehouseCode,
                            WarehouseName = listA[0].WarehouseName,
                            ApproveState = "2",
                            Remark = "",
                            CreatePerson = MmsHelper.GetUserName(),
                            CreateTime = newDT,
                            ModifyPerson = MmsHelper.GetUserName(),
                            ModifyTime = newDT,
                            IsEnable = 1
                        });
                        int tempB = dbB.Sql(sqlC).Execute();

                        var listB = dbB.Sql(string.Format(@"SELECT * FROM dbo.MES_MaterialReturnDetail WHERE IsEnable = 1 AND MainID = {0};", listA[0].ID)).QueryMany<MES_MaterialReturnDetail>();

                        StringBuilder sb = new StringBuilder();
                        foreach (var item in listB)
                        {
                            WMS_BN_BillDetail bDetailModel = new WMS_BN_BillDetail()
                            {
                                BillCode = dno,
                                InventoryCode = item.InventoryCode,
                                InventoryName = item.InventoryName,
                                Specs = item.Model,
                                Unit = item.Unit,
                                MateNum = item.ReturnQuantity,
                                ConfirmNum = item.ReturnQuantity,
                                PBillCode = billCode,
                                Remark = "",
                                CreatePerson = MmsHelper.GetUserName(),
                                CreateTime = newDT,
                                ModifyPerson = MmsHelper.GetUserName(),
                                ModifyTime = newDT,
                                IsEnable = 1
                            };

                            var realStocks = db.Sql(string.Format(@"SELECT * FROM dbo.WMS_BN_RealStock WHERE IsEnable = 1 AND WarehouseCode = '{0}' AND InventoryCode = '{1}' ORDER BY BatchCode", listA[0].WarehouseCode, item.InventoryCode)).QueryMany<WMS_BN_RealStock>();
                            WMS_BN_RealStock realStock = null;

                            if (realStocks.Count <= 0)
                            {
                                throw new Exception(@"仓库没有物料！");
                            }
                            else
                            {
                                var num = Convert.ToInt32(bDetailModel.ConfirmNum);

                                foreach (var i in realStocks)
                                {
                                    if (i.RealStock == null)
                                    {
                                        throw new Exception(@"仓库物料数量异常！");
                                    }
                                    else
                                    {
                                        if ((i.RealStock ?? 0) < num)
                                        {

                                        }
                                        else
                                        {
                                            realStock = i;
                                            bDetailModel.BatchCode = i.BatchCode;
                                            break;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(bDetailModel.BatchCode))
                                {
                                    throw new Exception(@"仓库物料数量不够，无法出库！");
                                }
                            }

                            sb.Append(WinFormClientService.GetInsertSQL(bDetailModel));

                            sb.Append(WinFormClientService.GetUpdateSQL(nameof(WMS_BN_RealStock), new KeyValuePair<string, object>("ID", realStock.ID), new
                            {
                                RealStock = realStock.RealStock - bDetailModel.ConfirmNum,
                                TotalStock = realStock.TotalStock - bDetailModel.ConfirmNum,
                                ModifyPerson = bDetailModel.ModifyPerson,
                                ModifyTime = bDetailModel.ModifyTime,
                            }));
                        }

                        string sqlD = sb.ToString();
                        int tempC = dbB.Sql(sqlD).Execute();

                        bool result = tempA > 0 && tempB > 0 && tempC > 0;

                        if (result)
                        {
                            dbB.Commit();
                        }
                        else
                        {
                            dbB.Rollback();
                        }

                        return new ResultModel()
                        {
                            Result = result,
                            Msg = result ? @"审核成功！" : "审核失败！"
                        };
                    }
                    catch (Exception ex)
                    {
                        dbB.Rollback();

                        throw new Exception(ex.Message);
                    }
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

    public class MES_MaterialReturnDetail : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public int? ReturnQuantity { get; set; }

        public string Unit { get; set; }
        public string Model { get; set; }
        public string Material { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

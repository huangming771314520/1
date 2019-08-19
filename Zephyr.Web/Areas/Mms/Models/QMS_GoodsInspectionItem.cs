using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_GoodsInspectionItemService : ServiceBase<QMS_GoodsInspectionItem>
    {
        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;

            var rowsAffected = 0;
            string sql = String.Format(@"SELECT t1.*,
       t3.ProductPurchaseCode,
       t4.Model,
       t4.Unit,
       t4.UnitPrice,
       t4.TotalPrice
FROM dbo.QMS_GoodsCheck t1
    INNER JOIN dbo.MES_GoodsInspection t2
        ON t1.PBillCode = t2.BillCode
           AND t2.IsEnable = 1
    INNER JOIN dbo.MES_PurchaseOrderMain t3
        ON t2.PurchaseOrderCode = t3.BillCode
           AND t3.IsEnable = 1
           AND t3.BillState = 2
    INNER JOIN dbo.MES_PurchaseOrderDetail t4
        ON t4.MainID = t3.ID
           AND t4.IsEnable = 1 where t1.BillCode='{0}'", BillCode);
            string insertSql = "";
            dynamic GoodsCheck = db.Sql(sql).QueryMany<dynamic>();
            if (GoodsCheck.Count == 0)
            {
                msg = "请保存数据后审核";
                return 0;
            }
            if (GoodsCheck[0].BillState == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                using (var trans = db.UseTransaction(true))
                {
                    #region
                    /*
                    sql = string.Format(@"update QMS_GoodsCheck set BillState=2 where BillCode='{0}'", BillCode);
                    rowsAffected = db.Sql(sql).Execute();
                    var dno = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "CGRK", "", "");
                    insertSql = string.Format(@" INSERT dbo.WMS_BN_BillMain
	(
	    ID,
	    BillCode,
	    BillType,
	    ContractCode,
	    ProjectName,
	    SalesmanCode,
	    Salesman,
	    DepartmentID,
	    DepartmentName,
	    SupplierCode,
	    SupplierName,
	    WarehouseCode,
	    WarehouseName,
	    ApproveState,
	    OrderBillCode,
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
	    ( SELECT Value FROM HBHC_Sys.dbo.sys_code WHERE Text='采购入库' ),                                  --BillType,
	    '{1}',        -- ContractCode - varchar(50)                                                         ContractCode,
	    '{2}',        -- ProjectName - varchar(50)                                                          ProjectName,
	    '{3}',        -- SalesmanCode - varchar(50)                                                         SalesmanCode,
	    '{4}',        -- Salesman - varchar(50)                                                             Salesman,
	    '{5}',        -- DepartmentID - varchar(50)                                                         DepartmentID,
	    '{6}',        -- DepartmentName - varchar(50)                                                       DepartmentName,
	    '{7}',        -- SupplierCode - varchar(50)                                                         SupplierCode,
	    '{8}',        -- SupplierName - varchar(50)                                                         SupplierName,
	    '{9}',        -- WarehouseCode - varchar(50)                                                        WarehouseCode,
	    '{10}',        -- WarehouseName - varchar(50)                                                       WarehouseName,
	    '1',                                                                                            -- ApproveState,
	    '{11}',        --OrderBillCode,                                                  
	    '{12}',        --Remark,                                                        
	    '{13}',        --CreatePerson,)                                                       
	    GETDATE(), -- CreateTime,                                                        
	    '{14}',           --ModifyPerson,                                                        
	     GETDATE(),        -- ModifyTime,                                                        
	     1        -- IsEnable                                                        
	    )", dno, GoodsCheck[0].ContractCode, GoodsCheck[0].ProjectName, GoodsCheck[0].SalesmanCode, GoodsCheck[0].Salesman, GoodsCheck[0].DepartmentID, GoodsCheck[0].DepartmentName, GoodsCheck[0].SupplierCode,
          GoodsCheck[0].SupplierName, GoodsCheck[0].WarehouseID, GoodsCheck[0].WarehouseName, GoodsCheck[0].ProductPurchaseCode, "", MmsHelper.GetUserName(), MmsHelper.GetUserName());
                    insertSql += string.Format(@"INSERT INTO dbo.WMS_BN_BillDetail
(
    BillCode,
    OrderBillCode,
    InventoryCode,
    InventoryName,
    Specs,
    Unit,
    MateNum,
    ConfirmNum,
    UnitPrice,
    TotalPrice,
    BatchCode,
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
    '{1}',        -- OrderBillCode - varchar(50)
    N'{2}',       -- InventoryCode - nvarchar(50)
    N'{3}',       -- InventoryName - nvarchar(50)
    '{4}',        -- Specs - varchar(50)
    N'{5}',       -- Unit - nvarchar(10)
    '{6}',       -- MateNum - float
    '{7}',       -- ConfirmNum - float
    '{8}',       -- UnitPrice - float
    '{9}',       -- TotalPrice - float
    '{10}',        -- BatchCode - varchar(50)
    '{11}',        -- PBillCode - varchar(50)
    N'',       -- Remark - nvarchar(500)
    N'{12}',       -- CreatePerson - nvarchar(50)
    GETDATE(), -- CreateTime - datetime
    N'{13}',       -- ModifyPerson - nvarchar(50)
    GETDATE(), -- ModifyTime - datetime
    1          -- IsEnable - int
    )", dno, GoodsCheck[0].ProductPurchaseCode, GoodsCheck[0].InventoryCode, GoodsCheck[0].InventoryName, GoodsCheck[0].Model, GoodsCheck[0].Unit, GoodsCheck[0].QualifiedQuantity,
     GoodsCheck[0].QualifiedQuantity, GoodsCheck[0].UnitPrice, GoodsCheck[0].TotalPrice, GoodsCheck[0].BatchCode, GoodsCheck[0].BillCode, MmsHelper.GetUserName(), MmsHelper.GetUserName());
                    
                     var res = db.Sql(insertSql).Execute();
                    if (rowsAffected > 0 && res == 2)
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
                     */
                    #endregion

                    string sqlA = string.Format(@"update QMS_GoodsCheck set BillState=2 where BillCode='{0}'", BillCode);
                    string bCode = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "CGRK", "", "");
                    DateTime newDT = DateTime.Now;
                    string sqlB = WinFormClientService.GetInsertSQL(new WMS_BN_BillMain()
                    {
                        ID = db.Sql("select MAX(id)+1 from WMS_BN_BillMain").QuerySingle<int>(),
                        BillCode = bCode,
                        BillType = db.Sql("SELECT Value FROM HBHC_Sys.dbo.sys_code WHERE Text='采购入库'").QuerySingle<int>(),
                        ContractCode = GoodsCheck[0].ContractCode,
                        ProjectName = GoodsCheck[0].ProjectName,
                        SalesmanCode = GoodsCheck[0].SalesmanCode,
                        Salesman = GoodsCheck[0].Salesman,
                        DepartmentID = GoodsCheck[0].DepartmentID,
                        DepartmentName = GoodsCheck[0].DepartmentName,
                        SupplierCode = GoodsCheck[0].SupplierCode,
                        SupplierName = GoodsCheck[0].SupplierName,
                        WarehouseCode = GoodsCheck[0].WarehouseID,
                        WarehouseName = GoodsCheck[0].WarehouseName,
                        ApproveState = "1",
                        OrderBillCode = GoodsCheck[0].ProductPurchaseCode,
                        Remark = string.Empty,
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = newDT,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = newDT,
                        IsEnable = 1
                    });
                    string sqlC = WinFormClientService.GetInsertSQL(new WMS_BN_BillDetail()
                    {
                        BillCode = bCode,
                        OrderBillCode = GoodsCheck[0].ProductPurchaseCode,
                        InventoryCode = GoodsCheck[0].InventoryCode,
                        InventoryName = GoodsCheck[0].InventoryName,
                        Specs = GoodsCheck[0].Model,
                        Unit = GoodsCheck[0].Unit,
                        MateNum = GoodsCheck[0].QualifiedQuantity,
                        ConfirmNum = GoodsCheck[0].QualifiedQuantity,
                        UnitPrice = GoodsCheck[0].UnitPrice,
                        TotalPrice = GoodsCheck[0].TotalPrice,
                        BatchCode = GoodsCheck[0].BatchCode,
                        PBillCode = GoodsCheck[0].BillCode,
                        Remark = string.Empty,
                        CreatePerson = MmsHelper.GetUserName(),
                        CreateTime = newDT,
                        ModifyPerson = MmsHelper.GetUserName(),
                        ModifyTime = newDT,
                        IsEnable = 1
                    });

                    string sqlTemp = string.Format(@"
SELECT tbB.* FROM MES_PurchaseOrderMain tbA LEFT JOIN MES_PurchaseOrderDetail tbB ON tbA.ID = tbB.MainID AND tbB.IsEnable = 1 WHERE tbA.IsEnable = 1
AND tbB.InventoryCode = '{0}'
AND tbA.BillCode IN (
SELECT PurchaseOrderCode FROM dbo.MES_GoodsInspection WHERE IsEnable = 1 AND BillCode = '{1}'
)
", GoodsCheck[0].InventoryCode, GoodsCheck[0].PBillCode);
                    MES_PurchaseOrderDetail mES_PurchaseOrderDetail = db.Sql(sqlTemp).QuerySingle<MES_PurchaseOrderDetail>();

                    if (mES_PurchaseOrderDetail != null)
                    {
                        string sqlD = WinFormClientService.GetUpdateSQL(mES_PurchaseOrderDetail, new
                        {
                            ModifyPerson = MmsHelper.GetUserName(),
                            ModifyTime = newDT,
                            CheckedQuantity = (mES_PurchaseOrderDetail.CheckedQuantity ?? 0) + (GoodsCheck[0].QualifiedQuantity ?? 0)
                        });

                        bool tempA = db.Sql(sqlA).Execute() > 0;
                        bool tempB = db.Sql(sqlB).Execute() > 0;
                        bool tempC = db.Sql(sqlC).Execute() > 0;
                        bool tempD = db.Sql(sqlD).Execute() > 0;

                        bool result = tempA && tempB && tempC && tempD;
                        if (result)
                        {
                            trans.Commit();
                            msg = "检验记录审核成功";
                            return 1;
                        }
                        else
                        {
                            trans.Rollback();
                            msg = "检验记录审核失败";
                            return 0;
                        }
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

    public class QMS_GoodsInspectionItem : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int? MainID { get; set; }
        /// <summary>
        /// 来料检验项目序号
        /// </summary>
        public string GoodsCheckItemCode { get; set; }
        public string BillCode { get; set; }
        /// <summary>
        /// 检验项目编码
        /// </summary>
        public string InspectionItemCode { get; set; }
        public string InspectionItemName { get; set; }
        /// <summary>
        /// 技术要求
        /// </summary>
        public string InspectionStandard { get; set; }
        /// <summary>
        /// 实测记录
        /// </summary>
        public string InspectionResult { get; set; }
        public int? IsQualified { get; set; }


        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

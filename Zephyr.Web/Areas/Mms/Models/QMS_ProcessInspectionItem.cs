using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using System.Linq;


//using System;
//using System.Collections.Generic;
//using System.Text;
//using Zephyr.Core;
//using System.Linq;
//using Zephyr.Areas.Areas.Mms.Common;
//using Zephyr.Areas.Mms.Controllers;
//using System.Net.Http;
//using System.Net;
//using System.Web;
//using System.Xml;
//using System.IO;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_ProcessInspectionItemService : ServiceBase<QMS_ProcessInspectionItem>
    {
        public int AuditBillCode(string BillCode, out string msg)
        {
            msg = string.Empty;
            //var rowsAffected = 0;
            string sql = String.Format(@" 
SELECT t1.*,
t1.QualifiedQuntity,
       t3.WarehouseID,
       t3.WarehouseName,
       t2.CheckQuantity
       ,t4.QuantityUnit
	   ,t4.Model
	   , t4.InventoryCode
	   ,t4.InventoryName
	   ,t3.WorkshopID
	   ,t3.WorkshopName
FROM dbo.QMS_ProcessInspection t1
    LEFT JOIN MES_ProcessInspectionRequest t2
        ON t1.PBillCode = t2.BillCode
           AND t1.IsEnable = 1
           AND t2.IsEnable = 1
           AND t2.BillState = 2
    LEFT JOIN dbo.MES_BN_ProductProcessRoute t3
        ON t2.ProductProcessRouteID = t3.ID
           AND t3.IsEnable = 1
		   inner JOIN dbo.SYS_Part t4 ON t1.PartCode=t4.PartCode
		   AND t4.IsEnable=1  where t1.BillCode='{0}'", BillCode);
            dynamic stateList = db.Sql(sql).QueryMany<dynamic>();
            if (stateList[0].BillState == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                sql = string.Format(@"update QMS_ProcessInspection set BillState=2 where BillCode='{0}'", BillCode);
                //string insertSql=string.Format(@"")
               
                using (db.UseTransaction(true))
                {
                    int countMain = db.Sql(sql).Execute();
                    string documentNo = "";
                    documentNo = MmsHelper.GetOrderNumber("WMS_BN_BillMain", "BillCode", "ZXRK", "", "");
                    //billstate = 2;
                    #region WMS_BN_BillMain表
                    string sqlA = string.Format(@"
INSERT INTO dbo.WMS_BN_BillMain
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
    PickPersonCode,
    PickPerson,
    ApproveState,
    ApprovePerson,
    ApproveDate,
    ApproveRemark,
    Remark,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    IsEnable
)
VALUES
(   {0},         -- ID - int
    '{1}',        -- BillCode - varchar(50)
    {2},         -- BillType - int
    '{3}',        -- ContractCode - varchar(50)
    '{4}',        -- ProjectName - varchar(50)
    '',        -- SalesmanCode - varchar(50)
    '',        -- Salesman - varchar(50)
    '',        -- DepartmentID - varchar(50)
    '',        -- DepartmentName - varchar(50)
    '',        -- SupplierCode - varchar(50)
    '',        -- SupplierName - varchar(50)
    '{5}',        -- WarehouseCode - varchar(50)
    '{6}',        -- WarehouseName - varchar(50)
    '',        -- PickPersonCode - varchar(50)
    '',        -- PickPerson - varchar(50)
    '1',        -- ApproveState - varchar(50)
    '',        -- ApprovePerson - varchar(50)
    GETDATE(), -- ApproveDate - datetime
    '',        -- ApproveRemark - varchar(500)
    '',        -- Remark - varchar(500)
    '{7}',        -- CreatePerson - varchar(50)
    GETDATE(), -- CreateTime - datetime
    '{7}',        -- ModifyPerson - varchar(50)
    GETDATE(), -- ModifyTime - datetime
    1          -- IsEnable - int
)
", Convert.ToInt32(new WMS_BN_BillMainService().GetNewKey("ID", "Maxplus")), documentNo, 8, stateList[0].ContractCode,
    stateList[0].ProjectName, stateList[0].WarehouseID, stateList[0].WarehouseName, MmsHelper.GetUserName());
                    #endregion

                    int countA = db.Sql(sqlA).Execute();

                    #region WMS_BN_BillDetail表
                    string sqlB = string.Format(@"
INSERT INTO dbo.WMS_BN_BillDetail
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
    PackageCode,
    BatchCode,
    PBillCode,
    AccountabilityCode,
    Remark,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    IsEnable
)
VALUES
(   '{0}',        -- BillCode - varchar(50)
    '',        -- OrderBillCode - varchar(50)
    N'{1}',       -- InventoryCode - nvarchar(50)
    N'{2}',       -- InventoryName - nvarchar(50)
    '{3}',        -- Specs - varchar(50)
    N'{4}',       -- Unit - nvarchar(10)
    {5},       -- MateNum - float
    {6},       -- ConfirmNum - float
    0.0,       -- UnitPrice - float
    0.0,       -- TotalPrice - float
    '',        -- PackageCode - varchar(50)
    '',        -- BatchCode - varchar(50)
    '',        -- PBillCode - varchar(50)
    '',        -- AccountabilityCode - varchar(50)
    N'',       -- Remark - nvarchar(500)
    N'{7}',       -- CreatePerson - nvarchar(50)
    GETDATE(), -- CreateTime - datetime
    N'{7}',       -- ModifyPerson - nvarchar(50)
    GETDATE(), -- ModifyTime - datetime
    1          -- IsEnable - int
)
", documentNo, stateList[0].InventoryCode, stateList[0].InventoryName, stateList[0].Model,
    stateList[0].Unit, stateList[0].QualifiedQuntity, stateList[0].QualifiedQuntity, MmsHelper.GetUserName());
                    #endregion

                    int countB = db.Sql(sqlB).Execute();
                    int countC = 0;
                    bool aaa = true;
                    bool ccc = true;
                    //更新库存表

                    //库存表
                    List<WMS_BN_RealStock> realStockData = db.Sql("SELECT * FROM WMS_BN_RealStock WHERE IsEnable = 1").QueryMany<WMS_BN_RealStock>();

                    var realStocks = realStockData.Where(s =>
                    {
                        bool a = s.WarehouseCode == null ? false : s.WarehouseCode.Equals(stateList[0].WarehouseID);
                        bool b = s.InventoryCode == null ? false : s.InventoryCode.Equals(stateList[0].InventoryCode);
                        bool c = s.BatchCode == null ? false : s.BatchCode.Equals(stateList[0].BatchCode);
                        return a && b && c;
                    }).ToList();
                    string updateStockSal = "";

                    if (stateList[0].WarehouseName == stateList[0].WorkshopName)
                    {
                        #region
                            //没有库存则新增
                            if (realStocks.Count <= 0)
                            {
                                WMS_BN_RealStock realStockModel = new WMS_BN_RealStock()
                                {
                                    InventoryCode = stateList[0].InventoryCode,
                                    InventoryName = stateList[0].InventoryName,
                                    Model = stateList[0].Model,
                                    RealStock = (double)stateList[0].QualifiedQuntity,
                                    TotalStock = (double)stateList[0].QualifiedQuntity,
                                    WarehouseCode = stateList[0].WarehouseID,
                                    WarehouseName = stateList[0].WarehouseName,
                                    BatchCode = stateList[0].BatchCode,
                                    Unit = stateList[0].Unit,
                                    CreatePerson =MmsHelper.GetUserName(),
                                    CreateTime = DateTime.Now,
                                    ModifyPerson = MmsHelper.GetUserName(),
                                    ModifyTime =DateTime.Now,
                                    IsEnable = 1
                                };
                                countC = db.Insert("WMS_BN_RealStock", realStockModel).AutoMap(x => x.ID).Execute();
                                updateStockSal += String.Format(@" update WMS_BN_BillMain set ApproveState=2 where BillCode = '{0}'", documentNo);
                            }
                            //有则更新数量
                            else if (realStocks.Count.Equals(1))
                            {
                                updateStockSal= string.Format("UPDATE WMS_BN_RealStock SET RealStock = {0},TotalStock = {1},ModifyPerson = '{2}',ModifyTime = '{3}' WHERE ID = {4} ;\r\n",
                                        realStocks[0].RealStock + stateList[0].QualifiedQuntity, realStocks[0].TotalStock + stateList[0].QualifiedQuntity, MmsHelper.GetUserName(), DateTime.Now, realStocks[0].ID);
                                updateStockSal += String.Format(@" update WMS_BN_BillMain set ApproveState=2 where BillCode = '{0}'", documentNo);
                            }
                            else
                            {
                                    return 0;
                                    msg = @"仓库物料种类繁多，可惜不知取哪一种！";
                            }
                            
                        #endregion
                            ccc = db.Sql(updateStockSal).Execute()>0;
                    }
                    if (aaa && countMain > 0 && countA > 0 && countB > 0 && ccc && countC>0)
                    {
                        db.Commit();
                        msg = "审核单据成功";
                        return 1;
                    }
                    else
                    {
                        db.Rollback();
                        msg = "审核单据失败，请确认数据";
                        return 1;
                    }

                }



            }
        }
    }

    public class QMS_ProcessInspectionItem : ModelBase
    {
        [Identity]
        [PrimaryKey]

        /*
         ,[ID]
      ,[MainID]
      ,[CheckRecord]
      ,[Inspector]
      ,[IsEnable]
      ,[CreatePerson]
      ,[CreateTime]
      ,[ModifyPerson]
      ,[ModifyTime]
      ,[FileName]
      ,[FileAddress]
      ,[InspectionItemCode]
      ,[InspectionItemName]
      ,[InspectionStandard]
         */
        public int ID { get; set; }
        /// <summary>
        /// 过程检验项目序号
        /// </summary>
        public string ProcessCheckItemCode { get; set; }

        public int? MainID { get; set; }
        //public string BillCode { get; set; }

        public string InspectionItemCode { get; set; }
        public string InspectionItemName { get; set; }
        public string InspectionStandard { get; set; }
        public string InspectionResult { get; set; }
        public string InspectionCode { get; set; }
        public int? IsEnable { get; set; }
        public string CheckRecord { get; set; }
        public string Inspector { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

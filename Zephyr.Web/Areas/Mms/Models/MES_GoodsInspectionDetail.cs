using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_GoodsInspectionDetailService : ServiceBase<MES_GoodsInspectionDetail>
    {
        public int AuditBillCode(string mainID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string insertSql = string.Empty;
            string sql = String.Format(@"  SELECT t1.*,
       t2.MainID,
       t2.InventoryCode,
       t2.InventoryName,
	   t2.ArrivalQuatity,
       t2.Model,
       t2.Unit,
       t2.Material
FROM MES_GoodsInspection t1
    LEFT JOIN MES_GoodsInspectionDetail t2
        ON t1.ID = t2.MainID
           AND t1.IsEnable = 1
           AND t2.IsEnable = 1 where t1.ID='{0}'", mainID);
            var resList = db.Sql(sql).QueryMany<dynamic>();
            if (resList.Count == 0)
            {
                msg = "请先保存单据后再审核";
                return 0;
            }
            if (resList[0].BillState == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                db.UseTransaction(true);
                sql = string.Format(@"update MES_GoodsInspection set BillState=2 where ID='{0}'", mainID);
                rowsAffected = db.Sql(sql).Execute();
                if (rowsAffected > 0)
                {
                    sql = string.Format(@"select ID from MES_PurchaseOrderMain where BillCode='{0}'", resList[0].PurchaseOrderCode);
                    int mID = db.Sql(sql).QuerySingle<int>();
                    sql = string.Empty;
                    var dno = MmsHelper.GetOrderNumber("QMS_GoodsCheck", "BillCode", "LLJY", "", "");
                    var fno = dno.Substring(0, 10);
                    var con = dno.Substring(10, 3);
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in resList)
                    {
                         
                        dno = fno + con;
                        int intCon = Convert.ToInt32(con);
                        intCon++;
                        var zeros = 3 - intCon.ToString().Length;
                        con = "";
                        for (int i = 1; i <= zeros; i++)
                            con += "0";
                        con += intCon.ToString();
                        sql = sql + string.Format(@" update MES_PurchaseOrderDetail set GoodsQuantity=ISNULL(GoodsQuantity,0)+ abs({0}) where MainID='{1}' and InventoryCode='{2}' ", item.ArrivalQuatity, mID, item.InventoryCode);
                        sb.Append( string.Format(@"INSERT INTO dbo.QMS_GoodsCheck
(
    ID,
    PBillCode,
    BillCode,
    ContractCode,
    ProjectName,
    SupplierCode,
    SupplierName,
    DepartmentID,
    DepartmentName,
    WarehouseID,
    WarehouseName,
    ProductionUnits,
    InventoryCode,
    InventoryName,
    Model,
    ArrivalDate,
    CheckQuantity,
    QualifiedQuantity,
    Unit,
    SalesmanCode,
    Salesman,
    BatchCode,
    BillState,
    IsEnable,
    CreatePerson,
    CreateTime,
    ModifyPerson,
    ModifyTime,
    Material,
    InspectionQuantity
)
VALUES
(   (select MAX(id)+1 from QMS_GoodsCheck),         -- ID - int
    '{0}',        -- PBillCode - varchar(50)
    '{1}',        -- BillCode - varchar(50)
    '{2}',        -- ContractCode - varchar(50)
    '{3}',        -- ProjectName - varchar(50)
    '{4}',        -- SupplierCode - varchar(50)
    '{5}',        -- SupplierName - varchar(50)
    '{6}',        -- DepartmentID - varchar(50)
    '{7}',        -- DepartmentName - varchar(50)
    '{8}',        -- WarehouseID - varchar(50)
    '{9}',        -- WarehouseName - varchar(50)
    '{10}',        -- ProductionUnits - varchar(50)
    '{11}',        -- InventoryCode - varchar(50)
    '{12}',        -- InventoryName - varchar(50)
    '{13}',        -- Model - varchar(50)
    '{14}', -- ArrivalDate - datetime
    '{15}',         -- CheckQuantity - int
    '{16}',         -- QualifiedQuantity - int
    '{17}',        -- Unit - varchar(50)
    '{18}',        -- SalesmanCode - varchar(50)
    '{19}',        -- Salesman - varchar(50)
    '',        -- BatchCode - varchar(50)
    1,         -- BillState - int
    1,         -- IsEnable - int
    '{20}',        -- CreatePerson - varchar(50)
    GETDATE(), -- CreateTime - datetime
    '{21}',        -- ModifyPerson - varchar(50)
    GETDATE(), -- ModifyTime - datetime
    '{22}',        -- Material - varchar(50)
    '{23}'          -- InspectionQuantity - int
    )", item.BillCode, dno, item.ContractCode, item.ProjectName, item.SupplierCode, item.SupplierName, item.DepartmentID, item.DepartmentName,
      item.WarehouseID, item.WarehouseName, "", item.InventoryCode, item.InventoryName, item.Model, item.ArrivalDate, item.ArrivalQuatity, item.ArrivalQuatity,
      item.Unit, item.SaleMan, item.UserCode, MmsHelper.GetUserName(), MmsHelper.GetUserName(), item.Material, item.ArrivalQuatity));
                    }
                    int res = db.Sql(sb.ToString()).Execute();
                    rowsAffected = db.Sql(sql).Execute();
                    if (rowsAffected > 0 && res==resList.Count)
                    {
                        msg = "操作成功";
                        db.Commit();
                        return rowsAffected;
                    }
                    else
                    {
                        msg = "操作失败";
                        db.Rollback();
                        return 0;
                    }
                    //return rowsAffected;
                }
                else
                {
                    msg = "采购订单审核失败";
                    db.Rollback();
                    return 0;
                }

                //sql = string.Format(@"update APS_ProductPurchaseMain set BillState=2 where PurchaseDocumentCode='{0}'", res.ProductPurchaseCode);
            }
        }

    }

    public class MES_GoodsInspectionDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int? MainID { get; set; }
        public string InventoryCode { get; set; }
        public string InVentoryName { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public int? ArrivalQuatity { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public string Material { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

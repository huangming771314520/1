using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProductPurchaseDetailService : ServiceBase<APS_ProductPurchaseDetail>
    {
        //采购订单选择生产请购
        public List<dynamic> GetProductPurchase(string CodeArr)
        {
            CodeArr = CodeArr.Substring(0, CodeArr.Length - 1);
            string sql = @"SELECT tab1.*,
       tab2.OrderQuantity,
	   tab2.CountOrderQuantity
FROM
(
    SELECT MAX(t1.MainID) AS ID,
           MAX(t4.PurchaseDocumentCode) AS PurchaseDocumentCode,
           t1.InventoryCode,
           MAX(t3.InventoryName) AS InventoryName,
           MAX(t3.Model) AS Model,
           MAX(t1.Unit) AS Unit,
           SUM(t1.TotalRequestQuantity) AS CountRequestQuantity,
           MAX(t1.TotalRequestQuantity) AS TotalRequestQuantity,
           MAX(t1.PlanArrivelDate) AS PlanArrivelDate,
           MAX(t2.RealStock) AS RealStock
    FROM APS_ProductPurchaseDetail AS t1
        LEFT JOIN WMS_BN_RealStock AS t2
            ON t1.InventoryCode = t2.InventoryCode
        LEFT JOIN SYS_Part t3
            ON t1.InventoryCode = t3.InventoryCode
        LEFT JOIN APS_ProductPurchaseMain t4
            ON t1.MainID = t4.ID
    WHERE t4.BillState = 2
          AND PurchaseState = 1
    GROUP BY t1.InventoryCode
) tab1
    LEFT JOIN
    (
        SELECT MAX(mpm.PurchaseDocumentCode) AS ProductPurchaseCode,
               mpd.InventoryCode,
               SUM(mpd.OrderQuantity) AS CountOrderQuantity,
               MAX(mpd.OrderQuantity) AS OrderQuantity
        FROM dbo.APS_ProductPurchaseMain mpm
            LEFT JOIN dbo.APS_ProductPurchaseDetail mpd
                ON mpm.ID = mpd.MainID
        WHERE mpm.BillState = 2
              AND mpm.IsEnable = 1
              AND mpd.IsEnable = 1
        GROUP BY InventoryCode
    ) tab2
        ON tab1.PurchaseDocumentCode = tab2.ProductPurchaseCode
           AND tab1.InventoryCode = tab2.InventoryCode

WHERE 1 = 1
      AND tab1.InventoryCode IN  ";
            string whereSql = "(";
            string[] sarr = CodeArr.Split(',');
            for (int i = 0; i < sarr.Length; i++)
            {
                whereSql = whereSql + "'" + sarr[i] + "'" + ",";
            }
            whereSql = whereSql.Substring(0, whereSql.Length - 1);
            whereSql = whereSql + ")";
            sql = sql + whereSql;
            return db.Sql(sql).QueryMany<dynamic>();
        }

        public int AuditBillCode(string mainID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select BillState FROM APS_ProductPurchaseMain  where ID='{0}'", mainID);
            var state = db.Sql(sql).QuerySingle<int>();
            if (state == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                try
                {
                    using (db.UseTransaction(true))
                    {
                        sql = string.Format(@"update APS_ProductPurchaseMain set BillState=2 where ID='{0}'", mainID);
                        rowsAffected = db.Sql(sql).Execute();
                        sql = string.Format(@"UPDATE APS_ProductPurchaseDetail SET PurchaseState=1 WHERE MainID ='{0}'", mainID);
                        rowsAffected = db.Sql(sql).Execute();
                        db.Commit();
                        msg = "生产请购单据审核成功";
                        return rowsAffected;
                    }
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    msg = "生产请购单据审核失败,请先保存数据后审核";
                    return 0;
                }

            }
        }

        public int SetType(string ids, string UserCode, string SaleMan, out string msg)
        {
            msg = "操作失败！";

            string sql = string.Format(@"update a set Saleman='{0}',UserCode='{1}'
from APS_ProductPurchaseDetail a where a.ID in {2}", SaleMan, UserCode, ids);
            var rowsAffected = db.Sql(sql).Execute();

            if (rowsAffected > 0)
            {
                msg = "操作成功！";
            }
            return rowsAffected;
        }

    }

    public class APS_ProductPurchaseDetail : ModelBase
    {
        [PrimaryKey]
        //public int ID { get; set; }
        //public int? MainID { get; set; }
        //public string InventoryCode { get; set; }
        //public int? SingleProductRequestQuantity { get; set; }
        //public decimal? TotalRequestQuantity { get; set; }
        //public string Remark { get; set; }
        //public int? PurchaseState { get; set; }
        //public int? IsEnable { get; set; }
        //public string CreatePerson { get; set; }
        //public DateTime? CreateTime { get; set; }
        //public string ModifyPerson { get; set; }
        //public DateTime? ModifyTime { get; set; }
        //public decimal? PurchaseQuantity { get; set; }
        //public decimal? StockQuantity { get; set; }
        //public decimal? RequestedQuantity { get; set; }
        //public string SaleMan { get; set; }
        //public string DepartmentCode { get; set; }
        //public string DepartmentName { get; set; }
        //public string WarehouseCode { get; set; }
        //public string WarehouseName { get; set; }
        //public string SupplierCode { get; set; }
        //public string SupplierName { get; set; }
        //public string OrderQuantity { get; set; }
        //public string PurchaseFeedback { get; set; }
        //public string PurchaseRemark { get; set; }
        //public string UserCode { get; set; }
        //public string PurchaseDate { get; set; }
        //public DateTime? PlanArrivelDate { get; set; } 
        //public string Unit { get; set; }

        /// <summary>
		/// 
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 生产请购单主表ID
        /// </summary>
        public int? MainID { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string InventoryCode { get; set; }

        /// <summary>
        /// 单台数量
        /// </summary>
        public int? SingleProductRequestQuantity { get; set; }

        /// <summary>
        /// 实需数量
        /// </summary>
        public decimal? TotalRequestQuantity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 请购状态
        /// </summary>
        public int? PurchaseState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IsEnable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? PurchaseQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? StockQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? RequestedQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SaleMan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public decimal? OrderQuantity { get; set; }

        /// <summary>
        /// 采购反馈
        /// </summary>
        public int? PurchaseFeedback { get; set; }

        /// <summary>
        /// 采购备注
        /// </summary>
        public string PurchaseRemark { get; set; }

        /// <summary>
        /// 采购员编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 请购日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 计划到货日期
        /// </summary>
        public DateTime? PlanArrivelDate { get; set; }

        /// <summary>
        /// 配套表ID
        /// </summary>
        public int? MatchTableID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_PurchaseOrderDetailService : ServiceBase<MES_PurchaseOrderDetail>
    {
        //采购订单显示合同信息浮动窗
        public string GetContractInfo(string contractCode)
        {
            string sql = String.Format(@"select top 1 ProjectName from PMS_BN_Project where contractCode='{0}'", contractCode);
            return db.Sql(sql).QuerySingle<string>();
        }

        //到货报检单选择采购订单
        public List<dynamic> GetPurchaseOrder(string mainID)
        {
            string sql = String.Format(@"select InventoryCode,InventoryName,Model,Unit,GoodsQuantity from MES_PurchaseOrderDetail where mainid='{0}'", mainID);
            return db.Sql(sql).QueryMany<dynamic>();
        }
        public int AuditBillCode(string mainID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select * FROM MES_PurchaseOrderMain t1 left join MES_PurchaseOrderDetail t2 on t1.ID=t2.MainID where t1.ID='{0}'", mainID);
            //if sql.Contains
            var resList = db.Sql(sql).QueryMany<dynamic>();
            if (resList[0].BillState == 2)
            {
                msg = "单据已审核";
                return 0;
            }
            else if (resList.Count == 0)
            {
                msg = "请先保存数据再审核";
                return 0;
            }
            else
            {
                int isFalse = 0;
                int res = 0;
                try
                {
                    db.UseTransaction(true);
                    sql = string.Format(@"update MES_PurchaseOrderMain set BillState=2 where BillCode='{0}'", resList[0].BillCode);
                    rowsAffected = db.Sql(sql).Execute();
                    if (rowsAffected > 0)
                    {
                        msg = "采购订单审核成功";
                        sql = String.Format(@"   select t2.ID as id from MES_PurchaseOrderMain t1 left join APS_ProductPurchaseMain t2 on t1.ProductPurchaseCode=t2.PurchaseDocumentCode 
 where t1.ID='{0}'", mainID);
                        string mID = db.Sql(sql).QuerySingle<string>();
                        sql = "";
                        foreach (var item in resList)
                        {
                            res++;
                            sql = sql + string.Format(@" update APS_ProductPurchaseDetail set PurchaseState=2 where MainID='{0}' and InventoryCode='{1}' ", mID, item.InventoryCode);
                        }
                        rowsAffected = db.Sql(sql).Execute();

                        //遍历采购订单明细表，生成采购过程数据

                        foreach (var item in resList)
                        {
                            //item.MatchTableID
                            MES_BN_MatchingTableDetail MatchingTable = new MES_BN_MatchingTableDetailService().GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", item.MatchTableID));
                            MatchingTable = MatchingTable ??new MES_BN_MatchingTableDetail();
                            string sqlMaterial = string.Format(@" SELECT MaterialCode from dbo.PRS_Process_BOM WHERE InventoryCode='{0}' AND IsEnable=1", item.InventoryCode);
                            Mes_PurchaseProcess process = new Mes_PurchaseProcess();
                            process.ContractCode = item.ContractCode;
                            //process.ProductID = MatchingTable.ID ?? 0;
                            //process.ProductID = item.ProcessCode;
                            process.UserCode = item.UserCode;
                            if(MatchingTable.ProjectDetailID!=null)
                            {
                                process.ProductID = MatchingTable.ProjectDetailID;
                            }
                            process.SaleMan = item.SaleMan;
                            process.InventoryCode = item.InventoryCode;
                            process.InventoryName = item.InventoryName;
                            process.Model = item.Model;
                            process.MaterialCode = db.Sql(sqlMaterial).QuerySingle<string>();
                            process.SupplierCode = item.SupplierCode;
                            process.Quantity = item.OrderQuantity;
                            process.PurchaseCode = item.BillCode;
                            process.PrchaseDate = item.OrderDate;
                            process.IsEnable = 1;
                            process.PlanArrivelDate = item.PlanArrivelDate;
                            process.CreatePerson = MmsHelper.GetUserName();
                            process.CreateTime = DateTime.Now;
                            process.ModifyPerson = MmsHelper.GetUserName();
                            process.ModifyTime = DateTime.Now;
                            var detailID = db.Insert<Mes_PurchaseProcess>("Mes_PurchaseProcess", process)
                  .AutoMap(x => x.ID).ExecuteReturnLastId<int>();
                        }

                        if (rowsAffected == res)
                        {
                            msg = "操作成功";
                            db.Commit();
                            return 1;
                        }
                        else
                        {
                            isFalse++;
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

                }
                catch (Exception ex)
                {
                    msg = "采购订单审核失败";
                    db.Rollback();
                    return 0;
                }
               
                //sql = string.Format(@"update APS_ProductPurchaseMain set BillState=2 where PurchaseDocumentCode='{0}'", res.ProductPurchaseCode);


            }
        }
    }

    public class MES_PurchaseOrderDetail : ModelBase
    {
        [Identity]
        [PrimaryKey]
        /// <summary>
		/// 
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 采购订单主表ID
        /// </summary>
        public int? MainID { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string InventoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public int? OrderQuantity { get; set; }

        /// <summary>
        /// 到货数量
        /// </summary>
        public int? GoodsQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? UnitPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? TotalPrice { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public int? IsFinish { get; set; }

        /// <summary>
        /// 是否可用
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
        public string WarehouseID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CheckedQuantity { get; set; }

        public DateTime? PlanArrivelDate { get; set; }

        /// <summary>
        /// 配套表ID
        /// </summary>
        public int? MatchTableID { get; set; }

    }
}

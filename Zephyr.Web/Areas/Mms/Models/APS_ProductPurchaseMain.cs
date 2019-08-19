using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProductPurchaseMainService : ServiceBase<APS_ProductPurchaseMain>
    {
        public bool InsertPurchase(string code, string deID, int? pType, out string msg)
        {
            msg = string.Empty;
            string sql = string.Format(@"SELECT DISTINCT
   --合同编号
       A.ContractCode,
--配套类型
a.Type,
       --项目名称
     '(' +A.ContractCode+')'+ A.ProjectName ProjectName,
       --生产批次计划编码
       A.ProductPlanCode,a.Type DepartmentCode,a.TypeName 
FROM [dbo].[MES_BN_MatchingTableDetail] A
WHERE a.IsEnable=1 and a.ApproveState=1 and Type!=1 
and A.ContractCode = '{0}' and A.ProjectDetailID = '{1}';", code, deID);
            //List<MES_BN_MatchingTableDetail>  list = db.Sql(sql).QueryMany<MES_BN_MatchingTableDetail>();
            var list = db.Sql(sql).QueryMany<dynamic>();
            if (pType == 5)
            {
                var matchData = list.Where(item => item.TypeName.Equals("采购供应"));
                list.Remove(list.SingleOrDefault(item => item.TypeName.Equals("采购供应")));
                
            }
            else
            {
                var matchData = list.Where(item => item.TypeName.Equals("外协配套"));
                list.Remove(list.SingleOrDefault(item => item.TypeName.Equals("外协配套")));
                list.Remove(list.SingleOrDefault(item => item.TypeName.Equals("铸件配套")));
            }
            if (list.Where(a => a.Type == "0").ToList().Count > 0)
            {
                msg = "请先维护配套部门，并审核后再进行生产请购";
                return false;
            }

            int rowsAffected = 0;
            int isfalse = 0;
            using (var trans = db.UseTransaction(true))
            {

                foreach (var item in list)
                {
                    string sql1 = "";
                    int PurchaseType = 1;
                    //if (item.TypeName == "铸件配套" || item.TypeName == "外协配套")
                    if (pType==5)
                    {
                        sql1 = string.Format(@" SELECT t.InventoryCode,
       MAX(t.InventoryName) InventoryName,
       MAX(BomQuantity) BomQuantity,
       MAX(NeedQuantity) NeedQuantity,
       MAX(RealStock) RealStock,
       SUM(PurchaseQuantity) PurchaseQuantity,
       MAX(PurchaseAdvanceTime) PurchaseAdvanceTime,
       MAX(t.QuantityUnit) Unit,
       MAX(MatchTableID) MatchTableID
FROM
(
    SELECT B.InventoryCode,
           B.InventoryName,
           B.PurchaseAdvanceTime,
           B.QuantityUnit,
           A.ID MatchTableID,
           A.BomQuantity,
           A.NeedQuantity,
           (
               SELECT ISNULL(SUM(RealStock), 0)
               FROM dbo.WMS_BN_RealStock c
               WHERE c.InventoryCode = B.InventoryCode
           ) RealStock,
           (
               SELECT ISNULL(SUM(PurchaseQuantity), 0)
               FROM dbo.APS_ProductPurchaseDetail d
               WHERE d.InventoryCode = B.InventoryCode
                     AND d.IsEnable = '1'
                     AND d.PurchaseState = '2'
           ) PurchaseQuantity
    FROM MES_BN_MatchingTableDetail A
	INNER JOIN dbo.PRS_Process_BOM C ON c.PartCode=a.PartCode
	AND c.IsEnable=1
        INNER JOIN SYS_Part B
            ON c.New_InventoryCode = B.InventoryCode
               AND A.IsEnable = 1
               AND B.IsEnable = 1
    
where A.ApproveState=1 and  a.ContractCode='{0}' 
and type='{1}' 
and typeName!='加工件' ) as t
group by t.InventoryCode   ", code, item.DepartmentCode);
                        PurchaseType = 5;
                    }
                    else
                    {
                        sql1 = string.Format(@" select t.InventoryCode,max(t.InventoryName)InventoryName,max(BomQuantity)BomQuantity,max(NeedQuantity)NeedQuantity,max(RealStock) RealStock,
sum(PurchaseQuantity) PurchaseQuantity,max(PurchaseAdvanceTime) PurchaseAdvanceTime ,MAX(t.QuantityUnit) Unit  ,MAX(MatchTableID)  MatchTableID 
from ( select B.InventoryCode,b.InventoryName,B.PurchaseAdvanceTime, B.QuantityUnit,a.ID MatchTableID,
a.BomQuantity ,
a.NeedQuantity,
(select isnull(sum(RealStock),0) from dbo.WMS_BN_RealStock c where c.InventoryCode=b.InventoryCode) RealStock,
(select isnull(sum(PurchaseQuantity),0) from dbo.APS_ProductPurchaseDetail d where d.InventoryCode=b.InventoryCode and d.isenable='1' and 
d.PurchaseState='2'
) PurchaseQuantity
 FROM MES_BN_MatchingTableDetail A 
inner join sys_part B 
on A.PartCode=B.PartCode and A.IsEnable=1 and b.IsEnable=1 
where A.ApproveState=1 and ContractCode='{0}' 
and type='{1}' 
and typeName!='加工件' ) as t
group by t.InventoryCode  ", code, item.DepartmentCode);
                    }
                    APS_ProductPurchaseMain master = new APS_ProductPurchaseMain();
                    master.ID = trans.Sql(@"select isnull(max(ID),0)+1 from APS_ProductPurchaseMain").QuerySingle<int>();
                    master.PurchaseDocumentCode = MmsHelper.GetOrderNumber("APS_ProductPurchaseMain", "PurchaseDocumentCode", "SCQG", "", "");
                    master.ContractCode = item.ContractCode;
                    master.PurchaseDate = DateTime.Now;
                    master.DepartmentCode = item.DepartmentCode == null ? "" : item.DepartmentCode;
                    master.IsEnable = 1;
                    master.BillState = 1;
                    master.PurchaseType = PurchaseType;
                    master.ProductPlanCode = item.ProductPlanCode;
                    master.CreateTime = DateTime.Now;
                    master.CreatePerson = MmsHelper.GetUserName();
                    master.ModifyTime = DateTime.Now;
                    master.ModifyPerson = MmsHelper.GetUserName();
                    string insertSql = string.Format(@"insert into APS_ProductPurchaseMain (ID,PurchaseDocumentCode,ContractCode,PurchaseDate,DepartmentCode,IsEnable,BillState,
ProductPlanCode,CreateTime,CreatePerson,ModifyTime,ModifyPerson,PurchaseType) values(((select isnull(max(ID),0)+1 from APS_ProductPurchaseMain)),'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11})", master.PurchaseDocumentCode
, master.ContractCode, master.PurchaseDate, master.DepartmentCode, master.IsEnable, master.BillState, master.ProductPlanCode, master.CreateTime, master.CreatePerson, master.ModifyTime, master.ModifyPerson, master.PurchaseType);
                    rowsAffected = db.Sql(insertSql).Execute();
                    if (rowsAffected == 0)
                    {
                        isfalse++;
                        //trans.Rollback();
                    }



                    dynamic detailList = db.Sql(sql1).QueryMany<dynamic>();

                    foreach (var detailItem in detailList)
                    {

                        APS_ProductPurchaseDetail detailMaster = new APS_ProductPurchaseDetail();
                        detailMaster.MainID = master.ID;
                        detailMaster.MatchTableID = detailItem.MatchTableID;
                        detailMaster.InventoryCode = detailItem.InventoryCode;
                        detailMaster.SingleProductRequestQuantity = detailItem.BomQuantity;
                        detailMaster.TotalRequestQuantity = detailItem.NeedQuantity;
                        detailMaster.IsEnable = 1;
                        detailMaster.CreatePerson = MmsHelper.GetUserName();
                        detailMaster.CreateTime = DateTime.Now;
                        detailMaster.ModifyPerson = MmsHelper.GetUserName();
                        detailMaster.ModifyTime = DateTime.Now;
                        detailMaster.PlanArrivelDate = Convert.ToDateTime(master.PurchaseDate).AddDays(detailItem.PurchaseAdvanceTime == null ? 0 : detailItem.PurchaseAdvanceTime);
                        detailMaster.StockQuantity = detailItem.RealStock == null ? 0 : Convert.ToDecimal(detailItem.RealStock);
                        detailMaster.RequestedQuantity = detailItem.PurchaseQuantity == null ? 0 : Convert.ToDecimal(detailItem.PurchaseQuantity);
                        detailMaster.PurchaseQuantity = detailMaster.TotalRequestQuantity - detailMaster.RequestedQuantity - detailMaster.StockQuantity;
                        detailMaster.DepartmentCode = master.DepartmentCode;
                        detailMaster.Unit = detailItem.Unit;
                        detailMaster.DepartmentName = trans.Sql(@"select DepartmentName from SYS_BN_Department where DepartmentCode='" + detailMaster.DepartmentCode + "' and IsEnable=1").QuerySingle<string>();
                        string detailInsertSql = string.Format(@"insert into APS_ProductPurchaseDetail (MainID,InventoryCode,SingleProductRequestQuantity,TotalRequestQuantity,IsEnable
,CreateTime,CreatePerson,ModifyTime,ModifyPerson,PurchaseQuantity,StockQuantity,RequestedQuantity,PurchaseState,DepartmentCode,DepartmentName,PlanArrivelDate,Unit,MatchTableID) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',0,'{12}','{13}','{14}','{15}',{16})", detailMaster.MainID
, detailMaster.InventoryCode, detailMaster.SingleProductRequestQuantity, detailMaster.TotalRequestQuantity, detailMaster.IsEnable, master.CreateTime, master.CreatePerson, master.ModifyTime, master.ModifyPerson,
detailMaster.PurchaseQuantity, detailMaster.StockQuantity, detailMaster.RequestedQuantity, detailMaster.DepartmentCode, detailMaster.DepartmentName, detailMaster.PlanArrivelDate, detailMaster.Unit, detailMaster.MatchTableID);
                        rowsAffected = db.Sql(detailInsertSql).Execute();
                        if (rowsAffected == 0)
                        {
                            isfalse++;
                            //trans.Rollback();
                        }
                    }

                }
                if (isfalse == 0)
                {
                    trans.Commit();
                    msg = "新建项目采购任务成功";
                    return true;
                }
                else
                {
                    trans.Rollback();
                    msg = "新建项目采购任务失败";
                    return false;
                }
            }

        }
    }

    public class APS_ProductPurchaseMain : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string PurchaseDocumentCode { get; set; }
        public string ContractCode { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string WarehouseID { get; set; }
        public string ProductPlanCode { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public string DepartmentCode { get; set; }

        /// <summary> 
		/// 生产请购类型 
		/// </summary> 
		public int? PurchaseType { get; set; }
    }
}

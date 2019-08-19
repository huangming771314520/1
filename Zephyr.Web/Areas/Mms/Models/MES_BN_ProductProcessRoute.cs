using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;
using System.Dynamic;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.CommonWrap;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_BN_ProductProcessRouteService : ServiceBase<MES_BN_ProductProcessRoute>
    {
        public int Insert(MES_BN_ProductProcessRoute model)
        {
            model.CreatePerson = MmsHelper.GetUserCode();
            model.CreateTime = DateTime.Now;
            var rowsAffected = model.ID = db.Insert<MES_BN_ProductProcessRoute>("MES_BN_ProductProcessRoute", model)
     .AutoMap(x => x.ID)
     .ExecuteReturnLastId<int>();
            return rowsAffected;
        }
        public int Update(MES_BN_ProductProcessRoute model)
        {
            string sql = String.Format(@"select * from MES_BN_ProductProcessRoute where id='{0}' ", model.ID);
            var single = db.Sql(sql).QuerySingle<MES_BN_ProductProcessRoute>();
            model.CreatePerson = single.CreatePerson;
            model.CreateTime = single.CreateTime;
            model.ModifyPerson = MmsHelper.GetUserCode();
            model.ModifyTime = DateTime.Now;

            var rowsAffected = db.Update<MES_BN_ProductProcessRoute>("MES_BN_ProductProcessRoute", model)
     .AutoMap(x => x.ID)
     .Where(x => x.ID)
     .Execute();
            return model.ID;
        }
        public int Update2(MES_BN_ProductProcessRoute model)
        {
            string sql = String.Format(@"update MES_BN_ProductProcessRoute set ManHour={1} where id='{0}' ", model.ID, model.ManHour);
            return db.Sql(sql).Execute();
        }
        public List<PRS_Process_BOM> GetGYBom(string parentCode)
        {
            List<PRS_Process_BOM> boomlist = new List<PRS_Process_BOM>();
            string sql = String.Format(@"select * from PRS_Process_BOM where IsEnable=1 and PartCode='{0}' ", parentCode);
            var list = db.Sql(sql).QueryMany<PRS_Process_BOM>();
            while (list.Count != 0)
            {
                boomlist.AddRange(list);
                var part = "(";
                foreach (var item in list)
                {
                    part += "'" + item.PartCode + "',";
                }
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                sql = String.Format(@"select * from PRS_Process_BOM where IsEnable=1 and ParentCode in {0} ", part);
                list = db.Sql(sql).QueryMany<PRS_Process_BOM>();
            }
            return boomlist;
        }

        public List<dynamic> GetProductPlanList(List<dynamic> plan_list)
        {
            var ProcessTypeList = plan_list.Select(p => Convert.ToString(p.ProcessType)).ToArray();
            string ContractCode = plan_list[0]["ContractCode"];
            string PlanCode = plan_list[0]["PlanCode"];
            int ProductID = plan_list[0]["ProductID"];
            string PartCode = plan_list[0]["PartCode"];
            int Quantity = plan_list[0]["Quantity"];
            var BomList = TreeNodeManage.GetTreeNodeList<dynamic>(
                 TreeNodeManage.Instance()
                 .SetNode("PartCode")
                 .SetParentNode("ParentCode", PartCode)
                 .SetTableName("PRS_Process_BOM"))
                 .Where(b => b.IsSelfMade == "1" && b.IsEnable == 1 && b.PartCode == PartCode).ToList();

            var ProcessRouteList = new MES_BN_ProductProcessRouteService().GetModelList().Where(p => p.IsEnable == 1 && ProcessTypeList.Contains(p.ProcessModelType)).ToList();

            return (from p in ProcessRouteList
                    join b in BomList
                    on p.PartCode equals b.PartCode
                    orderby p.ProcessModelType, p.ProcessLineCode
                    select new
                    {
                        ContractCode = ContractCode,
                        ProductID = ProductID,
                        p.ProcessModelType,
                        p.PartCode,
                        RootPartCode = PartCode,
                        MonthPlanCode = PlanCode,
                        p.ProcessCode,
                        p.ProcessName,
                        b.PartFigureCode,
                        b.PartName,
                        b.MaterialCode,
                        Quantity = b.PartQuantity * Quantity,
                        BomQty = b.PartQuantity,
                        PlanType = 1,
                        p.ManHour,
                        p.WorkshopID,
                        p.WorkshopName,
                        p.EquipmentID,
                        p.EquipmentName,
                        p.WorkGroupID,
                        p.WorkGroupName
                    }).ToList<dynamic>();
        }

        public List<dynamic> getProductProcessRoute(string ContractCode, string PartCode, int Quantity, string type)
        {
            List<PRS_Process_BOM> boomlist = GetGYBom(PartCode);
            List<dynamic> purchaselist = new List<dynamic>();//采购计划
            var part = "(";
            foreach (var item in boomlist)
            {
                if (type == "1")
                {
                    if (item.IsSelfMade == "1")
                    {
                        part += "'" + item.PartCode + "',";
                    }
                }
                if (type == "0")
                {
                    if (item.IsSelfMade == "0")
                    {
                        dynamic expObj = new ExpandoObject();//动态类new
                        expObj.PartCode = item.PartCode;
                        expObj.PartFigureCode = item.PartFigureCode;
                        expObj.ProcessCode = "";
                        expObj.ProcessName = "";
                        expObj.PlanType = 2;
                        expObj.Quantity = Quantity * item.PartQuantity;
                        expObj.BomQty = item.PartQuantity;
                        purchaselist.Add(expObj);
                    }

                }
            }
            if (type == "0")
            {
                return purchaselist;
            }
            part = part.Remove(part.Length - 1, 1);
            part += ")";
            if (part == ")")
            {
                return new List<dynamic>();
            }
            string sql = string.Format("select t2.PartFigureCode,t2.PartName,t2.MaterialCode,t1.* from MES_BN_ProductProcessRoute t1 left join PRS_Process_BOM t2 on t1. PartCode=t2.PartCode and t1.IsEnable=1 where t2.IsEnable=1 and  t1.ContractCode='{0}' and t1.PartCode in {2}", ContractCode, type, part);
            List<dynamic> list = db.Sql(sql).QueryMany<dynamic>();
            foreach (var item in list)
            {
                item.PlanType = 1;
                var boom = (from p in boomlist where p.PartCode.ToLower() == item.PartCode.ToLower() select p).FirstOrDefault();
                item.BomQty = boom.PartQuantity;
                if (boom != null)
                    item.Quantity = Quantity * boom.PartQuantity;
                else
                    item.Quantity = 0;
            }
            return list;
        }

        public int GetProcessBom(string ContractCode, string PartCode)
        {
            string sql = string.Format(@"select COUNT(id) from PRS_Process_BOM WHERE  ParentCode='{0}'", PartCode);
            return db.Sql(sql).QuerySingle<int>();
        }
        public int DeleteProcessRoute(string id, string ContractCode, string PartCode)
        {
            string sql2 = string.Format(@"delete MES_BN_ProductProcessRoute where ID={0}", id);
            int r = db.Sql(sql2).Execute();
            sql2 = string.Format(@"delete PRS_ProcessWorkSteps where ProcessRouteID={0}", id);
            db.Sql(sql2).Execute();
            sql2 = string.Format(@"delete QMS_QualityReportDoc where FileType=5 and SourceID={0}", id);
            db.Sql(sql2).Execute();
            sql2 = string.Format(@"delete MES_BN_ProductProcessRouteEquipment where ProcessRouteID={0}", id);
            db.Sql(sql2).Execute();
            if (r > 0)
            {
                PostCheckOP(ContractCode, PartCode);
            }
            return r;
        }
        public void PostCheckOP(string ContractCode, string PartCode)
        {
            string sql = string.Format("select * from MES_BN_ProductProcessRoute where ContractCode='{0}' and PartCode ='{1}'   order by ProcessLineCode ", ContractCode, PartCode);
            List<MES_BN_ProductProcessRoute> list = db.Sql(sql).QueryMany<MES_BN_ProductProcessRoute>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].ProcessLineCode = i + 1;
            }
            var rowsAffected = 0;
            foreach (var item in list)
            {
                rowsAffected = db.Update<MES_BN_ProductProcessRoute>("MES_BN_ProductProcessRoute", item)
    .AutoMap(x => x.ID)
    .Where(x => x.ID)
    .Execute();
            }
        }

        public dynamic GetProductProcessRoute(string contractCode, string partCode, string processCode, string workGroupID)
        {
            string sql = string.Format(@"
SELECT tbA.*
FROM
(
    SELECT *
    FROM dbo.MES_BN_ProductProcessRoute
    WHERE ContractCode = '{0}'
          AND PartCode = '{1}'
          AND ProcessCode = '{2}'
          AND WorkGroupID = '{3}'
) tbA
    LEFT JOIN dbo.MES_BN_ProductProcessRouteDetail tbB
        ON tbA.ID = tbB.ProcessRouteID
    LEFT JOIN dbo.MES_BN_ProductProcessRouteEquipment tbC
        ON tbA.ID = tbC.ProcessRouteID;
", contractCode, partCode, processCode, workGroupID);
            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QuerySingle<dynamic>();
            }
        }
        public string GetAudit2(string ContractCode, string partCode)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var sqlprocess = @"update MES_BN_ProductProcessRoute set ApproveState=1,ApprovePerson='" + MmsHelper.GetUserCode() + "',ApproveDate='" + now + "' where ContractCode='" + ContractCode + "' and PartCode='{0}' ";
            var sqlprocessex = "";

            List<PRS_Process_BOM> boomlist = GetGYBom(partCode);
            var ids = "(";
            foreach (var item in boomlist)
            {
                sqlprocessex += string.Format(@sqlprocess, item.PartCode);
                ids += "'" + item.ID + "',";

            }
            ids = ids.Remove(ids.Length - 1, 1);
            ids += ")";
            string sql = string.Format("update PRS_Process_BOM set ApproveState=1,ApprovePerson='{0}',ApproveDate='" + now + "' where ID in {1}", MmsHelper.GetUserCode(), ids);
            db.Sql(sql).Execute();

            db.Sql(sqlprocessex).Execute();

            return "3";
        }

        public dynamic GetUpdateApproveState(string cCode, string partCode, int type)
        {
            string sqlA = string.Format(@"SELECT ID FROM dbo.MES_BN_ProductProcessRoute WHERE IsEnable = 1 AND ContractCode = '{0}' AND PartCode = '{1}' AND ProcessModelType = '{2}';", cCode, partCode, type);

            List<int> IDs = db.Sql(sqlA).QueryMany<int>();

            if (IDs.Count <= 0)
            {
                throw new Exception("审核失败！");
            }
            else
            {
                string sqlB = string.Format(@"UPDATE dbo.MES_BN_ProductProcessRoute SET ApproveState = '1',ApprovePerson = '{0}',ApproveDate = GETDATE() WHERE ID IN ({1})", MmsHelper.GetUserName(), string.Join(",", IDs));
                db.Sql(sqlB).Execute();

                return new
                {
                    result = true,
                    msg = @"发布成功！"
                };
            }
        }
    }

    public class MES_BN_ProductProcessRoute : ModelBase
    {
        [PrimaryKey]
        [Identity]
        public int ID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string PartCode { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDesc { get; set; }
        public int? ProcessLineCode { get; set; }
        public string WorkshopID { get; set; }
        public string EquipmentID { get; set; }
        public string WorkGroupID { get; set; }
        public string WarehouseID { get; set; }
        public string WorkshopName { get; set; }
        public string EquipmentName { get; set; }
        public string WorkGroupName { get; set; }
        public string WarehouseName { get; set; }
        public int? ManHour { get; set; }
        public int Unit { get; set; }
        public string FigureCode { get; set; }
        public string Remark { get; set; }
        //public int? IsNeedCheck { get; set; }
        public int? IsEnable { get; set; }
        public int? IsInspectionReport { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ApproveState { get; set; }
        public string ApprovePerson { get; set; }
        public string ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string ProcessModelType { get; set; }
    }
}

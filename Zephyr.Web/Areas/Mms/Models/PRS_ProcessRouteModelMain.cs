using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_ProcessRouteModelMainService : ServiceBase<PRS_ProcessRouteModelMain>
    {
        public dynamic GetProcessRouteModel(string code, string ContractCode, string PartCode, string ProjectName, string fcode,string type)
        {
            string sql = String.Format(@"select t1.*,t2.ProcessType from PRS_ProcessRouteModelDetail t1
left join PRS_BD_StandardProcess t2  on t1.processCode=t2.processCode
where t1.IsEnable=1 and MainID in (
select ID from PRS_ProcessRouteModelMain where ProcessRouteCode='{0}'
)   order by ProcessLineCode", code);
            var list = db.Sql(sql).QueryMany<dynamic>();
            if (list.Count > 0)
            {
                //string sql2 = string.Format(@"select * from MES_BN_ProductProcessRoute where ContractCode='{0}' and PartCode='{1}'", ContractCode, PartCode);
                //var pclist = db.Sql(sql).QueryMany<MES_BN_ProductProcessRoute>();
                //foreach (var item in pclist)
                //{

                //}

                string sql2 = string.Format(@"delete MES_BN_ProductProcessRoute where ContractCode='{0}' and PartCode='{1}'", ContractCode, PartCode);
                db.Sql(sql2).Execute();
                var rowsAffected = 0;
                db.UseTransaction(true);
                foreach (var item in list)
                {
                    MES_BN_ProductProcessRoute detail = new MES_BN_ProductProcessRoute();
                    detail.ContractCode = ContractCode;
                    detail.ProjectName = ProjectName;
                    detail.PartCode = PartCode;
                    detail.FigureCode = fcode;
                    detail.ProcessCode = item.ProcessCode;
                    detail.ProcessLineCode = item.ProcessLineCode;
                    detail.ProcessName = item.ProcessName;
                    detail.WorkshopID = item.DepartmentCode;
                    detail.Unit = 1;
                    detail.IsInspectionReport = 2;
                    detail.IsEnable = 1;
                    detail.CreatePerson = MmsHelper.GetUserName();
                    detail.CreateTime = DateTime.Now;
                    detail.ProcessModelType = type;

                    var detailID = db.Insert<MES_BN_ProductProcessRoute>("MES_BN_ProductProcessRoute", detail)
          .AutoMap(x => x.ID).ExecuteReturnLastId<int>();
                    if (detailID < 0)
                    {
                        db.Rollback();
                        return detailID;
                    }

                    sql = String.Format(@"select * from PRS_ProcessRouteModelEquipment where MainID = '{0}'", item.ID);
                    var eqpList = db.Sql(sql).QueryMany<PRS_ProcessRouteModelEquipment>();//
                    foreach (var eqp in eqpList)
                    {
                        MES_BN_ProductProcessRouteEquipment detail1 = new MES_BN_ProductProcessRouteEquipment();
                        detail1.ProcessRouteID = detailID;
                        detail1.EquipmentCode = eqp.EquipmentCode;
                        detail1.EquipmentName = eqp.EquipmentName;
                        detail1.epqID = eqp.epqID;
                        rowsAffected = db.Insert<MES_BN_ProductProcessRouteEquipment>("MES_BN_ProductProcessRouteEquipment", detail1)
             .AutoMap(x => x.ID)
             .Execute();
                        if (rowsAffected < 0)
                        {
                            db.Rollback();
                            return rowsAffected;
                        }
                    }

                    sql = String.Format(@"select * from PRS_ProcessModelWorkSteps where MainID = '{0}'", item.ID);
                    var stepList = db.Sql(sql).QueryMany<PRS_ProcessModelWorkSteps>();//
                    foreach (var step in stepList)
                    {
                        PRS_ProcessWorkSteps detail2 = new PRS_ProcessWorkSteps();
                        detail2.ProcessRouteID = detailID;
                        detail2.WorkStepsLineCode = step.WorkStepsLineCode;
                        detail2.WorkStepsName = step.WorkStepsName;
                        detail2.WorkStepsContent = step.WorkStepsContent;
                        detail2.ManHours = step.ManHours;
                        detail2.Unit = step.Unit;
                        detail2.IsEnable = 1;
                        detail2.CreatePerson = MmsHelper.GetUserName();
                        detail2.CreateTime = DateTime.Now;
                        detail2.ModifyPerson = MmsHelper.GetUserName();
                        detail2.ModifyTime = DateTime.Now;
                        rowsAffected = db.Insert<PRS_ProcessWorkSteps>("PRS_ProcessWorkSteps", detail2)
             .AutoMap(x => x.ID)
             .Execute();
                        if (rowsAffected < 0)
                        {
                            db.Rollback();
                            return rowsAffected;
                        }
                    }

                }

                db.Commit();
            }
            return list;
        }


        public string GetMaxCode()
        {
            string sql = @"select isnull(Max(ProcessCode), 0)+1 as maxCode from PRS_ProcessRouteModelMain";
            string code = db.Sql(sql).QuerySingle<string>();
            if (int.Parse(code) < 10)
            {
                code = "00" + code;
            }
            else if (int.Parse(code) < 100)
            {
                code = "0" + code;
            }

            return code;
        }

    }

    public class PRS_ProcessRouteModelMain : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string ProcessRouteCode { get; set; }
        public string ProcessRouteName { get; set; }
        public string ContractCode { get; set; }
        public string PartCode { get; set; }
        public string PartFigure { get; set; }
        public string Remark { get; set; }
        public int? BillState { get; set; }
        public int? IsEnable { get; set; }
        public string CraetePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ProcessModelType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_BD_ProcessRouteModelService : ServiceBase<PRS_BD_ProcessRouteModel>
    {
        public int SaveProcessRouteModel(dynamic data)
        {
            var pQuery = ParamQuery.Instance()
                .Select("*")
                .AndWhere("ContractCode", data["ContractCode"].ToString())
                .AndWhere("PartCode", data["PartCode"].ToString())
                .AndWhere("ProcessModelType", data["ProcessModelType"].ToString());

            var mainID = "";
            string cPerson= MmsHelper.GetUserName();
            var list = new PRS_ProcessRouteModelMainService().GetModelList(pQuery);
            if (list.Count > 0)
            {
                mainID = list[0].ID.ToString();
            }
            PRS_ProcessRouteModelMain master = new PRS_ProcessRouteModelMain();
            int rowsAffected = 0;
            if (string.IsNullOrEmpty(mainID))
            {
                master.ProcessRouteCode = MmsHelper.GetLSNumber("PRS_ProcessRouteModelMain", "ProcessRouteCode", "GYMX", "", "");
                master.ProcessRouteName = data["PartName"].ToString();
                master.BillState = 0;
                master.IsEnable = 1;
                master.ContractCode = data["ContractCode"].ToString();
                master.PartCode = data["PartCode"].ToString();
                master.PartFigure = data["PartFigureCode"].ToString();
                master.CraetePerson = MmsHelper.GetUserName();
                master.CreateTime = DateTime.Now;
                master.ModifyPerson = master.CraetePerson;
                master.ModifyTime = master.CreateTime;
                master.ProcessModelType = data["ProcessModelType"].ToString();
                db.UseTransaction(true);
                rowsAffected = db.Sql(@"insert into PRS_ProcessRouteModelMain (ID,ProcessRouteCode,ProcessRouteName,BillState,IsEnable,ContractCode,PartCode,PartFigure,
CraetePerson,CreateTime,ModifyPerson,ModifyTime,ProcessModelType) values(((select isnull( MAX(id),0)+1 from PRS_ProcessRouteModelMain)),@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)",
master.ProcessRouteCode, master.ProcessRouteName, master.BillState, master.IsEnable, master.ContractCode, master.PartCode, master.PartFigure, master.CraetePerson,
   master.CreateTime, master.ModifyPerson, master.ModifyTime, master.ProcessModelType).Execute();
                if (rowsAffected < 0)
                {
                    db.Rollback();
                    return rowsAffected;
                }
            }
            else
            {
                master = list[0];
            }
            var sql = String.Format(@"update PRS_ProcessRouteModelMain set BillState=0,ModifyPerson='{1}',ModifyTime='{2}' where ID = '{0}'", master.ID, MmsHelper.GetUserName(), DateTime.Now);
            db.Sql(sql).Execute();
            sql = String.Format(@"select * from PRS_ProcessRouteModelMain where ProcessRouteCode = '{0}'", master.ProcessRouteCode);
            dynamic next = db.Sql(sql).QuerySingle<dynamic>();//
            string delSql = "";
            sql = String.Format(@"select * from PRS_ProcessRouteModelDetail where MainID = '{0}'", next.ID);
            var detalList = db.Sql(sql).QueryMany<PRS_ProcessRouteModelDetail>();
            foreach (var item in detalList)
            {
                delSql = String.Format(@"delete PRS_ProcessRouteModelEquipment where MainID = '{0}'", item.ID);
                db.Sql(delSql).Execute();
                delSql = String.Format(@"delete PRS_ProcessModelWorkSteps where MainID = '{0}'", item.ID);
                db.Sql(delSql).Execute();
            }

            delSql = String.Format(@"delete PRS_ProcessRouteModelDetail where MainID = '{0}'", next.ID);
            db.Sql(delSql).Execute();
            
            foreach (var item in data.model)
            {
                PRS_ProcessRouteModelDetail detail = new PRS_ProcessRouteModelDetail();
                detail.MainID = next.ID;
                detail.ProcessCode = item.ProcessCode;
                detail.ProcessName = item.ProcessName;
                detail.ProcessLineCode = item.ProcessLineCode;
                detail.DepartmentCode = item.WorkshopID;
                detail.IsEnable = 1;
                detail.CraetePerson = MmsHelper.GetUserName();
                detail.CreateTime = DateTime.Now;
                detail.ModifyPerson = detail.CraetePerson;
                detail.ModifyTime = detail.CreateTime;
                var detailID = db.Insert<PRS_ProcessRouteModelDetail>("PRS_ProcessRouteModelDetail", detail)
      .AutoMap(x => x.ID).ExecuteReturnLastId<int>();
                if (detailID < 0)
                {
                    db.Rollback();
                    return detailID;
                }

                sql = String.Format(@"select * from MES_BN_ProductProcessRouteEquipment where ProcessRouteID = '{0}'", item.ID);
                var eqpList = db.Sql(sql).QueryMany<MES_BN_ProductProcessRouteEquipment>();//
                foreach (var eqp in eqpList)
                {
                    PRS_ProcessRouteModelEquipment detail1 = new PRS_ProcessRouteModelEquipment();
                    detail1.MainID = detailID;
                    detail1.EquipmentCode = eqp.EquipmentCode;
                    detail1.EquipmentName = eqp.EquipmentName;
                    detail1.epqID = eqp.epqID;
                    detail1.IsEnable = 1;
                    detail1.CreatePerson = MmsHelper.GetUserName();
                    detail1.CreateTime = DateTime.Now;
                    detail1.ModifyPerson = detail1.CreatePerson;
                    detail1.ModifyTime = detail1.CreateTime;
                    rowsAffected = db.Insert<PRS_ProcessRouteModelEquipment>("PRS_ProcessRouteModelEquipment", detail1)
         .AutoMap(x => x.ID)
         .Execute();
                    if (rowsAffected < 0)
                    {
                        db.Rollback();
                        return rowsAffected;
                    }
                }
                sql = String.Format(@"select * from PRS_ProcessWorkSteps where ProcessRouteID = '{0}'", item.ID);
                var stepList = db.Sql(sql).QueryMany<PRS_ProcessWorkSteps>();//
                foreach (var step in stepList)
                {
                    PRS_ProcessModelWorkSteps detail2 = new PRS_ProcessModelWorkSteps();
                    detail2.MainID = detailID;
                    detail2.WorkStepsLineCode = step.WorkStepsLineCode;
                    detail2.WorkStepsName = step.WorkStepsName;
                    detail2.WorkStepsContent = step.WorkStepsContent;
                    //detail2.ManHours = step.ManHours;
                    detail2.Unit = step.Unit;
                    detail2.IsEnable = 1;
                    detail2.CreatePerson = MmsHelper.GetUserName();
                    detail2.CreateTime = DateTime.Now;
                    detail2.ModifyPerson = detail2.CreatePerson;
                    detail2.ModifyTime = detail2.CreateTime;
                    rowsAffected = db.Insert<PRS_ProcessModelWorkSteps>("PRS_ProcessModelWorkSteps", detail2)
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
            return 1;
        }
    }

    public class PRS_BD_ProcessRouteModel : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string ProcessRouteCode { get; set; }
        public string ProcessRouteName { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessLineCode { get; set; }
        public string Remark { get; set; }
        public int? IsEnable { get; set; }
        public string CraetePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}

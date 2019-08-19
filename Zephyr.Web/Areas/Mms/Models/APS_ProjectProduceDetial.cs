using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.Linq;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class APS_ProjectProduceDetialService : ServiceBase<APS_ProjectProduceDetial>
    {
        public int BuildPlan(string id, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"select * from PRS_RectificationMain where ID = '{0}' ", id);
            var PRS_RectificationMain = db.Sql(sql).QuerySingle<dynamic>();

            sql = String.Format(@"select * from PRS_RectificationDetail where MainID = '{0}' ", id);
            var PRS_RectificationDetailList = db.Sql(sql).QueryMany<PRS_RectificationDetail>();
            if (PRS_RectificationDetailList.Count <= 0)
            {
                msg = "没有明细数据！";
                return 0;
            }
            foreach (var item in PRS_RectificationDetailList)
            {
                APS_ProjectProduceDetial plan = new APS_ProjectProduceDetial();
                plan.ContractCode = PRS_RectificationMain.ContractCode;
                plan.ProjectDetailID = PRS_RectificationMain.ProjectDetailID;
                plan.PartCode = item.PartCode;
                plan.ProcessCode = item.ProcessCode;
                plan.ProcessLineCode = item.ProcessLineCode;

                plan.WorkshopID = item.WorkshopCode;
                plan.WorkshopName = item.WorkshopName;
                plan.EquipmentID = item.EquipmentCode;
                plan.EquipmentName = item.EquipmentName;
                plan.WorkGroupID = item.WorkGroupCode;
                plan.WorkGroupName = item.WorkGroupName;
                plan.Quantity = item.Quantity;
                plan.ManHour = item.ManHour;
                plan.Unit = item.Unit;
                plan.IsEnable = 1;
                plan.PlanState = 0;
                plan.Unit = item.Unit;


                rowsAffected = plan.ID = db.Insert<APS_ProjectProduceDetial>("APS_ProjectProduceDetial", plan)
.AutoMap(x => x.ID)
.ExecuteReturnLastId<int>();

            }
            sql = String.Format(@"update PRS_RectificationMain set BillState=1 where ID = '{0}' ", id);
            db.Sql(sql).Execute();
            return rowsAffected;
        }
        public int GetIsExit(string ContractCode, string ProjectDetailID, string PlanType)
        {
            string sql = string.Format("select count(*) from APS_ProjectProduceDetial where ContractCode='{0}' and ProjectDetailID='{1}' and PlanType='{2}'", ContractCode, ProjectDetailID, PlanType);
            return db.Sql(sql).QuerySingle<int>();
        }

        public int PostPlanRelease(string type, string ids, out string msg)
        {
            msg = "操作失败！";

            var list = new APS_ProjectProduceDetialService().GetModelList(ParamQuery.Instance().AndWhere("ID", ids, Cp.In));
            if (list.Where(a => a.PlanedStartTime == null || a.PlanedFinishTime == null).Count() > 0)
            {
                msg = "计划开始时间或结束时间为空，请重排计划时间再下达！";
                return -1;
            }
            ids = "(" + ids + ")";
            string sql = string.Format(@"update APS_ProjectProduceDetial set PlanState='{0}' where ID in {1}", type, ids);
            var rowsAffected = db.Sql(sql).Execute();

            if (rowsAffected > 0)
            {
                msg = "操作成功！";
            }
            return rowsAffected;
        }

        //
        /// <summary>
        /// 计划排程自制
        /// </summary>
        /// <param name="type">排程类型正排1倒排2</param>
        /// <param name="ContractCode">合同号</param>
        /// <param name="ProjectDetailID">产品ID</param>
        /// <param name="PartCode">零件号</param>
        /// <param name="StartPlanTime">开始时间</param>
        /// <param name="EndPlanTime">结束时间</param>
        /// <param name="ProductPlanCode">生产批次号</param>
        /// <param name="DesignTaskCode">设计任务号</param>
        /// <param name="msg">返回错误信息</param>
        /// <returns></returns>
        public int PostBuildPlan(string type, string ContractCode, string ProjectDetailID, string PartCode, DateTime StartPlanTime, string MonthPlanCode, out string msg)
        {
            try
            {
                //判断是否在工作时间
                if ((StartPlanTime.TimeOfDay.Hours > 17 || (StartPlanTime.TimeOfDay.Hours == 17 && (StartPlanTime.TimeOfDay.Minutes > 0 || StartPlanTime.TimeOfDay.Seconds > 0))) || StartPlanTime.TimeOfDay.Hours < 8)
                {
                    msg = "计划时间必须在早上8点到下午5点之间！";
                    return 0;
                }
                //if ((EndPlanTime.TimeOfDay.Hours > 17 || (EndPlanTime.TimeOfDay.Hours == 17 && (EndPlanTime.TimeOfDay.Minutes > 0 || EndPlanTime.TimeOfDay.Seconds > 0))) || EndPlanTime.TimeOfDay.Hours < 8)
                //{
                //    msg = "计划时间必须在早上8点到下午5点之间！";
                //    return 0;
                //}
            }
            catch (Exception)
            {
                msg = "计划时间必须在早上8点到下午5点之间！";
                return 0;
            }

            msg = string.Empty;
            var rowsAffected = 0;

            string sql = String.Format(@"select * from APS_ProjectProduceDetial where PlanType=1 and ContractCode='{0}' and ProjectDetailID='{1}' and MonthPlanCode='{2}' and PartCode='{3}'", ContractCode, ProjectDetailID, MonthPlanCode, PartCode);
            //查出本次需要排计划的数据
            var APS_ProjectProduceDetialList = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();//db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            if (APS_ProjectProduceDetialList.Count == 0)
            {
                msg = "请先保存生产计划！";
                return 0;
            }
            if (APS_ProjectProduceDetialList.Where(a => a.ActualStartTime == null).Count() == 0)
            {
                msg = "生产计划正在执行！";
                return 0;
            }
            var manhour = (from p in APS_ProjectProduceDetialList where p.ManHour <= 0 || p.ManHour == null select p).Any();
            if (manhour)
            {
                msg = "工时定额不能为空！";
                return 0;
            }
            var quantity = (from p in APS_ProjectProduceDetialList where p.Quantity <= 0 || p.Quantity == null select p).Any();
            if (quantity)
            {
                msg = "生产数量不能小于0！";
                return 0;
            }
            //List<dynamic> boomlist = new List<dynamic>();//查出bom数据，里面加level区分等级
            //sql = String.Format(@"select * from PRS_Process_BOM where PartCode='{0}' ", PartCode);
            //var list = db.Sql(sql).QueryMany<dynamic>();
            //int level = 0;
            //if (list.Count == 0)
            //{
            //    msg = "没有BOM数据！";
            //    return 0;
            //}
            //while (list.Count != 0)
            //{
            //    var part = "(";
            //    foreach (var item in list)
            //    {
            //        part += "'" + item.PartCode + "',";
            //        item.Level = level;
            //    }
            //    boomlist.AddRange(list);
            //    level++;
            //    part = part.Remove(part.Length - 1, 1);
            //    part += ")";
            //    sql = String.Format(@"select * from PRS_Process_BOM where ParentCode in {0} ", part);
            //    list = db.Sql(sql).QueryMany<dynamic>();
            //}
            //算出bom里所有等级
            //var LevelList = (from p in boomlist group p by p.Level into g orderby g.Key descending select g.Key).ToList();
            //DateTime? LevelEndTime = null;
            //DateTime? LevelStartTime = null;
            string updateSql = "";//变量用来生成执行的sql语句
            string executeSql = string.Empty;//最后执行的sql语句
            //DateTime PlanTime = new DateTime();
            //if (type == "2")//倒排，写开始时间，结束时间，最晚开始时间，最晚结束时间
            //{
            //    updateSql = @" update dbo.APS_ProjectProduceDetial set PlanedStartTime ='{0}',PlanedFinishTime='{1}',LatestStartTime='{0}',LatestFinishTime='{1}' where ID = '{2}' ";
            //    LevelList = (from p in LevelList orderby p select p).ToList();
            //    PlanTime = EndPlanTime;
            //}
            //else//正排写开始时间，结束时间，最早开始时间，最早结束时间
            //{
            updateSql = @"update dbo.APS_ProjectProduceDetial set PlanedStartTime ='{0}',PlanedFinishTime='{1}',EarliestStartTime='{0}',EarliestFinishTime='{1}' where ID = '{2}' ";
            //PlanTime = StartPlanTime;
            //}
            //foreach (var item in LevelList)//循环bom等级
            //{
            //var part = (from p in boomlist where p.Level == item select p.PartCode).ToList();//查出最低级bom
            //var planList_part = (from p in APS_ProjectProduceDetialList where part.Contains(p.PartCode) select p).ToList();//从生产计划里查出最低级bom零件的计划
            //var planList_part = (from p in APS_ProjectProduceDetialList orderby p.ProcessLineCode select p).ToList();
            //foreach (var it in planList_part)
            //{
            //    var planList_OP = (from p in planList_part where p.PartCode == it.PartCode orderby p.ProcessLineCode select p).ToList();//按照工序行号排序正排
            //    DateTime? endTime = LevelEndTime == null ? PlanTime : LevelEndTime;
            //    DateTime? startTime = LevelStartTime == null ? PlanTime : LevelStartTime;
            //    if (type == "1")//正排
            //    {
            DateTime? endTime = StartPlanTime;
            foreach (var itm in APS_ProjectProduceDetialList.Where(a => a.ActualStartTime == null).OrderBy(a => a.ProcessLineCode))//往每道工序里写时间
            {
                itm.PlanedStartTime = endTime;//给开始时间
                if (Convert.ToDateTime(itm.PlanedStartTime).TimeOfDay.Hours == 17)//如果开始时间等于下午5点，就改成第二天8点
                {
                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddDays(1).AddHours(8);
                }
                int opMinute = (int)itm.ManHour * (int)itm.Quantity;//算出当前工序需要的时间，如果加上开始时间大于当天下午5点，就累加到第二天的时间
                if (opMinute > (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes)
                {
                    int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) / (9 * 60) + 1;
                    int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) % (9 * 60);
                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(8).AddDays(day).AddMinutes(less);
                }
                else//如果加上开始时间小于当天下午5点，直接累加时间
                {
                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).AddMinutes((Double)(itm.ManHour * itm.Quantity));
                }
                executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);//生成sql
                endTime = itm.PlanedFinishTime;//下一道工序开始时间
                //}
            }
            //else//倒排
            //{
            //    planList_OP = (from p in planList_OP orderby p.ProcessLineCode descending select p).ToList();//按照工序行号排序倒排
            //    foreach (var itm in planList_OP)
            //    {
            //        itm.PlanedFinishTime = startTime;//给结束时间
            //        int opMinute = (int)itm.ManHour * (int)itm.Quantity;
            //        //如果时间小于当天8点，就累减到前一天的时间，没有就直接累减时间
            //        if (opMinute > (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes)
            //        {
            //            int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) / (9 * 60) + 1;
            //            int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) % (9 * 60);
            //            itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(17).AddDays(-day).AddMinutes(-less);
            //        }
            //        else
            //        {
            //            itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).AddMinutes((Double)(-itm.ManHour * itm.Quantity));
            //        }
            //        executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);
            //        startTime = itm.PlanedStartTime;//下一道工序结束时间
            //    }
            //}
            //    }
            //    LevelEndTime = (from p in planList_part orderby p.PlanedFinishTime descending select p.PlanedFinishTime).FirstOrDefault();
            //    LevelStartTime = (from p in planList_part orderby p.PlanedStartTime select p.PlanedStartTime).FirstOrDefault();
            //}
            //到这结束下面是算最早最晚开始结束时间，和上面代码一样
            //LevelEndTime = null;
            //LevelStartTime = null;
            //type = type == "1" ? "2" : "1";
            //if (type == "2")
            //{
            //    updateSql = @" update dbo.APS_ProjectProduceDetial set LatestStartTime='{0}',LatestFinishTime='{1}' where ID = '{2}' ";
            //    LevelList = (from p in LevelList orderby p select p).ToList();
            //    PlanTime = EndPlanTime;
            //}
            //else
            //{
            //    updateSql = @" update dbo.APS_ProjectProduceDetial set EarliestStartTime='{0}',EarliestFinishTime='{1}' where ID = '{2}' ";
            //    LevelList = (from p in LevelList orderby p descending select p).ToList();
            //    PlanTime = StartPlanTime;
            //}
            //foreach (var item in LevelList)
            //{
            //    var part = (from p in boomlist where p.Level == item select p.PartCode).ToList();
            //    var planList_part = (from p in APS_ProjectProduceDetialList where part.Contains(p.PartCode) select p).ToList();
            //    foreach (var it in planList_part)
            //    {
            //        var planList_OP = (from p in planList_part where p.PartCode == it.PartCode orderby p.ProcessLineCode select p).ToList();
            //        DateTime? endTime = LevelEndTime == null ? PlanTime : LevelEndTime;
            //        DateTime? startTime = LevelStartTime == null ? PlanTime : LevelStartTime;
            //        if (type == "1")
            //        {

            //            foreach (var itm in planList_OP)
            //            {
            //                itm.PlanedStartTime = endTime;
            //                if (Convert.ToDateTime(itm.PlanedStartTime).TimeOfDay.Hours == 17)
            //                {
            //                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddDays(1).AddHours(8);
            //                }
            //                int opMinute = (int)itm.ManHour * (int)itm.Quantity;
            //                if (opMinute > (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes)
            //                {
            //                    int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) / (9 * 60) + 1;
            //                    int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) % (9 * 60);
            //                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(8).AddDays(day).AddMinutes(less);
            //                }
            //                else
            //                {
            //                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).AddMinutes((Double)(itm.ManHour * itm.Quantity));
            //                }
            //                executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);
            //                endTime = itm.PlanedFinishTime;
            //            }
            //        }
            //        else
            //        {
            //            planList_OP = (from p in planList_OP orderby p.ProcessLineCode descending select p).ToList();
            //            foreach (var itm in planList_OP)
            //            {
            //                itm.PlanedFinishTime = startTime;
            //                int opMinute = (int)itm.ManHour * (int)itm.Quantity;
            //                if (opMinute > (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes)
            //                {
            //                    int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) / (9 * 60) + 1;
            //                    int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) % (9 * 60);
            //                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(17).AddDays(-day).AddMinutes(-less);
            //                }
            //                else
            //                {
            //                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).AddMinutes((Double)(-itm.ManHour * itm.Quantity));
            //                }
            //                executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);
            //                startTime = itm.PlanedStartTime;
            //            }
            //        }
            //    }
            //    LevelEndTime = (from p in planList_part orderby p.PlanedFinishTime descending select p.PlanedFinishTime).FirstOrDefault();
            //    LevelStartTime = (from p in planList_part orderby p.PlanedStartTime select p.PlanedStartTime).FirstOrDefault();
            //}
            rowsAffected = db.Sql(executeSql).Execute();
            return rowsAffected;
        }
        //计划排程采购
        public int PostBuildPlan2(string type, string ContractCode, string ProjectDetailID, string PartCode, DateTime EndPlanTime, string ProductPlanCode, string DesignTaskCode, out string msg)
        {
            try
            {

                if ((EndPlanTime.TimeOfDay.Hours > 17 || (EndPlanTime.TimeOfDay.Hours == 17 && (EndPlanTime.TimeOfDay.Minutes > 0 || EndPlanTime.TimeOfDay.Seconds > 0))) || EndPlanTime.TimeOfDay.Hours < 8)
                {
                    msg = "计划时间必须在早上8点到下午5点之间！";
                    return 0;
                }
            }
            catch (Exception)
            {
                msg = "计划时间必须在早上8点到下午5点之间！";
                return 0;
            }

            msg = string.Empty;
            var rowsAffected = 0;

            //string sql = String.Format(@"select top 1 * from APS_ProjectProduceDetial where PlanType=1 and ContractCode='{0}' and ProjectDetailID='{1}' and ProductPlanCode='{2}'  ", ContractCode, ProjectDetailID, ProductPlanCode);
            //var planList = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();//db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            //var plantime = (from p in planList where p.PlanedFinishTime == null || p.PlanedStartTime == null select p).Any();
            //if (plantime)
            //{
            //    msg = "请先排生产计划！";
            //    return 0;
            //}

            string sql = String.Format(@"select * from APS_ProjectProduceDetial where ContractCode='{0}' and ProjectDetailID='{1}' and ProductPlanCode='{2}' and DesignTaskCode='{3}'  ", ContractCode, ProjectDetailID, ProductPlanCode, DesignTaskCode);

            var APS_ProjectProduceDetialList = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();//db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            if (APS_ProjectProduceDetialList.Count == 0)
            {
                msg = "请先保存采购计划！";
                return 0;
            }
            //var manhour = (from p in APS_ProjectProduceDetialList where p.ManHour <= 0 || p.ManHour == null select p).Any();
            //if (manhour)
            //{
            //    msg = "工时定额不能为空！";
            //    return 0;
            //}
            var quantity = (from p in APS_ProjectProduceDetialList where p.Quantity <= 0 || p.Quantity == null select p).Any();
            if (quantity)
            {
                msg = "生产数量不能小于0！";
                return 0;
            }
            List<dynamic> boomlist = new List<dynamic>();
            sql = String.Format(@"select * from SYS_BOM where PartCode='{0}' ", PartCode);
            var list = db.Sql(sql).QueryMany<dynamic>();
            int level = 0;
            if (list.Count == 0)
            {
                msg = "没有BOM数据！";
                return 0;
            }
            while (list.Count != 0)
            {
                var part = "(";
                foreach (var item in list)
                {
                    part += "'" + item.PartCode + "',";
                    item.Level = level;
                }
                boomlist.AddRange(list);
                level++;
                part = part.Remove(part.Length - 1, 1);
                part += ")";
                sql = String.Format(@"select * from SYS_BOM where ParentCode in {0} ", part);
                list = db.Sql(sql).QueryMany<dynamic>();
            }
            var LevelList = (from p in boomlist group p by p.Level into g orderby g.Key descending select g.Key).ToList();
            DateTime? LevelEndTime = null;
            DateTime? LevelStartTime = null;
            string updateSql = "";
            string executeSql = string.Empty;
            DateTime PlanTime = new DateTime();
            if (type == "2")
            {
                updateSql = @" update dbo.APS_ProjectProduceDetial set PlanedStartTime ='{0}',PlanedFinishTime='{1}',LatestStartTime='{0}',LatestFinishTime='{1}' where ID = '{2}' ";
                LevelList = (from p in LevelList orderby p select p).ToList();
                PlanTime = EndPlanTime;
            }


            foreach (var item in LevelList)
            {
                var part = (from p in boomlist where p.Level == item select p.PartCode).ToList();
                var planList_part = (from p in APS_ProjectProduceDetialList where part.Contains(p.PartCode) select p).ToList();
                foreach (var it in planList_part)
                {
                    var planList_OP = (from p in planList_part where p.PartCode == it.PartCode orderby p.ProcessLineCode select p).ToList();
                    DateTime? endTime = LevelEndTime == null ? PlanTime : LevelEndTime;
                    DateTime? startTime = LevelStartTime == null ? PlanTime : LevelStartTime;
                    if (type == "1")
                    {

                        foreach (var itm in planList_OP)
                        {
                            itm.PlanedStartTime = endTime;
                            if (Convert.ToDateTime(itm.PlanedStartTime).TimeOfDay.Hours == 17)
                            {
                                itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddDays(1).AddHours(8);
                            }
                            int opMinute = (int)itm.ManHour * (int)itm.Quantity;
                            if (opMinute > (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes)
                            {
                                int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) / (9 * 60) + 1;
                                int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) % (9 * 60);
                                itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(8).AddDays(day).AddMinutes(less);
                            }
                            else
                            {
                                itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).AddMinutes((Double)(itm.ManHour * itm.Quantity));
                            }
                            executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);
                            endTime = itm.PlanedFinishTime;
                        }
                    }
                    else
                    {
                        planList_OP = (from p in planList_OP orderby p.ProcessLineCode descending select p).ToList();
                        foreach (var itm in planList_OP)
                        {
                            itm.PlanedFinishTime = startTime;
                            if (itm.PlanType == 1)
                            {
                                int opMinute = (int)itm.ManHour * (int)itm.Quantity;
                                if (opMinute > (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes)
                                {
                                    int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) / (9 * 60) + 1;
                                    int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedFinishTime) - Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(8)).TotalMinutes) % (9 * 60);
                                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).Date.AddHours(17).AddDays(-day).AddMinutes(-less);
                                }
                                else
                                {
                                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).AddMinutes((Double)(-itm.ManHour * itm.Quantity));
                                }
                            }
                            if (itm.PlanType == 2)//如果是采购计划就把算好的开始时间减去这个零件的采购提前期PurchaseAdvanceTime
                            {
                                sql = String.Format(@"select PurchaseAdvanceTime from SYS_Part where PartCode = '{0}' ", itm.PartCode);
                                var time = db.Sql(sql).QuerySingle<int>();
                                if (time <= 0)
                                {
                                    msg = "零件号" + itm.PartCode + "的采购提前期必须大于0！";
                                    return 0;
                                }
                                itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedFinishTime).AddDays(-time);
                                executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);
                            }

                            startTime = itm.PlanedStartTime;
                        }
                    }
                }
                LevelEndTime = (from p in planList_part orderby p.PlanedFinishTime descending select p.PlanedFinishTime).FirstOrDefault();
                LevelStartTime = (from p in planList_part orderby p.PlanedStartTime select p.PlanedStartTime).FirstOrDefault();
            }





            rowsAffected = db.Sql(executeSql).Execute();
            return rowsAffected;
        }

        //生产计划
        public dynamic GetProduceInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime)
        {
            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string StartTime = PlanedStartTime.ToShortDateString() + " 00:00:00";
            string FinishTime = PlanedFinishTime.ToShortDateString() + " 23:59:59";
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select ProcessCode,PlanedStartTime,PlanedFinishTime,ActualStartTime,ActualFinishTime from APS_ProjectProduceDetial where PlanedStartTime>='{0}' and PlanedFinishTime<='{1}'", StartTime, FinishTime);
                list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            }
            List<dynamic> list_y = new List<dynamic>();

            List<dynamic> list_a = new List<dynamic>();
            List<dynamic> list_b = new List<dynamic>();
            List<dynamic> list_c = new List<dynamic>();

            List<dynamic> list_d = new List<dynamic>();
            List<dynamic> list_e = new List<dynamic>();
            List<dynamic> list_f = new List<dynamic>();

            List<dynamic> color_a = new List<dynamic>();
            List<dynamic> color_b = new List<dynamic>();
            List<dynamic> color_c = new List<dynamic>();
            List<dynamic> color_d = new List<dynamic>();
            List<dynamic> color_e = new List<dynamic>();
            List<dynamic> color_f = new List<dynamic>();

            for (int i = 0; i <= list.Count - 1; i++)
            {
                list_y.Add(list[i].ProcessCode);
                //计划开始时间
                double planstart = (Convert.ToDateTime(list[i].PlanedStartTime).Subtract(DateTime.Parse("1970-1-1"))).TotalMilliseconds;
                //计划结束时间
                double planfinish = (Convert.ToDateTime(list[i].PlanedFinishTime).Subtract(DateTime.Parse("1970-1-1"))).TotalMilliseconds;

                //截止时间
                double end = (Convert.ToDateTime(FinishTime).Subtract(DateTime.Parse("1970-1-1"))).TotalMilliseconds;

                //实际开始时间
                double actualstart = (Convert.ToDateTime(list[i].ActualStartTime).Subtract(DateTime.Parse("1970-1-1"))).TotalMilliseconds;

                //实际结束时间
                double actualfinish = (Convert.ToDateTime(list[i].ActualFinishTime).Subtract(DateTime.Parse("1970-1-1"))).TotalMilliseconds;

                list_a.Add(planstart);
                list_b.Add(planfinish - planstart);
                list_c.Add(end - planfinish);

                if (list[i].ActualStartTime != null)
                {
                    list_d.Add(actualstart);
                    list_e.Add(actualfinish - actualstart);
                }
                if (list[i].ActualFinishTime != null)
                {
                    list_f.Add(end - actualfinish);
                }

                color_a.Add("white");
                color_b.Add("blue");
                color_c.Add("white");
                color_d.Add("white");
                if (actualfinish > planfinish)
                {
                    color_e.Add("red");
                }
                else
                {
                    color_e.Add("green");
                }
                color_f.Add("white");
            }

            var model = new
            {
                ydata = list_y,
                adata = list_a,
                bdata = list_b,
                cdata = list_c,
                ddata = list_d,
                edata = list_e,
                fdata = list_f,
                colora = color_a,
                colorb = color_b,
                colorc = color_c,
                colord = color_d,
                colore = color_e,
                colorf = color_f
            };
            return model;
        }
        //生产计划_new
        public dynamic GetPlanInfo(DateTime? PlanedStartTime, DateTime? PlanedFinishTime, string PartCode)
        {


            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string sqlWhere = "";
            if (PlanedStartTime != null)
            {
                string StartTime = Convert.ToDateTime(PlanedStartTime.ToString()).ToString("yyyy-MM-dd 00:00:00");
                sqlWhere += string.Format(" and t.PlanedStartTime>='{0}'", StartTime);
            }
            if (PlanedFinishTime != null)
            {
                string finishTime = Convert.ToDateTime(PlanedFinishTime.ToString()).ToString("yyyy-MM-dd 00:00:00");
                sqlWhere += string.Format(" and t.PlanedFinishTime<='{0}'", finishTime);
            }
            if (PartCode == "" || PartCode == null)
            {

            }
            else
            {
                string[] codeArr = PartCode.Split(',');
                if (codeArr.Length > 0)
                {
                    sqlWhere += " and t.PartCode in (";
                    foreach (string item in codeArr)
                    {
                        sqlWhere += string.Format("'{0}',", item);
                    }
                    sqlWhere = sqlWhere.Remove(sqlWhere.Length - 1, 1);
                    sqlWhere += " )";
                }
            }

            //string StartTime = PlanedStartTime.ToShortDateString() + " 00:00:00";
            //string FinishTime = PlanedFinishTime.ToShortDateString() + " 23:59:59";
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select e.PartFigureCode+' '+c.ProcessName AS ReName,
                          PlanedStartTime,PlanedFinishTime,ActualStartTime,ActualFinishTime from APS_ProjectProduceDetial t
                          left join dbo.PMS_BN_Project a on t.ContractCode=a.ContractCode
                          left join dbo.PMS_BN_ProjectDetail b on t.ProjectDetailID=b.id
                          left join dbo.PRS_BD_StandardProcess c on t.ProcessCode=c.ProcessCode
                          left join dbo.SYS_Part d on t.PartCode=d.PartCode
                          left join dbo.SYS_BOM e on t.PartCode=e.PartCode
                          where 1=1 {0}", sqlWhere);
                sql += " order by e.PartFigureCode desc,t.ProcessLineCode  ";
                list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            }
            List<dynamic> list_y = new List<dynamic>();

            List<dynamic> list_a = new List<dynamic>();
            List<dynamic> list_b = new List<dynamic>();
            List<dynamic> list_c = new List<dynamic>();
            List<dynamic> list_d = new List<dynamic>();

            List<dynamic> list_color = new List<dynamic>();

            DateTime max = new DateTime();
            for (int i = 0; i <= list.Count - 1; i++)
            {
                list_y.Add(list[i].ReName);
                //计划开始时间
                string planstart = Convert.ToDateTime(list[i].PlanedStartTime).ToShortDateString();
                //计划结束时间
                string planfinish = (Convert.ToDateTime(list[i].PlanedFinishTime)).ToShortDateString();
                //实际开始时间
                string actualstart = (Convert.ToDateTime(list[i].ActualStartTime)).ToShortDateString();
                //实际结束时间
                string actualfinish = (Convert.ToDateTime(list[i].ActualFinishTime)).ToShortDateString();

                if (list[i].PlanedStartTime != null)
                {
                    if (Convert.ToDateTime(list[i].PlanedStartTime) > max)
                        max = Convert.ToDateTime(list[i].PlanedStartTime);
                    list_a.Add(planstart);
                }
                if (list[i].PlanedFinishTime != null)
                {
                    list_b.Add(planfinish);
                }
                if (list[i].ActualStartTime != null)
                {
                    list_c.Add(actualstart);
                }
                if (list[i].ActualFinishTime != null)
                {
                    list_d.Add(actualfinish);

                    DateTime dtp = Convert.ToDateTime(list[i].PlanedFinishTime);
                    DateTime dtf = Convert.ToDateTime(list[i].ActualFinishTime);
                    if (DateTime.Compare(dtp, dtf) >= 0)
                    {
                        list_color.Add("green");
                    }
                    else
                    {
                        list_color.Add("red");
                    }
                }
            }
            //list_a.Add(max.AddDays(1).ToShortDateString());
            //list_b.Add(max.AddDays(1).ToShortDateString());
            //list_c.Add(max.AddDays(1).ToShortDateString());
            //list_d.Add(max.AddDays(1).ToShortDateString());
            var model = new
            {
                ydata = list_y,
                adata = list_a,
                bdata = list_b,
                cdata = list_c,
                ddata = list_d,
                colordata = list_color,
                rows = list_y.Count

            };
            return model;
        }

        public dynamic GetPlanInfo_New(DateTime? PlanedStartTime, DateTime? PlanedFinishTime, string ContractCode, string ProjectDetailID, string PartCode)
        {

            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string sqlWhere = "";
            if (PlanedStartTime != null)
            {
                string StartTime = Convert.ToDateTime(PlanedStartTime.ToString()).ToString("yyyy-MM-dd 00:00:00");
                sqlWhere += string.Format(" and t.PlanedStartTime>='{0}'", StartTime);
            }
            if (PlanedFinishTime != null)
            {
                string finishTime = Convert.ToDateTime(PlanedFinishTime.ToString()).ToString("yyyy-MM-dd 23:59:59");
                sqlWhere += string.Format(" and t.PlanedFinishTime<='{0}'", finishTime);
            }
            if (ContractCode != null)
            {
                sqlWhere += string.Format(" and t.ContractCode='{0}' and ProjectDetailID='{1}' ", ContractCode, ProjectDetailID);
            }
            if (PartCode == "" || PartCode == null)
            {

            }
            else
            {
                string[] codeArr = PartCode.Split(',');
                if (codeArr.Length > 0)
                {
                    sqlWhere += " and t.PartCode in (";
                    foreach (string item in codeArr)
                    {
                        sqlWhere += string.Format("'{0}',", item);
                    }
                    sqlWhere = sqlWhere.Remove(sqlWhere.Length - 1, 1);
                    sqlWhere += " )";
                }
            }
            string sql = string.Format(@"select e.PartFigureCode+' '+ case when t.PlanType=1 then c.ProcessName else d.PartName end AS ReName,
                          PlanedStartTime,PlanedFinishTime,ActualStartTime,ActualFinishTime from APS_ProjectProduceDetial t
                          left join dbo.PMS_BN_Project a on t.ContractCode=a.ContractCode
                          left join dbo.PMS_BN_ProjectDetail b on t.ProjectDetailID=b.id
                          left join dbo.PRS_BD_StandardProcess c on t.ProcessCode=c.ProcessCode
                          left join (select distinct PartCode,max(InventoryCode) InventoryCode,max(PartName) PartName from SYS_Part group by PartCode) d on t.PartCode=d.PartCode
                          left join (select distinct PartCode,max(VersionCode) as VersionCode,max(PartFigureCode) as PartFigureCode from SYS_BOM group by PartCode ) e on t.PartCode=e.PartCode
                           where 1=1 {0}", sqlWhere);
            sql += " order by e.PartFigureCode desc,t.ProcessLineCode ";
            list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            if (list.Count == 0)
            {
                List<dynamic> dataList1 = new List<dynamic>();
                Dictionary<string, object> planItemMap = new Dictionary<string, object>();
                planItemMap.Add("name", "");
                planItemMap.Add("values", "");
                dataList1.Add(planItemMap);
                return dataList1;
            }

            List<dynamic> dataList = new List<dynamic>();
            foreach (var data in list)
            {
                Dictionary<string, object> planItemMap = new Dictionary<string, object>();
                planItemMap.Add("name", data.ReName);
                Dictionary<string, string> valueMap = new Dictionary<string, string>();
                valueMap.Add("from", "/Date(" + data.PlanedStartTime + ")/");
                valueMap.Add("to", "/Date(" + data.PlanedFinishTime + ")/");
                valueMap.Add("customClass", "ganttRed");
                valueMap.Add("desc", "计划开始时间~计划结束时间" + ":</br>" + data.PlanedStartTime + "到" + data.PlanedFinishTime + "</br>");
                List<Object> planValueList = new List<object>();
                planValueList.Add(valueMap);
                planItemMap.Add("values", planValueList);
                dataList.Add(planItemMap);

                Dictionary<string, object> planItemMap2 = new Dictionary<string, object>();
                Dictionary<string, string> valueMap2 = new Dictionary<string, string>();
                valueMap2.Add("from", "/Date(" + data.ActualStartTime + ")/");
                valueMap2.Add("to", "/Date(" + data.ActualFinishTime + ")/");
                valueMap2.Add("customClass", "ganttBlue");
                valueMap2.Add("desc", "实际开始时间~实际结束时间" + ":</br>" + data.ActualStartTime + "到" + data.ActualFinishTime + "</br>");
                List<Object> planValueList2 = new List<object>();
                planValueList2.Add(valueMap2);
                planItemMap2.Add("values", planValueList2);
                dataList.Add(planItemMap2);

            }

            return dataList;
        }
        public List<dynamic> GetProduceInfo(string workGroupID)
        {
            string sql = string.Format(@"
SELECT tempA.*,tempB.ProcessName,tempB.ProcessDesc
FROM (
SELECT tbA.*,tbB.ProductName,tbB.ProductType,tbB.Model,tbB.Specifications ,tbC.[Text]
FROM dbo.APS_ProjectProduceDetial AS tbA
LEFT JOIN dbo.PMS_BN_ProjectDetail AS tbB ON tbA.ProjectDetailID = tbB.ID
LEFT JOIN (SELECT * FROM XYHP_Sys.dbo.sys_code WHERE CodeType = 'ProductType') AS tbC ON tbB.ProductType = tbC.Value
WHERE tbA.PlanState = 1
AND tbA.ActualFinishTime IS NULL
AND tbA.WorkGroupID = '{0}'
) AS tempA
LEFT JOIN dbo.MES_BN_ProductProcessRoute AS tempB 
ON tempA.ContractCode = tempB.ContractCode
WHERE tempA.ContractCode = tempB.ContractCode AND tempA.PartCode = tempB.PartCode
AND tempA.ProcessCode = tempB.ProcessCode AND tempA.WorkGroupID = tempB.WorkGroupID
", workGroupID);
            using (var db = Db.Context("Mms"))
            {
                return db.Sql(sql).QueryMany<dynamic>();
            }
        }

        //车间资源
        public dynamic GetWorkshopInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string WorkshopID)
        {
            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string StartTime = PlanedStartTime.ToShortDateString() + " 00:00:00";
            string FinishTime = PlanedFinishTime.ToShortDateString() + " 23:59:59";
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select ProcessCode,PlanedStartTime,PlanedFinishTime,ActualStartTime,ActualFinishTime from APS_ProjectProduceDetial where WorkshopID='{0}' and PlanedStartTime>='{1}' and PlanedFinishTime<='{2}'", WorkshopID, StartTime, FinishTime);
                list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            }
            List<dynamic> list_x = new List<dynamic>();

            List<dynamic> list_a = new List<dynamic>();
            List<dynamic> list_b = new List<dynamic>();
            List<dynamic> list_c = new List<dynamic>();

            for (int i = 0; i <= list.Count - 1; i++)
            {

            }

            var model = new
            {

            };
            return model;
        }
        //设备资源
        public dynamic GetEquipmentInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string EquipmentID)
        {
            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string StartTime = PlanedStartTime.AddDays(-7).ToShortDateString() + " 00:00:00";
            string FinishTime = PlanedFinishTime.AddDays(7).ToShortDateString() + " 23:59:59";
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select EquipmentID, cast(datepart(YEAR,PlanedStartTime) as varchar(4))+'-'+
	                RIGHT('00'+CAST(MONTH(PlanedStartTime) AS VARCHAR(2)),2)+'-'+
	                RIGHT('00'+CAST(day(PlanedStartTime) AS VARCHAR(2)),2) as PlanedStart,
	                sum(ManHour) ManHour from APS_ProjectProduceDetial
                    where EquipmentID='{0}' AND  PlanedStartTime>='{1}' and PlanedFinishTime<='{2}'
                    group by EquipmentID,
                    cast(datepart(YEAR,PlanedStartTime) as varchar(4))+'-'+
                    RIGHT('00'+CAST(MONTH(PlanedStartTime) AS VARCHAR(2)),2)+'-'+
                    RIGHT('00'+CAST(day(PlanedStartTime) AS VARCHAR(2)),2);", EquipmentID, StartTime, FinishTime);
                list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            }
            List<dynamic> list_x = new List<dynamic>();
            List<dynamic> list_y = new List<dynamic>();

            for (int i = 0; i <= list.Count - 1; i++)
            {
                list_x.Add(list[i].PlanedStart);
                list_y.Add(list[i].ManHour);
            }

            var model = new
            {
                xdata = list_x,
                ydata = list_y
            };
            return model;
        }
        //班组资源
        public dynamic GetWorkGroupInfo(DateTime PlanedStartTime, DateTime PlanedFinishTime, string WorkGroupID)
        {
            List<APS_ProjectProduceDetial> list = new List<APS_ProjectProduceDetial>();
            string StartTime = PlanedStartTime.AddDays(-7).ToShortDateString() + " 00:00:00";
            string FinishTime = PlanedFinishTime.AddDays(7).ToShortDateString() + " 23:59:59";
            using (var db = Db.Context("Mms"))
            {
                string sql = string.Format(@"select WorkGroupID, cast(datepart(YEAR,PlanedStartTime) as varchar(4))+'-'+
	                RIGHT('00'+CAST(MONTH(PlanedStartTime) AS VARCHAR(2)),2)+'-'+
	                RIGHT('00'+CAST(day(PlanedStartTime) AS VARCHAR(2)),2) as PlanedStart,
	                sum(ManHour) ManHour from APS_ProjectProduceDetial
                    where WorkGroupID='{0}' AND  PlanedStartTime>='{1}' and PlanedFinishTime<='{2}'
                    group by WorkGroupID,
                    cast(datepart(YEAR,PlanedStartTime) as varchar(4))+'-'+
                    RIGHT('00'+CAST(MONTH(PlanedStartTime) AS VARCHAR(2)),2)+'-'+
                    RIGHT('00'+CAST(day(PlanedStartTime) AS VARCHAR(2)),2);", WorkGroupID, StartTime, FinishTime);
                list = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            }
            List<dynamic> list_x = new List<dynamic>();
            List<dynamic> list_y = new List<dynamic>();

            for (int i = 0; i <= list.Count - 1; i++)
            {
                list_x.Add(list[i].PlanedStart);
                list_y.Add(list[i].ManHour);
            }

            var model = new
            {
                xdata = list_x,
                ydata = list_y
            };
            return model;
        }

        public int PostBuildPlan_Blanking(string type, string ContractCode, DateTime StartPlanTime, out string msg)
        {
            try
            {
                //判断是否在工作时间
                if ((StartPlanTime.TimeOfDay.Hours > 17 || (StartPlanTime.TimeOfDay.Hours == 17 && (StartPlanTime.TimeOfDay.Minutes > 0 || StartPlanTime.TimeOfDay.Seconds > 0))) || StartPlanTime.TimeOfDay.Hours < 8)
                {
                    msg = "计划时间必须在早上8点到下午5点之间！";
                    return 0;
                }
            }
            catch (Exception)
            {
                msg = "计划时间必须在早上8点到下午5点之间！";
                return 0;
            }

            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"select * from APS_ProjectProduceDetial where PlanType=1 and ContractCode='{0}' and ProcessModelType='1'", ContractCode);
            //查出本次需要排计划的数据
            var APS_ProjectProduceDetialList = db.Sql(sql).QueryMany<APS_ProjectProduceDetial>();
            if (APS_ProjectProduceDetialList.Count == 0)
            {
                msg = "请先保存生产计划！";
                return 0;
            }
            if (APS_ProjectProduceDetialList.Where(a => a.ActualStartTime == null).Count() == 0)
            {
                msg = "生产计划正在执行！";
                return 0;
            }
            var manhour = (from p in APS_ProjectProduceDetialList where p.ManHour <= 0 || p.ManHour == null select p).Any();
            if (manhour)
            {
                msg = "工时定额不能为空！";
                return 0;
            }
            var quantity = (from p in APS_ProjectProduceDetialList where p.Quantity <= 0 || p.Quantity == null select p).Any();
            if (quantity)
            {
                msg = "生产数量不能小于0！";
                return 0;
            }
            string updateSql = "";//变量用来生成执行的sql语句
            string executeSql = string.Empty;//最后执行的sql语句
            updateSql = @"update dbo.APS_ProjectProduceDetial set PlanedStartTime ='{0}',PlanedFinishTime='{1}',EarliestStartTime='{0}',EarliestFinishTime='{1}' where ID = '{2}' ";
            DateTime? endTime = StartPlanTime;
            foreach (var itm in APS_ProjectProduceDetialList.Where(a => a.ActualStartTime == null).OrderBy(a => a.ProcessLineCode))//往每道工序里写时间
            {
                itm.PlanedStartTime = endTime;//给开始时间
                if (Convert.ToDateTime(itm.PlanedStartTime).TimeOfDay.Hours == 17)//如果开始时间等于下午5点，就改成第二天8点
                {
                    itm.PlanedStartTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddDays(1).AddHours(8);
                }
                int opMinute = (int)itm.ManHour * (int)itm.Quantity;//算出当前工序需要的时间，如果加上开始时间大于当天下午5点，就累加到第二天的时间
                if (opMinute > (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes)
                {
                    int day = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) / (9 * 60) + 1;
                    int less = (int)(opMinute - (Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(17) - Convert.ToDateTime(itm.PlanedStartTime)).TotalMinutes) % (9 * 60);
                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).Date.AddHours(8).AddDays(day).AddMinutes(less);
                }
                else//如果加上开始时间小于当天下午5点，直接累加时间
                {
                    itm.PlanedFinishTime = Convert.ToDateTime(itm.PlanedStartTime).AddMinutes((Double)(itm.ManHour * itm.Quantity));
                }
                executeSql += string.Format(updateSql, itm.PlanedStartTime, itm.PlanedFinishTime, itm.ID);//生成sql
                endTime = itm.PlanedFinishTime;//下一道工序开始时间
            }
            rowsAffected = db.Sql(executeSql).Execute();
            return rowsAffected;
        }
    }

    public class APS_ProjectProduceDetial : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public int MainID { get; set; }
        public string ContractCode { get; set; }
        public int? ProjectDetailID { get; set; }
        public int? ProductPlanMainID { get; set; }
        public string PartCode { get; set; }

        /*
         ProductPlanCode 
         ProcessName 
         */
        public string ProcessName { get; set; }
        public string ProductPlanCode { get; set; }
        public string ProcessCode { get; set; }
        public int? ProcessLineCode { get; set; }
        public string WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public string WorkGroupID { get; set; }
        public string WorkGroupName { get; set; }
        public int? Quantity { get; set; }
        public int? ManHour { get; set; }
        public int? Unit { get; set; }
        public DateTime? EarliestStartTime { get; set; }
        public DateTime? LatestStartTime { get; set; }
        public DateTime? PlanedStartTime { get; set; }
        public DateTime? EarliestFinishTime { get; set; }
        public DateTime? LatestFinishTime { get; set; }
        public DateTime? PlanedFinishTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualFinishTime { get; set; }
        public DateTime? FloatingHour { get; set; }
        public string PlanColor { get; set; }
        public int? PlanState { get; set; }
        public int? PlanType { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }

        public string PlanedStart { get; set; }

        public string ReName { get; set; }

        public string ApproveState { get; set; }

        public string ApprovePerson { get; set; }

        public DateTime? ApproveDate { get; set; }

        public string ApproveRemark { get; set; }
        public string ApsCode { get; set; }
        public int? BomQty { get; set; }
        public string DesignTaskCode { get; set; }
        public string RootPartCode { get; set; }
        public string MonthPlanCode { get; set; }
        public string ProcessModelType { get; set; }

        public string SavantCode { get; set; }
    }
}

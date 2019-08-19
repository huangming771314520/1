using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Areas.Mms.Controllers;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class PRS_DesignChangeRequestService : ServiceBase<PRS_DesignChangeRequest>
    {
        public int AuditBillCode(string ID, out string msg)
        {
            msg = string.Empty;
            var rowsAffected = 0;
            string sql = String.Format(@"  select * FROM PRS_DesignChangeRequest  where ID='{0}'", ID);
            var request = db.Sql(sql).QuerySingle<PRS_DesignChangeRequest>();
            if (request.RequestState == 1)
            {
                msg = "单据已审核";
                return 0;
            }
            else
            {
                using (db.UseTransaction(true))
                {
                    //List<PMS_DesignTaskDetail> dlist = new List<PMS_DesignTaskDetail>();
                    sql = string.Format(@"update PRS_DesignChangeRequest set RequestState=1 where ID='{0}'", ID);
                    rowsAffected = db.Sql(sql).Execute();
                    if (rowsAffected > 0)
                    {
                        msg = "检验记录审核成功";
                        PMS_DesignTaskDetail dt = new PMS_DesignTaskDetail();
                        //int d = GetMainID(request.ContractCode, request.ProductID);
                        var pQuery = ParamQuery.Instance().Select("top 1 ID").AndWhere("ContractCode", request.ContractCode).AndWhere("ProductID", request.ProductID);
                        int d = new PMS_ProductTaskService().GetModel(pQuery).ID;
                        dt.ID = -1;
                        dt.BillCode = MmsHelper.GetOrderNumber("PMS_DesignTaskDetail", "BillCode", "SJRW", "", "");
                        dt.MainID = d;
                        dt.ContractCode = request.ContractCode;
                        dt.ProductID = request.ProductID;
                        dt.TaskDescription = request.ChangeContent;
                        dt.TaskType = Convert.ToInt32(request.TypeID);
                        dt.IsEnable = 1;
                        dt.BillState = 0;
                        dt.BillCode = request.RequestCode;
                        dt.DesignType = 2;
                        dt.CreatePerson = MmsHelper.GetUserName();
                        dt.CreateTime = DateTime.Now;
                        dt.ModifyPerson = MmsHelper.GetUserName();
                        dt.ModifyTime = DateTime.Now;
                        //dlist.Add(dt);
                        rowsAffected = new PMS_DesignTaskDetailService().Insert(dt);
                        if (rowsAffected > 0)
                        {
                            db.Commit();
                            return rowsAffected;
                        }
                        else
                        {
                            db.Rollback();
                            msg = "审核失败，请先保存请求单据";
                            return 0;
                        }

                    }
                    else
                    {

                        db.Rollback();
                        msg = "审核失败，请先保存请求单据";
                        return 0;
                    }
                }
            }
        }
    }

    public class PRS_DesignChangeRequest : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string RequestCode { get; set; }
        public string ContractCode { get; set; }
        public string FigureNumber { get; set; }
        public string Reason { get; set; }
        public string ChangeContent { get; set; }
        public int? RequestState { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public string DocName { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string ProductID { get; set; }
        public string TypeID { get; set; }
    }
}

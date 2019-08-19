using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class MES_BN_MatchingTableDetailService : ServiceBase<MES_BN_MatchingTableDetail>
    {
        public int SetType(string ids, string type, string typeName, out string msg)
        {
            msg = "操作失败！";

            string sql = string.Format(@"update a set a.Type=(select Value from HBHC_Sys.dbo.sys_code where Text ='{1}'),a.TypeName='{1}' from MES_BN_MatchingTableDetail a  where a.ID in {2}", type, typeName, ids);
            var rowsAffected = db.Sql(sql).Execute();

            if (rowsAffected > 0)
            {
                msg = "操作成功！";
            }
            return rowsAffected;
        }

        public int GetIsExit(string ContractCode, string ProjectDetailID)
        // public int GetIsExit(string ContractCode, string ProjectDetailID,string ProductPlanCode)
        {
            //  string sql = string.Format("select count(*) from MES_BN_MatchingTableDetail where ContractCode='{0}' and ProjectDetailID='{1}' and ProductPlanCode='{2}'", ContractCode, ProjectDetailID, ProductPlanCode);
            string sql = string.Format("select count(*) from MES_BN_MatchingTableDetail where ContractCode='{0}' and ProjectDetailID='{1}' ", ContractCode, ProjectDetailID);
            return db.Sql(sql).QuerySingle<int>();
        }
        public List<string> GetNotShowBom(string PartCode, string ContractCode, string ProductName, string IsMaterial)
        {
            string sql = String.Format(@"select PartCode from V_MES_BN_MatchingTableDetail where ContractCode='{0}' and ProductName='{1}' and IsMaterial ={2}", ContractCode, ProductName, IsMaterial);
            return db.Sql(sql).QueryMany<string>();
        }
        public string GetAudit(string ContractCode, string ProjectDetailID)
        {
            string Sql2 = string.Format(@" select Top 1 ApproveState from MES_BN_MatchingTableDetail where ProjectDetailID = '{0}' and ContractCode = '{1}'", ProjectDetailID, ContractCode);
            var db = Db.Context("MMs");
            string res1 = db.Sql(Sql2).QuerySingle<string>();
            if (res1 == "1")
            {
                return "0";
            }
            string sql = string.Format(@"select count(1) from MES_BN_MatchingTableDetail 
 WHERE  ContractCode = '{0}' AND ProjectDetailID ={1}", ContractCode, ProjectDetailID);
            string res = db.Sql(sql).QuerySingle<string>();
            if (res == "0")
            {
                return "1";
            }
            else
            {
                sql = string.Format(@"select count(1) from MES_BN_MatchingTableDetail 
 WHERE  ContractCode = '{0}' and Type is null AND ProjectDetailID ={1}", ContractCode, ProjectDetailID);
                res = db.Sql(sql).QuerySingle<string>();
                if (res != "0")
                {
                    return "2";
                }
                else
                {
                    string Sql1 = string.Format(@"update MES_BN_MatchingTableDetail set ApproveState = '1' where ProjectDetailID = '{0}' AND ContractCode = '{1}'", ProjectDetailID, ContractCode);
                    int res2 = db.Sql(Sql1).Execute();
                    if (res2 > 0)
                    {
                        return "3";
                    }
                    else
                        return "4";
                }


            }


        }

        public void Cover_MES_BN_MatchingTableDetail(List<MES_BN_MatchingTableDetail> MES_BN_MatchingTableDetailList, List<PRS_Process_BOM> ProcessBomList)
        {
            try
            {
                db.UseTransaction(true);

                var MaxID = new MES_BN_MatchingTableDetailService().GetField<int>(ParamQuery.Instance().Select("max(ID) as ID").From("MES_BN_MatchingTableDetail"));

                string MES_BN_MatchingTableDetailListStr = string.Join(",", MES_BN_MatchingTableDetailList.Select(a => a.ID));
                db.Sql($"update MES_BN_MatchingTableDetail set IsEnable=0 where ID in({MES_BN_MatchingTableDetailListStr})").Execute();

                var PMS_BN_ProjectList = new PMS_BN_ProjectService().GetModelList();
                var PMS_BN_ProjectDetailList = new PMS_BN_ProjectDetailService().GetModelList();
                
                var UpdateList = new List<int>();
                ProcessBomList.ForEach(item =>
                {
                    var PartCode = item.PartCode;
                    var ParentCode = item.ParentCode;
                    var IsExist = MES_BN_MatchingTableDetailList.Where(a => a.PartCode == PartCode && a.PPartCode == ParentCode).Select(a => a.ID);
                    if (IsExist.Count() > 0)
                    {
                        UpdateList.Add(IsExist.FirstOrDefault());
                    }
                    else
                    {
                        MES_BN_MatchingTableDetail model = new MES_BN_MatchingTableDetail();
                        model.ID = ++MaxID;
                        model.PartCode = PartCode;
                        model.PartName = item.PartName;
                        model.PPartCode = item.ParentCode;
                        model.ContractCode = item.ContractCode;
                        model.ProjectDetailID = item.ProductID;
                        model.ProjectName = PMS_BN_ProjectDetailList.Where(a => a.ID == item.ProductID).FirstOrDefault().ProductName;
                        model.NeedQuantity = item.PartQuantity ?? 0 * PMS_BN_ProjectDetailList.Where(a => a.ID == item.ProductID).FirstOrDefault().Quantity ?? 0;
                        model.Type = item.IsSelfMade;
                        model.TypeName = item.IsSelfMade == "0" ? "成品件" : "加工件";
                        model.BomQuantity = item.PartQuantity ?? 0;
                        model.IsMaterial = 0;
                        model.IsEnable = 1;
                        model.ApproveState = "1";
                        model.CreatePerson = MmsHelper.GetUserName();
                        model.CreateTime = DateTime.Now;
                        //db.Insert<MES_BN_MatchingTableDetail>("MES_BN_MatchingTableDetail", model).AutoMap(a => a.ID).ExecuteReturnLastId<int>();
                        string sql = WinFormClientService.GetInsertSQL<MES_BN_MatchingTableDetail>(model);
                        db.Sql(sql).Execute();
                    }
                });

                if (UpdateList.Count > 0)
                {
                    db.Sql($"update MES_BN_MatchingTableDetail set IsEnable=1 where ID in({string.Join(",", UpdateList)})").Execute();
                }

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
            }
        }

        public void Cover_MES_BN_MatchingTableDetail2(List<MES_BN_MatchingTableDetail> MES_BN_MatchingTableDetailList, string pCode, string cCode, int pID)
        {
            try
            {
                db.UseTransaction(true);

                var MaxID = new MES_BN_MatchingTableDetailService().GetField<int>(ParamQuery.Instance().Select("max(ID) as ID").From("MES_BN_MatchingTableDetail"));

                string MES_BN_MatchingTableDetailListStr = string.Join(",", MES_BN_MatchingTableDetailList.Select(a => a.ID));
                db.Sql($"update MES_BN_MatchingTableDetail set IsEnable=0 where ID in({MES_BN_MatchingTableDetailListStr})").Execute();

                var PMS_BN_ProjectList = new PMS_BN_ProjectService().GetModelList();
                var PMS_BN_ProjectDetailList = new PMS_BN_ProjectDetailService().GetModelList();

                var UpdateList = new List<int>();

                List<PRS_Process_BOM_Blanking> ProcessBomBlankingList = db.Sql(string.Format(@"SELECT * FROM dbo.Get_ProcessBomBlanking('{0}','{1}',{2})", pCode, cCode, pID)).QueryMany<PRS_Process_BOM_Blanking>();
                ProcessBomBlankingList.ForEach(item =>
                {
                    var PartCode = item.PartCode;
                    var ParentCode = item.ParentCode;
                    var IsExist = MES_BN_MatchingTableDetailList.Where(a => a.PartCode == PartCode && a.PPartCode == ParentCode).Select(a => a.ID);
                    if (IsExist.Count() > 0)
                    {
                        UpdateList.Add(IsExist.FirstOrDefault());
                    }
                    else
                    {
                        MES_BN_MatchingTableDetail model = new MES_BN_MatchingTableDetail();
                        model.ID = ++MaxID;
                        model.PartCode = PartCode;
                        model.PartName = item.PartName;
                        model.PPartCode = item.ParentCode;
                        model.ContractCode = item.ContractCode;
                        model.ProjectDetailID = item.ProductID;
                        model.ProjectName = PMS_BN_ProjectDetailList.Where(a => a.ID == item.ProductID).FirstOrDefault().ProductName;
                        model.NeedQuantity = item.PartQuantity ?? 0 * PMS_BN_ProjectDetailList.Where(a => a.ID == item.ProductID).FirstOrDefault().Quantity ?? 0;
                        model.Type = item.IsSelfMade;
                        model.TypeName = item.IsSelfMade == "0" ? "成品件" : "加工件";
                        //model.BomQuantity = item.PartQuantity ?? 0;
                        model.BomQuantity = item.PartQuantityAll ?? 0;
                        model.IsMaterial = 0;
                        model.IsEnable = 1;
                        model.ApproveState = "1";
                        model.CreatePerson = MmsHelper.GetUserName();
                        model.CreateTime = DateTime.Now;
                        //db.Insert<MES_BN_MatchingTableDetail>("MES_BN_MatchingTableDetail", model).AutoMap(a => a.ID).ExecuteReturnLastId<int>();
                        string sql = WinFormClientService.GetInsertSQL<MES_BN_MatchingTableDetail>(model);
                        db.Sql(sql).Execute();
                    }
                });

                if (UpdateList.Count > 0)
                {
                    db.Sql($"update MES_BN_MatchingTableDetail set IsEnable=1 where ID in({string.Join(",", UpdateList)})").Execute();
                }

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
            }
        }
    }

    public class MES_BN_MatchingTableDetail : ModelBase
    {
        [PrimaryKey]

        #region
        /*
        public int ID { get; set; }
        public int? ProjectDetailID { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string PPartCode { get; set; }
        public string PPartName { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public string ProductPlanCode { get; set; } 
        public int? BomQuantity { get; set; }
        public int? NeedQuantity { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; } 
        public int? IsMaterial { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        */
        #endregion

        /// <summary>
		/// 
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 合同编码
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ProjectDetailID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 父零件编号
        /// </summary>
        public string PPartCode { get; set; }

        /// <summary>
        /// 父零件名称
        /// </summary>
        public string PPartName { get; set; }

        /// <summary>
        /// 子零件编号
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// 子零件名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public int? NeedQuantity { get; set; }

        /// <summary>
        /// 配套类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 是否包料
        /// </summary>
        public int? IsMaterial { get; set; }

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
        /// 更新人
        /// </summary>
        public string ModifyPerson { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApproveState { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string ApprovePerson { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        public string ApproveRemark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? BomQuantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductPlanCode { get; set; }

        /// <summary>
        /// 设计任务编码
        /// </summary>
        public string DesignTaskCode { get; set; }
    }
}

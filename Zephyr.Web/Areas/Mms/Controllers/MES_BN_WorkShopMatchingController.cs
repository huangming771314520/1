
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.CommonWrap;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_BN_WorkShopMatchingController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {
                    Department = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.SYS_BN_Department", "DepartmentCode value,DepartmentName text")
                },
                urls = new
                {
                    query = "/api/Mms/MES_BN_WorkShopMatching",
                    newkey = "/api/Mms/MES_BN_WorkShopMatching/getnewkey",
                    edit = "/api/Mms/MES_BN_WorkShopMatching/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    RootPartCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "PartCode", "ProcessType", "WorkShopCode" }
                }
            };

            return View(model);
        }
    }

    public class MES_BN_WorkShopMatchingApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='temp.PartCode'>
        <select>*</select>
        <from>(SELECT DISTINCT tbC.*,tbD.PartFigureCode,tbD.PartName,isnull(tbD.MaterialCode,'') MaterialCode,tbD.PartQuantity,(tbE.Quantity*tbD.PartQuantity) AS TotoalQuantity FROM 
(select tbB.RootPartCode,tbB.PartCode,[tbB].[1] AS Blanking,[tbB].[2] AS Welding,[tbB].[3] AS Machining,[tbB].[4] AS Assembling from 
(SELECT PartCode,ProcessType,WorkShopCode,RootPartCode FROM MES_BN_WorkShopMatching WHERE IsEnable=1) tbA
 pivot(max( WorkShopCode) for ProcessType IN ([1],[2],[3],[4])) tbB) tbC
 LEFT JOIN dbo.PRS_Process_BOM tbD ON tbC.PartCode=tbD.PartCode LEFT JOIN [dbo].[PMS_BN_ProjectDetail] tbE ON tbE.ID=tbD.ProductID) as temp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='RootPartCode' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_BN_WorkShopMatchingService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicList(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new MES_BN_WorkShopMatchingService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            MES_BN_WorkShopMatching
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_BN_WorkShopMatchingService();
            var result = service.Edit(null, listWrapper, data);
        }

        public dynamic PostIsExistWorkShopMatchingData(dynamic data)
        {
            string ParentCode = data.ParentCode;
            int IsExist = new MES_BN_WorkShopMatchingService().GetModelList(ParamQuery.Instance().AndWhere("RootPartCode", ParentCode)).Count;
            if (IsExist > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public dynamic PostCreateWorkShopMatchingData(dynamic data)
        {
            string ParentCode = data.ParentCode;
            string ContractCode = data.ContractCode;
            int ProjectDetailID = data.ProjectDetailID;
            string OperationType = data.OperationType;

            var list = TreeNodeManage.GetTreeNodeList<PRS_Process_BOM>(
                 TreeNodeManage.Instance()
                 .SetNode("PartCode")
                 .SetParentNode("ParentCode", ParentCode)
                 .SetTableName("PRS_Process_BOM")
                 .SetWhereSql($"where IsSelfMade=1 and ContractCode='{ContractCode}' order by ID"));

            var result = new MES_BN_WorkShopMatchingService().PostCreateWorkShopMatchingData(list, ParentCode, OperationType);
            return result;
        }

        public void PostChangeWorkShopCode(dynamic data)
        {
            string PartCode = data.PartCode;
            string Blanking = data.Blanking;
            string Welding = data.Welding;
            string Machining = data.Machining;
            string Assembling = data.Assembling;
            string RootPartCode = data.RootPartCode;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", Blanking);
            dic.Add("2", Welding);
            dic.Add("3", Machining);
            dic.Add("4", Assembling);

            var MES_BN_WorkShopMatchingList = new MES_BN_WorkShopMatchingService().GetModelList(ParamQuery.Instance().AndWhere("PartCode", PartCode).AndWhere("RootPartCode", RootPartCode).AndWhere("IsEnable", 1));

            foreach (KeyValuePair<string, string> item in dic)
            {
                string DepartmentCode = item.Value;
                //if (!string.IsNullOrWhiteSpace(DepartmentCode))
                //{
                string ProcessType = item.Key;
                if (MES_BN_WorkShopMatchingList.Where(a => a.ProcessType == ProcessType).Count() > 0)
                {
                    new MES_BN_WorkShopMatchingService().Update(
                        ParamUpdate.Instance()
                        .Update("MES_BN_WorkShopMatching")
                        .Column("WorkShopCode", DepartmentCode)
                        .AndWhere("PartCode", PartCode)
                        .AndWhere("ProcessType", ProcessType)
                        .AndWhere("RootPartCode", RootPartCode)
                        .AndWhere("IsEnable",1));
                }
                else
                {
                    new MES_BN_WorkShopMatchingService().Insert(
                       ParamInsert.Instance()
                       .Insert("MES_BN_WorkShopMatching")
                       .Column("WorkShopCode", DepartmentCode)
                       .Column("PartCode", PartCode)
                       .Column("ProcessType", ProcessType)
                       .Column("RootPartCode", RootPartCode)
                       .Column("IsEnable", 1));
                }
                //}
                //else
                //{
                //}
            }


        }

        public dynamic PostIsExistBlankingProcessBomData(dynamic data)
        {
            string ParentCode = data.ParentCode;
            string ContractCode = data.ContractCode;
            int ProductID = data.ProductID;
            int IsExist = new PRS_Process_BOM_BlankingService().GetModelList(
                ParamQuery.Instance()
                .AndWhere("ParentCode", ParentCode)
                .AndWhere("ContractCode", ContractCode)
                .AndWhere("ProductID", ProductID)).Count;
            return IsExist > 0 ? true : false;
        }

        public dynamic PostCreateBlankingProcessBomData(dynamic data)
        {
            string ParentCode = data.ParentCode;
            string ContractCode = data.ContractCode;
            int ProductID = data.ProductID;
            string OperationType = data.OperationType;

            //int IsExist = new PRS_Process_BOM_BlankingService().GetModelList(
            //    ParamQuery.Instance()
            //    .AndWhere("ParentCode", ParentCode)
            //    .AndWhere("ContractCode", ContractCode)
            //    .AndWhere("ProductID", ProductID)).Count;

            //if (IsExist > 0)
            //{
            //    return true;
            //}

            var PRS_Process_BOMList = TreeNodeManage.GetTreeNodeList<PRS_Process_BOM>(
                 TreeNodeManage.Instance()
                 .SetNode("PartCode")
                 .SetParentNode("ParentCode", ParentCode)
                 .SetTableName("PRS_Process_BOM")
                 .SetWhereSql("where IsEnable=1"));

            var PRS_Process_BOM_BlankingList = TreeNodeManage.GetTreeNodeList<PRS_Process_BOM_Blanking>(
                TreeNodeManage.Instance()
                .SetNode("PartCode")
                .SetParentNode("ParentCode", ParentCode)
                .SetTableName("PRS_Process_BOM_Blanking")
                .SetWhereSql("where IsEnable=1"));

            var result = new MES_BN_WorkShopMatchingService().PostCreateBlankingBomData(PRS_Process_BOMList, PRS_Process_BOM_BlankingList, ParentCode, ContractCode, ProductID, OperationType);
            return result;
        }
    }
}

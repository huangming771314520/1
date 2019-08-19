
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_WorkTicketMateController : Controller
    {
        public static string WorkTicketCode { get; set; }
        public ActionResult Index(string id)
        {
            WorkTicketCode = id;

            //查询工票表信息
            var WorkTicketCodeModel = new MES_WorkingTicketService().GetModelList(ParamQuery.Instance().AndWhere("WorkTicketCode", WorkTicketCode)).FirstOrDefault();
            var WorkshopCode = WorkTicketCodeModel.WorkshopCode;//工票信息车间编码
            var WorkshopName = WorkTicketCodeModel.WorkshopName;//工票信息车间名称
            var PartCode = WorkTicketCodeModel.PartCode;//工票信息零件编码
            //var WorkQuantity = WorkTicketCodeModel.WorkQuantity;//工票信息派工数量
            var ApsCode = WorkTicketCodeModel.ApsCode;//计划编码
            var ApproveState = WorkTicketCodeModel.ApproveState ?? 1;

            //查询计划中产品数量
            var PlanNumber = new APS_ProjectProduceDetialService().GetField<int>(
                ParamQuery.Instance()
                .Select("Quantity/BomQty")
                .From("APS_ProjectProduceDetial")
                .AndWhere("ApsCode", ApsCode));

            var MES_WorkshopBatchingList = new MES_WorkshopBatchingService().GetModelList(ParamQuery.Instance().AndWhere("WorkshopCode", WorkshopCode).AndWhere("PartCode", PartCode).AndWhere("IsEnable", 1));
            var MES_WorkshopBatchingDetailList = new List<MES_WorkshopBatchingDetail>();

            var MES_WorkTicketMateList = new List<dynamic>();
            MES_WorkTicketMateList = new MES_WorkTicketMateService().GetDynamicList(ParamQuery.Instance().AndWhere("WorkTicketCode", WorkTicketCode));

            if (MES_WorkTicketMateList.Count == 0)
            {
                var PRS_Process_BOMList =
                    new PRS_Process_BOMService()
                    .GetModelList();

                if (MES_WorkshopBatchingList.Count > 0)
                {
                    string BatchingCodeStr = "'" + string.Join("','", MES_WorkshopBatchingList.Select(a => a.BatchingCode)) + "'";
                    MES_WorkshopBatchingDetailList = new MES_WorkshopBatchingDetailService().GetModelList(ParamQuery.Instance().AndWhere("BatchingCode", BatchingCodeStr, Cp.In));
                }
                if (MES_WorkshopBatchingDetailList.Count > 0)
                {
                    MES_WorkTicketMateList = MES_WorkshopBatchingDetailList
                            .Join(PRS_Process_BOMList, a =>
                            new { PartCode = a.PartCode, ParentCode = a.ParentCode }, b => new { PartCode = b.PartCode, ParentCode = b.ParentCode }, (a, b) => new { a, b })
                            //.Join(PRS_Process_BOMList, c => new { ParentCode = c.b.ParentCode }, d => new { ParentCode = d.PartCode }, (c, d) => { c,d })
                            .Select(r => new
                            {
                                InventoryCode = r.b.InventoryCode,
                                PartCode = r.b.PartCode,
                                ParentCode = r.b.ParentCode,
                                InventoryName = r.a.PartName,
                                RequiredQuantity = r.a.BatchingNum,//需求数量
                                TotalQuantity = r.b.PartQuantity * PlanNumber,//需求总数
                                WorkTicketCode = WorkTicketCode,
                                WorkshopCode = WorkshopCode,
                                WorkshopName = WorkshopName,
                                IsEnable = 1,
                                IsCrux = r.b.IsCrux
                            }).ToList<dynamic>();
                }
            }

            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {
                    MES_WorkTicketMateList = MES_WorkTicketMateList,
                    ApproveState = ApproveState
                },
                urls = new
                {
                    query = "/api/Mms/MES_WorkTicketMate",
                    newkey = "/api/Mms/MES_WorkTicketMate/getnewkey",
                    edit = "/api/Mms/MES_WorkTicketMate/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WorkTicketCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "WorkTicketCode", "InventoryCode", "PartCode", "ParentCode", "InventoryName", "RequiredQuantity", "TotalQuantity", "WorkshopCode", "WorkshopName", "CreatePerson", "CreateTime", "IsEnable", "IsCrux" }
                }
            };
            return View(model);
        }
    }

    public class MES_WorkTicketMateApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>MES_WorkTicketMate</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='WorkTicketCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_WorkTicketMateService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("WorkTicketCode", MES_WorkTicketMateController.WorkTicketCode));
            return result;
        }

        public string GetNewKey()
        {
            return new MES_WorkTicketMateService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            MES_WorkTicketMate
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new MES_WorkTicketMateService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class MES_WorkingTicketReportController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_WorkingTicketReport",
                    remove = "/api/Mms/MES_WorkingTicketReport/",
                    billno = "/api/Mms/MES_WorkingTicketReport/getnewbillno",
                    audit = "/api/Mms/MES_WorkingTicketReport/audit/",
                    edit1 = "/Mms/MES_WorkingTicketReport/Edit"
                },
                resx = new
                {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    ProductID = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_ProjectDetail", "ID value,ProductName text"),
                    RootPartCode = new sys_codeService().GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PRS_Process_BOM", "PartCode value,PartFigureCode text")
                },
                form = new
                {
                    ContractCode = "",
                    ProductID = "",
                    RootPartCode = ""
                },
                idField = "ID"
            };

            return View(model);
        }
        [MvcMenuFilter(false)]
        public ActionResult Edit()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_WorkingTicketReportDetail",
                    remove = "/api/Mms/MES_WorkingTicketReportDetail/",
                    billno = "/api/Mms/MES_WorkingTicketReportDetail/getnewbillno",
                    audit = "/api/Mms/MES_WorkingTicketReportDetail/audit/",
                    edit = "/Mms/MES_WorkingTicketReportDetail/edit/"
                },
                resx = new
                {
                    detailTitle = "单据明细",
                    noneSelect = "请先选择一条单据！",
                    deleteConfirm = "确定要删除选中的单据吗？",
                    deleteSuccess = "删除成功！",
                    auditSuccess = "单据已审核！"
                },
                dataSource = new
                {
                    //dsPurpose = new sys_codeService().GetValueTextListByType("Purpose")
                },
                form = new
                {
                    ContractCode = "",
                    RootPartCode = "",
                    WorkshopCode = ""
                },
                idField = "BatchingCode"
            };

            return View(model);
        }
    }

    public class MES_WorkingTicketReportApiController : ApiController
    {
        public string GetNewBillNo()
        {
            return new PRS_Process_BOMService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            string ContractCode = query["ContractCode"];
            string ProductID = query["ProductID"];
            string RootPartCode = query["RootPartCode"];
            var list = new List<dynamic>();
            string sql = string.Format(@";WITH cte AS 
   (SELECT * FROM PRS_Process_BOM
    WHERE ISNULL(ParentCode, '') = '{2}'
    UNION ALL
    SELECT temp.* FROM cte INNER JOIN PRS_Process_BOM temp ON cte.PartCode = temp.ParentCode
    WHERE temp.ContractCode = '{0}' AND temp.ProductID = '{1}')
    SELECT
    a.ContractCode,
    a.ProductID,
    b.RootPartCode,
    b.PartCode,
    f.ProjectName,
	(SELECT TOP 1 PartFigureCode FROM dbo.PRS_Process_BOM WHERE PartCode = b.RootPartCode) RootFigureCode,
	(SELECT TOP 1 PartName FROM dbo.PRS_Process_BOM WHERE PartCode = b.RootPartCode) RootPartName,
    a.PartFigureCode,
    a.PartName,
    a.InventoryCode,
    a.MaterialCode,
    a.PartQuantity,
    e.Quantity,
    (e.Quantity * a.PartQuantity) AS RequirementNum,
    isnull(b.PickingQuantity,0) PickingQuantity
    FROM(SELECT * FROM cte UNION(SELECT * FROM dbo.PRS_Process_BOM WHERE PartCode = '{2}')) a
    LEFT JOIN
    (
        SELECT g.ContractCode,
               g.ProjectDetailID ProductID,
               g.RootPartCode,
               h.PartCode,
               SUM(c.RequiredQuantity) PickingQuantity
        FROM MES_WorkTicketMate c
        JOIN MES_WorkingTicket d ON c.WorkTicketCode = d.WorkTicketCode AND d.IsEnable=1
        JOIN dbo.APS_ProjectProduceDetial g ON g.ApsCode=d.ApsCode AND g.ProcessModelType IN('2','3','4') AND g.ContractCode='{0}' AND g.ProjectDetailID='{1}' AND g.RootPartCode='{2}'
		JOIN (SELECT DISTINCT PartCode,InventoryCode FROM dbo.PRS_Process_BOM) h ON h.InventoryCode=c.InventoryCode
        GROUP BY 
		g.ContractCode,
        g.ProjectDetailID,
        g.RootPartCode,
        h.PartCode
    ) AS b
    ON a.PartCode = b.PartCode
    LEFT JOIN PMS_BN_Projectdetail e
    ON a.ProductID = e.ID
    LEFT JOIN dbo.PMS_BN_Project f
    ON f.ContractCode = a.ContractCode
	ORDER BY b.RootPartCode DESC", ContractCode, ProductID, RootPartCode);
            using (var db = Db.Context("Mms"))
            {
                list = db.Sql(sql).QueryMany<dynamic>();
                if (ContractCode == "" && ProductID == "" && RootPartCode == "")
                {
                    list.Clear();
                }
            }
            return list;
        }
    }

    public class MES_WorkingTicketReportDetailApiController : ApiController
    {
        public string GetNewBillNo()
        {
            return new MES_WorkingTicketService().GetNewKey("WorkTicketCode", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='temp.RootPartCode'>
    <select>*</select>
    <from>(SELECT d.ContractCode,d.ProjectName,c.ProjectDetailID ProductID,c.RootPartCode,a.IsEnable,
(SELECT TOP 1 PartFigureCode FROM dbo.PRS_Process_BOM WHERE PartCode = c.RootPartCode) RootFigureCode,
(SELECT TOP 1 PartName FROM dbo.PRS_Process_BOM WHERE PartCode = c.RootPartCode) RootPartName,
(SELECT TOP 1 PartFigureCode FROM dbo.PRS_Process_BOM WHERE InventoryCode = b.InventoryCode) PartFigureCode,b.InventoryCode,
(SELECT TOP 1 PartCode FROM dbo.PRS_Process_BOM WHERE InventoryCode = b.InventoryCode) PartCode,
(SELECT TOP 1 PartName FROM dbo.PRS_Process_BOM WHERE InventoryCode = b.InventoryCode) PartName,
(SELECT TOP 1 MaterialCode FROM dbo.PRS_Process_BOM WHERE InventoryCode = b.InventoryCode) MaterialCode,
a.WorkshopCode,(SELECT TOP 1 DepartmentName FROM dbo.SYS_BN_Department WHERE DepartmentCode=a.WorkshopCode) WorkshopName,
a.ProcessName,a.WorkStepsName,ISNULL(b.RequiredQuantity,0) RequiredQuantity
FROM MES_WorkingTicket a LEFT JOIN MES_WorkTicketMate b ON a.WorkTicketCode=b.WorkTicketCode LEFT JOIN dbo.APS_ProjectProduceDetial c ON c.ApsCode=a.ApsCode
LEFT JOIN PMS_BN_Project d ON d.ContractCode=c.ContractCode
WHERE a.IsEnable=1) as temp</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='temp.ContractCode'		cp='equal'></field>   
        <field name='temp.ProductID'		cp='equal'></field>   
        <field name='temp.InventoryCode'		cp='equal'></field>   
        <field name='temp.RootPartCode'		cp='equal'></field>
    </where>
</settings>");
            var service = new MES_WorkingTicketService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicList(pQuery.AndWhere("RequiredQuantity", 0, Cp.NotEqual));
            return result;
        }
    }
}

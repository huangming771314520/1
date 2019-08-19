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
    public class MES_WorkshopBatchReportController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_WorkshopBatchReport",
                    remove = "/api/Mms/MES_WorkshopBatchReport/",
                    billno = "/api/Mms/MES_WorkshopBatchReport/getnewbillno",
                    audit = "/api/Mms/MES_WorkshopBatchReport/audit/",
                    edit1 = "/Mms/MES_WorkshopBatchReport/Edit"
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
                    query = "/api/Mms/MES_WorkshopBatchReportDetail",
                    remove = "/api/Mms/MES_WorkshopBatchReportDetail/",
                    billno = "/api/Mms/MES_WorkshopBatchReportDetail/getnewbillno",
                    audit = "/api/Mms/MES_WorkshopBatchReportDetail/audit/",
                    edit = "/Mms/MES_WorkshopBatchReportDetail/edit/"
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

    public class MES_WorkshopBatchReportApiController : ApiController
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
    a.MaterialCode,
    a.PartQuantity,
    e.Quantity,
    (e.Quantity * a.PartQuantity) AS RequirementNum,
    isnull(b.BatchingNum,0) BatchingNum
    FROM(SELECT * FROM cte UNION(SELECT * FROM dbo.PRS_Process_BOM WHERE PartCode = '{2}')) a
    LEFT JOIN
    (
        SELECT d.ContractCode,
               d.ProductID,
               d.RootPartCode,
               c.PartCode,
               SUM(c.BatchingNum) BatchingNum
        FROM[dbo].[MES_WorkshopBatchingDetail] c
            JOIN[dbo].[MES_WorkshopBatching] d
                ON c.BatchingCode = d.BatchingCode
                   AND d.ContractCode = '{0}'
                   AND d.ProductID = '{1}'
            AND d.RootPartCode = '{2}' and d.IsEnable=1
        GROUP BY d.ContractCode,
                 d.ProductID,
                 d.RootPartCode,
                 c.PartCode
    ) AS b
    ON a.PartCode = b.PartCode
    LEFT JOIN PMS_BN_Projectdetail e
    ON a.ProductID = e.ID
    LEFT JOIN dbo.PMS_BN_Project f
    ON f.ContractCode = a.ContractCode", ContractCode, ProductID, RootPartCode);
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

    public class MES_WorkshopBatchReportDetailApiController : ApiController
    {
        public string GetNewBillNo()
        {
            return new MES_WorkshopBatchingService().GetNewKey("BatchingCode", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            //string ContractCode = query["ContractCode"];
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='a.BatchingCode'>
    <select>a.ContractCode,c.ProjectName,a.RootPartCode,a.IsEnable,
(SELECT TOP 1 PartFigureCode FROM dbo.PRS_Process_BOM WHERE PartCode = a.RootPartCode) RootFigureCode,
(SELECT TOP 1 PartName FROM dbo.PRS_Process_BOM WHERE PartCode = a.RootPartCode) RootPartName,
(SELECT TOP 1 PartFigureCode FROM dbo.PRS_Process_BOM WHERE PartCode = b.PartCode) PartFigureCode,b.PartCode,
(SELECT TOP 1 PartName FROM dbo.PRS_Process_BOM WHERE PartCode = b.PartCode) PartName,
(SELECT TOP 1 MaterialCode FROM dbo.PRS_Process_BOM WHERE PartCode = b.PartCode) MaterialCode,
a.WorkshopCode,(SELECT TOP 1 DepartmentName FROM dbo.SYS_BN_Department WHERE DepartmentCode=a.WorkshopCode) WorkshopName,b.BatchingNum,
b.OutDeptCode,(SELECT WarehouseName FROM SYS_BN_Warehouse WHERE WarehouseCode=b.OutDeptCode) OutDeptName</select>
    <from>MES_WorkshopBatching a LEFT JOIN MES_WorkshopBatchingDetail b ON a.BatchingCode=b.BatchingCode LEFT JOIN PMS_BN_Project c ON c.ContractCode=a.ContractCode</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='a.ContractCode'		cp='equal'></field>   
        <field name='a.ProductID'		cp='equal'></field>   
        <field name='b.PartCode'		cp='equal'></field>   
        <field name='a.RootPartCode'		cp='equal'></field>
    </where>
</settings>");
            var service = new MES_WorkshopBatchingService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicList(pQuery.AndWhere("a.IsEnable", 1));
            return result;
        }
    }
}

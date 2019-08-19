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
    public class MES_SavantManageReportController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_SavantManageReport",
                    remove = "/api/Mms/MES_SavantManageReport/",
                    billno = "/api/Mms/MES_SavantManageReport/getnewbillno",
                    audit = "/api/Mms/MES_SavantManageReport/audit/",
                    edit1 = "/Mms/MES_SavantManageReport/Edit"
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

        public ActionResult Index_Record()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_SavantManageRecordReport",
                    remove = "/api/Mms/MES_SavantManageRecordReport/",
                    billno = "/api/Mms/MES_SavantManageRecordReport/getnewbillno",
                    audit = "/api/Mms/MES_SavantManageRecordReport/audit/",
                    edit1 = "/Mms/MES_SavantManageRecordReport/Edit"
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
    }

    public class MES_SavantManageReportApiController : ApiController
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
            string sql = string.Format(@";WITH cte AS(
SELECT * FROM dbo.PRS_Process_BOM_Blanking WHERE ISNULL(ParentCode,'')='{2}'
UNION ALL
SELECT temp.* FROM dbo.PRS_Process_BOM_Blanking temp INNER JOIN cte ON cte.PartCode=temp.ParentCode WHERE temp.ContractCode='{0}' AND temp.ProductID='{1}'
)
SELECT a.ContractCode,d.ProjectName,e.ProductName,a.PartFigureCode,a.PartName,b.BlankingNum,b.SavantCode,c.SpareMateSize,c.SpareMateNum,c.SplitNum,a.InPlanceSize
FROM (SELECT * FROM cte UNION ALL SELECT * FROM dbo.PRS_Process_BOM_Blanking WHERE PartCode='{2}') a
LEFT JOIN (SELECT * FROM [MES_SavantAndPBom]) b ON b.ProcessBomID=a.ID
LEFT JOIN [MES_SavantManage] c ON c.SavantCode=b.SavantCode
LEFT JOIN PMS_BN_Project d ON d.ContractCode=a.ContractCode
LEFT JOIN PMS_BN_ProjectDetail e ON e.ID=a.ProductID
WHERE a.MateType IN (1,2,3)", ContractCode, ProductID, RootPartCode);
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

    public class MES_SavantManageRecordReportApiController : ApiController
    {
        public string GetNewBillNo()
        {
            return new PRS_Process_BOMService().GetNewKey("ID", "dateplus");
        }

        public dynamic Get(RequestWrapper query)
        {
            string ContractCode = query["ContractCode"];
            string ProductID = query["ProductID"];
            var list = new List<dynamic>();
            string sql = string.Format(@"SELECT 
d.ContractCode,e.ProjectName,d.ProductID,f.ProductName,d.PartFigureCode,
d.PartName,a.SpareMateNum,a.SavantCode,c.BoardCode,c.BiankingSize,
g.BoardInventoryCode,g.BoardInventoryName 
FROM dbo.MES_SavantManage a 
LEFT JOIN PRS_BlankingResult b ON a.SavantCode=b.SavantCode
LEFT JOIN mes_BlankingResult c ON b.ID=c.BlankingResultID
LEFT JOIN dbo.PRS_Process_BOM_Blanking d ON d.ID=a.ProcessBomID
LEFT JOIN dbo.PMS_BN_Project e ON e.ContractCode=d.ContractCode
LEFT JOIN dbo.PMS_BN_ProjectDetail f ON f.ID=d.ProductID
LEFT JOIN Mes_BlankingBoard g ON g.ID=c.MainID where d.ContractCode='{0}' and d.ProductID='{1}'", ContractCode, ProductID);
            using (var db = Db.Context("Mms"))
            {
                list = db.Sql(sql).QueryMany<dynamic>();
                if (ContractCode == "" && ProductID == "")
                {
                    list.Clear();
                }
            }
            return list;
        }
    }
}

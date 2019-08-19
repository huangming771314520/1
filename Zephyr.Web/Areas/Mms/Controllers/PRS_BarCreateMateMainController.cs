
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
    public class PRS_BarCreateMateMainController : Controller
    {
        public ActionResult Index()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/PRS_BarCreateMateMain",
                    newkey = "/api/Mms/PRS_BarCreateMateMain/getnewkey",
                    edit = "/api/Mms/PRS_BarCreateMateMain/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ProductID = "",
                    ContractCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "InventoryCode", "InventoryName", "SetMateName", "SetMateNum", "InPlanceSize", "BlankingSize", "MateParamValue" }
                }
            };
            return View(model);
        }
    }

    public class PRS_BarCreateMateMainApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            #region 原SQL
            /*
             SELECT a.ID,
       a.InventoryCode,
       a.InventoryName PartName,
       a.New_InventoryCode SetMateName,
       a.PartFigureCode FigureCode,
       b.InventoryName,
       b.Model,
       b.QuantityUnit,
       a.SetMateNum,
       a.InPlanceSize,
       a.BlankingSize,
       a.MateParamValue,
       MateType
FROM PRS_Process_BOM a
    LEFT JOIN SYS_Part b
        ON b.InventoryCode = a.New_InventoryCode
WHERE 1=1
             */
            #endregion

            query.LoadSettingXmlString(@"
    <settings defaultOrderBy=' New_InventoryCode,PartFigureCode'>
        <select>*</select>
        <from>(SELECT a.ID,
       a.ContractCode,   --合同编号
       c.ProjectName,    --工程项目
	   d.ID AS ProductID,
       d.ProductName,    --产品名称
       a.PartFigureCode, --零件图号
       a.PartName,  --零件名称
       a.MaterialCode,   --材质
       b.InventoryCode,
       b.InventoryName,  --材料名称
       b.Model,          --型号规格
       a.SetMateNum,     --定料数量
       a.InPlanceSize,   --到位尺寸
       a.BlankingSize,   --下料尺寸
       a.MateParamValue, --直径
       MateType,          --材料类型
       a.New_InventoryCode
FROM dbo.PRS_Process_BOM_Blanking a
    INNER JOIN dbo.PMS_BN_Project c
        ON a.ContractCode = c.ContractCode
    INNER JOIN dbo.PMS_BN_ProjectDetail d
        ON c.ProjectID = d.MainID
           AND a.ProductID = d.ID
    LEFT JOIN SYS_Part b
        ON b.InventoryCode = a.New_InventoryCode
WHERE a.MateType IN (2,3)
) as temp</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='ProductID' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_Process_BOMService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicList(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_Process_BOMService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public dynamic GetBomList(string BomList)
        {
            return new PRS_BarCreateMateService().GetBomList(BomList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bomList"></param>
        /// <returns></returns>
        public bool GetIsSum(string bomList)
        {
            bool IsCheck = new PRS_BarCreateMateService().GetIsSum2(bomList).Select(a => Convert.ToString(a.New_InventoryCode)).Distinct().Count() > 1 ? false : true;
            return IsCheck;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_Process_BOM
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_Process_BOMService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}

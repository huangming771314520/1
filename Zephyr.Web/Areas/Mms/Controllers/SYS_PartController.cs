
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
    public class SYS_PartController : Controller
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
                    query = "/api/Mms/SYS_Part",
                    newkey = "/api/Mms/SYS_Part/getnewkey",
                    edit = "/api/Mms/SYS_Part/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PartCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "InventoryCode", "InventoryName", "MinStock", "Weight", "MaxStock", "PartCode", "PartName", "PartTypeID", "Spec", "Model", "SafetyStock", "PurchaseAdvanceTime", "EconomicBatchQuantity", "MinPackQuantity", "EnconanmicOrderQuantity", "QuantityUnit", "FigureCode", "QualityCode", "CorrespondingBarcode", "IsSelfMade", "IsSupplyMade", "IsCastforgeMatch", "IsOutsouceWeiding", "IsElectroHydraulicMatch", "IsMarketMatch", "WarehouseCode", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
    }

    public class SYS_PartApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>t1.*,t2.TypeName</select>
        <from>SYS_Part as t1 left join SYS_PartType as t2 on t1.PartTypeID = t2.PartTypeCode</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='PartCode' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_PartService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new SYS_PartService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_Part
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_PartService();
            var result = service.Edit(null, listWrapper, data);
        }


        public List<dynamic> GetSysPartByCodeAndType(string partCode, string partType)
        {
            return new SYS_PartService().GetSysPartByCodeAndType(partCode, partType);
        }

        public dynamic GetUpdatePartBarCodeByPartCode(string partCode)
        {
            return new SYS_PartService().GetUpdatePartBarCodeByPartCode(partCode);
        }

        public dynamic PostSimBomByPName()
        {
            string PartName = HttpContext.Current.Request["PartName"].ToString();
            string InventoryCode = HttpContext.Current.Request["InventoryCode"].ToString();
            string InventoryName = HttpContext.Current.Request["InventoryName"].ToString();
            string Spec = HttpContext.Current.Request["Spec"].ToString();
            string Model = HttpContext.Current.Request["Model"].ToString();
            int SearchType = Convert.ToInt32(HttpContext.Current.Request["SearchType"]);

            int page = Convert.ToInt32(HttpContext.Current.Request["page"]);
            int rows = Convert.ToInt32(HttpContext.Current.Request["rows"]);

            if (SearchType == 0)
            {
                return new SYS_PartService().GetSimBomByPName(PartName, page, rows);
            }
            else
            {
                return new SYS_PartService().GetPartListByKeyWord(InventoryCode, InventoryName, Spec, Model, page, rows);
            }

        }

        public dynamic GetQuantityUnitList()
        {
            return new SYS_PartService().GetQuantityUnitList();
        }

        public dynamic PostUpdatePart()
        {
            string PartID = HttpContext.Current.Request["PartID"].ToString();
            string Weight = HttpContext.Current.Request["Weight"].ToString();
            string PartName = HttpContext.Current.Request["PartName"].ToString();
            string PartCode = HttpContext.Current.Request["PartCode"].ToString();

            string BomID = HttpContext.Current.Request["BomID"].ToString();
            string PartICode = HttpContext.Current.Request["PartICode"].ToString();
            string PartIName = HttpContext.Current.Request["PartIName"].ToString();

            string Model = HttpContext.Current.Request["Model"].ToString();

            string wCode = HttpContext.Current.Request["wCode"].ToString();
            string wName = HttpContext.Current.Request["wName"].ToString();
            string qUnit = HttpContext.Current.Request["qUnit"].ToString();

            return new SYS_PartService().GetUpdatePart(PartID, Weight, PartName, PartCode, BomID, PartICode, PartIName, Model, wCode, wName, qUnit);
        }

        public dynamic GetSelfMotionUpdatePart(string PartFCode)
        {
            return new SYS_PartService().GetSelfMotionUpdatePart(PartFCode);
        }

        public dynamic GetSelfMotionUpdatePart2(string pIDs)
        {
            return new SYS_PartService().GetSelfMotionUpdatePart2(pIDs);
        }

    }
}

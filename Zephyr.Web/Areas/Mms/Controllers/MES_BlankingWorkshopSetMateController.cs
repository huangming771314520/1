
using Newtonsoft.Json.Linq;
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
    public class MES_BlankingWorkshopSetMateController : Controller
    {
        public static List<int> MateTypeListId = new List<int>();
        public ActionResult Index(int id)
        {
            var code = new sys_codeService();

            var MateTypeList = code.GetValueTextListByType_Xttcqw("MateType");

            if (id == 1)
            {
                MateTypeListId = new List<int>() { 1, 2, 3 };
                MateTypeList = MateTypeList.Where(a => MateTypeListId.Contains(Convert.ToInt32(a.value))).ToList();
            }
            else if (id == 2)
            {
                MateTypeListId = new List<int>() { 0, 4, 5 };
                MateTypeList = MateTypeList.Where(a => MateTypeListId.Contains(Convert.ToInt32(a.value))).ToList();
            }

            var model = new
            {
                dataSource = new
                {
                    ProductID = code.GetValueTextListByType_Xttcqw("HBHC_Mes.dbo.PMS_BN_ProjectDetail", "ID value,ProductName text"),
                    MateTypeList = MateTypeList
                },
                urls = new
                {
                    query = "/api/Mms/MES_BlankingWorkshopSetMate",
                    newkey = "/api/Mms/MES_BlankingWorkshopSetMate/getnewkey",
                    edit = "/api/Mms/MES_BlankingWorkshopSetMate/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    ContractCode = "",
                    ProductID = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "MaterialInventoryName", "MaterialInventoryCode", "ID", "PartFigureCode", "PartName", "MaterialCode", "MateType", "SetMateName", "New_InventoryCode", "New_PartName", "New_Model", "MateParamValue", "InPlanceSize", "BlankingSize", "BlankingNum", "SetMateNum" }
                }
            };

            return View(model);
        }
    }

    public class MES_BlankingWorkshopSetMateApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            string MateTypeListId = string.Join(",", MES_BlankingWorkshopSetMateController.MateTypeListId);

            /*
             --参考：https://bbs.csdn.net/topics/230028389

SELECT * FROM dbo.PRS_Process_BOM ORDER BY PartFigureCode

SELECT * FROM dbo.PRS_Process_BOM ORDER BY PartFigureCode COLLATE SQL_Latin1_General_CP437_BIN

SELECT * FROM dbo.PRS_Process_BOM ORDER BY replace (PartFigureCode,'-','0') 
             */

            query.LoadSettingXmlString(@"
    <settings defaultOrderBy=' temp.PartFigureCode COLLATE SQL_Latin1_General_CP437_BIN '>
        <select>*</select>
        <from>
(SELECT ID,PartFigureCode,PartName,MaterialCode,MateType,SetMateName,ContractCode,ProductID,
New_InventoryCode,New_PartName,New_Model,MateParamValue,InPlanceSize,BlankingSize,BlankingNum,SetMateNum
FROM dbo.PRS_Process_BOM_Blanking a WHERE a.MateType IN({0})) as temp
        </from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='temp.ContractCode' 		 cp='like'></field>
                <field name='temp.ProductID' 		 cp='equal'></field>
        </where>
    </settings>", MateTypeListId);
            var service = new PRS_Process_BOM_BlankingService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_Process_BOM_BlankingService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_Process_BOM_Blanking
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_Process_BOM_BlankingService();

            if (data.list.updated.ToString() != "[]")
            {
                foreach (JToken row in data["list"]["updated"].Children())
                {
                    string SetMateName = row["SetMateName"].ToString();

                    string New_PartName = row["New_PartName"].ToString();

                    string New_Model = row["New_Model"].ToString();


                    //处理同上

                    //row["New_PartName"] = row["MaterialInventoryName"];//MaterialInventoryName代替InventoryName
                    //row["New_Model"] = row["MaterialInventoryCode"];//MaterialInventoryCode代替Model
                    //string inventoryCode = row["New_InventoryCode"].ToString();
                    //var inventory = service.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", row["ID"].ToString()));
                    SYS_Part part = new SYS_PartService().GetModel(ParamQuery.Instance().Select("*").AndWhere("InventoryCode", SetMateName));
                    bool mEquals = New_Model.Equals(part.Model == null ? "" : part.Model);
                    bool nEquals = New_PartName.Equals(part.InventoryName == null ? "" : part.InventoryName);
                    bool res = mEquals && nEquals;
                    row["MaterialInventoryName"] = part.InventoryName;
                    row["MaterialInventoryCode"] = part.Model;

                    row["New_InventoryCode"] = "";

                    if (res)
                    {
                        row["New_InventoryCode"] = row["SetMateName"];
                    }
                    /*else
                    {
                        if (row["New_InventoryCode"].ToString().Equals(row["SetMateName"].ToString()))
                        {
                            row["New_InventoryCode"] = "";
                        }
                        else
                        {

                        }
                    }*/

                    ////处理同上
                    //row["New_InventoryCode"] = row["SetMateName"];
                    //row["New_PartName"] = row["MaterialInventoryName"];//MaterialInventoryName代替InventoryName
                    //row["New_Model"] = row["MaterialInventoryCode"];//MaterialInventoryCode代替Model
                    //string inventoryCode = row["New_InventoryCode"].ToString();
                    //var inventory = service.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", row["ID"].ToString()));
                    //var pQuery = ParamQuery.Instance().Select("*").AndWhere("InventoryCode", inventory.SetMateName);
                    //SYS_Part part = new SYS_PartService().GetModel(pQuery);
                    //bool mEquals = row["MaterialInventoryCode"].ToString().Equals(part.Model == null ? "" : part.Model);
                    //bool nEquals = row["MaterialInventoryName"].ToString().Equals(part.InventoryName == null ? "" : part.InventoryName);
                    //bool res = mEquals && nEquals;
                    //row["MaterialInventoryName"] = part.InventoryName;
                    //row["MaterialInventoryCode"] = part.Model;
                    //if (!res)
                    //{
                    //    row["New_InventoryCode"] = "";
                    //}
                }
            }

            var result = service.Edit(null, listWrapper, data);
        }
    }
}

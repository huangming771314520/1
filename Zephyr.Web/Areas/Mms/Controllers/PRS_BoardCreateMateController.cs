
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class PRS_BoardCreateMateController : Controller
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
                    query = "/api/Mms/PRS_BoardCreateMate",
                    newkey = "/api/Mms/PRS_BoardCreateMate/getnewkey",
                    edit = "/api/Mms/PRS_BoardCreateMate/edit"
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
                    InventoryCode = "",
                    Model = "",
                    Specs = "",
                    InventoryName=""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "InventoryCode", "InventoryName", "Model", "Specs", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "InventoryNum", "Unit", "New_InventoryCode", "New_InventoryName", "New_Model", "New_Specs", "Remark" }
                }
            };

            return View(model);
        }
    }

    public class PRS_BoardCreateMateApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>(SELECT a.ID ID,b.InventoryCode,a.ContractCode,a.IsEnable, New_InventoryName InventoryName,a.New_Model Model, a.New_Specs Specs,b.QuantityUnit , a.InventoryNum,a.Unit,a.Remark FROM  PRS_BoardCreateMate a left join SYS_Part b on a.InventoryCode=b.InventoryCode) AS t1</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='equal'></field>
                <field name='InventoryName' 		 cp='like'></field>
                <field name='Model' 		 cp='equal'></field>
                <field name='Specs' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_BoardCreateMateService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_BoardCreateMateService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_BoardCreateMate
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_BoardCreateMateService();
            //var result = service.Edit(null, listWrapper, data);
            if (data.list.inserted.ToString() != "[]")
            {
                foreach (JToken row in data["list"]["inserted"].Children())
                {
                    //首先页面中显示用户只能维护InventoryCode，InventoryName，Model，但是不能确定用户是否在选中完现有的材料信息后有没有进行修改型号规格。
                    //所以先默认存储的新存货编码，存货名称，型号规格数据都为页面传过来的数据
                    row["New_InventoryCode"] = row["InventoryCode"];
                    row["New_InventoryName"] = row["InventoryName"];
                    row["New_Model"] = row["Model"];
                    row["IsEnable"] = 1;
                    //row["New_Specs"] = row["Specs"];
                    //获取用户选中的存货编码，并以此为条件查询出原本次存货编码下物料的型号规格数据
                    string inventoryCode = row["New_InventoryCode"].ToString();
                    var pQuery = ParamQuery.Instance().Select("*").AndWhere("InventoryCode", row["InventoryCode"].ToString());
                    SYS_Part part = new SYS_PartService().GetModel(pQuery);
                    //判断数据库中原有的型号规格与页面传入的型号规格是否形同
                    bool mEquals = row["Model"].ToString().Equals(part.Model == null ? "" : part.Model);
                    //bool sEquals = row["Specs"].ToString().Equals(part.Spec==null?"": part.Spec);
                    //判断数据库中原有的存货名称与页面传入的存货名称是否形同
                    bool nEquals = row["InventoryName"].ToString().Equals(part.InventoryName == null ? "" : part.InventoryName);
                    bool res = mEquals && nEquals;
                    //同时对页面中的InventoryName进行赋值，保存选中物料编码下物料原有的存货名称
                    row["InventoryName"] = part.InventoryName;
                    //row["ID"] = "";
                    //同时对页面中的Model进行赋值，保存选中物料编码下物料原有的型号规格
                    row["Model"] = part.Model;
                    row["New_InventoryCode"] = part.InventoryCode;
                    //row["Specs"] = part.Spec;
                    //当页面传入的存货名称和型号规格有任何数据与数据库中的存货名称和型号规格不同时，即证明此物料的存货编码应该新增，所以清除掉新存货编码数据
                    if (!res)
                    {
                        row["New_InventoryCode"] = "";
                    }
                }
            }

            if (data.list.updated.ToString() != "[]")
            {
                foreach (JToken row in data["list"]["updated"].Children())
                {
                    //处理同上
                    row["New_InventoryCode"] = row["InventoryCode"];
                    row["New_InventoryName"] = row["InventoryName"];
                    row["New_Model"] = row["Model"];
                    row["IsEnable"] = 1;
                    //row["New_Specs"] = row["Specs"];
                    string inventoryCode = row["New_InventoryCode"].ToString();
                    var inventory = service.GetModel(ParamQuery.Instance().AndWhere("IsEnable", 1).AndWhere("ID", row["ID"].ToString()));
                   
                    var pQuery = ParamQuery.Instance().Select("*").AndWhere("InventoryCode", inventory.InventoryCode);
                    
                    SYS_Part part = new SYS_PartService().GetModel(pQuery);
                    bool mEquals = row["Model"].ToString().Equals(part.Model == null ? "" : part.Model);
                    //bool sEquals = row["Specs"].ToString().Equals(part.Spec == null ? "" : part.Spec);
                    bool nEquals = row["InventoryName"].ToString().Equals(part.InventoryName == null ? "" : part.InventoryName);
                    bool res = mEquals && nEquals;
                    row["InventoryName"] = part.InventoryName;
                    row["Model"] = part.Model;
                    //row["Specs"] = part.Spec;
                    if (!res)
                    {
                        row["New_InventoryCode"] = "";
                    }
                }
            }
            var result = service.Edit(null, listWrapper, data);
        }
    }
}

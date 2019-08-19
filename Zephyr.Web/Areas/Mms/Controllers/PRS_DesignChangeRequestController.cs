
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
    public class PRS_DesignChangeRequestController : Controller
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
                    query = "/api/Mms/PRS_DesignChangeRequest",
                    newkey = "/api/Mms/PRS_DesignChangeRequest/getnewkey",
                    edit = "/api/Mms/PRS_DesignChangeRequest/edit",
                    storageSave = "/api/Mms/PRS_DesignChangeRequest/PostStorage",
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
                    RequestCode = "",
                    FigureNumber = "",
                    RequestState = "0"
                },
                defaultRow = new
                {
                    IsEnable = 1,
                    RequestState = 0,
                    RequestCode = "系统生成"
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "RequestCode", "ContractCode", "FigureNumber", "Reason", "ChangeContent", "RequestState", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime", "ProductID", "TypeID" }
                }
            };

            return View(model);
        }
    }

    public class PRS_DesignChangeRequestApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>PRS_DesignChangeRequest</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='RequestCode' 		 cp='like'></field>
                <field name='FigureNumber' 		 cp='like'></field>
<field name='RequestState' 		 cp='like'></field>
        </where>
    </settings>");
            var service = new PRS_DesignChangeRequestService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery.AndWhere("IsEnable", 1));
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_DesignChangeRequestService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }
        public string GetBillCode()
        {
            string documentNo = MmsHelper.GetOrderNumber("PRS_DesignChangeRequest", "RequestCode", "GGSQ", "", "");
            return documentNo;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_DesignChangeRequest
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_DesignChangeRequestService();
            List<PMS_DesignTaskDetail> dlist = new List<PMS_DesignTaskDetail>();
            if (data.list.inserted.ToString() != "[]")
            {
                
                var dno = MmsHelper.GetOrderNumber("PRS_DesignChangeRequest", "RequestCode", "GGSQ", "", "");
                var fno = dno.Substring(0, 10);
                var con = dno.Substring(10, 3);

                foreach (JToken row in data["list"]["inserted"].Children())
                {
                    row["RequestCode"] = fno + con;
                    int intCon = Convert.ToInt32(con);
                    intCon++;
                    var zeros = 3 - intCon.ToString().Length;
                    con = "";
                    for (int i = 1; i <= zeros; i++)
                        con += "0";
                    con += intCon.ToString();

                    //var d = GetMainID(row["ContractCode"].ToString(), row["ProductID"].ToString());
                    //PMS_DesignTaskDetail dt = new PMS_DesignTaskDetail();
                    //dt.ID = -1;
                    //dt.MainID = d;
                    //dt.ContractCode = row["ContractCode"].ToString();
                    //dt.ProductID = row["ProductID"].ToString();
                    //dt.TaskDescription =row["ChangeContent"].ToString() ;
                    //dt.TaskType = Convert.ToInt32(row["TypeID"]);
                    //dt.IsEnable = 1;
                    //dt.BillState = 0;
                    //dt.BillCode = row["RequestCode"].ToString();
                    //dt.DesignType = 2;
                    //dlist.Add(dt);
                }
            }

            var result = service.Edit(null, listWrapper, data);
            //if (dlist.Count>0)
            //{
            //    new PMS_ContractInfoApiController().PostDesignDetailOnSave(dlist);
            //}

        }


        public int GetMainID(string contcode, string productID)
        {
            var pQuery = ParamQuery.Instance().Select("top 1 ID").AndWhere("ContractCode", contcode).AndWhere("ProductID", productID);
            return new PMS_ProductTaskService().GetModel(pQuery).ID;

        }
        public int GetDelete(string id)
        {
            return new PRS_DesignChangeRequestService().GetDelete(id);
        }
        public string PostStorage(string id)
        {
            string msg = "";

            var result = new PRS_DesignChangeRequestService().AuditBillCode(id, out msg);

            return msg;
        }

    }
}

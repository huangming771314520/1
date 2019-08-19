
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class PRS_BlankingPlateDetailController : Controller
    {
        public static int MainID = 0;
        public static string NewPName = string.Empty;

        public ActionResult Index(string newPName, int id = 0)
        {
            NewPName = newPName;
            MainID = id;
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/PRS_BlankingPlateDetail",
                    newkey = "/api/Mms/PRS_BlankingPlateDetail/getnewkey",
                    edit = "/api/Mms/PRS_BlankingPlateDetail/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlateSize = ""
                },
                defaultRow = new
                {
                    MainID = id,
                    IsEnable = 1
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "MainID", "PlateSize", "Weight", "LineLength", "BlankingType", "CreatePerson", "CreateTime", "IsEnable", "InventoryCode" }
                }
            };

            return View(model);
        }
    }

    public class PRS_BlankingPlateDetailApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ID'>
        <select>*</select>
        <from>PRS_BlankingPlateDetail</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='PlateSize' 		 cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_BlankingPlateDetailService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicList(pQuery.AndWhere("MainID", PRS_BlankingPlateDetailController.MainID));
            return result;
        }

        public string GetNewKey()
        {
            return new PRS_BlankingPlateDetailService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_BlankingPlateDetail
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");

            //data.list.inserted[0].MainID

            StringBuilder sb = new StringBuilder();
            long maxICode = new SYS_PartService().GetSysPartMaxICode("050203");
            string newPName = PRS_BlankingPlateDetailController.NewPName;
            DateTime newDT = DateTime.Now;

            for (var i = 0; i < data.list.inserted.Count; i++)
            {
                string iCode = "050203" + (++maxICode).ToString().PadLeft(12, '0');

                data.list.inserted[i].InventoryCode = iCode;

                sb.Append(WinFormClientService.GetInsertSQL(new SYS_Part()
                {
                    InventoryCode = iCode,
                    InventoryName = newPName,
                    Model = $"尺寸={data.list.inserted[i].PlateSize},重量={data.list.inserted[i].Weight},线长={data.list.inserted[i].LineLength}",
                    IsEnable = 1,
                    CreatePerson = MmsHelper.GetUserName(),
                    CreateTime = newDT,
                    ModifyPerson = MmsHelper.GetUserName(),
                    ModifyTime = newDT
                }));
            }

            for (var i = 0; i < data.list.updated.Count; i++)
            {
                sb.Append(WinFormClientService.GetUpdateSQL(nameof(SYS_Part), new KeyValuePair<string, object>("InventoryCode", data.list.updated[i].InventoryCode), new
                {
                    Model = $"尺寸={data.list.updated[i].PlateSize},重量={data.list.updated[i].Weight},线长={data.list.updated[i].LineLength}"
                }));
            }

            string sql = sb.ToString();

            using (var db = Db.Context("Mms"))
            {
                db.Sql(sql).Execute();
            }

            //dynamic inserted_list= data.list.inserted;
            //dynamic deleted_list = data.list.deleted;
            //dynamic updated_list = data.list.updated;
            //var PRS_BlankingPlateDetail_IDList = new List<int>();
            //foreach (dynamic item in inserted_list)
            //{
            //    int ID = item.ID;
            //    PRS_BlankingPlateDetail_IDList.Add(ID);
            //}
            //foreach (dynamic item in deleted_list)
            //{
            //    int ID = item.ID;
            //    PRS_BlankingPlateDetail_IDList.Add(ID);
            //}
            //foreach (dynamic item in updated_list)
            //{
            //    int ID = item.ID;
            //    PRS_BlankingPlateDetail_IDList.Add(ID);
            //}
            //bool flag = true;
            //new Mes_BlankingResultService().PostCreateBoard(out flag, PRS_BlankingPlateDetail_IDList);

            var service = new PRS_BlankingPlateDetailService();
            var result = service.Edit(null, listWrapper, data);
        }
    }
}

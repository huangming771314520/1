
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Areas.CommonWrap;
using Zephyr.Core;
using Zephyr.Models;

namespace Zephyr.Areas.Mms.Controllers
{
    public class SYS_BOMController : Controller
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
                    query = "/api/Mms/SYS_BOM",
                    newkey = "/api/Mms/SYS_BOM/getnewkey",
                    edit = "/api/Mms/SYS_BOM/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PartFigureCode = "",
                    PartCode = "",
                    ParentCode = ""
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "PartFigureCode", "PartCode", "PartName", "InventoryCode", "InventoryName", "PartQuantity", "ParentCode", "Weight", "Totalweight", "MaterialCode", "MaterialInventoryCode", "MaterialInventoryName", "MaterialQuantity", "VersionCode", "IsEnable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
    }

    public class SYS_BOMApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            string parentCode = query["PartCode"].ToString();
            return new PMS_BN_PartFileService().GetBom(parentCode);
        }

        public string GetNewKey()
        {
            return new SYS_BOMService().GetNewKey("ID", "maxplus").PadLeft(6, '0'); ;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            SYS_BOM
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new SYS_BOMService();
            var result = service.Edit(null, listWrapper, data);
        }
        public List<dynamic> GetBom(string pCode,string cCode,int pID)
        {
            var list = new SYS_BOMService().GetBom2(pCode,cCode,pID);

            list = (from p in list
                    group p by p.PartCode into g
                    select new
                    {
                        PartCode = g.Key,
                        PartFigureCode = g.Max(m => m.PartFigureCode),
                        IsSelfMade = g.Max(m => m.IsSelfMade),
                        PartName = g.Max(m => m.PartName),
                        TotalStock = g.Max(m => m.TotalStock),
                        PartQuantityAll = g.Sum(m => m.PartQuantityAll??0),
                        ParentCode = g.Max(m => m.ParentCode),
                        ParentName = g.Max(m => m.ParentName)
                    }).ToList<dynamic>();
            return list;

        }

        public void GetBom_Update(string pCode,string cCode,int pID)
        {
            //var ProcessBomList = TreeNodeManage.GetTreeNodeList<PRS_Process_BOM>(
            //      TreeNodeManage.Instance()
            //      .SetNode("PartCode")
            //      .SetParentNode("ParentCode", parentCode)
            //      .SetTableName("PRS_Process_BOM")
            //      //.SetNodeLevel(model.IsLevel)
            //      //.SetTreeSetting(model.TreeSetting)
            //      .SetWhereSql("where IsEnable=1")
            //      );

            var MES_BN_MatchingTableDetailList =
                  TreeNodeManage.GetTreeNodeList<MES_BN_MatchingTableDetail>(
                  TreeNodeManage.Instance()
                  .SetNode("PartCode")
                  .SetParentNode("PPartCode", pCode)
                  .SetTableName("MES_BN_MatchingTableDetail")
                  //.SetNodeLevel(model.IsLevel)
                  //.SetTreeSetting(model.TreeSetting)
                  .SetWhereSql("where IsEnable=1")
                  );
            new MES_BN_MatchingTableDetailService().Cover_MES_BN_MatchingTableDetail2(MES_BN_MatchingTableDetailList, pCode, cCode, pID);

        }
    }
}

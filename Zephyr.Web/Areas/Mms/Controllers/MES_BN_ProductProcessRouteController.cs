using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Zephyr.Core;
using Zephyr.Models;
using System.Linq;
using System.Web;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Utils;
using System.IO;
using Zephyr.Areas.CommonWrap;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class MES_BN_ProductProcessRouteController : Controller
    {
        public ActionResult Index()
        {
            var model = new
            {
                urls = new
                {
                    query = "/api/Mms/MES_BN_ProductProcessRoute",
                    remove = "/api/Mms/MES_BN_ProductProcessRoute/",
                    billno = "/api/Mms/MES_BN_ProductProcessRoute/getnewbillno",
                    audit = "/api/Mms/MES_BN_ProductProcessRoute/audit/",
                    edit = "/api/Mms/MES_BN_ProductProcessRoute/edit/",
                    newkey = "/api/Mms/MES_BN_ProductProcessRoute/GetNewRowId/",
                    edit_toplist = "/api/Mms/MES_BN_ProductProcessRoute/Edit_TopList/",
                    checkop = "/api/Mms/MES_BN_ProductProcessRoute/PostCheckOP/",
                },
                resx = new
                {
                    rejected = "已撤消修改！",
                    editSuccess = "保存成功！",
                    auditPassed = "单据已通过审核！",
                    auditReject = "单据已取消审核！"
                },
                dataSource = new
                {
                    pageData = new MES_BN_ProductProcessRouteApiController().GetPageData(),
                },
                form = new
                {
                    ContractCode = "",
                    PartCode = "",
                    defaults = new MES_BN_ProductProcessRoute().Extend(new { }),
                    primaryKeys = new string[] { "ID" }
                },
                tabs = new object[]{
                        new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","ProcessRouteID","ToolCode","ToolName","ToolNum","IsEnable","CreatePerson","CreateTime","ModifyPerson","ModifyTime","ProcessType"},
                          defaults = new {}
                        },
                          new{
                          type = "grid",
                          rowId = "ID",
                          relationId = "ID",
                          postFields = new string[] { "ID","ProcessRouteID","EquipmentCode","EquitmentName","EquitmentType","AffiliatedWorkshopID","EquiptmentParms","EquipmentState","IsEnable"},
                          defaults = new {}
                        }
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ContractCode", "ProjectName", "PartCode", "ProcessCode", "ProcessLineCode", "ProcessName", "ProcessDesc", "WorkshopID", "EquipmentID", "WorkGroupID", "WarehouseID", "WorkshopName", "EquipmentName", "WorkGroupName", "WarehouseName", "ManHour", "Unit", "FigureCode", "Remark", "IsEnable", "IsInspectionReport" }
                }
            };
            return View(model);
        }
        public ActionResult Index2()
        {
            return View();
        }

        /// <summary>
        /// 自制外购功能 需转至生产管理模块
        /// </summary>
        /// <returns></returns>
        public ActionResult IsSalefyMade()
        {
            return View();
        }

        public ActionResult SaveProcessModel()
        {
            return View();
        }
        public ActionResult ProcessHourCalc()
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/V_ProcessRouteWorkSteps",
                    newkey = "/api/Mms/V_ProcessRouteWorkSteps/getnewkey",
                    edit = "/api/Mms/V_ProcessRouteWorkSteps/edit"
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
                    ProjectName = "",
                    FigureCode = "",
                    ManHours = "null"
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "ManHours", "mID" }
                }
            };

            return View(model);
        }
        public FileStreamResult FileDownload(int id)
        {
            try
            {
                var doc = new QMS_QualityReportDocService().getDoc(id);
                return File(new FileStream(doc.FileAddress, FileMode.Open), "text/plain", doc.DocName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 工艺卡片编制
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessCardEdit()
        {
            return View();
        }

        /// <summary>
        /// 工艺路线编制
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRouteEdit(string cCode, string pName, string figureCode, string pCode, string type)
        {
            ViewData["cCode"] = cCode;
            ViewData["pName"] = pName;
            ViewData["figureCode"] = figureCode;
            ViewData["pCode"] = pCode;
            ViewData["type"] = type;

            string typeName = string.Empty;
            switch (type)
            {
                case "1": typeName = "下料"; break;
                case "2": typeName = "焊接"; break;
                case "3": typeName = "机加工"; break;
                case "4": typeName = "装配"; break;
                default: break;
            }

            ViewData["typeName"] = typeName;

            return View();
        }

        public ActionResult ProcessRouteEditTest(string cCode, string pName, string figureCode, string pCode, string type)
        {
            ViewData["cCode"] = cCode;
            ViewData["pName"] = pName;
            ViewData["figureCode"] = figureCode;
            ViewData["pCode"] = pCode;
            ViewData["type"] = type;

            string typeName = string.Empty;
            switch (type)
            {
                case "1": typeName = "下料"; break;
                case "2": typeName = "焊接"; break;
                case "3": typeName = "机加工"; break;
                case "4": typeName = "装配"; break;
                default: break;
            }

            ViewData["typeName"] = typeName;

            var MES_BN_WorkShopMatchingList = new MES_BN_WorkShopMatchingService().GetModelList
                 (
                 ParamQuery.Instance()
                 .AndWhere("PartCode", pCode)
                 .AndWhere("ProcessType", type)
                 .AndWhere("IsEnable", 1)
                 );
            string WorkshopID = string.Empty;
            if (MES_BN_WorkShopMatchingList.Count > 0)
            {
                WorkshopID = MES_BN_WorkShopMatchingList.FirstOrDefault().WorkShopCode;
            }
            ViewData["WorkshopID"] = WorkshopID;

            return View();
        }

        /// <summary>
        /// 工艺图纸上传
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessBlueprint()
        {
            return View();
        }

        /// <summary>
        /// 工艺图纸上传客户端版
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessBlueprint_ftp()
        {
            return View();
        }

        /// <summary>
        /// 下料数量
        /// </summary>
        /// <returns></returns>
        public ActionResult ProduceNum()
        {
            return View();
        }

        /// <summary>
        /// 覆盖工艺BOM
        /// </summary>
        /// <returns></returns>
        public ActionResult CoverProcessBom()
        {
            return View();
        }

    }

    public class MES_BN_ProductProcessRouteApiController : ApiController
    {
        public string GetNewRowId(string type, string key, int qty = 1)
        {
            switch (type)
            {
                case "grid0":
                    var service0 = new MES_BN_ProductProcessRouteDetailService();
                    return service0.GetNewKey("ID", "maxplus", qty, null);
                case "grid1":
                    var service1 = new MES_BN_ProductProcessRouteEquipmentService();
                    return service1.GetNewKey("ID", "maxplus", qty, null);
                case "grid":
                    var service = new MES_BN_ProductProcessRouteService();
                    return service.GetNewKey("ID", "maxplus", qty, null);
                default:
                    return "";
            }
        }

        //public dynamic PostProductPlanList(dynamic data)
        //{
        //    List<dynamic> plan_list = data.ToObject<List<dynamic>>();
        //    var plan_count = plan_list.Select(p => p.PartCode).Distinct().Count();
        //    var ProcessTypeList = plan_list.Select(p => Convert.ToString(p.ProcessType)).Distinct();
        //    var ProductPlanList = new List<dynamic>();
        //    string ProjectName = "";
        //    string ProductName = "";
        //    if (plan_count == 1)
        //    {
        //        string PartCode = plan_list.First().PartCode;
        //        string PlanCode = plan_list.First().PlanCode;
        //        string ContractCode = plan_list.First().ContractCode;
        //        int ProductID = plan_list.First().ProductID;

        //        ProjectName = new PMS_BN_ProjectService().GetModel(ParamQuery.Instance().AndWhere("ContractCode", ContractCode)).ProjectName;
        //        ProductName = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("ID", ProductID)).ProductName;

        //        var IsExist = new APS_ProjectProduceDetialService().GetModelList(ParamQuery.Instance().AndWhere("MonthPlanCode", PlanCode).AndWhere("RootPartCode", PartCode)).Count == 0 ? true : false;
        //        if (IsExist)
        //        {
        //            ProductPlanList = new MES_BN_ProductProcessRouteService().GetProductPlanList(plan_list);
        //        }
        //    }
        //    return new { list = ProductPlanList, ProjectName = ProjectName, ProductName = ProductName, ProcessTypeList = ProcessTypeList };
        //}

        //public dynamic PostProductPlanList(dynamic data)
        //{
        //    List<dynamic> plan_list = data.ToObject<List<dynamic>>();
        //    var plan_count = plan_list.Select(p => p.PartCode).Distinct().Count();
        //    var ProcessTypeList = plan_list.Select(p => Convert.ToString(p.ProcessType)).Distinct();
        //    var ProductPlanList = new List<dynamic>();
        //    string ProjectName = "";
        //    string ProductName = "";
        //    if (plan_count == 1)
        //    {
        //        string PartCode = plan_list.First().PartCode;
        //        string PlanCode = plan_list.First().PlanCode;
        //        string ContractCode = plan_list.First().ContractCode;
        //        int ProductID = plan_list.First().ProductID;

        //        ProjectName = new PMS_BN_ProjectService().GetModel(ParamQuery.Instance().AndWhere("ContractCode", ContractCode)).ProjectName;
        //        ProductName = new PMS_BN_ProjectDetailService().GetModel(ParamQuery.Instance().AndWhere("ID", ProductID)).ProductName;

        //        var IsExist = new APS_ProjectProduceDetialService().GetModelList(ParamQuery.Instance().AndWhere("MonthPlanCode", PlanCode).AndWhere("RootPartCode", PartCode)).Count == 0 ? true : false;
        //        if (IsExist)
        //        {
        //            ProductPlanList = new MES_BN_ProductProcessRouteService().GetProductPlanList(plan_list);
        //        }
        //    }
        //    return new { list = ProductPlanList, ProjectName = ProjectName, ProductName = ProductName, ProcessTypeList = ProcessTypeList };
        //}

        public dynamic GetProductProcessRoutePlan(string ContractCode, string PartCode, string Quantity, string type)
        {
            return new MES_BN_ProductProcessRouteService().getProductProcessRoute(ContractCode, PartCode, Convert.ToInt32(Quantity), type);
        }

        public int GetProcessBom(string ContractCode, string PartCode)
        {
            return new MES_BN_ProductProcessRouteService().GetProcessBom(ContractCode, PartCode);
        }
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
<settings defaultOrderBy='ProcessLineCode'>
    <select>*</select>
    <from>MES_BN_ProductProcessRoute</from>
    <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
        <field name='ContractCode'		cp='equal'></field>  
<field name='PartCode'		cp='equal'></field>   
    </where>
</settings>");
            var service = new MES_BN_ProductProcessRouteService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        public dynamic GetUpdateApproveState(string cCode, string partCode, int type)
        {
            try
            {
                return new MES_BN_ProductProcessRouteService().GetUpdateApproveState(cCode, partCode, type);
            }
            catch (Exception ex)
            {
                return new
                {
                    result = false,
                    msg = ex.Message
                };
            }
        }

        public dynamic GetPageData(string id = "", string tabName = "")
        {
            var pQuery = ParamQuery.Instance();
            if (id == "")
            {
                var result = new
                {
                    tab0 = "",
                };
                return result;
            }
            else
            {
                List<dynamic> data = new List<dynamic>();
                if (tabName == "工具明细")
                {
                    data = new MES_BN_ProductProcessRouteDetailService().GetDynamicList(pQuery.AndWhere("ProcessRouteID", id));
                }
                else if (tabName == "设备明细")
                {
                    data = new MES_BN_ProductProcessRouteEquipmentService().GetDynamicList(pQuery.AndWhere("ProcessRouteID", id));
                }
                var result = new
                {
                    rows = data,
                    total = data.Count
                };
                return result;
            }
        }

        // 地址：POST api/mms/send 功能：保存收料单数据
        [System.Web.Http.HttpPost]
        public void Edit_TopList(dynamic data)
        {
            var formWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>
        MES_BN_ProductProcessRoute
    </table>
    <where>
        <field name='ID' cp='equal'></field>
    </where>
</settings>
");
            var service = new MES_BN_ProductProcessRouteService();
            var result = service.Edit(null, formWrapper, data);
        }
        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var tabsWrapper = new List<RequestWrapper>();
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_BN_ProductProcessRouteDetail</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));
            tabsWrapper.Add(RequestWrapper.Instance().LoadSettingXmlString(@"
<settings>
    <table>MES_BN_ProductProcessRouteEquipment</table>
    <where>
        <field name='ID' cp='equal'></field>      
    </where>
</settings>"));

            var service = new MES_BN_ProductProcessRouteService();
            var result = service.EditPage(data, null, tabsWrapper);
        }
        public void PostCheckOP(string ContractCode, string PartCode)
        {
            new MES_BN_ProductProcessRouteService().PostCheckOP(ContractCode, PartCode);
        }
        public dynamic GetBoomTree(string PartCode, string ContractCode, string ProductName)
        {
            return new SYS_BOMService().GetBoomTree(PartCode, ContractCode, ProductName);
        }

        public dynamic GetProcessRouteModel(string code, string ContractCode, string PartCode, string ProjectName, string fcode, string type)
        {
            return new PRS_ProcessRouteModelMainService().GetProcessRouteModel(code, ContractCode, PartCode, ProjectName, fcode, type);

        }

        public dynamic GetProductProcessRoute(string contractCode, string partCode, string processCode, string workGroupID)
        {
            return new MES_BN_ProductProcessRouteService().GetProductProcessRoute(contractCode, partCode, processCode, workGroupID);
        }
        public string GetAudit1(string ContractCode, string partCode)
        {
            return new MES_BN_ProductProcessRouteService().GetAudit2(ContractCode, partCode);
        }
        //查询工艺BOM树
        public dynamic GetGYBOMTree(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
                return null;
            return new PRS_Process_BOMService().GetGYBOMTree(PartCode, VersionCode);
            //return new PRS_Process_BOMService().GetGYBomTreeGrid(PartCode);
        }

        public dynamic GetIsSalefyMadeBomTree(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
                return null;
            return new PRS_Process_BOMService().GetGYBomTreeGrid(PartCode);
        }

        public dynamic GetProduceNumBomTree(string PartCode, string VersionCode, string ContractCode, int ProductID)
        {
            if (PartCode == "0")
                return null;
            return new PRS_Process_BOMService().GetProduceNumBomTreeGrid(PartCode, ContractCode, ProductID);
        }

        public dynamic GetAutoUpdateBlankingNum(string partCode)
        {
            return new PRS_Process_BOMService().GetAutoUpdateBlankingNum(partCode);
        }

        public dynamic GetProcessCardEditBomTree(string PartCode, string VersionCode, string ContractCode)
        {
            if (PartCode == "0")
                return null;
            return new PRS_Process_BOMService().GetProcessCardEditBomTreeGrid(PartCode, ContractCode);
        }

        public dynamic GetBlueprintPageBomTreeGrid(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
            {
                return null;
            }
            else
            {
                return new PRS_Process_BOMService().GetBlueprintPageBomTreeGrid(PartCode);
            }
        }

        public dynamic GetCoverProcessBomPageBomTreeGrid(string PartCode, string VersionCode)
        {
            if (PartCode == "0")
            {
                return null;
            }
            else
            {
                return new PRS_Process_BOMService().GetCoverProcessBomPageBomTreeGrid(PartCode);
            }
        }

        public void GetUpdateProcessFigureIsEnableByProcessBomID(int processBomID)
        {
            new PRS_ProcessFigureService().UpdateProcessFigureIsEnableByProcessBomID(processBomID);
        }

        public dynamic PostUpdateProcessFigureByBlueprintUpload(dynamic dynamic)
        {
            return new PRS_ProcessFigureService().UpdateProcessFigureByBlueprintUpload(dynamic);
        }

        //更新工艺BOM树
        public void GetUpdateGYBOMTree(string PartCode, string VersionCode, string ContractCode, string ProductID)
        {
            if (PartCode == "0")
                return;
            new PRS_Process_BOMService().GetUpdateGYBOMTree(PartCode, VersionCode, ContractCode, ProductID);
        }
        //工艺BOM实体
        public dynamic GetPBOMModel(string id, string ParentCode)
        {
            return new PRS_Process_BOMService().GetPBOMModel(id, ParentCode);
        }
        //工艺路线实体
        public dynamic GetProcessRoute(string ProcessModelType, string ContractCode, string PartCode)
        {
            return new PRS_Process_BOMService().GetProcessRouteModel(ContractCode, PartCode, ProcessModelType);
        }
        //工艺路线设备
        public dynamic GetProcessRouteEQP(string id)
        {
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("ProcessRouteID", id);
            var re = new MES_BN_ProductProcessRouteEquipmentService().GetModelList(pQuery);
            return re;
        }
        //工艺路线文件
        public dynamic GetProcessRouteFile(string id)
        {
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("SourceID", id).AndWhere("FileType", 5).AndWhere("IsEnable", true);
            var re = new QMS_QualityReportDocService().GetModelList(pQuery);
            return re;
        }

        public dynamic GetUpdateProcessIsSelfMade(string id, string value)
        {
            return new PRS_Process_BOMService().GetUpdateProcessIsSelfMade(id, value);
        }

        public dynamic GetUpdateProcessIsEnable(string id, string value)
        {
            throw new Exception("暂未实现！");
        }

        public dynamic GetUpdateProcessIsSelfMadeAndIsEnable(string id, string selfMadeValue, string enableValue)
        {
            return new PRS_Process_BOMService().GetUpdateProcessIsSelfMadeAndIsEnable(id, selfMadeValue, enableValue);
        }

        public dynamic GetUpdateProcessIsSelfMade2(string pCode, int selfMadeValue)
        {
            return new PRS_Process_BOMService().GetUpdateProcessIsSelfMade2(pCode, selfMadeValue);
        }

        public dynamic GetUpdateIsCrux(int ID, int IsCrux)
        {
            return new PRS_Process_BOMService()
                 .Update(ParamUpdate.Instance()
                 .Update("PRS_Process_BOM")
                 .Column("IsCrux", IsCrux)
                 .AndWhere("ID", ID));
        }

        public dynamic GetUpdateProcessMaterialNum(string pCode, int blankingNum, int purchaseNum, int restructNum)
        {
            return new PRS_Process_BOMService().GetUpdateProcessMaterialNum(pCode, blankingNum, purchaseNum, restructNum);
        }

        public dynamic GetDeleteProcessRoute(string id, string ContractCode, string PartCode)
        {
            return new MES_BN_ProductProcessRouteService().DeleteProcessRoute(id, ContractCode, PartCode);
        }
        [System.Web.Http.HttpPost]
        public string PostFileData(string id, string tName, string tId, int type)
        {
            if (tId != null && tId != "")
            {
                tId = tId.Remove(tId.Length - 1, 1);
                string sql = string.Format(@"
delete QMS_QualityReportDoc where ID in({0})", tId);
                using (var db = Db.Context("Mms"))
                {
                    db.Sql(sql).Execute();
                }
            }
            string info = string.Empty;
            try
            {
                //获取客户端上传的文件集合
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                //判断是否存在文件
                Random rd = new Random();
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        //获取文件集合中的第一个文件(每次只上传一个文件)
                        HttpPostedFile file = files[i];
                        if (file.FileName == "")
                            continue;
                        //定义文件存放的目标路径
                        string targetDir = HttpContext.Current.Server.MapPath("~/Upload/" + new HomeApiController().GetPath(tName));
                        //创建目标路径
                        ZFiles.CreateDirectory(targetDir);
                        //组合成文件的完整路径
                        var r = rd.Next(9999, 99999);
                        string newFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + r + "." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1, file.FileName.Length - file.FileName.LastIndexOf('.') - 1);
                        string path = System.IO.Path.Combine(targetDir, System.IO.Path.GetFileName(newFileName));
                        //保存上传的文件到指定路径中
                        try
                        {
                            file.SaveAs(path);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        info = "上传成功";

                        //string filePath = HttpContext.Current.Server.MapPath("~/Upload/" + path) + newFileName;
                        string InsertSql = string.Format(@"insert into QMS_QualityReportDoc values('',{5},'{0}','{1}',1,'{2}',getdate(),'{2}',getdate(),'{3}','{4}')", newFileName, path, MmsHelper.GetUserName(), file.FileName, id, type);
                        using (var db = Db.Context("Mms"))
                        {
                            db.Sql(InsertSql).Execute();
                        }
                    }

                }
                else
                {
                    info = "上传失败";
                }
            }
            catch
            {
                info = "上传失败";
            }
            return info;
        }
        /// <summary>
        /// 工艺BOm保存/删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int PostOnSave(List<PRS_Process_BOM> model)
        {
            if (model.Count < 0)
            {
                return 0;
            }
            else
            {
                int result = 0;
                if (model[0].ID <= 0)
                {
                    result = new PRS_Process_BOMService().Insert(model[0]);
                }
                else
                {
                    result = new PRS_Process_BOMService().Update(model[0]);
                }
                return result;
            }
        }
        /// <summary>
        /// 工艺路线保存/删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int PostOnGYSave(List<MES_BN_ProductProcessRoute> model)
        {
            if (model.Count < 0)
            {
                return 0;
            }
            else
            {
                int result = 0;
                if (model[0].ID <= 0)
                {
                    result = new MES_BN_ProductProcessRouteService().Insert(model[0]);
                }
                else
                {
                    result = new MES_BN_ProductProcessRouteService().Update(model[0]);
                }
                return result;
            }
        }

        public dynamic GetUpdateProcessStatistical(string pCode, int type)
        {
            return new PRS_Process_BOMService().GetUpdateProcessStatistical(pCode, type);
        }

        [System.Web.Http.HttpPost]
        public void PostUpdateSortID(List<MES_BN_ProductProcessRoute> model)
        {
            model.Where(m => m.ID != 0).ToList().ForEach(m =>
            {
                new MES_BN_ProductProcessRouteService().Update(ParamUpdate.Instance()
                    .Update("MES_BN_ProductProcessRoute")
                    .Column("ProcessLineCode", m.ProcessLineCode)
                    .AndWhere("ID", m.ID)
                    );
            });
        }

        [System.Web.Http.HttpPost]
        public int PostOnGYSave2(List<MES_BN_ProductProcessRoute> model)
        {
            if (model.Count < 0)
            {
                return 0;
            }
            else
            {
                int result = 0;
                if (model[0].ID <= 0)
                {
                    result = new MES_BN_ProductProcessRouteService().Insert(model[0]);
                }
                else
                {
                    result = new MES_BN_ProductProcessRouteService().Update2(model[0]);
                }
                return result;
            }
        }
        /// <summary>
        /// 工步保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public void PostOnSaveWorkStep(dynamic data)
        {
            List<PRS_ProcessWorkSteps> model = new List<PRS_ProcessWorkSteps>();

            foreach (var item in data.model)
            {
                int? ManHours = item.ManHours;
                PRS_ProcessWorkSteps step = new PRS_ProcessWorkSteps();
                step.WorkStepsLineCode = item.WorkStepsLineCode;
                step.WorkStepsName = item.WorkStepsName;
                step.WorkStepsContent = item.WorkStepsContent;
                step.ManHours = ManHours;
                step.Unit = Convert.ToInt32(item.Unit);
                step.ProcessRouteID = Convert.ToInt32(data["gyid"].ToString());
                step.CreatePerson = MmsHelper.GetUserName();
                step.CreateTime = DateTime.Now;
                step.ModifyPerson = step.CreatePerson;
                step.ModifyTime = step.CreateTime;
                step.IsEnable = 1;
                model.Add(step);
            }
            if (model.Count > 0)
            {
                new PRS_ProcessWorkStepsService().Insert(model);
            }
        }
        /// <summary>
        /// 工艺模型保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int PostOnSaveProcessRouteModel(dynamic data)
        {
            return new PRS_BD_ProcessRouteModelService().SaveProcessRouteModel(data);
        }
        public void GetDeleteWorkStep(int gyid)
        {
            new PRS_ProcessWorkStepsService().Delete(gyid);
        }
        [System.Web.Http.HttpPost]
        public void PostOnSaveEQP(dynamic data)
        {
            new MES_BN_ProductProcessRouteEquipmentService().SaveEQP(data["eqp"].ToString(), data["gyid"].ToString());
        }
        public dynamic GetProcessWorkSteps(string id)
        {
            var pQuery = ParamQuery.Instance().Select("*").AndWhere("ProcessRouteID", id);
            var re = new PRS_ProcessWorkStepsService().GetModelList(pQuery);
            return re;
        }


        public dynamic PostUpLoadPRouteBlueprint(dynamic data)
        {
            try
            {
                List<QMS_QualityReportDoc> list = new List<QMS_QualityReportDoc>();
                DateTime newDT = DateTime.Now;

                int type = Convert.ToInt32(data["Type"]);
                string sourceID = data["SourceID"];

                foreach (var item in data["Files"])
                {
                    list.Add(new QMS_QualityReportDoc()
                    {
                        FileType = 5,
                        FileName = item["FileName"],
                        FileAddress = item["FileAddress"],
                        IsEnable = false,
                        CreatePerson = data["User"]["UserName"],
                        CreateTime = newDT,
                        ModifyPerson = data["User"]["UserName"],
                        ModifyTime = newDT,
                        DocName = item["DocName"],
                        SourceID = data["TempID"]
                    });
                }

                return new QMS_QualityReportDocService().SetQualityReportDocData(list, sourceID, type);
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Result = false,
                    Msg = ex.Message
                };
            }
        }

        public dynamic GetUpdateQualityReportDocIsEnable(string tempID, string newID)
        {
            return new QMS_QualityReportDocService().UpdateQualityReportDocIsEnable(tempID, newID);
        }


        public dynamic GetDelPRouteBlueprint(string tempID)
        {
            return new QMS_QualityReportDocService().DelPRouteBlueprint(tempID);
        }

        public void PostSetMateType(dynamic data)
        {
            int ProcessBomID = data.ProcessBomID;
            string MateType = data.MateType;
            if (string.IsNullOrWhiteSpace(MateType))
            {
                new PRS_Process_BOMService().Update(
                    ParamUpdate.Instance()
                    .Update("PRS_Process_BOM")
                    .Column("MateType", DBNull.Value)
                    .AndWhere("ID", ProcessBomID));
            }
            else
            {
                new PRS_Process_BOMService().Update(
                    ParamUpdate.Instance()
                    .Update("PRS_Process_BOM")
                    .Column("MateType", MateType)
                    .AndWhere("ID", ProcessBomID));
            }
        }

    }

    public class V_ProcessRouteWorkStepsApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
    <settings defaultOrderBy='ContractCode,PartCode,ProcessCode,WorkStepsLineCode'>
        <select>*</select>
        <from>V_ProcessRouteWorkSteps</from>
        <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true'>
                <field name='ContractCode' 		 cp='like'></field>
                <field name='ProjectName' 		 cp='like'></field>
                <field name='FigureCode' 		 cp='like'></field>
                <field name='ManHours' 		 cp='IsNull'></field>
        </where>
    </settings>");
            var service = new MES_BN_ProductProcessRouteService();
            var pQuery = query.ToParamQuery();
            var result = service.GetDynamicListWithPaging(pQuery);
            return result;
        }

        [System.Web.Http.HttpPost]
        public void Edit(dynamic data)
        {
            var listWrapper = RequestWrapper.Instance().LoadSettingXmlString(@"
    <settings>
        <table>
            PRS_ProcessWorkSteps
        </table>
        <where>
            <field name='ID' cp='equal'></field>
        </where>
    </settings>");
            var service = new PRS_ProcessWorkStepsService();
            //var result = service.Edit(null, listWrapper, data);
            //记录更改工时定额的工步的ID
            string ids = "";
            //用于保存直接更改工艺路线工时的脚本字符串
            string mids = "";
            //用于保存更改工步工时定额的脚本数据
            string updateSteps = "";
            //判断更改后执行结果的受影响行数
            int updatePPR = 0;
            if (data.list.updated.ToString() != "[]")
            {
                foreach (JToken row in data["list"]["updated"].Children())
                {
                    ids += row["ID"];
                    //获取工步ID
                    var getId = row["ID"] ?? "";

                    if (!string.IsNullOrEmpty(getId.ToString()))
                    {
                        //工步ID不为空时，生成更改工步工时定额的脚本
                        ids += ",";
                        updateSteps += string.Format(@"UPDATE a
SET ManHours= '{0}'
FROM PRS_ProcessWorkSteps  a
WHERE ID ='{1}'", row["ManHours"].ToString(), row["ID"].ToString());
                    }
                    //当工步ID为空时表示是一个没有工步的工序，直接生成更改工艺路线工时定额的脚本
                    else
                    {
                        mids += string.Format(@"UPDATE a
SET ManHour= '{0}'
FROM MES_BN_ProductProcessRoute  a
WHERE ID ='{1}'", row["ManHours"].ToString(), row["mID"].ToString());
                        updatePPR++;
                    }
                }
                if (ids.Length > 0)
                {
                    //将多个工步id最后的，号去掉，此时保存的id字符串可以直接作为sql中的in条件
                    ids = ids.Remove(ids.Length - 1, 1);
                }

            }
            int isFalse = 0;
            var db = Db.Context("Mms");
            using (db.UseTransaction(true))
            {
                //有工步的工艺路线，编辑工步工时同时将工序工时汇总
                if (!string.IsNullOrEmpty(ids))
                {
                    //执行编辑工步工时定额，判断返回值
                    var res = db.Sql(updateSteps).Execute();
                    if (res < updatePPR)
                    {
                        db.Rollback();
                        isFalse++;
                    }
                    //使用更改工步关联工序的工时定额的存储过程
                    var document = db.StoredProcedure("CalcProcessManHour")
                .Parameter("stepids", ids);
                    int ret = document.Execute();
                    if (ret <= 0)
                    {
                        db.Rollback();
                        isFalse++;
                    }
                }
                //没有工步的工序，直接更改工序中的工时定额。
                if (!string.IsNullOrEmpty(mids))
                {
                    var res = db.Sql(mids).Execute();
                    if (res < updatePPR)
                    {
                        db.Rollback();
                        isFalse++;
                    }
                }
                if (isFalse > 0)
                {
                    db.Rollback();
                }
                else
                {
                    db.Commit();
                }

            }

        }
    }
}

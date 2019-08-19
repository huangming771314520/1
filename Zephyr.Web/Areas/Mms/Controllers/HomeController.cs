using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Zephyr.Core;
using Zephyr.Models;
using Zephyr.Areas;
using System.IO;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;
using Zephyr.Areas.Areas.Mms.Common;
using Zephyr.Utils;
using System.Xml.Linq;
using Zephyr.Areas.CommonWrap;
using System.Net.Http;
using System.Net;
using System.Configuration;
using Zephyr.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace Zephyr.Areas.Mms.Controllers
{
    [MvcMenuFilter(false)]
    public class HomeController : Controller
    {
        public static string fileGuid = "";
        // GET: /MMS/Home/
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DrugDialog(string planCode)
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/MES_BN_DrugBatch",
                    newkey = "/api/Mms/MES_BN_DrugBatch/getnewkey",
                    edit = "/api/Mms/MES_BN_DrugBatch/edit",
                    release = "/api/Mms/MES_BN_ProductPlan/PostConfirmRelease?planCode=" + planCode
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    PlanCode = planCode,
                },
                defaultRow = new
                {
                    PlanCode = planCode,
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "PlanCode", "DrugDose", "BatchCode", "State", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
        public ActionResult BomDialog(string parentNo, string planCode)
        {
            var code = new sys_codeService();
            var model = new
            {
                dataSource = new
                {

                },
                urls = new
                {
                    query = "/api/Mms/MES_BN_ProductPlan/GetBom",
                    newkey = "/api/Mms/MES_BD_Bom/getnewkey",
                    edit = "/api/Mms/MES_BD_Bom/edit",
                    choose = "/api/Mms/MES_BN_ProductPlan/PostChooseNewVerion?planCode=" + planCode
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    MateCode = "",
                    MateName = "",
                    ParentNo = parentNo,
                    MateNum = "",
                    Enable = "",
                    ProductVerison = "",
                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { "ID", "MateCode", "MateName", "ParentNo", "MateNum", "ProductVerison", "MateVersion", "Enable", "CreatePerson", "CreateTime", "ModifyPerson", "ModifyTime" }
                }
            };

            return View(model);
        }
        public ActionResult CommonDialog(string xmlName)
        {
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").ToList();
            Dictionary<string, string> form = new Dictionary<string, string>();
            foreach (var item in fieldList)
            {
                var searchType = item.Attribute("searchType");
                string field = ReadXml.getXmlElementAttr(item, "field");
                if (searchType != null)
                {
                    form.Add(field, "");
                }
            }
            var model = new
            {
                form = form,
                xmlName = xmlName
            };

            ViewBag.xmlName = xmlName;
            return View(model);
        }
        public ActionResult CommonDialog3(string xmlName)
        {
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").ToList();
            Dictionary<string, string> form = new Dictionary<string, string>();
            foreach (var item in fieldList)
            {
                var searchType = item.Attribute("searchType");
                string field = ReadXml.getXmlElementAttr(item, "field");
                if (searchType != null)
                {
                    form.Add(field, "");
                }
            }
            var model = new
            {
                form = form,
                xmlName = xmlName
            };

            ViewBag.xmlName = xmlName;
            return View(model);
        }
        public ActionResult CommonDialog2(string xmlName, string parm)
        {
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").ToList();
            Dictionary<string, string> form = new Dictionary<string, string>();
            foreach (var item in fieldList)
            {
                var searchType = item.Attribute("searchType");
                string field = ReadXml.getXmlElementAttr(item, "field");
                if (searchType != null)
                {
                    var currentValue = "";
                    if (parm != null)
                    {
                        string[] p = parm.Split(':');
                        if (field == p[0])
                        {
                            form.Add(field, p[1]);
                        }
                        else
                            form.Add(field, currentValue);
                    }
                    else
                        form.Add(field, currentValue);
                }
            }
            var model = new
            {
                form = form,
                xmlName = xmlName
            };

            ViewBag.xmlName = xmlName;
            return View(model);
        }
        public ActionResult UploadDialog(string id)
        {
            return View();
        }

        public ActionResult UploadDialog2(string id)
        {
            return View();
        }

        public ActionResult UploadDialog3(string id)
        {
            return View();
        }

        public ActionResult LookupMaterial()
        {
            return View();
        }
        public ActionResult LookupMaterialInlist()
        {
            return View();
        }
        public ActionResult LookupWHCon()
        {
            return View();
        }

        public ActionResult LookupMaterialPutOff()
        {
            return View();
        }
        public ActionResult LookupMaterialOrder()
        {
            return View();
        }
        public ActionResult LookupJitOrder()
        {
            return View();
        }
        public ActionResult LookupEquipment()
        {
            return View();
        }

        //通用多文件上传弹窗
        public ActionResult CommonUploadFile()
        {
            return View();
        }

        //通用导入excel to sqlserver
        public ActionResult CommonImportExcel()
        {
            return View();
        }
        public ActionResult CommonImportExcelDetail()
        {
            return View();
        }
        public ActionResult Wo_Station_LeadFile()
        {
            return View();
        }

        public ActionResult LookupDirectIn()
        {
            return View();
        }

        public ActionResult UserSkillMatrix()
        {
            return View();
        }

        public ActionResult LookupRecipient()
        {
            return View();
        }
        public ActionResult LookupProInsFileUpload()
        {
            return View();
        }
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string Object_ID = "", string TableName = "")
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Upload/" + TableName + "/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);      //原始文件名称
                    string fileExtension = Path.GetExtension(fileName);
                    //文件扩展名
                    string saveName = "[" + DateTime.Now.ToString("yyyyMMddhhmmss") + "]" + fileName; //保存文件名称
                    fileData.SaveAs(string.Format("{0}\\{1}", filePath, saveName));
                    string userName = MmsHelper.GetUserName();
                    string InsertSql = string.Format(@"update PMS_BN_PartFile set FileAddress='{0}' where ID ='{1}'", filePath + saveName, Object_ID);
                    using (var db = Db.Context("Mms"))
                    {
                        //db.UseTransaction(true);
                        int effectRow = db.Sql(InsertSql).Execute();
                    }
                    return Content("true");
                }
                catch (Exception ex)
                {
                    return Content("false");
                }
            }
            else
            {
                return Content("false");
            }
        }
        private byte[] ReadFileBytes(HttpPostedFileBase fileData)
        {
            byte[] data;
            using (Stream inputStream = fileData.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;
        }
        /// <summary>
        /// 获取流程附件html,添加可删除附件链接
        /// </summary>
        /// <param name="strGuid">附件guid</param>
        /// <returns>string</returns>
        public ActionResult GetAttachmentHtml(string guid, string TableName)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return Content(html);

            string selectSql = string.Format(@"select ID,FileAddress from PMS_BN_PartFile where ID='{0}' ", guid);
            var db = Db.Context("Mms");
            DataTable dt = new DataTable();
            using (db)
            {
                dt = db.Sql(selectSql).QuerySingle<DataTable>();
            }
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Object_Id = dt.Rows[i]["ID"].ToString();
                    string fileName = dt.Rows[i]["FileAddress"].ToString();
                    string path = urlconvertor(fileName);
                    var fileNameArray = fileName.Split('\\');
                    var Length = fileNameArray.Length - 1;
                    var fileArray = fileName.Split('.');
                    int fileArrayLength = fileArray.Length - 1;
                    fileGuid = fileName;
                    fileName = System.Web.HttpContext.Current.Server.UrlEncode(fileName);

                    sb.Append("<tr>");
                    sb.AppendFormat("<td style='width:20px'> <img border='0' width='16px' height='16px' src='/Content/images/cross.png' onclick=\"deleteAttach('{0}','{1}',this)\"/> </td> ", Object_Id, fileName);
                    sb.AppendFormat(@"<td><li><span>[ 附件{0} ]</span>", i + 1);
                    //sb.AppendFormat(@"<img border='0' width='16px' height='16px' src='/Content/images/file_extension/{0}.png' />", fileArray[fileArrayLength]);
                    sb.AppendFormat(@"<a href='\{0}?ext={1}' target='_blank' name='{2}'>&nbsp;{2}</a></li> </td> ", path, fileArray[fileArrayLength], fileNameArray[Length]);
                    sb.Append("</tr>");
                }

                string result = string.Format("<table style='border:0px solid #A8CFEB;'>{0}</table>", sb.ToString());
                return Content(result);
            }
            else
            {
                return Content(html);
            }
        }
        private string urlconvertor(string imagesurl1)
        {
            string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"/", @"/");
            return imagesurl2;
        }

        /// <summary>
        /// 删除单个附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id, string fileName)
        {
            string result = "false";
            string deleteSql = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                deleteSql = string.Format(@"delete from BD_FileAddress where Object_Id='{0}'", id);
                int effect = Db.Context("Mms").Sql(deleteSql).Execute();
                if (effect > 0)
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    result = "ture";
                }
            }
            return Content(result);
        }
        public FileStreamResult FileDownload(string id, string fileName, string path)
        {
            try
            {
                string sql = string.Format(@"update [PRS_DesignChangeRequest] set RequestState=2 where id='{0}'", id);
                int effect = Db.Context("Mms").Sql(sql).Execute();
                return File(new FileStream(path, FileMode.Open), "text/plain", fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public FileStreamResult FileDownload2(string id, string docName, string path)
        {
            try
            {
                return File(new FileStream(path, FileMode.Open), "text/plain", docName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class HomeApiController : ApiController
    {



        //获取guid
        public List<string> GetGuidArray(int total)
        {
            var listGuid = new List<string>();
            string guid = string.Empty;
            for (int i = 0; i < total; i++)
            {
                guid = System.Guid.NewGuid().ToString();
                listGuid.Add(guid);
            }
            return listGuid;
        }


        public string GetPath(string xmlName)//上传路径xml节点名称
        {
            string path = string.Empty;
            //将XML文件加载进来
            XDocument document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Views/Shared/UploadXml/") + xmlName + ".xml");
            //获取到XML的根元素进行操作
            XElement root = document.Root;
            //XElement ele = root.Element("BOOK");
            ////获取name标签的值
            //XElement shuxing = ele.Element("name");
            //Console.WriteLine(shuxing.Value);
            //获取根元素下的所有子元素
            IEnumerable<XElement> enumerable = root.Elements("path");
            foreach (XElement item in enumerable)
            {
                path += item.Value.Trim();
            }
            return path;

        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostUpload(string docName, string path)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            //path:PRS_ProcessFigure/
            //fileName:2019070309511121870.png
            var fileName = docName + "." + file.FileName.Substring(file.FileName.LastIndexOf('.') + 1, file.FileName.Length - file.FileName.LastIndexOf('.') - 1);
            this.SaveAs(HttpContext.Current.Server.MapPath("~/Upload/" + path) + fileName, file);

            HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new { name = file.FileName };


            HttpContext.Current.Response.Write(serializer.Serialize(result));
            HttpContext.Current.Response.StatusCode = 200;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private void SaveAs(string saveFilePath, HttpPostedFile file)
        {
            long lStartPos = 0;
            int startPosition = 0;
            int endPosition = 0;
            var contentRange = HttpContext.Current.Request.Headers["Content-Range"];
            //bytes 10000-19999/1157632
            if (!string.IsNullOrEmpty(contentRange))
            {
                contentRange = contentRange.Replace("bytes", "").Trim();
                contentRange = contentRange.Substring(0, contentRange.IndexOf("/"));
                string[] ranges = contentRange.Split('-');
                startPosition = int.Parse(ranges[0]);
                endPosition = int.Parse(ranges[1]);
            }
            var filePath = saveFilePath.Substring(0, saveFilePath.LastIndexOf('\\'));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            System.IO.FileStream fs;
            if (System.IO.File.Exists(saveFilePath))
            {
                fs = System.IO.File.OpenWrite(saveFilePath);
                lStartPos = fs.Length;

            }
            else
            {
                fs = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Create);
                lStartPos = 0;
            }
            if (lStartPos > endPosition)
            {
                fs.Close();
                return;
            }
            else if (lStartPos < startPosition)
            {
                lStartPos = startPosition;
            }
            else if (lStartPos > startPosition && lStartPos < endPosition)
            {
                lStartPos = startPosition;
            }
            fs.Seek(lStartPos, System.IO.SeekOrigin.Current);
            byte[] nbytes = new byte[512];
            int nReadSize = 0;
            nReadSize = file.InputStream.Read(nbytes, 0, 512);
            while (nReadSize > 0)
            {
                fs.Write(nbytes, 0, nReadSize);
                nReadSize = file.InputStream.Read(nbytes, 0, 512);
            }
            fs.Close();
        }

        [System.Web.Http.HttpPost]
        public void PostUpdate(string ID, string newFileName, string oldFileName, string path, string tableName)
        {
            newFileName = newFileName + "." + oldFileName.Substring(oldFileName.LastIndexOf('.') + 1, oldFileName.Length - oldFileName.LastIndexOf('.') - 1);
            string filePath = HttpContext.Current.Server.MapPath("~/Upload/" + path) + newFileName;
            string InsertSql = string.Format(@"update " + tableName + " set FileAddress='{0}',FileName='{1}',DocName='{2}' where ID ='{3}'", filePath, newFileName, oldFileName, ID);
            using (var db = Db.Context("Mms"))
            {
                db.Sql(InsertSql).Execute();
            }
        }

        [System.Web.Http.HttpPost]
        //public void PostUpdate3()
        public void PostUpdate3(dynamic data)
        {
            int id = data.id;
            string docName = data.docName;
            string fileName = data.fileName;
            string filePath = data.filePath;
            //int id, string docName, string fileName, string filePath
            //HttpPostedFile fileData = HttpContext.Current.Request.Files["PrsFile"];
            //var filestream = fileData.InputStream;
            //var filename = fileData.FileName;
            //byte[] FileBytes = FtpHelper.StreamToBytes(filestream);

            //var id = Convert.ToInt32(HttpContext.Current.Request["id"]);
            //var docName = HttpContext.Current.Request["docName"].ToString();
            //var fileName = HttpContext.Current.Request["fileName"].ToString();
            //var filePath = HttpContext.Current.Request["filePath"].ToString();
            //var tableName = HttpContext.Current.Request["tableName"].ToString();

            PRS_Process_BOM ppBom = new PRS_Process_BOMService().SelectPRS_Process_BOM(id.ToString());

            if (docName.IndexOf('.').Equals(-1))
            {
                throw new Exception("无法读取文件名！");
            }
            else
            {
                DateTime newDT = DateTime.Now;
                fileName += Path.GetExtension(docName);
                //filePath = HttpContext.Current.Server.MapPath("~/Upload/" + filePath) + fileName;
                //string FtpServer = "ftp://" + ConfigurationManager.AppSettings["FtpServer"] + ":" + ConfigurationManager.AppSettings["FtpPort"];
                //filePath = FtpServer + "/" + filePath + fileName;

                PRS_ProcessFigure model = new PRS_ProcessFigure()
                {
                    ContractCode = ppBom.ContractCode,
                    ProductID = ppBom.ProductID,
                    PartCode = ppBom.PartCode,
                    DocName = docName,
                    FileName = fileName,
                    FileAddress = filePath,
                    IsEnable = 1,
                    //CreatePerson = MmsHelper.GetUserName(),
                    CreateTime = newDT,
                    //ModifyPerson = MmsHelper.GetUserName(),
                    ModifyTime = newDT
                };

                SqlConnection conn;
                SqlCommand cmd;
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["Mms"];
                conn = new SqlConnection(cs.ConnectionString);
                cmd = new SqlCommand(@"
insert into PRS_ProcessFigure(ContractCode,ProductID,PartCode,DocName,FileName,FileAddress,
IsEnable,CreatePerson,CreateTime,ModifyPerson,ModifyTime) values (@ContractCode,@ProductID,@PartCode,@DocName,@FileName,@FileAddress,
@IsEnable,@CreatePerson,@CreateTime,@ModifyPerson,@ModifyTime)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(new SqlParameter[]{
                    new SqlParameter(){ParameterName="@ContractCode", SqlDbType= SqlDbType.VarChar,Value=model.ContractCode },
                    new SqlParameter(){ParameterName="@ProductID", SqlDbType= SqlDbType.Int,Value=model.ProductID },
                    new SqlParameter(){ParameterName="@PartCode", SqlDbType= SqlDbType.VarChar,Value=model.PartCode },
                    new SqlParameter(){ParameterName="@DocName", SqlDbType= SqlDbType.NVarChar,Value=model.DocName },
                    new SqlParameter(){ParameterName="@FileName", SqlDbType= SqlDbType.NVarChar,Value=model.FileName },
                    new SqlParameter(){ParameterName="@FileAddress", SqlDbType= SqlDbType.NVarChar,Value=model.FileAddress},
                    new SqlParameter(){ParameterName="@IsEnable", SqlDbType= SqlDbType.Int,Value=model.IsEnable },
                    new SqlParameter(){ParameterName="@CreatePerson", SqlDbType= SqlDbType.VarChar,Value="" },
                    new SqlParameter(){ParameterName="@CreateTime", SqlDbType= SqlDbType.DateTime,Value=model.CreateTime },
                    new SqlParameter(){ParameterName="@ModifyPerson", SqlDbType= SqlDbType.VarChar,Value="" },
                    new SqlParameter(){ParameterName="@ModifyTime", SqlDbType= SqlDbType.DateTime,Value=model.ModifyTime },
                    //new SqlParameter(){ParameterName="@FileContent", SqlDbType= SqlDbType.Image,Value=model.FileContent }
                });
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                //string sql = WinFormClientService.GetInsertSQL(model);
                //using (var db = Db.Context("Mms"))
                //{
                //    //db.Insert("PRS_ProcessFigure").Column("FileContent", FileContent,DataTypes.by)
                //    //db.Sql(sql).Execute();
                //}
            }
        }


        [System.Web.Http.HttpPost]
        public void PostUpdate5(int id, string docName, string fileName, string filePath, string tableName)
        {
            PRS_Process_BOM ppBom = new PRS_Process_BOMService().SelectPRS_Process_BOM(id.ToString());

            if (docName.IndexOf('.').Equals(-1))
            {
                throw new Exception("无法读取文件名！");
            }
            else
            {
                DateTime newDT = DateTime.Now;
                fileName += Path.GetExtension(docName);
                //filePath = HttpContext.Current.Server.MapPath("~/Upload/" + filePath) + fileName;
                string FtpServer = "ftp://" + ConfigurationManager.AppSettings["FtpServer"] + ":" + ConfigurationManager.AppSettings["FtpPort"];
                filePath = FtpServer + "/" + filePath + fileName;

                PRS_ProcessFigure model = new PRS_ProcessFigure()
                {
                    ContractCode = ppBom.ContractCode,
                    ProductID = ppBom.ProductID,
                    PartCode = ppBom.PartCode,
                    DocName = docName,
                    FileName = fileName,
                    FileAddress = filePath,
                    IsEnable = 1,
                    CreatePerson = MmsHelper.GetUserName(),
                    CreateTime = newDT,
                    ModifyPerson = MmsHelper.GetUserName(),
                    ModifyTime = newDT
                };

                string sql = WinFormClientService.GetInsertSQL(model);

                using (var db = Db.Context("Mms"))
                {
                    db.Sql(sql).Execute();
                }
            }
        }
        [System.Web.Http.HttpGet]
        public dynamic GetCommonDialogData()
        {
            //获取url中的条件，注意requestData是弹出框中的查询条件，当动态添加的查询条件不在弹出框中时，需要先将requestData中的属性删除
            var requestData = new NameValueCollection(HttpContext.Current.Request.QueryString);
            //获取删选标识字段名
            string PrimaryID = requestData["PrimaryID"];
            //去掉弹出框中不需要的查询条件
            requestData.Remove("xmlName");
            //
            var xmlName = HttpContext.Current.Request.QueryString["xmlName"].ToString();
            //var selectID = Request.QueryString["selectID"].ToString();
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            var settingsXml = targetXml.Element("settings");
            var setting = RequestWrapper.Instance().LoadSettingXmlString(settingsXml.ToString());

            if (HttpContext.Current.Request.QueryString["PrimaryID"] != null)
            {
                requestData.Remove("PrimaryID");
            }

            if (!string.IsNullOrWhiteSpace(PrimaryID))
            {
                requestData.Remove(PrimaryID);
            }
            var query = setting.SetRequestData(requestData).ToParamQuery();
            if (!string.IsNullOrWhiteSpace(PrimaryID))
            {
                string NotInList = HttpContext.Current.Request.QueryString[PrimaryID].ToString();
                if (!string.IsNullOrEmpty(NotInList))
                {
                    query.AndWhere(PrimaryID, NotInList, Cp.NotIn);
                }

            }
            var service = setting.GetService();
            var data = service.GetDynamicListWithPaging(query);
            return data;
        }
    }
}

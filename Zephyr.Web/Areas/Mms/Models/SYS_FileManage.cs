using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Zephyr.Core;
using Zephyr.Utils.NPOI.SS.Formula.Functions;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_FileManageService : ServiceBase<SYS_FileManage>
    {
        public ResultModel SaveAs(string saveFilePath, Stream stream)
        {
            try
            {
                long lStartPos = 0;
                int startPosition = 0;
                int endPosition = 0;
                var contentRange = HttpContext.Current.Request.Headers["Content-Range"];

                if (!string.IsNullOrEmpty(contentRange))
                {
                    contentRange = contentRange.Replace("bytes", "").Trim();
                    contentRange = contentRange.Substring(0, contentRange.IndexOf("/"));
                    string[] ranges = contentRange.Split('-');
                    startPosition = int.Parse(ranges[0]);
                    endPosition = int.Parse(ranges[1]);
                }

                FileStream fs;
                if (File.Exists(saveFilePath))
                {
                    fs = File.OpenWrite(saveFilePath);
                    lStartPos = fs.Length;
                }
                else
                {
                    fs = new FileStream(saveFilePath, FileMode.Create);
                    lStartPos = 0;
                }

                if (lStartPos > endPosition)
                {
                    fs.Close();
                    stream.Close();

                    return new ResultModel()
                    {
                        Result = true
                    };
                }
                else if (lStartPos < startPosition)
                {
                    lStartPos = startPosition;
                }
                else if (lStartPos > startPosition && lStartPos < endPosition)
                {
                    lStartPos = startPosition;
                }

                fs.Seek(lStartPos, SeekOrigin.Current);
                byte[] nbytes = new byte[512];
                int nReadSize = 0;
                nReadSize = stream.Read(nbytes, 0, 512);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    nReadSize = stream.Read(nbytes, 0, 512);
                }

                fs.Close();
                stream.Close();

                return new ResultModel()
                {
                    Result = true
                };
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

        public ResultModel InsertData(SYS_FileManage model)
        {
            try
            {
                //string sqlA = string.Format(@"SELECT * FROM dbo.SYS_FileManage WHERE BindTableName = '{0}' AND BindCode = '{1}' AND MD5Code = '{2}'", model.BindTableName, model.BindCode, model.MD5Code);
                string sqlA = string.Format(@"SELECT * FROM dbo.SYS_FileManage WHERE BindTableName = '{0}' AND BindCode = '{1}'", model.BindTableName, model.BindCode);
                List<SYS_FileManage> items = db.Sql(sqlA).QueryMany<SYS_FileManage>();

                string sqlB = string.Empty;
                string sqlC = string.Empty;

                if (items.Count <= 0)
                {
                    sqlB = WinFormClientService.GetInsertSQL(model);
                }
                else if (items.Count.Equals(1))
                {
                    sqlB = WinFormClientService.GetUpdateSQL(nameof(SYS_FileManage), new KeyValuePair<string, object>("ID", items[0].ID), new
                    {
                        FileName = model.FileName,
                        FileSuffix = model.FileSuffix,
                        FileAddress = model.FileAddress,
                        MD5Code = model.MD5Code,
                        CreateTime = DateTime.Now
                    });
                }
                else
                {
                    throw new Exception(@"内部数据错误！");
                }

                sqlC = WinFormClientService.GetUpdateSQL(nameof(SYS_DrawingApplication), new KeyValuePair<string, object>("RequestCode", model.BindCode), new
                {
                    RequestStatus = 2
                });

                db.UseTransaction(true);

                int nA = db.Sql(sqlB).Execute();
                int nB = db.Sql(sqlC).Execute();
                bool result = nA > 0 && nB > 0;

                if (result)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }

                return new ResultModel()
                {
                    Result = result
                };
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

    }

    public class SYS_FileManage : ModelBase
    {
        [PrimaryKey]
        [Identity]

        /// <summary>
		/// 主键ID
		/// </summary>
		public long ID { get; set; }

        /// <summary>
        /// 绑定所属表名
        /// </summary>
        public string BindTableName { get; set; }

        /// <summary>
        /// 绑定所属业务ID
        /// </summary>
        public string BindCode { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件分类：根据业务需分类字段
        /// </summary>
        public int? FileType { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileSuffix { get; set; }

        /// <summary>
        /// 文件上传地址
        /// </summary>
        public string FileAddress { get; set; }

        /// <summary>
        /// 文件的唯一码
        /// </summary>
        public string MD5Code { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string CreatePerson { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
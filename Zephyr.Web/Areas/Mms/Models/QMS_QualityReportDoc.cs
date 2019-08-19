using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class QMS_QualityReportDocService : ServiceBase<QMS_QualityReportDoc>
    {
        public QMS_QualityReportDoc getDoc(int id)
        {
            var sql = string.Format("select * from QMS_QualityReportDoc where id={0}", id);
            return db.Sql(sql).QuerySingle<QMS_QualityReportDoc>();
        }

        public ResultModel SetQualityReportDocData(List<QMS_QualityReportDoc> list, string sourceID, int type)
        {
            try
            {
                if (list.Count <= 0)
                {
                    throw new Exception(@"无数据！");
                }

                StringBuilder sb = new StringBuilder();

                switch (type)
                {
                    case 1://新增

                        break;
                    case 2://编辑
                        sb.Append(string.Format(@"UPDATE dbo.QMS_QualityReportDoc SET IsEnable = 0 WHERE SourceID = '{0}';", sourceID));
                        break;
                    default: break;
                }

                foreach (var item in list)
                {
                    sb.Append(WinFormClientService.GetInsertSQL(item));
                }

                db.UseTransaction(true);

                bool result = db.Sql(sb.ToString()).Execute() > 0;

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
                    Result = result,
                    Msg = result ? "成功！" : "写入数据失败！"
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

        public ResultModel UpdateQualityReportDocIsEnable(string tempID, string newID)
        {
            try
            {
                string sql = string.Format(@"UPDATE dbo.QMS_QualityReportDoc SET IsEnable = 1,SourceID = '{0}' WHERE SourceID = '{1}'", newID, tempID);

                bool result = db.Sql(sql).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "成功！" : "失败！"
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

        public dynamic DelPRouteBlueprint(string tempID)
        {
            try
            {
                string sql = string.Format(@"UPDATE dbo.QMS_QualityReportDoc SET IsEnable = 0 WHERE SourceID = '{0}';", tempID);

                bool result = db.Sql(sql).Execute() > 0;

                return new ResultModel()
                {
                    Result = result,
                    Msg = result ? "成功！" : "失败！ "
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

    public class QMS_QualityReportDoc : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string BillCode { get; set; }
        public int? FileType { get; set; }
        public string FileName { get; set; }
        public string FileAddress { get; set; }
        public bool? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string DocName { get; set; }
        public string SourceID { get; set; }
    }
}

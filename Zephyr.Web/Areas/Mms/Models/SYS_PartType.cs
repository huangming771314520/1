using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class SYS_PartTypeService : ServiceBase<SYS_PartType>
    {
        protected override void OnAfterHandleExcel(ref DataTable dtSheet)
        {
            var service = new SYS_PartTypeService();
            foreach (DataRow item in dtSheet.Rows)
            {

                string dName = item["PartTypeCode"].ToString();
                if (dName.Length > 2)
                {
                    item["PPartTypeCode"] = dName.Substring(0, dName.Length - 2);

                    var row = dtSheet.Select("PartTypeCode='" + item["PPartTypeCode"] + "'");
                    if (row.Length > 0)
                    {
                        item["PTypeName"] = row[0]["TypeName"];
                    }

                }


                var PartTypeCode = item["PartTypeCode"].ToString();
                var Query = ParamDelete.Instance().AndWhere("PartTypeCode", PartTypeCode);
                service.Delete(Query);
            }
            base.OnAfterHandleExcel(ref dtSheet);

        }

        public int UserSyn()
        {
            try
            {
                List<SYS_BN_User> mmsUserList = new List<SYS_BN_User>();
                using (var db = Db.Context("Mms"))
                {
                    mmsUserList = db.Sql(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1").QueryMany<SYS_BN_User>();
                }
                using (var db = Db.Context("Sys"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE dbo.sys_user SET IsEnable = 0;\r\n");

                    foreach (var item in mmsUserList)
                    {
                        sb.Append(string.Format("INSERT INTO dbo.sys_user(UserCode,UserName,Password,IsEnable) VALUES ('{0}','{1}','1','1');\r\n", item.UserCode, item.UserName));
                    }

                    try
                    {
                        db.UseTransaction(true);
                        int temp = db.Sql(sb.ToString()).Execute();
                        if (temp > 0)
                        {
                            db.Commit();
                            return 1;
                        }
                        else
                        {
                            db.Rollback();
                            return 0;
                        }
                    }
                    catch
                    {
                        db.Rollback();
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }


            //            using (db.UseTransaction(true))
            //            {
            //                try
            //                {
            //                    string constr = ConfigurationManager.ConnectionStrings["Sys"].ConnectionString;
            //                    string[] strArr = constr.Split('=');
            //                    string databaseName = strArr[strArr.Length - 1];
            //                    //string sql = @"SELECT usercode,MAX(UserName) UserName FROM dbo.SYS_BN_User GROUP by usercode HAVING COUNT(usercode)>1";
            //                    //var user = db.Sql(sql).QuerySingle<dynamic>();
            //                    //if (user==null)
            //                    //{
            //                    //    return 0;
            //                    //}
            //                    string sql = string.Format(@" select * from SYS_BN_User 
            //delete {0}.[dbo].[sys_user] --where userCode not in (select userCode from SYS_BN_User)", databaseName);
            //                    int rss = db.Sql(sql).Execute();
            //                    sql = string.Format(@"insert INTO {0}.[dbo].[sys_user](UserCode,UserName,Password,isEnable)
            //  select UserCode,UserName,1 as Password,1 as isEnable 
            //  from SYS_BN_User --where userCode not in (select userCode from [HBHC_Sys].[dbo].[sys_user]) 
            //  ", databaseName);
            //                    rss = db.Sql(sql).Execute();
            //                    db.Commit();
            //                    return 1;
            //                }
            //                catch (Exception ex)
            //                {
            //                    db.Rollback();
            //                    return 0;
            //                }
            //                //var data = Db.Context("Sys");

            //            }

        }
        public int DepartSyn()
        {
            try
            {
                List<SYS_BN_Department> mmsDepartmentList = new List<SYS_BN_Department>();
                using (var db = Db.Context("Mms"))
                {
                    mmsDepartmentList = db.Sql(@"SELECT * FROM dbo.SYS_BN_Department WHERE IsEnable = 1").QueryMany<SYS_BN_Department>();
                }
                using (var db = Db.Context("Sys"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("DELETE dbo.sys_organize;\r\n");

                    foreach (var item in mmsDepartmentList)
                    {
                        sb.Append(string.Format("INSERT INTO dbo.sys_organize(OrganizeCode,ParentCode,OrganizeName) VALUES ('{0}','{1}','{2}');\r\n", item.DepartmentCode, item.ParentCode, item.DepartmentName));
                    }

                    try
                    {
                        db.UseTransaction(true);
                        int temp = db.Sql(sb.ToString()).Execute();
                        if (temp > 0)
                        {
                            db.Commit();
                            return 1;
                        }
                        else
                        {
                            db.Rollback();
                            return 0;
                        }
                    }
                    catch
                    {
                        db.Rollback();
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }


            //           db.UseTransaction(true);
            //           string sql = @"delete [HBHC_Sys].[dbo].sys_organize --where OrganizeCode not in (select DepartmentCode from SYS_BN_Department) 
            //insert INTO [HBHC_Sys].[dbo].sys_organize(OrganizeCode,ParentCode,OrganizeName)
            // select DepartmentCode,ParentCode,DepartmentName 
            // from SYS_BN_Department --where DepartmentCode not in (select OrganizeCode from [HBHC_Sys].[dbo].sys_organize) 
            //";
            //           int rss = db.Sql(sql).Execute();
            //           if (rss <= 0)
            //           {
            //               db.Rollback();
            //           }
            //           else
            //           {
            //               db.Commit();
            //           }
            //           return rss;
        }
        public int OrganizeMapSyn()
        {
            try
            {
                List<SYS_BN_User> mmsUserList = new List<SYS_BN_User>();
                using (var db = Db.Context("Mms"))
                {
                    mmsUserList = db.Sql(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1").QueryMany<SYS_BN_User>();
                }
                using (var db = Db.Context("Sys"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("DELETE dbo.sys_userOrganizeMap;\r\n");

                    foreach (var item in mmsUserList)
                    {
                        sb.Append(string.Format("INSERT INTO dbo.sys_userOrganizeMap(OrganizeCode,UserCode) VALUES ('{0}','{1}');\r\n", item.DepartmentCode, item.UserCode));
                    }

                    try
                    {
                        db.UseTransaction(true);
                        int temp = db.Sql(sb.ToString()).Execute();
                        if (temp > 0)
                        {
                            db.Commit();
                            return 1;
                        }
                        else
                        {
                            db.Rollback();
                            return 0;
                        }
                    }
                    catch
                    {
                        db.Rollback();
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }


            //            db.UseTransaction(true);
            //            string sql = @"
            // Delete HBHC_Sys.dbo.sys_userOrganizeMap 
            //  insert into HBHC_Sys.dbo.sys_userOrganizeMap 
            //select DepartmentCode,UserCode from HBHC_Mes.dbo.SYS_BN_User";
            //            int rss = db.Sql(sql).Execute();
            //            if (rss <= 0)
            //            {
            //                db.Rollback();
            //            }
            //            else
            //            {
            //                db.Commit();
            //            }
            //            return rss;
        }

        //**************************************************  华丽的分割线  **************************************************//

        //新同步方法

        public dynamic NewDeptSyn()
        {
            try
            {
                List<SYS_BN_Department> mmsDepartmentList = new List<SYS_BN_Department>();
                using (var dbA = Db.Context("Mms"))
                {
                    mmsDepartmentList = dbA.Sql(@"SELECT * FROM dbo.SYS_BN_Department WHERE IsEnable = 1").QueryMany<SYS_BN_Department>();
                }

                using (var dbB = Db.Context("Sys"))
                {
                    List<string> deptCodeList = dbB.Sql(@"SELECT OrganizeCode FROM dbo.sys_organize").QueryMany<string>();

                    StringBuilder sb = new StringBuilder();

                    foreach (var item in mmsDepartmentList)
                    {
                        if (deptCodeList.Contains(item.DepartmentCode))
                        {

                        }
                        else
                        {
                            sb.Append(string.Format("INSERT INTO dbo.sys_organize(OrganizeCode,ParentCode,OrganizeName) VALUES ('{0}','{1}','{2}');\r\n", item.DepartmentCode, item.ParentCode, item.DepartmentName));
                        }
                    }

                    var sql = sb.ToString();

                    if (string.IsNullOrEmpty(sql))
                    {
                        return new ResultModel()
                        {
                            Result = true,
                            Msg = @"数据已是最新，无需同步！"
                        };
                    }

                    dbB.UseTransaction(true);

                    try
                    {
                        int temp = dbB.Sql(sql).Execute();

                        if (temp > 0)
                        {
                            dbB.Commit();
                            return new ResultModel()
                            {
                                Result = true,
                                Msg = @"同步成功！"
                            };
                        }
                        else
                        {
                            dbB.Rollback();
                            return new ResultModel()
                            {
                                Result = true,
                                Msg = @"同步失败！"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbB.Rollback();
                        return new ResultModel()
                        {
                            Result = false,
                            Msg = ex.Message
                        };
                    }
                }
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

        public dynamic NewUserSyn()
        {
            try
            {
                List<SYS_BN_User> mmsUserList = new List<SYS_BN_User>();
                using (var dbA = Db.Context("Mms"))
                {
                    mmsUserList = dbA.Sql(@"SELECT * FROM dbo.SYS_BN_User WHERE IsEnable = 1").QueryMany<SYS_BN_User>();
                }

                using (var dbB = Db.Context("Sys"))
                {
                    List<string> userCodeList = dbB.Sql(@"SELECT UserCode FROM dbo.sys_user").QueryMany<string>();

                    StringBuilder sb = new StringBuilder();

                    #region 同步用户表

                    sb.Append("UPDATE dbo.sys_user SET IsEnable = 0;\r\n");

                    foreach (var item in mmsUserList)
                    {
                        if (userCodeList.Contains(item.UserCode))
                        {
                            sb.Append(string.Format("UPDATE dbo.sys_user SET IsEnable = 1,UserName = '{0}' WHERE UserCode = '{1}';\r\n", item.UserName, item.UserCode));
                        }
                        else
                        {
                            sb.Append(string.Format("INSERT INTO dbo.sys_user(UserCode,UserName,Password,IsEnable) VALUES ('{0}','{1}','1','1');\r\n", item.UserCode, item.UserName));
                        }
                    }

                    #endregion

                    #region 同步组织架构

                    sb.Append("DELETE dbo.sys_userOrganizeMap;\r\n");

                    foreach (var item in mmsUserList)
                    {
                        sb.Append(string.Format("INSERT INTO dbo.sys_userOrganizeMap(OrganizeCode,UserCode) VALUES ('{0}','{1}');\r\n", item.DepartmentCode, item.UserCode));
                    }

                    #endregion

                    var sql = sb.ToString();

                    dbB.UseTransaction(true);

                    try
                    {
                        int temp = dbB.Sql(sql).Execute();

                        if (temp > 0)
                        {
                            dbB.Commit();
                            return new ResultModel()
                            {
                                Result = true,
                                Msg = @"同步成功！"
                            };
                        }
                        else
                        {
                            dbB.Rollback();
                            return new ResultModel()
                            {
                                Result = true,
                                Msg = @"同步失败！"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbB.Rollback();
                        return new ResultModel()
                        {
                            Result = false,
                            Msg = ex.Message
                        };
                    }
                }
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

    public class SYS_PartType : ModelBase
    {
        [Identity]
        [PrimaryKey]
        public int ID { get; set; }
        public string PartTypeCode { get; set; }
        public string TypeName { get; set; }
        public string PPartTypeCode { get; set; }
        public string PTypeName { get; set; }
        public int? IsEnable { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ModifyPerson { get; set; }
        public DateTime? ModifyTime { get; set; }
    }
}


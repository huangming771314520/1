using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;
using Zephyr.Areas.Areas.Mms.Common;

namespace Zephyr.Models
{
    public class sys_userRoleMapService : ServiceBase<sys_userRoleMap>
    {
        public int GetISExistByUsercodeAndRole(string userCode,string userRole)
        {

            int ret = 0;
            var db = Db.Context("Sys");
            using (db)
            {
                string sql = string.Format("select COUNT(1) from sys_userRoleMap where UserCode='{0}' and RoleCode='{1}'", userCode, userRole);
                ret = db.Sql(sql).QuerySingle<int>();
            }
            return ret;
        }
        //根据登录ID 获取角色ID
        public string GetRoleCode(string UserCode)
        { 
        using(var db=Db.Context("Sys"))
        {
            string rolecode = db.Sql(@" select top 1 RoleCode from sys_userRoleMap where UserCode=@0 order by ID desc").Parameters(UserCode).QuerySingle<string>();
            return rolecode;
        }
        }
    }
    
    public class sys_userRoleMap : ModelBase
    {

        [Identity]
        [PrimaryKey]
        public int ID
        {
            get;
            set;
        }

        public string UserCode
        {
            get;
            set;
        }

        public string RoleCode
        {
            get;
            set;
        }

    }
}

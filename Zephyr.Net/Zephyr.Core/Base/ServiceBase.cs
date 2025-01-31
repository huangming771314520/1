﻿/*************************************************************************
 * 文件名称 ：ServiceBase.cs                          
 * 描述说明 ：定义数据服务基类
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2012-11-10 (施睿智)
 * 修订信息 : 
 **************************************************************************/

using System.Dynamic;
using Zephyr.Data;

namespace Zephyr.Core
{
    public class ServiceBase : ServiceBase<ModelBase>
    {
        public ServiceBase(string Module)
            : base(Module)
        {

        }
    }

    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        public ServiceBase()
        {
            ModuleName = AttributeHelper.GetModuleAttribute(this.GetType());
            Msg = new AjaxMessge();
        }

        public ServiceBase(string moduleName)
        {
            ModuleName = moduleName;
            Msg = new AjaxMessge();
        }

        ~ServiceBase()
        {
            try
            {
                db.Dispose();
            }
            catch
            {
            }
        }

        public static ServiceBase<T> Instance()
        {
            return new ServiceBase<T>();
        }

        public int StoredProcedure(ParamSP param)
        {
            var result = 0;
            Logger("执存储过程", () => result = BuilderParse(param).Execute());
            return result;
        }

        public dynamic ScrollKeys(string key, string value, ParamQuery where = null)
        {
            dynamic result = new ExpandoObject();
            Logger("获取上一条下一条数据", () =>
            {
                result.current = value;
                var pFirst = ParamQuery.Instance().Select("top 1 " + key).OrderBy(key);
                var pPrevious = ParamQuery.Instance().Select("top 1 " + key).AndWhere(key, value, Cp.Less).OrderBy(key + " desc");
                var pNext = ParamQuery.Instance().Select("top 1 " + key).AndWhere(key, value, Cp.Greater).OrderBy(key);
                var pLast = ParamQuery.Instance().Select("top 1 " + key).OrderBy(key + " desc");

                if (where != null)
                {
                    foreach (var item in where.GetData().Where)
                    {
                        pFirst.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pPrevious.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pNext.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pLast.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                    }
                }

                result.first = GetField<string>(pFirst) ?? value;
                result.previous = GetField<string>(pPrevious) ?? value;
                result.next = GetField<string>(pNext) ?? value;
                result.last = GetField<string>(pLast) ?? value;

                result.previousEnable = !object.Equals(result.previous, result.current);
                result.nextEnable = !object.Equals(result.next, result.current);
                result.firstEnable = result.previousEnable && !object.Equals(result.first, result.current);
                result.lastEnable = result.nextEnable && !object.Equals(result.last, result.current);
            });
            return result;
        }

        public string GetNewKey(string field, string rule, int qty = 1, ParamQuery pQuery = null)
        {
            var result = string.Empty;

            Logger("获取新主键", () =>
            {
                for (var i = 0; i < qty; i++)
                {
                    string newkey, table = typeof(T).Name; ;
                    switch (rule)
                    {
                        case "guid":
                            newkey = NewKey.guid();
                            break;
                        case "datetime":
                            newkey = NewKey.datetime();
                            break;
                        case "dateplus":
                            newkey = NewKey.dateplus(db, table, field, "yyyyMMdd", 4);
                            break;
                        case "maxplus":
                        default:
                            newkey = NewKey.maxplus(db, table, field, pQuery);
                            break;
                        case "inventory":
                            newkey = "GJPD" + NewKey.inventory(db, table, field, pQuery);
                            break;
                    }

                    result += "," + newkey;
                }
            });

            return result.Trim(',');
        }
        public string GetAudit1(string whereSql)
        {
            string tableName = typeof(T).Name;

            string sql = "select top 1 ApproveState from " + tableName + " where " + whereSql;
            string state = db.Sql(sql).QuerySingle<string>();
            if (state == "1")
            {
                return "1";
            }
            else
            {
                sql = "update " + tableName + " set ApproveState=1 where " + whereSql;
                int res = db.Sql(sql).Execute();
                if (res > 0)
                {
                    return string.Empty;
                }
                else
                    return "2";
            }
        }
        public string AuditSearchEditBillState(string whereSql)
        {
            string tableName = typeof(T).Name;

            string sql = "select top 1 BillState from " + tableName + " where " + whereSql;
            string state = db.Sql(sql).QuerySingle<string>();
            if (state == "2")
            {
                return "1";
            }
            else
            {
                sql = "update " + tableName + " set BillState=2 where " + whereSql;
                int res = db.Sql(sql).Execute();
                if (res > 0)
                {
                    return string.Empty;
                }
                else
                    return "2";
            }
        }

        public int GetAud( string id)
        {
            string tableName = typeof(T).Name;
            string sql = "select BillState from " + tableName + " where id=" + id;
            return db.Sql(sql).QuerySingle<int>();

        }
        public int GetDelete(string id)
        {
            string tableName = typeof(T).Name;
            string sql = "update " + tableName + " set IsEnable=0 where id=" + id;
            return  db.Sql(sql).Execute();

        }
        #region 变量
        private IDbContext _db;
        protected IDbContext db
        {
            get
            {
                if (_db == null)
                    _db = Db.Context(ModuleName);

                return _db;
            }
        }

        public AjaxMessge Msg { get; set; }
        public string ModuleName { get; set; }

        #endregion
    }
}

/*************************************************************************
 * 文件名称 ：ServiceBaseUtils.cs                          
 * 描述说明 ：定义数据服务基类中的工具类
 * 
 * 创建信息 : create by shiruizhi@qq.com on 2012-11-10 (施睿智)
 * 修订信息 : 
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Zephyr.Data;

namespace Zephyr.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        private static Dictionary<string, object> GetPersonDateForCreate()
        {
            var user = FormsAuth.GetUserData().UserName;
            var dict = new Dictionary<string, object>
                {
                    {APP.FIELD_UPDATE_PERSON, user},
                    {APP.FIELD_UPDATE_DATE, DateTime.Now},
                    {APP.FIELD_CREATE_PERSON, user},
                    {APP.FIELD_CREATE_DATE, DateTime.Now},
                    {"UpdatePerson",user},
                    {"UpdateDate",DateTime.Now},
                    {"CreateDate",DateTime.Now}
                };
            return dict;
        }

        private static Dictionary<string, object> GetPersonDateForUpdate()
        {
            var user = FormsAuth.GetUserData().UserName;
            var dict = new Dictionary<string, object>
                {
                    {APP.FIELD_UPDATE_PERSON, user},
                    {APP.FIELD_UPDATE_DATE, DateTime.Now}
                };
            return dict;
        }

        protected ISelectBuilder<T> BuilderParse(ParamQuery param)
        {
            if (param == null)
            {
                param = new ParamQuery();
            }

            var data = param.GetData();
            var unionStr = data.Union;
            if (unionStr.Length > 0)
            {
                var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
                var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                    .From(sFrom)
                    .Where(data.WhereSql)
                    .GroupBy(data.GroupBy)
                    .Having(data.Having)
                    .OrderBy(data.OrderBy)
                    .Paging(data.PagingCurrentPage, data.PagingItemsPerPage)
                    .Union(data.Union);
                return selectBuilder;
            }
            else
            {
                var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
                var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                    .From(sFrom)
                    .Where(data.WhereSql)
                    .GroupBy(data.GroupBy)
                    .Having(data.Having)
                    .OrderBy(data.OrderBy)
                    .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
                return selectBuilder;
            }

        }

        //protected IInsertBuilder BuilderParse(ParamInsert param)
        //{
        //    var data = param.GetData();
        //    var insertBuilder = db.Insert(data.From.Length == 0 ? typeof(T).Name : data.From);

        //    var dict = GetPersonDateForCreate();

        //    foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
        //        insertBuilder.Column(column.Key, column.Value);

        //    var properties = Zephyr.Utils.ZReflection.GetProperties(typeof(T));
        //    foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
        //        insertBuilder.Column(item.Key, item.Value);

        //    return insertBuilder;
        //}

        /// <summary>
        /// 拓展和修正该方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected IInsertBuilder BuilderParse(ParamInsert param)
        {
            var data = param.GetData();
            var FromTable = data.From.Length == 0 ? typeof(T).Name : data.From.Replace("\r\n", "").Trim();
            var insertBuilder = db.Insert(FromTable);

            var dict = GetPersonDateForCreate();

            //获取程序集 反射出程序集下特定Model类
            System.Reflection.Assembly t = System.Reflection.Assembly.Load("Zephyr.Web");
            Type TypeModel = t.GetType("Zephyr.Models." + FromTable);
            var properties = Zephyr.Utils.ZReflection.GetProperties(TypeModel);//获取所有属性

            //定义实体类上自增标识列集合
            List<string> IdentityFieldList = new List<string>();
            //取属性上的自定义特性
            foreach (var propInfo in properties)
            {
                object[] attributes = propInfo.Value.GetCustomAttributes(typeof(IdentityAttribute), false);
                if (attributes.Length > 0)
                {
                    IdentityFieldList.Add(propInfo.Key);
                }
            }

            //查询出不包含 CreatePerson,CreateDate,UpdatePerson,UpdateDate 和 不包含 IdentityFieldList自增标识列 的集合
            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key) && !IdentityFieldList.Contains(column.Key)))
                insertBuilder.Column(column.Key, column.Value);

            //查询出包含 CreatePerson,CreateDate,UpdatePerson,UpdateDate 并给予这些字段赋值
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key)))
                insertBuilder.Column(item.Key, item.Value);

            return insertBuilder;
        }

        protected IUpdateBuilder BuilderParse(ParamUpdate param)
        {
            var data = param.GetData();
            var FromTable = data.Update.Length == 0 ? typeof(T).Name : data.Update.Replace("\r\n", "").Trim();
            var updateBuilder = db.Update(FromTable);

            var dict = GetPersonDateForUpdate();
            //查询出不包含 CreatePerson,CreateDate,UpdatePerson,UpdateDate 的字段并赋值
            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
                updateBuilder.Column(column.Key, column.Value);

            //
            //获取程序集 反射出程序集下特定Model类
            System.Reflection.Assembly t = System.Reflection.Assembly.Load("Zephyr.Web");
            Type TypeModel = t.GetType("Zephyr.Models." + FromTable);
            var properties = Zephyr.Utils.ZReflection.GetProperties(TypeModel);//获取所有属性
            //
            //查询出包含 CreatePerson,CreateDate,UpdatePerson,UpdateDate 并给予这些字段赋值
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key)))
                updateBuilder.Column(item.Key, item.Value);

            updateBuilder.Where(data.WhereSql);

            return updateBuilder;
        }

        //protected IUpdateBuilder BuilderParse(ParamUpdate param)
        //{
        //    var data = param.GetData();
        //    var updateBuilder = db.Update(data.Update.Length == 0 ? typeof(T).Name : data.Update);

        //    var dict = GetPersonDateForUpdate();
        //    foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
        //        updateBuilder.Column(column.Key, column.Value);

        //    var properties = Zephyr.Utils.ZReflection.GetProperties(typeof(T));
        //    foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
        //        updateBuilder.Column(item.Key, item.Value);

        //    updateBuilder.Where(data.WhereSql);

        //    return updateBuilder;
        //}

        protected IDeleteBuilder BuilderParse(ParamDelete param)
        {
            var data = param.GetData();
            var deleteBuilder = db.Delete(data.From.Length == 0 ? typeof(T).Name : data.From);
            deleteBuilder.Where(data.WhereSql);

            return deleteBuilder;
        }

        protected IStoredProcedureBuilder BuilderParse(ParamSP param)
        {
            var data = param.GetData();
            var spBuilder = db.StoredProcedure(data.Name);
            foreach (var item in data.Parameter)
                spBuilder.Parameter(item.Key, item.Value);

            foreach (var item in data.ParameterOut)
                spBuilder.ParameterOut(item.Key, item.Value);

            return spBuilder;
        }

        protected int queryRowCount(ParamQuery param, dynamic rows)
        {
            if (rows != null)
                if (null == param || param.GetData().PagingItemsPerPage == 0)
                    return rows.Count;

            var RowCountParam = param;
            var sql = BuilderParse(RowCountParam.Paging(1, 0).OrderBy(string.Empty)).GetSql();
            return db.Sql(@"select count(*) from ( " + sql + " ) tb_temp").QuerySingle<int>();
        }
    }
}

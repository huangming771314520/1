using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;
using System.ComponentModel.DataAnnotations;

namespace Zephyr.Models
{
    public class sys_codeService : ServiceBase<sys_code>
    {
        public sys_code Getsys_codeByTypeAndID(string type, int ID)
        {
            var pQuery = ParamQuery.Instance()
                .Select("*")
                .AndWhere("CodeType", type)
                .AndWhere("Value", ID);

            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
                return new sys_code();
        }
        public string Getsys_codeByTypeAndtext(string type, string text)
        {
            var pQuery = ParamQuery.Instance()
                .Select("*")
                .AndWhere("CodeType", type)
                .AndWhere("Text", text);

            var list = base.GetModelList(pQuery);
            if (list.Count > 0)
            {
                return list[0].Value;
            }
            else
                return "";
        }
        public List<dynamic> GetValueTextListByType(string codeType)
        {
            var pQuery = ParamQuery.Instance()
                .Select("value,text")
                .AndWhere("CodeType", codeType);

            return base.GetDynamicList(pQuery);
        }
        public List<dynamic> GetAllValueTextList()
        {
            var pQuery = ParamQuery.Instance()
                .Select("Value as value,Text as text,CodeType as codetype");
            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetAllValueTextList(string codeTypeList)
        {
            var pQuery = ParamQuery.Instance().AndWhere("CodeType", codeTypeList, Cp.In)
                .Select("Value as value,Text as text,CodeType as codetype");
            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetCommonValueTextList(string select = "", string from = "")
        {
            var pQuery = ParamQuery.Instance()
               .Select(select)
               .From(from);
            return base.GetDynamicList(pQuery);
        }

        #region 张欢欢 添加
        /// <summary>
        /// 获取定值下拉框
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public List<dynamic> GetValueTextListByType_Xttcqw(string codeType)
        {
            var pQuery = ParamQuery.Instance()
                .Select("Value as value,Text as text")
                .AndWhere("CodeType", codeType);

            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetValueTextListByType_Xttcqw(string tableName, string sql)
        {
            var pQuery = ParamQuery.Instance()
                .From(tableName)
                .Select(sql);
            return base.GetDynamicList(pQuery);
        }


        public List<dynamic> GetTableValueTextList_Xttcqw(string tableName, string valueColumns, string TextColumns)
        {
            var pQuery = ParamQuery.Instance()
                .From(tableName)
                .Select(" distinct " + valueColumns + " as value ," + TextColumns + " as text");
            return base.GetDynamicList(pQuery);
        }
        #endregion

        public List<dynamic> GetValueTextListByType_lwx(string codeType)
        {
            var pQuery = ParamQuery.Instance()
                .Select("Value as value,Text as text")
                .AndWhere("CodeType", codeType);

            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetMeasureUnitListByType()
        {
            var pQuery = ParamQuery.Instance()
                .Select("Text as value,Text as text")
                .AndWhere("CodeType", "MeasureUnit");

            return base.GetDynamicList(pQuery);
        }

        public List<dynamic> GetYearItems()
        {
            var result = new List<dynamic>();
            var startYear = DateTime.Now.Year - 10;
            for (var y = startYear; y < startYear + 20; y++)
                result.Add(new { value = y, text = y });

            return result;
        }

        public List<dynamic> GetMonthItems()
        {
            var result = new List<dynamic>();
            var startMonth = 1;
            for (var m = startMonth; m < startMonth + 12; m++)
                result.Add(new { value = m, text = m });

            return result;
        }
        public List<dynamic> GetYearMonthItems()
        {
            var result = new List<dynamic>();
            var startMonth = 1;
            var currentYear = DateTime.Now.Year.ToString();
            string _value = "";
            for (var m = startMonth; m < startMonth + 12; m++)
            {
                _value = currentYear + "-" + (m >= 10 ? m.ToString() : "0" + m.ToString());

                result.Add(new { value = _value, text = _value });
            }
            currentYear = DateTime.Now.AddYears(1).Year.ToString();
            for (var m = startMonth; m < startMonth + 12; m++)
            {
                _value = currentYear + "-" + (m >= 10 ? m.ToString() : "0" + m.ToString());

                result.Add(new { value = _value, text = _value });
            }

            return result;
        }
        public string GetDefaultCode(string sType)
        {
            var pQuery = ParamQuery.Instance().Select("top 1 Code")
                .AndWhere("CodeType", sType)
                .AndWhere("IsEnable", true)
                .AndWhere("IsDefault", true);

            return base.GetField<string>(pQuery);
        }
    }

    public class sys_code : ModelBase
    {

        [PrimaryKey]
        public string Code { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        public string ParentCode { get; set; }

        public string Seq { get; set; }

        public bool? IsEnable { get; set; }

        public bool? IsDefault { get; set; }

        public string Description { get; set; }

        public string CodeTypeName { get; set; }

        public string CodeType { get; set; }

        public string CreatePerson { get; set; }

        public string CreateDate { get; set; }

        public string UpdatePerson { get; set; }

        public string UpdateDate { get; set; }



    }
}

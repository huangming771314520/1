using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Zephyr.Areas.CommonWrap;
using Zephyr.Models;
using Zephyr.Utils;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString GetSearchToolbar(this HtmlHelper htmlHelper, string xmlName)
        {
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").Where(f => f.Attribute("searchType") != null).ToList();
            int coulmnIndex = 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class='container_16' style='background-color: #E0ECFF; border-bottom: 1px solid #8DB2E3;'>");
            foreach (var item in fieldList)
            {
                string searchType = item.Attribute("searchType").Value;
                string title = item.Attribute("title").Value;
                string field = item.Attribute("field").Value;
                if (searchType == "text")
                {
                    sb.AppendFormat("<div class='grid_2 lbl'>{0}</div>", title);
                    sb.AppendFormat("<div class='grid_4 val'><input type='text' data-bind='{0}' class='z-txt'/></div>", "value:form." + field);
                }
                if (searchType == "select")
                {
                    string type = item.Element("searchBinding").Attribute("type").Value;
                    if (type == "tree")
                    {
                        string tree = item.Element("searchBinding").Value;
                        sb.AppendFormat("<div class='grid_2 lbl'>{0}</div>", title);
                        sb.AppendFormat("<div class='grid_4 val'><input type='text' data-bind='{0}' {1} class='z-txt easyui-combotree'/></div>", "combotreeValue:form." + field, tree);
                    }
                    else if (type == "single" || type == "dictionary" || type == "fix")
                    {
                        string selectJsonData = CommonSearchDialog.GetSelectList(type, item.Element("searchBinding"));
                        sb.AppendFormat("<div class='grid_2 lbl'>{0}</div>", title);
                        sb.AppendFormat("<div class='grid_4 val'><input type='text' data-bind='{0}' class='z-txt easyui-combobox'/></div>", "comboboxValue:form." + field + ",datasource:" + selectJsonData);
                    }
                }
                if (searchType == "daterange")
                {
                    sb.AppendFormat("<div class='grid_2 lbl'>{0}</div>", title);
                    sb.AppendFormat("<div class='grid_4 val'><input type='text' data-bind='{0}' class='z-txt easyui-daterange'/></div>", "value:form." + field);
                }
                if (coulmnIndex % 2 == 0 && coulmnIndex != fieldList.Count)
                {
                    sb.AppendFormat("<div class='clear'></div>");
                }
                coulmnIndex++;
            }
            sb.AppendFormat("<div class='grid_4 val' style='float:right;'>");
            sb.AppendFormat("<a href='#' plain='true' class='easyui-linkbutton' icon='icon-search' title='查询' data-bind='click:searchClick'>查询</a>");
            sb.AppendFormat("<a href='#' plain='true' class='easyui-linkbutton' icon='icon-clear' title='清空' data-bind='click:clearClick'>清空</a>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div class='clear'></div></div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetDataGridColumns(this HtmlHelper htmlHelper, string xmlName)
        {
            XElement targetXml = CommonSearchDialog.GetSearchXml(xmlName);
            List<XElement> fieldList = targetXml.Element("ColumnList").Elements("column").ToList();
            List<Dictionary<string, object>> columsList = new List<Dictionary<string, object>>();
            Dictionary<string, string> form = new Dictionary<string, string>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<table id='CommonDialogForm' data-bind='datagrid:gridSetting'>");
            sb.AppendFormat("<thead><tr>");
            foreach (var item in fieldList)
            {
                string title = ReadXml.getXmlElementAttr(item, "title");
                string field = ReadXml.getXmlElementAttr(item, "field");
                string hidden = ReadXml.getXmlElementAttr(item, "hidden");
                string align = ReadXml.getXmlElementAttr(item, "align");
                string sortable = ReadXml.getXmlElementAttr(item, "sortable");
                string width = ReadXml.getXmlElementAttr(item, "width");
                string formatter = ReadXml.getXmlElementAttr(item, "formatter");
                string checkbox = ReadXml.getXmlElementAttr(item, "checkbox");
                string gridFormat = "";
                string gridEditor = ReadXml.getXmlElementValue(item, "gridEditor");
                if (formatter != "")
                {
                    if (formatter == "gridFormat")
                    {
                        gridFormat = ReadXml.getXmlElementValue(item, "gridFormat");
                        formatter = "";
                    }
                    else
                    {
                        formatter = string.Format("formatter='{0}'", formatter);
                    }
                }
                if (hidden != "")
                {
                    hidden = string.Format("hidden='{0}'", hidden);
                }
                if (sortable != "")
                {
                    sortable = string.Format("sortable='{0}'", sortable);
                }
                if (checkbox != "")
                {
                    checkbox = string.Format("checkbox='{0}'", checkbox);
                }
                if (!string.IsNullOrWhiteSpace(gridEditor))
                {
                    gridEditor = string.Format("editor=\"{0}\"", gridEditor);
                }
                sb.AppendFormat("<th field='{0}' {1} align='{2}' width='{3}' {4} {6} {7} {8} {9}>{5}</th>", field, sortable, align, width, formatter, title, checkbox, gridFormat, hidden, gridEditor);
            }
            sb.AppendFormat("</tr></thead></table>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString RenderToolbar(this HtmlHelper htmlHelper)
        {
            var buttons = new sys_menuService().GetCurrentUserMenuButtons_extend();
            var toolbar = new TagBuilder("div");
            toolbar.AddCssClass("z-toolbar");
            var addition = string.Empty;
            foreach (var btn in buttons)
            {
                var link = new TagBuilder("a");
                link.MergeAttribute("href", "#");
                if (btn.ButtonCode.Equals("download"))
                {
                    link.MergeAttribute("class", "easyui-splitbutton");
                    link.MergeAttribute("data-options", "menu:'#dropdown',iconCls:'icon-download'");
                    addition += DropDownDiv();
                }
                else
                {
                    link.MergeAttribute("id", "a_" + btn.ButtonCode);
                    link.MergeAttribute("plain", "true");
                    link.MergeAttribute("class", "easyui-linkbutton");
                    link.MergeAttribute("icon", btn.ButtonIcon);
                    link.MergeAttribute("title", btn.ButtonName);
                    link.MergeAttribute("data-bind", btn.ButtonClickCode);
                }
                link.SetInnerText(btn.ButtonName);
                toolbar.InnerHtml += link.ToString();
            }
            return new MvcHtmlString(toolbar.ToString() + addition);
        }

        public static MvcHtmlString HideColumn(this HtmlHelper htmlHelper, List<sys_roleMenuColumnMap> cols, string field)
        {
            var result = string.Empty;
            var fields = cols.Where(x => x.FieldName == field);
            if (fields.Count() > 0 && fields.First().IsReject == true)
                result = "hidden=true";

            return new MvcHtmlString(result);
        }

        private static string DropDownDiv()
        {
            var div = new TagBuilder("div");
            div.MergeAttribute("id", "dropdown");
            div.MergeAttribute("style", "width:100px; display:none;");

            var div1 = new TagBuilder("div");
            div1.MergeAttribute("suffix", "xls");
            div1.MergeAttribute("data-options", "iconCls:'icon-ext-xls'");
            div1.MergeAttribute("data-bind", "click:downloadClick");
            div1.SetInnerText("Excel2003");
            div.InnerHtml += div1.ToString();

            var div2 = new TagBuilder("div");
            div2.MergeAttribute("suffix", "xlsx");
            div2.MergeAttribute("data-options", "iconCls:'icon-page_excel'");
            div2.MergeAttribute("data-bind", "click:downloadClick");
            div2.SetInnerText("Excel2007");
            div.InnerHtml += div2.ToString();

            var div3 = new TagBuilder("div");
            div3.MergeAttribute("suffix", "doc");
            div3.MergeAttribute("data-options", "iconCls:'icon-ext-doc'");
            div3.MergeAttribute("data-bind", "click:downloadClick");
            div3.SetInnerText("Word2003");
            div.InnerHtml += div3.ToString();

            return div.ToString();
        }
    }
}
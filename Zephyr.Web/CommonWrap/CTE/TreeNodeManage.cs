using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Zephyr.Core;

namespace Zephyr.Areas.CommonWrap
{
    public class TreeNodeModel
    {
        public string ConnName { get; set; }
        public string TableName { get; set; }
        public string NodeField { get; set; }
        public string ParentNodeField { get; set; }
        public string ParentNodeValue { get; set; }
        public bool IsLevel { get; set; }
        public string TreeSetting { get; set; }
        public string WhereSql { get; set; }
        public string OrderBy { get; set; }
        public TreeNodeModel()
        {
            ConnName = "Mms";
            TableName = "";
            NodeField = "";
            ParentNodeField = "";
            ParentNodeValue = "";
            IsLevel = false;
            TreeSetting = "";
            WhereSql = "";
            OrderBy = "";
        }
    }

    public class TreeNodeManage
    {
        public TreeNodeModel TreeNodeModelData;
        public TreeNodeManage()
        {
            TreeNodeModelData = new TreeNodeModel();
        }
        public static TreeNodeManage Instance()
        {
            return new TreeNodeManage();
        }
        public TreeNodeManage SetTableName(string TableName)
        {
            TreeNodeModelData.TableName = TableName;
            return this;
        }
        public TreeNodeManage SetNode(string ColumnName)
        {
            TreeNodeModelData.NodeField = ColumnName;
            return this;
        }
        public TreeNodeManage SetParentNode(string ColumnName, string ColumnValue = "")
        {
            TreeNodeModelData.ParentNodeField = ColumnName;
            TreeNodeModelData.ParentNodeValue = ColumnValue;
            return this;
        }
        public TreeNodeManage SetNodeLevel(bool IsLevel = true)
        {
            TreeNodeModelData.IsLevel = IsLevel;
            return this;
        }
        public TreeNodeManage SetTreeSetting(string TreeSetting = "")
        {
            TreeNodeModelData.TreeSetting = TreeSetting;
            return this;
        }
        public TreeNodeManage SetWhereSql(string WhereSql = "")
        {
            TreeNodeModelData.WhereSql = WhereSql;
            return this;
        }
        public TreeNodeManage SetOrderBy(string OrderBy="")
        {
            TreeNodeModelData.OrderBy = OrderBy;
            return this;
        }
        public TreeNodeModel GetData()
        {
            return TreeNodeModelData;
        }
        public static List<T> GetTreeNodeList<T>(TreeNodeManage Params) where T : new()
        {
            var TreeParams = Params.GetData();

            string ModelName = typeof(T).Name;

            //获取程序集 反射出程序集下特定Model类
            System.Reflection.Assembly t = System.Reflection.Assembly.Load("Zephyr.Web");
            Type TypeModel = t.GetType("Zephyr.Models." + TreeParams.TableName);
            var properties = Zephyr.Utils.ZReflection.GetProperties(TypeModel);//获取所有属性
            var AllFields = properties.Keys.ToList();
            if (ModelName == "Object")//如果用dynamic则需要替换
            {
                AllFields.Remove("ID");
                AllFields.Add("ID as new_id");
            }
            var fields = string.Join(",", AllFields);
            var temp_fields = "temp." + string.Join(",temp.", AllFields);

            var sb_not_self = new StringBuilder();

            string[] levels = new string[3];
            if (TreeParams.IsLevel)
            {
                levels[0] = ",1 as levels";
                levels[1] = ",cte.levels+1";
                levels[2] = ",0 as levels";

                fields += levels[0];
                temp_fields += levels[1];
            }
            //if (!string.IsNullOrWhiteSpace(TreeParams.TreeSetting))
            //{
            //    fields += TreeParams.TreeSetting;
            //    temp_fields += TreeParams.TreeSetting;
            //}
            sb_not_self.AppendFormat(@";with cte as
(
select {4} from {0} where isnull({2},'')='{3}'
union all
select {5} from cte inner join {0} temp on cte.{1}=temp.{2}
)
select {6}* from cte {7}",
            TreeParams.TableName, // 0
            TreeParams.NodeField,  // 1
            TreeParams.ParentNodeField,  // 2
            TreeParams.ParentNodeValue,  // 3
            fields,  // 4
            temp_fields,  // 5
            TreeParams.TreeSetting,  // 6
            TreeParams.WhereSql // 7
            );

            //if (!string.IsNullOrWhiteSpace(TreeParams.ParentNodeValue))
            //{
            //    sb.AppendFormat(@" union select {0}*{1} from {2} where {3}='{4}'", TreeParams.TreeSetting, levels[2], TreeParams.TableName, TreeParams.NodeField, TreeParams.ParentNodeValue);
            //}
            string tree_node_sql = sb_not_self.ToString();

            List<T> list = new List<T>();
            using (var db = Db.Context(TreeParams.ConnName))
            {
                list = db.Sql(tree_node_sql).QueryMany<T>();
                if (!string.IsNullOrWhiteSpace(TreeParams.ParentNodeValue))
                {
                    var sb_self = new StringBuilder();
                    sb_self.AppendFormat(@"select {0} from {1} where {2}='{3}'", TreeParams.TreeSetting + fields + levels[2], TreeParams.TableName, TreeParams.NodeField, TreeParams.ParentNodeValue);
                    string tree_not_self_node_sql = sb_self.ToString();
                    if (list.Count > 0)
                    {
                        list.Add(db.Sql(tree_not_self_node_sql).QueryMany<T>().FirstOrDefault());
                    }
                    else
                    {
                        list = db.Sql(tree_not_self_node_sql).QueryMany<T>();
                    }
                }
            }
            return list;
        }

        public static List<T> GetTreeNodeList_Pro<T>(TreeNodeManage Params) where T : new()
        {
            var TreeParams = Params.GetData();

            string ModelName = typeof(T).Name;

            //获取程序集 反射出程序集下特定Model类
            System.Reflection.Assembly t = System.Reflection.Assembly.Load("Zephyr.Web");
            Type TypeModel = t.GetType("Zephyr.Models." + TreeParams.TableName);
            var properties = Zephyr.Utils.ZReflection.GetProperties(TypeModel);//获取所有属性
            var AllFields = properties.Keys.ToList();
            if (ModelName == "Object")//如果用dynamic则需要替换
            {
                AllFields.Remove("ID");
                AllFields.Add("ID as new_id");
            }
            var fields = string.Join(",", AllFields);
            var temp_fields = "temp." + string.Join(",temp.", AllFields);

            var sb_not_self = new StringBuilder();

            string[] levels = new string[3];
            if (TreeParams.IsLevel)
            {
                levels[0] = ",1 as levels";
                levels[1] = ",cte.levels+1";
                levels[2] = ",0 as levels";

                fields += levels[0];
                temp_fields += levels[1];
            }
            //if (!string.IsNullOrWhiteSpace(TreeParams.TreeSetting))
            //{
            //    fields += TreeParams.TreeSetting;
            //    temp_fields += TreeParams.TreeSetting;
            //}
            sb_not_self.AppendFormat(@";with cte as
(
select {4} from {0} where isnull({2},'')='{3}'
union all
select {5} from cte inner join {0} temp on cte.{1}=temp.{2} {7}
)
select {6}* from cte {8}",
            TreeParams.TableName, // 0
            TreeParams.NodeField,  // 1
            TreeParams.ParentNodeField,  // 2
            TreeParams.ParentNodeValue,  // 3
            fields,  // 4
            temp_fields,  // 5
            TreeParams.TreeSetting,  // 6
            TreeParams.WhereSql, // 7
            TreeParams.OrderBy // 8
            );

            //if (!string.IsNullOrWhiteSpace(TreeParams.ParentNodeValue))
            //{
            //    sb.AppendFormat(@" union select {0}*{1} from {2} where {3}='{4}'", TreeParams.TreeSetting, levels[2], TreeParams.TableName, TreeParams.NodeField, TreeParams.ParentNodeValue);
            //}
            string tree_node_sql = sb_not_self.ToString();

            List<T> list = new List<T>();
            using (var db = Db.Context(TreeParams.ConnName))
            {
                list = db.Sql(tree_node_sql).QueryMany<T>();
                if (!string.IsNullOrWhiteSpace(TreeParams.ParentNodeValue))
                {
                    var sb_self = new StringBuilder();
                    sb_self.AppendFormat(@"select {0} from {1} where {2}='{3}'", TreeParams.TreeSetting + fields + levels[2], TreeParams.TableName, TreeParams.NodeField, TreeParams.ParentNodeValue);
                    string tree_not_self_node_sql = sb_self.ToString();
                    if (list.Count > 0)
                    {
                        list.Add(db.Sql(tree_not_self_node_sql).QueryMany<T>().FirstOrDefault());
                    }
                    else
                    {
                        list = db.Sql(tree_not_self_node_sql).QueryMany<T>();
                    }
                }
            }
            return list;
        }
    }
}
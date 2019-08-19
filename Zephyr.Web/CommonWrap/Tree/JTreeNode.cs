using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Zephyr.Areas.CommonWrap
{
    public class JTreeNode
    { 
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public bool? Checked { get; set; }
        public string iconCls { get; set; }
        public object attributes { get; set; }
        public JTreeNode[] children { get; set; }

        public static string ConvertToJson(JTreeNode node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.AppendFormat("\"id\":\"{0}\",", node.id);

            if (!string.IsNullOrWhiteSpace(node.state))
            {
                sb.AppendFormat("\"state\":\"{0}\",", node.state);
            }
            if (!string.IsNullOrWhiteSpace(node.iconCls))
            {
                sb.AppendFormat("\"iconCls\":\"{0}\",", node.iconCls);
            }
            if (node.Checked != null)
            {
                sb.AppendFormat("\"checked\":\"{0},\"", node.Checked);
            }

            // to append attributes...
            if (node.attributes != null)
            {
                var attributesType = node.attributes.GetType();
                foreach (var item in attributesType.GetProperties())
                {
                    var value = item.GetValue(node.attributes, null);
                    if (value != null)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", item.Name, value);
                    }
                }
            }

            //recursive append children
            if (node.children != null && node.children.Length > 0)
            {
                StringBuilder sbChildren = new StringBuilder();
                foreach (var item in node.children)
                {
                    sbChildren.AppendFormat("{0},", ConvertToJson(item));
                }
                sb.AppendFormat("\"children\":[{0}],", sbChildren.ToString().TrimEnd(','));
            }


            sb.AppendFormat("\"text\":\"{0}\"", node.text);

            sb.Append("}");

            return sb.ToString();
        }

        public static void ConstructJTreeNode(ref JTreeNode node, List<TreeModel> treelist)
        {
            var pid = node.id;
            var children = from i in treelist where i.ParentIdField == pid select i;
            if (children.Count() > 0)
            {
                List<JTreeNode> temp = new List<JTreeNode>();
                foreach (var item in children)
                {
                    var model = new JTreeNode()
                    {
                        id = item.IdField,
                        text = item.TextField,
                        state = "closed"
                    };
                    temp.Add(model);
                }
                node.children = temp.ToArray();
            }
            //递归遍历
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Length; i++)
                {
                    ConstructJTreeNode(ref node.children[i], treelist);
                }
            }
            else
            {
                node.state = "open";
            }
        }
    }
}
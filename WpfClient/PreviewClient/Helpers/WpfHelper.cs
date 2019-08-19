using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PreviewClient.Helpers
{
    public class WpfHelper
    {

        public static string GetRandomColorStr()
        {
            //Random random = new Random(Guid.NewGuid().GetHashCode());
            //switch (random.Next(1, 4))
            //{
            //    case 1: return "#FFDC6141";
            //    case 2: return "#FF5679AE";
            //    case 3: return "#FF367C56";
            //    default: return "#FFFFFFFF";
            //}
            return "#0080FF";
        }

        public static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }

                    List<T> childItems = FindVisualChildren<T>(child);
                    if (childItems != null && childItems.Count() > 0)
                    {
                        foreach (var item in childItems)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }


    }
}

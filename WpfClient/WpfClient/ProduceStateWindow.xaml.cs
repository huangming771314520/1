using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;

namespace WpfClient
{
    /// <summary>
    /// ProduceStateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceStateWindow : Window
    {
        public MessageBoxResult Result = MessageBoxResult.Cancel;
        public int Num = 0;
        private int gridIndex = 2;

        public ProduceStateWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderThickness = new Thickness(2)
            };

            btnCancel.Children.Add(border);

            txtNum.Text = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity == null ? string.Empty : StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity.ToString();
            gridNum.Visibility = Visibility.Hidden;
            pageContainer.Focus();
        }

        private void PageContainer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderThickness = new Thickness(2)
            };
            Grid grid = gridAllState.Children[gridIndex] as Grid;

            switch ((int)e.Key)
            {
                //enter-回车
                case 6:
                    Num = Convert.ToInt32(txtNum.Text);
                    switch (gridIndex)
                    {
                        case 0: Result = MessageBoxResult.Yes; break;
                        case 1: Result = MessageBoxResult.No; break;
                        case 2: Result = MessageBoxResult.Cancel; break;
                        default: break;
                    }
                    Close();
                    break;
                //左
                case 23:
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        if (grid.Children[i] is Border)
                        {
                            grid.Children.Remove(grid.Children[i]);
                        }
                    }
                    gridIndex = (gridIndex - 1) < 0 ? 2 : (gridIndex - 1);
                    (gridAllState.Children[gridIndex] as Grid).Children.Add(border);
                    gridNum.Visibility = gridIndex.Equals(0) ? Visibility.Visible : Visibility.Hidden;
                    break;
                //右
                case 25:
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        if (grid.Children[i] is Border)
                        {
                            grid.Children.Remove(grid.Children[i]);
                        }
                    }
                    gridIndex = (gridIndex + 1) > 2 ? 0 : (gridIndex + 1);
                    (gridAllState.Children[gridIndex] as Grid).Children.Add(border);
                    gridNum.Visibility = gridIndex.Equals(0) ? Visibility.Visible : Visibility.Hidden;
                    break;
                //上
                case 24:
                //+
                case 141:
                    if (gridNum.IsVisible)
                    {
                        txtNum.Text = (Convert.ToInt32(txtNum.Text) + 1).ToString();
                    }
                    break;
                //下
                case 26:
                //-
                case 143:
                    if (gridNum.IsVisible)
                    {
                        txtNum.Text = ((Convert.ToInt32(txtNum.Text) - 1) <= 0 ? 0 : (Convert.ToInt32(txtNum.Text) - 1)).ToString();
                    }
                    break;
                default: break;
            }

            e.Handled = true;
        }

        private void PageContainer_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

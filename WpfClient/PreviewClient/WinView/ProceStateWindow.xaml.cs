using Model;
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

namespace PreviewClient.WinView
{
    /// <summary>
    /// ProceStateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProceStateWindow : Window
    {
        public MessageBoxResult Result = MessageBoxResult.Cancel;
        public double ProduceNum = 0;
        private int gridIndex = 2;

        public ProceStateWindow()
        {
            InitializeComponent();
        }

        private void WindowProceState_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderThickness = new Thickness(2)
            };

            btnCancel.Children.Add(border);

            //nudProduceNum.Value = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity == null ? 0 : Convert.ToDouble(StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity);
            txtNum.Text = StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity == null ? "0" : StoreInfoModel.ProducePlanInfo.GetProjectProduceDetial.Quantity.ToString();
            ProduceNumContainer.Visibility = Visibility.Hidden;
            MainWindow.Focus();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                BorderThickness = new Thickness(2)
            };
            Grid grid = gridAllState.Children[gridIndex] as Grid;

            switch (e.Key)
            {
                //enter-回车
                case Key.Enter:
                    //ProduceNum = Convert.ToDouble(nudProduceNum.Value);
                    ProduceNum = Convert.ToDouble(txtNum.Text);
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
                case Key.Left:
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        if (grid.Children[i] is Border)
                        {
                            grid.Children.Remove(grid.Children[i]);
                        }
                    }
                    gridIndex = (gridIndex - 1) < 0 ? 2 : (gridIndex - 1);
                    (gridAllState.Children[gridIndex] as Grid).Children.Add(border);
                    ProduceNumContainer.Visibility = gridIndex.Equals(0) ? Visibility.Visible : Visibility.Hidden;
                    break;
                //右
                case Key.Right:
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        if (grid.Children[i] is Border)
                        {
                            grid.Children.Remove(grid.Children[i]);
                        }
                    }
                    gridIndex = (gridIndex + 1) > 2 ? 0 : (gridIndex + 1);
                    (gridAllState.Children[gridIndex] as Grid).Children.Add(border);
                    ProduceNumContainer.Visibility = gridIndex.Equals(0) ? Visibility.Visible : Visibility.Hidden;
                    break;
                //上
                case Key.Up:
                //+
                case Key.OemPlus:
                    if (ProduceNumContainer.IsVisible)
                    {
                        //nudProduceNum.Value = Convert.ToDouble(nudProduceNum.Value) + 1;
                        txtNum.Text = (Convert.ToDouble(txtNum.Text) + 1).ToString();
                    }
                    break;
                //下
                case Key.Down:
                //-
                case Key.OemMinus:
                    if (ProduceNumContainer.IsVisible)
                    {
                        //nudProduceNum.Value = (Convert.ToDouble(nudProduceNum.Value) - 1) <= 0 ? 0 : (Convert.ToDouble(nudProduceNum.Value) - 1);
                        txtNum.Text = (Convert.ToDouble(txtNum.Text) - 1) <= 0 ? "0" : (Convert.ToDouble(txtNum.Text) - 1).ToString();
                    }
                    break;
                default: break;
            }

            e.Handled = true;
        }

    }
}

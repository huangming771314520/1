using MesClient.ViewModels;
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

namespace MesClient.Views.WinViews
{
    /// <summary>
    /// WindowPlanExecuteState.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPlanExecuteState : Window
    {
        /// <summary>
        /// 选择状态
        /// </summary>
        public int SelectState { get; set; } = 2;

        /// <summary>
        /// 暂停原因
        /// </summary>
        public int PauseReson { get; set; } = 1;

        /// <summary>
        /// 完工数量
        /// </summary>
        public int SelectNum { get; set; }

        public WindowPlanExecuteState()
        {
            InitializeComponent();

            DataContext = new PlanExecuteStateViewModel(this);
        }

        private void BtnComplete_GotFocus(object sender, RoutedEventArgs e)
        {
            CompleteNumContainer.Visibility = Visibility.Visible;
            PauseResonContainer.Visibility = Visibility.Hidden;
        }

        private void BtnStop_GotFocus(object sender, RoutedEventArgs e)
        {
            CompleteNumContainer.Visibility = Visibility.Hidden;
            PauseResonContainer.Visibility = Visibility.Visible;
        }

        private void BtnCancel_GotFocus(object sender, RoutedEventArgs e)
        {
            CompleteNumContainer.Visibility = Visibility.Hidden;
            PauseResonContainer.Visibility = Visibility.Hidden;
        }
    }
}

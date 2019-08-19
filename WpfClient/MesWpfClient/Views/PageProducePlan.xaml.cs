using MesWpfClient.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MesWpfClient.Views
{
    /// <summary>
    /// PageProducePlan.xaml 的交互逻辑
    /// </summary>
    public partial class PageProducePlan : Page
    {
        public PageProducePlan()
        {
            InitializeComponent();

            DataContext = new ProducePlanViewModel();
        }

        private void LstPlanCard_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBox)sender).Focus();
        }
    }
}

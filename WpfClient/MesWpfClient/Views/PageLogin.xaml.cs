using MesWpfClient.ViewModels;
using System.Windows.Controls;

namespace MesWpfClient.Views
{
    /// <summary>
    /// PageLogin.xaml 的交互逻辑
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();

            DataContext = new LoginViewModel();
        }

        private void UCode_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ((TextBox)sender).Focus();
        }
    }
}

using PreviewClient.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreviewClient.View
{
    /// <summary>
    /// BlankingRecordPage.xaml 的交互逻辑
    /// </summary>
    public partial class BlankingRecordPage : Page
    {
        public BlankingRecordPage()
        {
            InitializeComponent();

            DataContext = new BlankingRecordViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


        }

    }

}

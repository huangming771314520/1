using MesWpfClient.ViewModels;
using System.Windows.Controls;

namespace MesWpfClient.Views
{
    /// <summary>
    /// PageBlankingRecord.xaml 的交互逻辑
    /// </summary>
    public partial class PageBlankingRecord : Page
    {
        public PageBlankingRecord()
        {
            InitializeComponent();

            DataContext = new BlankingRecordViewModel();
        }
    }
}

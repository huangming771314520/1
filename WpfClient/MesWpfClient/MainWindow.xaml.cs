using MesWpfClient.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MesWpfClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(this);

            InputMethod.SetIsInputMethodEnabled(this, false);
            InputMethod.SetPreferredImeState(this, InputMethodState.Off);
        }
    }
}

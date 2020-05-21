using System.Windows;
using DownloadManager.ViewModels;

namespace DownloadManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        public HomeView()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            var homeViewModel = new HomeViewModel();
            DataContext = homeViewModel;
            homeViewModel.Closing += (s, e) => Close();
        }

        public HomeView(HomeViewModel homeViewModel)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DataContext = homeViewModel;
            homeViewModel.Closing += (s, e) => Close();
        }
    }
}

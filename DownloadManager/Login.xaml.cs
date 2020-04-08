using System.Windows;
using it_company.ViewModels;

namespace it_company.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoginViewModel loginViewModel = new LoginViewModel();

            DataContext = loginViewModel;

            loginViewModel.Closing += (s, e) => Close();

            ClientWcf.Connect();
        }

        public Login(string email)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.TbEmail.Text = email;
            LoginViewModel loginViewModel = new LoginViewModel(email);

            DataContext = loginViewModel;

            loginViewModel.Closing += (s, e) => Close();
        }

    }
}


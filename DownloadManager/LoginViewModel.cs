using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using it_company.Models;
using it_company.Repository;
using it_company.Views;

namespace it_company.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel()
        {
            
        }
        public LoginViewModel(string email)
        {
            Email = email;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler Closing;

        public bool Validate = false;

        private RelayCommand _login;
        private RelayCommand _register;
        private RelayCommand _forgotPassword;
        private RelayCommand _exit;

        private string _email;
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public RelayCommand Login
        {
            get
            {
                return _login ??
                       (_login = new RelayCommand(o =>
                       {
                           if (Validate)
                           {
                               MessageBox.Show("Please, check if all fields were filled in correct way! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                               return;
                           }


                           using (DataContext dataContext = new DataContext())
                           {
                               UserRepository userRepository = new UserRepository(dataContext);

                               var user = userRepository.GetAll(i => i.Email == Email).FirstOrDefault();

                               if (user != null)
                               {
                                   if (user.PasswordHash == _password.GetHashCode())
                                   {
                                       var role = user.Role;
                                       switch (role)
                                       {
                                           case Role.Employee:
                                               EmployeeWorkspace empWrk = new EmployeeWorkspace(ref user);
                                               empWrk.Show();
                                               break;
                                           case Role.Pm:
                                               PmWorkspace pmWrk = new PmWorkspace(ref user);
                                               pmWrk.Show();
                                               break;
                                           case Role.Admin:
                                               AdminWorkspace admWrk = new AdminWorkspace(ref user);
                                               admWrk.Show();
                                               break;
                                       }
                                       MessageBox.Show($" {user.FName}" + " " + $" {user.LName} ",
                                           "You have successfully logged in as", MessageBoxButton.OK, MessageBoxImage.Information);
                                   }
                               }
                               else
                               {
                                   MessageBox.Show("User with this Email doesn't exists!");
                                   return;
                               }

                           }

                           Closing?.Invoke(this, EventArgs.Empty);
                       }));
            }
        }

        public RelayCommand Register
        {
            get
            {
                return _register ??
                       (_register = new RelayCommand(o =>
                       {
                           Register reg = new Register();
                           reg.Show();
                           Closing?.Invoke(this, EventArgs.Empty);
                       }));
            }
        }

        public RelayCommand Exit
        {
            get
            {
                return _exit ??
                       (_exit = new RelayCommand(o =>
                       {
                           Environment.Exit(0);
                       }));
            }
        }

        public RelayCommand ForgotPassword
        {
            get
            {
                return _forgotPassword ??
                       (_forgotPassword = new RelayCommand(o =>
                       {
                           ForgotPassword forgotPassword = new ForgotPassword();
                           forgotPassword.Show();
                           Closing?.Invoke(this, EventArgs.Empty);
                       }));
            }
        }
    }
}

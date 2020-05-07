using System;
using System.ComponentModel;
using System.Windows.Forms;
using DownloadManager.Models;
using DownloadManager.Views;

namespace DownloadManager.ViewModels
{
    class AddUrlViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler Closing;

        private string _url;
        private HomeViewModel _homeViewModel;

        private RelayCommand _addUrl;

        public AddUrlViewModel(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
        }

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
        //url for test
        //http://ipv4.download.thinkbroadband.com/200MB.zip

        public RelayCommand AddUrl
        {
            get
            {
                return _addUrl ??
                       (_addUrl = new RelayCommand(o =>
                       {
                           if (!string.IsNullOrEmpty(_url))
                           {
                               // Create receiver, command, and invoker
                               DownloadView dwnViewModel = new DownloadView(_homeViewModel, Url);
                               Command command = new OpenCommand(dwnViewModel);
                               Invoker invoker = new Invoker();

                               // Set and execute command
                               invoker.SetCommand(command);
                               invoker.ExecuteCommand();
                           }
                           else
                           {
                               MessageBox.Show("Please write your URL.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                       }));
            }
        }



    }
}

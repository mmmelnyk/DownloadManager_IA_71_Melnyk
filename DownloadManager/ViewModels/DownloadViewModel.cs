using System;
using System.ComponentModel;
using System.Net;
using DownloadManager.Views;

namespace DownloadManager.ViewModels
{
    class DownloadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public DownloadViewModel(HomeView homeView)
        {
            _homeView = homeView;
        }

        public event EventHandler Closing;

        private RelayCommand _browsePath;
        private RelayCommand _startDownload;
        private RelayCommand _stopDownload;

        private WebClient _webClient;

        private string _url;
        public string FileName { get; set; }
        public double FileSize { get; set; }
        public double Percentage { get; set; }
        public HomeView _homeView;

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }


        public RelayCommand BrowsePath
        {
            get 
            { return _browsePath = new RelayCommand(o =>
                {

                });
            }
        }

        public RelayCommand StartDownload
        {
            get
            {
                return _startDownload = new RelayCommand(o =>
                {
                    _webClient = new WebClient();
                    _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
                    _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
                });

            }
        }

        private void _webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public RelayCommand StopDownload
        {
            get
            {
                return _stopDownload = new RelayCommand(o =>
                {

                });

            }
        }
    }
}

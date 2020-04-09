using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
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
            _webClient = new WebClient();
            _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
            _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
            _url = Url;//check this
        }

        private void _webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Minimum = 0;
            var received = double.Parse(e.BytesReceived.ToString());
            _fileSize = double.Parse(e.TotalBytesToReceive.ToString());
            Progress = received / _fileSize * 100;
            ProgressLabel = $"Downloaded {$"{Progress:0.##}"}%";
            Database.FilesRow filesRow;
        }
        public event EventHandler Closing;

        private RelayCommand _browsePath;
        private RelayCommand _startDownload;
        private RelayCommand _stopDownload;

        private WebClient _webClient;

        private string _defaultPath;
        private string _url;
        public string FileName { get; set; }
        private double _fileSize;
        private double _minimum;
        public HomeView _homeView;
        private double _progress;
        private string _progressLabel;

        public string ProgressLabel
        {
            get => _progressLabel;
            set
            {
                _progressLabel = value;
                OnPropertyChanged("ProgressLabel");//may be not necessary 
            }
        }

        public double Progress
        {
            get => _progress; 
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");//may be not necessary 
            }
        }

        public double Maximum
        {
            get => _fileSize;
            set
            {
                _fileSize = value;
                OnPropertyChanged(nameof(Maximum));
            }
        }

        public double Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                OnPropertyChanged(nameof(Minimum));
            }
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

        public string DefaultPath
        {
            get => _defaultPath;
            set
            {
                _defaultPath = value;
                OnPropertyChanged(nameof(DefaultPath));
            }
        }

        public RelayCommand BrowsePath
        {
            get
            {
                return _browsePath ??
                       (_browsePath = new RelayCommand(o =>
                       {
                           FolderBrowserDialog fbd = new FolderBrowserDialog();
                           if (fbd.ShowDialog() == DialogResult.OK)
                           {
                               DefaultPath = fbd.SelectedPath;
                               Properties.Settings.Default.Path = _defaultPath;
                               Properties.Settings.Default.Save();
                           }
                       }));
            }
        }

        public RelayCommand StartDownload
        {
            get
            {
                return _startDownload = new RelayCommand(o =>
                {
                    Uri uri = new Uri(this.Url);
                    FileName = System.IO.Path.GetFileName(uri.AbsolutePath);
                    _webClient.DownloadFileAsync(uri, Properties.Settings.Default.Path + "/" + FileName);
                });

            }
        }

        public RelayCommand StopDownload
        {
            get
            {
                return _stopDownload = new RelayCommand(o =>
                {
                    _webClient.CancelAsync();
                });

            }
        }
    }
}

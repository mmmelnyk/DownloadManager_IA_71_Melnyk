using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Net;
using System.Windows.Forms;
using DownloadManager.Models;
using DownloadManager.Views;

namespace DownloadManager.ViewModels
{
    class DownloadViewModel : INotifyPropertyChanged, IObservable<File>
    {
        //observable starts
        //possible observers are: main window, email notifier or browser plugin
        private List<IObserver<File>> observers;

        public IDisposable Subscribe(IObserver<File> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<File>> _observers;
            private IObserver<File> _observer;

            public Unsubscriber(List<IObserver<File>> observers, IObserver<File> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void PublishNewFile(File file)
        {
            foreach (var observer in observers)
            {
                if (file == null)
                    observer.OnError(new Exception());
                else
                    observer.OnNext(file);
            }
        }

        public void EndDownload()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
        //observable ends
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public DownloadViewModel(HomeViewModel homeViewModel, string url)
        {
            _url = url;
            observers = new List<IObserver<File>>();
            homeViewModel.Subscribe(this);
            _webClient = new WebClient();
            _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
            _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
        }

        private void _webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _newFile = new File()
            {
                Url = this.Url,
                FileName = this.FileName,
                FileSize = (String.Format("{0:0.##} KB", this._fileSize / 1024)),
                DateTime = DateTime.Now
            };
            _filesContext.Files.Add(_newFile);
            _filesContext.SaveChanges();
            this.PublishNewFile(_newFile);
            HomeView home = new HomeView();
            var command = new OpenCommand(home);
            var invoker = new Invoker();

            // Set and execute command
            invoker.SetCommand(command);
            invoker.ExecuteCommand();
        }

        private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var received = double.Parse(e.BytesReceived.ToString());
            _fileSize = double.Parse(e.TotalBytesToReceive.ToString());
            Progress = received / _fileSize * 100;
            ProgressLabel = $"Downloaded {$"{Progress:0.##}"}%";
        }
        public event EventHandler Closing;

        private RelayCommand _browsePath;
        private RelayCommand _startDownload;
        private RelayCommand _stopDownload;

        readonly WebClient _webClient; 
        readonly FilesContext _filesContext = new FilesContext();
        private string _defaultPath = Properties.Settings.Default.Path;
        private string _url;
        public string FileName { get; set; }
        private double _fileSize;
        private string _progressLabel;
        private File _newFile;

        public string ProgressLabel
        {
            get => _progressLabel;
            set
            {
                _progressLabel = value;
                OnPropertyChanged("ProgressLabel");//may be not necessary 
            }
        }

        public double Progress { get; set; }

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

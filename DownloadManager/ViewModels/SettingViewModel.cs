using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DownloadManager.ViewModels
{
    class SettingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler Closing;

        private RelayCommand _browsePath;
        private RelayCommand _savePath;

        private string _defaultPath;

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
                               _defaultPath = fbd.SelectedPath;
                           }
                       }));
            }
        }

        public RelayCommand SavePath
        {
            get
            {
                return _savePath ??
                       (_savePath = new RelayCommand(o =>
                       {
                           if (!string.IsNullOrEmpty(_defaultPath))
                           {
                               Properties.Settings.Default.Path = _defaultPath;
                               Properties.Settings.Default.Save();
                               Closing?.Invoke(this, EventArgs.Empty);
                           }
                           else
                           {
                               MessageBox.Show("Please select your path.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                       }));
            }
        }
    }
}

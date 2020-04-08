using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool Validate = false;

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
                               DefaultPath = fbd.SelectedPath;
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
                           //smth
                           Closing?.Invoke(this, EventArgs.Empty);
                       }));
            }
        }
    }
}

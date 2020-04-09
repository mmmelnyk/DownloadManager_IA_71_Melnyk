using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadManager.Views;

namespace DownloadManager.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler Closing;

        private RelayCommand _openSetting;

        public RelayCommand OpenSetting
        {
            get
            {
                return _openSetting ??
                       (_openSetting = new RelayCommand(o =>
                       {
                           SettingView settingView = new SettingView();
                       }));
            }
        }
    }
}

using System;
using System.ComponentModel;

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

        private RelayCommand _submitUrl;

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        } 

        public RelayCommand SubmitUrl
        {
            get
            {
                return _submitUrl ??
                       (_submitUrl = new RelayCommand(o =>
                       {

                       }));
            }
        }



    }
}

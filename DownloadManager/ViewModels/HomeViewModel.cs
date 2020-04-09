using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
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
        private RelayCommand _test;

        public RelayCommand OpenSetting
        {
            get
            {
                return _openSetting ??
                       (_openSetting = new RelayCommand(o =>
                       {
                           SettingView settingView = new SettingView();
                           settingView.ShowDialog();
                       }));
            }
        }

        public RelayCommand Test
        {
            get
            {
                return _test ??
                       (_test = new RelayCommand(o =>
                       {
                           var collection = new DownloadsCollection();
                           collection.AddItem("First");
                           collection.AddItem("Second");
                           collection.AddItem("Third");

                           Console.WriteLine("Straight traversal:");

                           foreach (var element in collection)
                           {
                               MessageBox.Show(element.ToString());
                           }

                           Console.WriteLine("\nReverse traversal:");

                           collection.ReverseDirection();

                           foreach (var element in collection)
                           {
                               Console.WriteLine(element);
                           }
                       }));
            }
        }
    }
}

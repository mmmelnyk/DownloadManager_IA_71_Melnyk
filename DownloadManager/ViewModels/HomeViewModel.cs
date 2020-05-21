using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using DownloadManager.Models;
using DownloadManager.Views;

namespace DownloadManager.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged, IObserver<File>
    {
        //TODO url for test
        //http://ipv4.download.thinkbroadband.com/200MB.zip
        //observer part start
        private IDisposable _unsubscriber;

        public virtual void Subscribe(IObservable<File> provider)
        {
            if (provider != null)
                _unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            //TODO inplement download competed alert
            Console.WriteLine("The download has completed!");
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e)
        {
            //TODO inplement error download alert
            Console.WriteLine("Error!");
        }

        public virtual void OnNext(File value)
        {
            MessageBox.Show("New Url is added!");
            FilesList.Add(value);
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
        //observer part end

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _activeStatus = true;

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler Closing;

        private RelayCommand _openSetting;
        private RelayCommand _addUrl;
        private RelayCommand _removeUrl;
        private List<File> _filesList = new List<File>();

        public List<File> FilesList
        {
            get => _filesList;
            set
            {
                _filesList = value;
                OnPropertyChanged(nameof(FilesList));
            }
        }

        public RelayCommand OpenSetting
        {
            get
            {
                return _openSetting ??
                       (_openSetting = new RelayCommand(o =>
                       {
                           // Create receiver, command, and invoker
                           SettingView settingView = new SettingView();
                           Command command = new OpenCommand(settingView);
                           Invoker invoker = new Invoker();

                           // Set and execute command
                           invoker.SetCommand(command);
                           invoker.ExecuteCommand();
                       }));
            }
        }

        public RelayCommand AddUrl
        {
            get
            {
                return _addUrl ??
                       (_addUrl = new RelayCommand(o =>
                       {
                           // Create receiver, command, and invoker
                           AddUrlView addUrlView = new AddUrlView(this);//receiver 
                           Command command = new OpenCommand(addUrlView);
                           Invoker invoker = new Invoker();

                           // Set and execute command
                           invoker.SetCommand(command);
                           invoker.ExecuteCommand();
                       }));
            }
        }

        public RelayCommand RemoveUrl
        {
            get
            {
                return _removeUrl ??
                       (_removeUrl = new RelayCommand(o =>
                       {
                           //TODO delete url function
                           MessageBox.Show("Item is deleted");
                       }));
            }
        }

        //private RelayCommand _test;
        //public RelayCommand Test
        //{
        //    get
        //    {
        //        return _test ??
        //               (_test = new RelayCommand(o =>
        //               {
        //                   using (var ctx = new FilesContext())
        //                   {
        //                       var file = new File()
        //                       {
        //                           FileName = "Bill",
        //                           DateTime = DateTime.Now
        //                       };
        //                       ctx.Files.Add(file);
        //                       ctx.SaveChanges();
        //                   }
        //                   var collection = new DownloadsCollection();
        //                   collection.AddItem("First");
        //                   collection.AddItem("Second");
        //                   collection.AddItem("Third");

        //                   Console.WriteLine("Straight traversal:");

        //                   foreach (var element in collection)
        //                   {
        //                       MessageBox.Show(element.ToString());
        //                   }

        //                   //Console.WriteLine("\nReverse traversal:");

        //                   collection.ReverseDirection();

        //               }));
        //    }
        //}
    }
}

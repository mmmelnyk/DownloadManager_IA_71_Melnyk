using System;
using System.Net;
using System.Net.PeerToPeer;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using DownloadManager.P2P;
using System.Configuration;
using System.ServiceModel.Channels;

namespace DownloadManager.Views
{
    /// <summary>
    /// Interaction logic for NetworkView.xaml
    /// </summary>
    public partial class NetworkView
    {
        private P2PService _localService;
        private string _serviceUrl;
        private ServiceHost _host;
        private PeerName peerName;
        private PeerNameRegistration _peerNameRegistration;

        public NetworkView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get config from app.config
            string port = ConfigurationManager.AppSettings["port"];
            string username = ConfigurationManager.AppSettings["username"];
            string serviceUrl = null;

            // set form title
            this.Title = string.Format($"P2P node - {username}");
            
            //get url-address of service using Ipv4 and port from app config 
            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    serviceUrl = string.Format($"net.tcp://{address}:{port}/P2PService");
                    break;
                }
            }

            // check if address is not null
            if (serviceUrl == null)
            {
                MessageBox.Show(this, "Can not define service address WCF.", "Networking Error",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }

            // registration and run WCF
            _localService = new P2PService(this, username);
            _host = new ServiceHost(_localService, new Uri(serviceUrl ?? throw new InvalidOperationException()));
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            _host.AddServiceEndpoint(typeof(IP2PService), binding, serviceUrl);
            try
            {
                _host.Open();
            }
            catch (AddressAlreadyInUseException)
            {
                MessageBox.Show(this, "Can not listen, the port is busy", "WCF Error",
                   MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }

            // Create peer name
            peerName = new PeerName("P2P Sample", PeerNameType.Unsecured);

            // get ready to register participant in the local cloud
            _peerNameRegistration = new PeerNameRegistration(peerName, int.Parse(port))
            {
                Cloud = Cloud.AllLinkLocal
            };
            // run registration
            _peerNameRegistration.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // stop registration
            _peerNameRegistration.Stop();

            // stop wcf service
            _host.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // create recognizers and event handlers
            PeerNameResolver resolver = new PeerNameResolver();
            resolver.ResolveProgressChanged +=
                resolver_ResolveProgressChanged;
            resolver.ResolveCompleted +=
                resolver_ResolveCompleted;

            PeerList.Items.Clear();
            RefreshButton.IsEnabled = false;

            // parse names async
            resolver.ResolveAsync(new PeerName("0.P2P Sample"), 1);
        }

        void resolver_ResolveCompleted(object sender, ResolveCompletedEventArgs e)
        {
            // error is no peers
            if (PeerList.Items.Count == 0)
            {
                PeerList.Items.Add(
                   new PeerEntry
                   {
                       DisplayString = "No peers found",
                       ButtonsEnabled = false
                   });
            }
            // enable refresh
            RefreshButton.IsEnabled = true;
        }

        void resolver_ResolveProgressChanged(object sender, ResolveProgressChangedEventArgs e)
        {
            PeerNameRecord peer = e.PeerNameRecord;

            foreach (IPEndPoint ep in peer.EndPointCollection)
            {
                if (ep.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    try
                    {
                        string endpointUrl = string.Format($"net.tcp://{ep.Address}:{ep.Port}/P2PService");
                        NetTcpBinding binding = new NetTcpBinding {Security = {Mode = SecurityMode.None}};
                        IP2PService serviceProxy = ChannelFactory<IP2PService>.CreateChannel(
                            binding, new EndpointAddress(endpointUrl));
                        PeerList.Items.Add(
                            new PeerEntry
                            {
                                PeerName = peer.PeerName,
                                ServiceProxy = serviceProxy,
                                DisplayString = serviceProxy.GetName(),
                                ButtonsEnabled = true
                            });
                    }
                    catch (EndpointNotFoundException)
                    {
                        PeerList.Items.Add(
                            new PeerEntry
                            {
                                PeerName = peer.PeerName,
                                DisplayString = "Unknown peer",
                                ButtonsEnabled = false
                            });
                    }
            }
        }

        private void PeerList_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).Name == "MessageButton")
            {
                // get peer and proxy to send a message
                var peerEntry = ((Button)e.OriginalSource).DataContext as PeerEntry;
                if (peerEntry != null && peerEntry.ServiceProxy != null)
                {
                    try
                    {
                        var message = peerEntry.MessageString;
                        peerEntry.ServiceProxy.SendMessage($"Hi there! {message}", ConfigurationManager.AppSettings["username"]);
                    }
                    catch (CommunicationException)
                    {

                    }
                }
            }
        }

        internal void DisplayMessage(string message, string sender)
        {
            // Show received message (called from wcf)
            MessageBox.Show(this, message, string.Format("Message from {0}", sender),
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.PeerToPeer;
using System.ServiceModel;
using System.Threading.Tasks;
using DownloadManager.Views;

namespace DownloadManager.P2P
{
    class P2PManager
    {
        private P2PService localService;
        private string serviceUri;
        private ServiceHost host;
        private PeerName peerName;
        private PeerNameRegistration peerNameRegistration;
        private Random random = new Random();
        public string userName;
        private string[] userNames = { "William", "James", "Oliver", "Richard", "Noah" };
        private List<PeerEntry> peerList;

        static object obj = new Object();
        public P2PManager()
        {
            peerList = new List<PeerEntry>();

            userName = userNames[random.Next(0, 4)];
            string port = random.Next(1000, 10000).ToString();
            string machineName = Environment.MachineName;
            string serviceUri = null;
            Console.Title = string.Format($"P2P app {userName}");

            var hostName = Dns.GetHostName();//get host name from config file
            var addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    serviceUri = string.Format($"net.tcp://{address}:{port}/P2PService");
                    break;
                }
            }

            if (serviceUri == null)
            {
                Console.WriteLine("Not able to determine endpoint for wcf");
            }
            else
            {
                localService = new P2PService(new NetworkView(), userName);
                host = new ServiceHost(localService, new Uri(serviceUri));
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                host.AddServiceEndpoint(typeof(IP2PService), binding, serviceUri);
                try
                {
                    host.Open();
                }
                catch (AddressAlreadyInUseException e)
                {
                    Console.WriteLine(e);
                }
            }
            //creating equal peer member
            peerName = new PeerName("P2P Sample", PeerNameType.Unsecured);

            //prepare peer member registration in local cloud
            peerNameRegistration = new PeerNameRegistration(peerName, int.Parse(port));
            peerNameRegistration.Cloud = Cloud.AllLinkLocal;
            //start registration process
            peerNameRegistration.Start();
        }

        public async Task Refresh()
        {
            //creating recognizer and add event handler

            PeerNameResolver resolver = new PeerNameResolver();
            resolver.ResolveProgressChanged +=
                new EventHandler<ResolveProgressChangedEventArgs>(resolver_ResolverProgressChanged);
            resolver.ResolveCompleted +=
                new EventHandler<ResolveCompletedEventArgs>(resolver_ResolveCompleted);

            //prepare to add new peers
            peerList.Clear();

            await Task.Run(() => resolver.ResolveAsync(new PeerName("0.P2P Sample"), 1));
        }

        private void resolver_ResolveCompleted(object sender, ResolveCompletedEventArgs e)
        {
            if (peerList.Count == 0)
            {

            }
        }

        private void resolver_ResolverProgressChanged(object sender, ResolveProgressChangedEventArgs e)
        {
            PeerNameRecord peer = e.PeerNameRecord;//taking link for record with peer name that was detected

            var endPointCollection = peer.EndPointCollection;
            foreach (IPEndPoint endPoint in endPointCollection)
            {
                if (endPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    try
                    {
                        string endPointUrl = string.Format($"net.tcp://{endPoint.Address}:{endPoint.Port}/P2PService");
                        NetTcpBinding binding = new NetTcpBinding();
                        binding.Security.Mode = SecurityMode.None;

                        IP2PService serviceProxy =
                            ChannelFactory<IP2PService>.CreateChannel(binding, new EndpointAddress(endPointUrl));
                        peerList.Add(new PeerEntry
                        {
                            PeerName = peer.PeerName,
                            ServiceProxy = serviceProxy,
                            DisplayString = serviceProxy.GetName()
                        });
                    }
                    catch (Exception exception)
                    {


                    }
                }
            }
        }

        public void DisplayPeers()
        {
            if (peerList.Count != 0)
            {
                foreach (var peer in peerList)
                {
                    Console.WriteLine($"peer name: {peer.PeerName.ToString()}, peer string: {peer.DisplayString}");
                }
            }
            else
            {
                Console.WriteLine("Peers not found");
            }

        }

        public void SendMessage(string match)
        {
            //get peer and proxy to send message
            var peerEntry = peerList.Find(peer => peer.DisplayString == match);
            if (peerEntry != null && peerEntry.ServiceProxy != null)
            {
                try
                {
                    peerEntry.ServiceProxy.SendMessage("Test Message", userName);
                }
                catch (CommunicationException)
                {

                }
            }
        }

        public void Stop()
        {
            Console.WriteLine("Stop registration and host");
            peerNameRegistration.Stop();//stop registration
            host.Close();//stop wcf
        }
    }
}

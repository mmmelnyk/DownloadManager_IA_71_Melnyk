using System.Net.PeerToPeer;

namespace DownloadManager.P2P
{
    class PeerEntry
    {
        public PeerName PeerName { get; set; }
        public IP2PService ServiceProxy { get; set; }
        public string DisplayString { get; set; }
        public string MessageString { get; set; }
        public bool ButtonsEnabled { get; set; }
    }
}

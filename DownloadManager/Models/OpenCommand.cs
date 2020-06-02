using System.Windows;

namespace DownloadManager.Models
{
    class OpenCommand : Command
    {
        private readonly Window _receiver;
        public OpenCommand(Window receiver)
        {
            _receiver = receiver;
        }
        public override void Execute()
        {
            _receiver.ShowDialog();
        }
    }
}

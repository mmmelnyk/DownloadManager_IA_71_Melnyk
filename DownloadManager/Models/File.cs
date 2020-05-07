using System;

namespace DownloadManager.Models
{
    public class File
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public DateTime DateTime { get; set; }
    }
}

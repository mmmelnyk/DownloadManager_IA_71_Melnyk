using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Models
{
    class FileItem : Component
    {
        public FileItem(string name) : base(name) { }

        public override string GetName()
        {
            return _name;
        }

        public override bool IsComposite()
        {
            return false;
        }
    }
}

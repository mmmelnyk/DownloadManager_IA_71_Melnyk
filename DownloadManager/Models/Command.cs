using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Models
{
    public abstract class Command
    { 
        public abstract void Execute();
    }
}

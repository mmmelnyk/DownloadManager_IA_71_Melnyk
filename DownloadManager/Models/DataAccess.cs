using System.Collections.Generic;

namespace DownloadManager.Models
{// template class
    abstract class DataAccess { 
        //protected string connectionString;
        protected FilesContext filesContext;
        protected List<File> filesList= new List<File>();

        public virtual void Connect()
        {
            filesContext = new FilesContext();
            //connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\projects\c#\DownloadManager_IA_71_Melnyk\DownloadManager\Files.mdf;Integrated Security=True";
        }

        public abstract void Select();
        public abstract List<File> Process();

        public virtual void Disconnect()
        {
            //connectionString = "";
            filesContext = null;
        }

        // template method

        public List<File> Run()
        {
            Connect();
            Select();
            var tmp = Process();
            Disconnect();
            return tmp;
        }
    }
}

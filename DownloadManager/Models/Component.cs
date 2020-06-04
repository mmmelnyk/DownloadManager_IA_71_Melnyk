using System;

namespace DownloadManager.Models
{
    abstract class Component
    {
        protected string _name;
        
        //protected Component() {}

        protected Component(string name)
        {
            _name = name;
        }

        public abstract string GetName();

        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsComposite()
        {
            return true;
        }

    }
}

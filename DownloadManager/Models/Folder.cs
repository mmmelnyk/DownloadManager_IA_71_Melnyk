using System.Collections.Generic;
namespace DownloadManager.Models
{
    class Folder : Component
    {
        public Folder(string name):base(name) { }

        protected List<Component> _children = new List<Component>();

        public override void Add(Component component)
        {
            this._children.Add(component);
        }

        public override void Remove(Component component)
        {
            this._children.Remove(component);
        }

        public override string GetName()
        {
            return _name;
        }

        public List<Component> ReturnComponents()
        {
           return _children;
        }
    }
}

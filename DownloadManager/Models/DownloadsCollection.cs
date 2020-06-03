using System.Collections;
using System.Collections.Generic;
using DownloadManager.Models;


namespace DownloadManager
{
    class DownloadsCollection : IteratorAggregate
    {
        private List<string> _collection = new List<string>();

        bool _direction = false;

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public List<string> getItems()
        {
            return _collection;
        }

        public void AddItem(string item)
        {
            this._collection.Add(item);
        }

        public override IEnumerator GetEnumerator()
        {
            return new DownloadsIterator(this, _direction);
        }
    }
}

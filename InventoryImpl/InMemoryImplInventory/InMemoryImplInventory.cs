using InMemoryImplInventory.DataStructures;
using InventoryCommon.Events;
using InventoryCommon.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InMemoryImplInventory
{
    public class InMemoryImplInventory : IInventory, IDisposable
    {
        private const int EXPIRATION_MONITORING_TIMEOUT = 1000;

        private readonly ConcurrentDictionary<TypeTitleItemIndex, Item_> _items = new ConcurrentDictionary<TypeTitleItemIndex, Item_>();
        private readonly CancellationTokenSource _expirationMonitoringCancellationTokenSource = new CancellationTokenSource();

        public InMemoryImplInventory()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ExpirationMonitoringThreadFunc), _expirationMonitoringCancellationTokenSource.Token);
        }

        #region public methods

        public IItem AddItem(string type, string title, DateTime expirationDate, IDictionary<string, object> attributes = null)
        {
            var index = new TypeTitleItemIndex(type, title);
            Item_ item = new Item_(index, expirationDate, attributes);
            if (!_items.TryAdd(index, item))
            {
                throw new Exception(string.Format("Item of type '{0}' with title '{1}' already exists", type, title));
            }
            return item;
        }

        public IItem GetItem(string type, string title)
        {
            var index = new TypeTitleItemIndex(type, title);
            Item_ item = null;
            _items.TryGetValue(index, out item);
            return item;
        }

        public bool RemoveItem(string type, string title)
        {
            var index = new TypeTitleItemIndex(type, title);
            bool isRemoved = false;
            Item_ item = null;

            isRemoved = _items.TryRemove(index, out item);
            if (isRemoved)
            {
                OnItemRemoved(item);
            }
            return isRemoved;
        }

        public event EventHandler<ItemRemovedEventArgs> ItemRemoved;

        public event EventHandler<ItemExpiredEventArgs> ItemExpired;

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region private methods

        private void OnItemRemoved(IItem item)
        {
            if (null != this.ItemRemoved)
            {
                ItemRemoved(this, new ItemRemovedEventArgs(item));
            }
        }

        private void OnItemExpired(IItem item)
        {
            if (null != this.ItemExpired)
            {
                ItemExpired(this, new ItemExpiredEventArgs(item));
            }
        }

        private void ExpirationMonitoringThreadFunc(object obj)
        {
            CancellationToken cancelationToken = (CancellationToken)obj;
            while (!cancelationToken.IsCancellationRequested)
            {
                foreach (var item in _items.Values)
                {
                    if (item.ExpirationTime <= DateTime.UtcNow && !item.IsExpiredEventSent)
                    {
                        item.IsExpiredEventSent = true;
                        OnItemExpired(item);
                    }
                }
                Thread.Sleep(EXPIRATION_MONITORING_TIMEOUT);
            }
        }

        private void Dispose(bool isDisposing)
        {
            _expirationMonitoringCancellationTokenSource.Cancel();
            _expirationMonitoringCancellationTokenSource.Dispose();
            if (isDisposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        ~InMemoryImplInventory()
        {
            Dispose(false);
        }
    }
}

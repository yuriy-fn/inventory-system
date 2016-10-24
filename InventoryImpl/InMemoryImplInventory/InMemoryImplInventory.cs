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
    /// <summary>
    /// Inventory implemenation where in memory data structures are used as a storage
    /// </summary>
    public class InMemoryImplInventory : IInventory
    {
        /// <summary>
        /// defines timeout for the next items expiration check / notifications
        /// </summary>
        private const int EXPIRATION_MONITORING_TIMEOUT = 1000;

        /// <summary>
        /// internal items storage
        /// </summary>
        private readonly ConcurrentDictionary<string, Item_> _items = new ConcurrentDictionary<string, Item_>();

        /// <summary>
        /// cancelation token source to stop items expiration checking thread
        /// </summary>
        private readonly CancellationTokenSource _expirationMonitoringCancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initialize inventory
        /// </summary>
        public InMemoryImplInventory()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ExpirationMonitoringThreadFunc), _expirationMonitoringCancellationTokenSource.Token);
        }

        #region public methods

        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="label">item label</param>
        /// <param name="type">item type</param>
        /// <param name="expirationDate">item expiration date</param>
        /// <param name="attributes">item dynamic attributes</param>
        /// <returns>new item</returns>
        /// <exception cref="Exception">Item already exists</exception>
        public IItem AddItem(string label, string type, DateTime expirationDate, IDictionary<string, object> attributes = null)
        {
            Item_ item = new Item_(label, type, expirationDate, attributes);
            if (!_items.TryAdd(label, item))
            {
                throw new Exception(string.Format("Item with label '{0}' already exists", label));
            }
            return item;
        }

        /// <summary>
        /// Return existent item by label from the inventory
        /// </summary>
        /// <param name="label">item label</param>
        /// <returns>existent item; null, if item is not found</returns>
        public IItem GetItem(string label)
        {
            Item_ item = null;
            _items.TryGetValue(label, out item);
            return item;
        }

        /// <summary>
        /// Remove item from the inventory
        /// </summary>
        /// <param name="label">item label</param>
        /// <returns>true, if item was found and removed</returns>
        public bool RemoveItem(string label)
        {
            bool isRemoved = false;
            Item_ item = null;

            isRemoved = _items.TryRemove(label, out item);
            if (isRemoved)
            {
                OnItemRemoved(item);
            }
            return isRemoved;
        }

        /// <summary>
        /// Event is sent if item was removed
        /// </summary>
        public event EventHandler<ItemRemovedEventArgs> ItemRemoved;

        /// <summary>
        /// Event is sent if item has expired (or already expired item has been added to the inventory)
        /// </summary>
        public event EventHandler<ItemExpiredEventArgs> ItemExpired;

        /// <summary>
        /// Dispose inventory
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Send ItemRemoved event to all subscribers
        /// </summary>
        /// <param name="item">removed item</param>
        private void OnItemRemoved(IItem item)
        {
            if (null != this.ItemRemoved)
            {
                ItemRemoved(this, new ItemRemovedEventArgs(item));
            }
        }

        /// <summary>
        /// Send ItemExpired event to all subscribers
        /// </summary>
        /// <param name="item"></param>
        private void OnItemExpired(IItem item)
        {
            if (null != this.ItemExpired)
            {
                ItemExpired(this, new ItemExpiredEventArgs(item));
            }
        }

        /// <summary>
        /// Thread function where all items in the storage are checked periodically if they are expired
        /// </summary>
        /// <param name="obj">cancellation token</param>
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

        /// <summary>
        /// Dispose inventory
        /// </summary>
        /// <param name="isDisposing">true, if called from parameterless Dispose method</param>
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

        /// <summary>
        /// Inventory class finalizer
        /// </summary>
        ~InMemoryImplInventory()
        {
            Dispose(false);
        }
    }
}

using InventoryCommon.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryCommon.Interfaces
{
    /// <summary>
    /// Inventory functionality declaration
    /// </summary>
    public interface IInventory : IDisposable
    {
        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="type">item type</param>
        /// <param name="title">item title</param>
        /// <param name="expirationDate">item expiration date</param>
        /// <param name="attributes">item dynamic attributes</param>
        /// <returns>new item</returns>
        /// <exception cref="Exception">Item already exists</exception>
        IItem AddItem(string type, string title, DateTime expirationDate, IDictionary<string, object> attributes = null);

        /// <summary>
        /// Return existent item by type and title from the inventory
        /// </summary>
        /// <param name="type">item type</param>
        /// <param name="title">item title</param>
        /// <returns>existent item; null, if item is not found</returns>
        IItem GetItem(string type, string title);

        /// <summary>
        /// Remove item from the inventory
        /// </summary>
        /// <param name="type">item type</param>
        /// <param name="title">item title</param>
        /// <returns>true, if item was found and removed</returns>
        bool RemoveItem(string type, string title);

        /// <summary>
        /// Event is sent if item was removed
        /// </summary>
        event EventHandler<ItemRemovedEventArgs> ItemRemoved;

        /// <summary>
        /// Event is sent if item has expired (or already expired item has been added to the inventory)
        /// </summary>
        event EventHandler<ItemExpiredEventArgs> ItemExpired;
    }
}

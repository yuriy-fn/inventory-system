using InventoryCommon.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryCommon.Interfaces
{
    public interface IInventory
    {
        IItem AddItem(string type, string title, DateTime expirationDate, IDictionary<string, object> attributes = null);
        IItem GetItem(string type, string title);
        bool RemoveItem(string type, string title);

        event EventHandler<ItemRemovedEventArgs> ItemRemoved;
        event EventHandler<ItemExpiredEventArgs> ItemExpired;
    }
}

using InventoryCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Events
{
    public class ItemExpiredEventArgs : EventArgs
    {
        private IItem _item;

        public ItemExpiredEventArgs(IItem item)
        {
            _item = item;
        }

        public IItem Item { get { return _item; } }
    }
}

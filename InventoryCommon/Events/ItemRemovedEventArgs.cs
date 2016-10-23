using InventoryCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Events
{
    public class ItemRemovedEventArgs : EventArgs
    {
        private IItem _item;

        public ItemRemovedEventArgs(IItem item)
        {
            _item = item;
        }

        public IItem Item { get { return _item; } }
    }
}

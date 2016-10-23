using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Interfaces
{
    public interface IItem
    {
        string Type { get; }
        string Title { get; }
        DateTime ExpirationTime { get; }

        object this[string attributeName] { get; set; }
    }
}

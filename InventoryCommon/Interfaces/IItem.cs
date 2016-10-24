using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryCommon.Interfaces
{
    /// <summary>
    /// Inventory item functionality declaration
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Get item label
        /// </summary>
        string Label { get; }
        
        /// <summary>
        /// Get item type
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Get item expiration time
        /// </summary>
        DateTime ExpirationTime { get; }

        /// <summary>
        /// Get or set dynamic attribute
        /// </summary>
        /// <param name="attributeName">attribute name</param>
        /// <returns>attribute value</returns>
        object this[string attributeName] { get; set; }
    }
}

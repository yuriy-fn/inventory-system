using InventoryCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InMemoryImplInventory.DataStructures
{
    /// <summary>
    /// inventory item
    /// </summary>
    internal class Item_ : IItem
    {
        private string _label;
        private string _type;
        private DateTime _expirationTime;
        private Dictionary<string, object> _attributes;

        private bool _isExpiredEventSent = false;

        /// <summary>
        /// initialize inventory item
        /// </summary>
        /// <param name="typeTitleIndex">index</param>
        /// <param name="expirationTime">expiration time</param>
        /// <param name="attributes">dynamic attributes</param>
        public Item_(string label, string type, DateTime expirationTime, IDictionary<string, object> attributes)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentNullException("Item label can't be empty or whitespace", "label");
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException("Item type can't be empty or whitespace", "type");
            }
            _label = label;
            _type = type;
            _expirationTime = expirationTime;

            if (null != attributes)
            {
                _attributes = new Dictionary<string, object>(attributes);
            }
        }

        /// <summary>
        /// Get or set dynamic attribute
        /// </summary>
        /// <param name="attributeName">attribute name</param>
        /// <returns>attribute value</returns>
        public object this[string attributeName]
        {
            get
            {
                object attributeValue = null;
                if (null != _attributes)
                {
                    _attributes.TryGetValue(attributeName, out attributeValue);
                }
                return attributeValue;
            }
            set
            {
                if (null == _attributes)
                {
                    _attributes = new Dictionary<string, object>();
                }
                _attributes[attributeName] = value;
            }
        }

        /// <summary>
        /// Get item label
        /// </summary>
        public string Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Get item type
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Get item expiration time
        /// </summary>
        public DateTime ExpirationTime
        {
            get { return _expirationTime; }
        }

        /// <summary>
        /// Indicate if event that the item is expired was sent
        /// </summary>
        internal bool IsExpiredEventSent
        {
            get { return _isExpiredEventSent; }
            set { _isExpiredEventSent = value; }
        }
    }
}

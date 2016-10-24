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
        private TypeTitleItemIndex _typeTitleIndex;
        private DateTime _expirationTime;
        private Dictionary<string, object> _attributes;

        private bool _isExpiredEventSent = false;

        /// <summary>
        /// initialize inventory item
        /// </summary>
        /// <param name="typeTitleIndex">index</param>
        /// <param name="expirationTime">expiration time</param>
        /// <param name="attributes">dynamic attributes</param>
        public Item_(TypeTitleItemIndex typeTitleIndex, DateTime expirationTime, IDictionary<string, object> attributes)
        {
            if (null == typeTitleIndex)
            {
                throw new ArgumentNullException("typeTitleIndex");
            }
            _typeTitleIndex = typeTitleIndex;
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
        /// Get item type
        /// </summary>
        public string Type
        {
            get { return _typeTitleIndex.Type; }
        }

        /// <summary>
        /// Get item title
        /// </summary>
        public string Title
        {
            get { return _typeTitleIndex.Title; }
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

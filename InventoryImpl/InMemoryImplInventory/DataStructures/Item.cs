using InventoryCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InMemoryImplInventory.DataStructures
{
    internal class Item_ : IItem
    {
        private TypeTitleItemIndex _typeTitleIndex;
        private DateTime _expirationTime;
        private Dictionary<string, object> _attributes;

        private bool _isExpiredEventSent = false;

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

        public string Type
        {
            get { return _typeTitleIndex.Type; }
        }

        public string Title
        {
            get { return _typeTitleIndex.Title; }
        }

        public DateTime ExpirationTime
        {
            get { return _expirationTime; }
        }

        internal TypeTitleItemIndex TypeTitleIndex { get { return _typeTitleIndex; } }

        internal bool IsExpiredEventSent
        {
            get { return _isExpiredEventSent; }
            set { _isExpiredEventSent = value; }
        }
    }
}

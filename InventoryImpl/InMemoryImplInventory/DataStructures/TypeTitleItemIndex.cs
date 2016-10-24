using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryImplInventory.DataStructures
{
    /// <summary>
    /// Item unique index (combination of item type and title) 
    /// </summary>
    internal class TypeTitleItemIndex
    {
        private string _type;
        private string _title;

        /// <summary>
        /// Initialize item index
        /// </summary>
        /// <param name="type">item type</param>
        /// <param name="title">item title</param>
        public TypeTitleItemIndex(string type, string title)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException("Item type can't be empty or whitespace", "type");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("Item title can't be empty or whitespace", "title");
            }

            _type = type;
            _title = title;
        }

        /// <summary>
        /// Get item type
        /// </summary>
        public string Type { get { return _type; } }

        /// <summary>
        /// Get item title
        /// </summary>
        public string Title { get { return _title; } }
    
        /// <summary>
        /// Defines if the index equals another object
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>true if equals</returns>
        public override bool Equals(object obj)
        {
            bool equals = false;
            var itemIndex = obj as TypeTitleItemIndex;
            if (null != itemIndex)
            {
                equals = (this._title == itemIndex._title && this._type == itemIndex._type);
            }
            return equals;
        }

        /// <summary>
        /// Define index hash code
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _title.GetHashCode();
        }
    }
}

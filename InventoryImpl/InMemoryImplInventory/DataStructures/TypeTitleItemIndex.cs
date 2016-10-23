using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryImplInventory.DataStructures
{
    internal class TypeTitleItemIndex
    {
        private string _type;
        private string _title;

        public TypeTitleItemIndex(string type, string title)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException("Item type can't be empty or whitespaces", "type");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("Item title can't be empty or whitespaces", "title");
            }

            _type = type;
            _title = title;
        }

        public string Type { get { return _type; } }
        public string Title { get { return _title; } }
    
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

        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _title.GetHashCode();
        }
    }
}

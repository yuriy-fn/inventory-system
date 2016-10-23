using InventoryCommon.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUnit.InventorySystemTests
{
    [TestFixture]
    public abstract class InventorySystemTestClass
    {
        protected abstract IInventory Inventory { get; }
 
        [Test]
        public void AddNewItemTest()
        {
            string itemType = "TestType";
            string itemTitle = "TestTitle";
            var expirationTime = DateTime.UtcNow.AddYears(5);
            string attr1Name = "attr1", attr2Name = "attr2";
            int attr1Value = 1, attr2Value = 2;
            {
                var item = this.Inventory.AddItem(itemType, itemTitle, expirationTime,
                    new Dictionary<string, object>() { { attr1Name, attr1Value }, { attr2Name, attr2Value } });
            }
            {
                var item = this.Inventory.GetItem(itemType, itemTitle);
                Assert.AreEqual(item.Type, itemType);
                Assert.AreEqual(item.Title, itemTitle);
                Assert.AreEqual(item.ExpirationTime, expirationTime);
                Assert.AreEqual(item[attr1Name], attr1Value);
                Assert.AreEqual(item[attr2Name], attr2Value);
            }
        }

        [Test]
        public void TryGetNonExistentItemByTypeTest()
        {
            string itemTitle = "TestTitle";
            this.Inventory.AddItem("TestType", itemTitle, DateTime.MaxValue);
            var item = this.Inventory.GetItem("TestType2", itemTitle);
            Assert.IsNull(item);
        }

        [Test]
        public void TryGetNonExistentItemByTitleTest()
        {
            string itemType = "TestType";
            this.Inventory.AddItem(itemType, "TestTitle", DateTime.MaxValue);
            var item = this.Inventory.GetItem(itemType, "TestTitle2");
            Assert.IsNull(item);
        }

        [Test]
        public void TryAddExistentItemTest()
        {
            string itemType = "TestType";
            string itemTitle = "TestTitle";
            this.Inventory.AddItem(itemType, itemTitle, DateTime.MaxValue);
            Assert.That(() => this.Inventory.AddItem(itemType, itemTitle, DateTime.MaxValue), Throws.TypeOf<Exception>());
        }

        [Test]
        public void RemoveExistentItemTest()
        {
            string itemType = "TestType";
            string itemTitle = "TestTitle";
            this.Inventory.AddItem(itemType, itemTitle, DateTime.MaxValue);
            bool wasRemoved = this.Inventory.RemoveItem(itemType, itemTitle);
            Assert.IsTrue(wasRemoved);
            var item = this.Inventory.GetItem(itemType, itemTitle);
            Assert.IsNull(item);
        }

        [Test]
        public void RemoveExistentItemEventTest()
        {
            IItem removedItem = null;

            this.Inventory.ItemRemoved += (sender, e) =>
                {
                    removedItem = e.Item;
                };
            string itemType = "TestType";
            string itemTitle = "TestTitle";
            var item = this.Inventory.AddItem(itemType, itemTitle, DateTime.MaxValue);
            this.Inventory.RemoveItem(itemType, itemTitle);
            Assert.AreEqual(item, removedItem);
        }

        [Test]
        public void TryRemoveNonExistentItemTest()
        {
            string itemType = "TestType";
            this.Inventory.AddItem(itemType, "TestTitle", DateTime.MaxValue);
            bool wasRemoved = this.Inventory.RemoveItem(itemType, "TestTitle2");
            Assert.IsFalse(wasRemoved);
        }

        [Test]
        public void ExpiredItemEventTest()
        {
            var expiredEvent = new AutoResetEvent(false);
            IItem expiredItem = null;

            this.Inventory.ItemExpired += (sender, e) =>
            {
                expiredItem = e.Item;
                expiredEvent.Set();
            };
            string itemType = "TestType";
            string itemTitle = "TestTitle";
            var item = this.Inventory.AddItem(itemType, itemTitle, DateTime.UtcNow.AddMilliseconds(1000));

            expiredEvent.WaitOne(10000);// wait 10 sec max

            Assert.AreEqual(item, expiredItem);
        }
    }
}

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
            string itemLabel = "TestLabel";
            var expirationTime = DateTime.UtcNow.AddYears(5);
            string attr1Name = "attr1", attr2Name = "attr2";
            int attr1Value = 1, attr2Value = 2;
            {
                var item = this.Inventory.AddItem(itemLabel, itemType, expirationTime,
                    new Dictionary<string, object>() { { attr1Name, attr1Value }, { attr2Name, attr2Value } });
            }
            {
                var item = this.Inventory.GetItem(itemLabel);
                Assert.AreEqual(item.Type, itemType);
                Assert.AreEqual(item.Label, itemLabel);
                Assert.AreEqual(item.ExpirationTime, expirationTime);
                Assert.AreEqual(item[attr1Name], attr1Value);
                Assert.AreEqual(item[attr2Name], attr2Value);
            }
        }

        [Test]
        public void TryAddNewItemEmptyLabelTest()
        {
            Assert.That(() => this.Inventory.AddItem(null, "TestType", DateTime.MaxValue), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TryAddNewItemEmptyTypeTest()
        {
            Assert.That(() => this.Inventory.AddItem("TestLabel", null, DateTime.MaxValue), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void UpdateItemAttributeTest()
        {
            string itemLabel = "TestLabel";

            var expirationTime = DateTime.UtcNow.AddYears(5);
            string attr1Name = "attr1";
            int attr1Value = 1, attr1NewValue = 10;
            {
                var item = this.Inventory.AddItem(itemLabel, "TestLabel", expirationTime,
                    new Dictionary<string, object>() { { attr1Name, attr1Value } });
            }
            {
                var item = this.Inventory.GetItem(itemLabel);
                Assert.AreEqual(item[attr1Name], attr1Value);
                item[attr1Name] = attr1NewValue;
            }
            {
                var item = this.Inventory.GetItem(itemLabel);
                Assert.AreEqual(item[attr1Name], attr1NewValue);
            }
        }

        [Test]
        public void SetItemNewAttributeTest()
        {
            string itemLabel = "TestLabel";
            var expirationTime = DateTime.UtcNow.AddYears(5);
            string attr1Name = "attr1";
            int attr1Value = 1;
            {
                var item = this.Inventory.AddItem(itemLabel, "TestLabel", expirationTime);
            }
            {
                var item = this.Inventory.GetItem(itemLabel);
                item[attr1Name] = attr1Value;
            }
            {
                var item = this.Inventory.GetItem(itemLabel);
                Assert.AreEqual(item[attr1Name], attr1Value);
            }
        }

        [Test]
        public void TryGetNonExistentItemByTypeTest()
        {
            this.Inventory.AddItem("TestLabel", "TestType", DateTime.MaxValue);
            var item = this.Inventory.GetItem("TestLabel2");
            Assert.IsNull(item);
        }

        [Test]
        public void TryAddExistentItemTest()
        {
            string itemLabel = "TestLabel";
            this.Inventory.AddItem(itemLabel, "TestType", DateTime.MaxValue);
            Assert.That(() => this.Inventory.AddItem(itemLabel, "TestType2", DateTime.MaxValue), Throws.TypeOf<Exception>());
        }

        [Test]
        public void RemoveExistentItemTest()
        {
            string itemLabel = "TestLabel";
            this.Inventory.AddItem(itemLabel, "TestType", DateTime.MaxValue);
            bool wasRemoved = this.Inventory.RemoveItem(itemLabel);
            Assert.IsTrue(wasRemoved);
            var item = this.Inventory.GetItem(itemLabel);
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
            string itemLabel = "TestLabel";
            var item = this.Inventory.AddItem(itemLabel, "TestType", DateTime.MaxValue);
            this.Inventory.RemoveItem(itemLabel);
            Assert.AreEqual(item, removedItem);
        }

        [Test]
        public void TryRemoveNonExistentItemTest()
        {
            this.Inventory.AddItem("TestLabel", "TestType", DateTime.MaxValue);
            bool wasRemoved = this.Inventory.RemoveItem("TestLabel2");
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
            var item = this.Inventory.AddItem("TestLabel", "TestType", DateTime.UtcNow.AddMilliseconds(1000));

            expiredEvent.WaitOne(10000);// wait 10 sec max

            Assert.AreEqual(item, expiredItem);
        }
    }
}

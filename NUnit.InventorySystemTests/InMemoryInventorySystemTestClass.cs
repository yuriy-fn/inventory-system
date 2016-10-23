using InventoryCommon.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.InventorySystemTests
{
    [TestFixture]
    [Category("In Memory Inventory System")]
    public class InMemoryInventorySystemTestClass : InventorySystemTestClass
    {
        private IInventory _inventory;

        /// <summary>
        /// Create a new instance of an empty InMemoryImplInventory for each test
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            _inventory = new InMemoryImplInventory.InMemoryImplInventory();
        }

        protected override IInventory Inventory
        {
            get { return _inventory; }
        }
    }
}

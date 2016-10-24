The solution consists of three parts:
1) InventoryCommon: contains common interfaces and data structures
2) InMemoryImplInventory: implemenation of the IInventory and other common interface, utilizing in memory data structures as a storage
3) NUnit.InventorySystemTests: NUnit based unit tests

IInventory implemenation (InMemoryImplInventory etc) can be registered in DI container. External caller has access to the inventory and items only through interfaces.
IInventory and IItem can be extended if needed. Also IItem dynamic arguments provide extra extensibility.

IItem is uniquely identified by the combination of its type and title.

Notifications about expired items will be send either immediately if expired items are added to the inventory, or later at the moment when the item expires.

Unit Tests are common ones (InventorySystemTestClass), and can be reused to test different implemenations (InMemoryInventorySystemTestClass etc.)
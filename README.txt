The solution consists of three parts:
1) InventoryCommon: contains common interfaces and data structures
2) InMemoryImplInventory: implemenation of the IInventory and other common interface, utilizing in memory data structures as a storage
3) NUnit.InventorySystemTests: NUnit based unit tests

IInventory implemenation (InMemoryImplInventory etc) can be registered in DI container. External caller has access to the inventory and items only calling public interfaces.
IInventory and IItem can be enriched or new interfaces with extra methods, which inherit from IInventory/IItem, can be added, to extend functionality.
Also IItem dynamic arguments provide extra extensibility.

Notifications about expired items will be send either immediately if expired items are added to the inventory, or later at the moment when the item expires.

InMemoryImplInventory uses ConcurrentDictionary as internal storage, which makes the Inventory thread safe as well as provides better performance comparing to using locks.

Unit Tests are common ones (InventorySystemTestClass), and can be reused to test different implemenations (InMemoryInventorySystemTestClass etc.)

QUESTIONS:
1) What kind of API is needed (RESTful Web API, WCF)? The current API is simple .NET assembly/interface, where actual implemenation can be registered in DI container.
	In case of WCF or Web API implementation additional data containers classes (Item etc) will be needed.
2) What kind of security must be implemented? Does it require users registration / logging? Should it be role-based access as "read-only", "read-write" etc?

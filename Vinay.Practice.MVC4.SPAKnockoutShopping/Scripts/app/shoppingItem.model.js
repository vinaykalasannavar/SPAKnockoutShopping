(function (ko, datacontext) {
    datacontext.shoppingItem = shoppingItem;
    datacontext.shoppingCategoryList = shoppingCategoryList;
    datacontext.checkoutList = checkoutList;
    datacontext.checkoutItem = checkoutItem;

    function shoppingItem(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.shoppingItemId = data.shoppingItemId;
        self.title = ko.observable(data.title);//TODO: chagce this input to label and correct the binding
        self.price = ko.observable(data.price);
        self.isDone = ko.observable(data.isDone);
        self.shoppingCategoryListId = data.shoppingCategoryListId;

        // Non-persisted properties
        self.errorMessage = ko.observable();

        saveChanges = function () {
            return datacontext.saveChangedShoppingItem(self);
        };

        // Auto-save when these properties change
        self.isDone.subscribe(saveChanges);
        self.title.subscribe(saveChanges);

        self.toJson = function () { return ko.toJSON(self) };
    };

    function shoppingCategoryList(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.shoppingCategoryListId = data.shoppingCategoryListId;
        self.userId = data.userId || "to be replaced";
        self.title = ko.observable(data.title || "My Shopping lists");
        self.shoppingitems = ko.observableArray(importShoppingItems(data.shoppingItems));

        // Non-persisted properties
        self.isEditingListTitle = ko.observable(false);
        self.newShoppingItemTitle = ko.observable();
        self.errorMessage = ko.observable();

        self.deleteShoppingItem = function () {
            var shoppingItem = this;
            return datacontext.deleteShoppingItem(shoppingItem)
                 .done(function () {
                     self.shoppingitems.remove(shoppingItem);
                 });
        };

        self.buyProduct = function () {
            var productItem = this;

            var addedProductItems = datacontext.readAddedProductItemsFromCookie();
            if (addedProductItems == null) {
                addedProductItems = { checkoutListId: 10, userId:1, checkoutItems: [self.newProductJson(productItem, 10)] /**/ };
            } else {
                var itemFound = self.searchAddedProductItems(addedProductItems, productItem);
                if (itemFound == null) {
                    addedProductItems.checkoutItems[addedProductItems.checkoutItems.length] = self.newProductJson(productItem, 10);
                } else {
                    itemFound.quantity++;
                }
            }

            var serialisedCookieVal = JSON.stringify(addedProductItems);
            datacontext.saveAddedProductItemsToCookie(serialisedCookieVal);
        };

        self.newProductJson = function (productItem, checkoutListId) {
            return { categoryId: productItem.shoppingCategoryListId, productId: productItem.shoppingItemId, quantity: 1, /*checkoutListId: checkoutListId */};
        };
        self.searchAddedProductItems = function (addedProductItems, productItem) {
            var itemFound = null;
            for (var index = 0; index < addedProductItems.checkoutItems.length; index++) {
                var eachProdItem = addedProductItems.checkoutItems[index];
                if (eachProdItem.categoryId == productItem.shoppingCategoryListId && eachProdItem.productId == productItem.shoppingItemId) {
                    itemFound = eachProdItem;
                    break;
                }
            }
            return itemFound;
        };

        // Auto-save when these properties change
        self.title.subscribe(function () {
            return datacontext.saveChangedShoppingCategoryList(self);
        });

        self.toJson = function () { return ko.toJSON(self) };
    };

    function checkoutList(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.checkoutListId = data.checkoutListId;
        self.userId = data.userId;
        self.checkoutListId = data.checkoutListId;
        self.totalPrice = ko.observable(data.totalPrice);
        self.checkoutItems = ko.observableArray(importCheckoutItems(data.checkoutItems));

        // Non-persisted properties
        self.errorMessage = ko.observable();

        self.toJson = function () { return ko.toJSON(self) };
    };

    function checkoutItem(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.checkoutItemId = data.checkoutItemId;
        self.categoryId = data.categoryId;
        self.productid = data.productId;
        self.price = data.price;
        self.productname = data.productName;
        self.quantity = data.quantity;
        self.subtotal = data.subTotal;
        self.checkoutListId = data.checkoutListId;

        // Non-persisted properties
        self.errorMessage = ko.observable();

        saveChanges = function () {
            return datacontext.saveChangedCheckoutItem(self);
        };

        self.toJson = function () { return ko.toJSON(self) };
    };

    // convert raw shoppingItem data objects into array of ShoppingItems
    function importShoppingItems(ShoppingItems) {
        /// <returns value="[new shoppingItem()]"></returns>
        return $.map(ShoppingItems || [],
                function (todoItemData) {
                    return datacontext.createShoppingItem(todoItemData);
                });
    }

    // convert raw shoppingItem data objects into array of ShoppingItems
    function importCheckoutItems(checkoutItems) {
        /// <returns value="[new shoppingItem()]"></returns>
        return $.map(checkoutItems || [],
                function (checkoutItemData) {
                    return datacontext.createCheckoutItem(checkoutItemData);
                });
    }

    shoppingCategoryList.prototype.addTodo = function () {
        var self = this;
        if (self.newShoppingItemTitle()) { // need a title to save
            var shoppingItem = datacontext.createShoppingItem(
                {
                    title: self.newShoppingItemTitle(),
                    shoppingCategoryListId: self.shoppingCategoryListId
                });
            self.shoppingitems.push(shoppingItem);
            datacontext.saveNewShoppingItem(shoppingItem);
            self.newShoppingItemTitle("");
        }
    };
})(ko, shoppingApp.datacontext);
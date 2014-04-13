window.shoppingApp.shoppingCategoryListViewModel = (function (ko, datacontext) {
    /// <field name="shoppingCategoryLists" value="[new datacontext.shoppingCategoryList()]"></field>
    var shoppingCategoryLists = ko.observableArray(),
        checkoutLists = ko.observableArray(),
        error = ko.observable(),
        addshoppingCategoryList = function () {
            var shoppingCategoryList = datacontext.createShoppingCategoryList();
            shoppingCategoryList.isEditingListTitle(true);
            datacontext.saveNewShoppingCategoryList(shoppingCategoryList)
                .then(addSucceeded)
                .fail(addFailed);

            function addSucceeded() {
                showshoppingCategoryList(shoppingCategoryList);
            }
            function addFailed() {
                error("Save of new shoppingCategoryList failed");
            }
        },
        addCheckoutList = function () {
            var addedProductItems = datacontext.readAddedProductItemsFromCookie();
            if (addedProductItems != null) {
                addedProductItems.toJson = function () { return ko.toJSON(this) };
                datacontext.saveNewCheckoutList(addedProductItems)
                    .then(addSucceeded)
                    .fail(addFailed);

                function addSucceeded() {

                    showCheckoutList();
                }
                function addFailed() {
                    error("CheckoutList failed :( please try again later...");
                }
            } else {
                alert('Your basket is empty, please buy something!');
            }
        },
        finishCheckoutList = function () {
            alert('It is assumed that you bought your items, emptying your basket now...');

            datacontext.clearAddedProductItemsFromCookie();
            showshoppingCategoryList();
        },
        showshoppingCategoryList = function (shoppingCategoryList) {
            $("#checkoutForm").hide("slide", function () {
                $("#shoppingForm").show("slide", function () {
                });
            });
        },
        deleteShoppingCategoryItem = function (shoppingCategoryList) {
            shoppingCategoryLists.remove(shoppingCategoryList);
            datacontext.deleteShoppingCategoryItem(shoppingCategoryList)
                .fail(deleteFailed);

            function deleteFailed() {
                showshoppingCategoryList(shoppingCategoryList); // re-show the restored list
            }
        },
        showCheckoutList = function (checkoutList) {
            $("#shoppingForm").hide("slide", function () {
                $("#checkoutForm").show("slide", function () {

                    if (!datacontext.displayedCheckedOut) {
                        checkoutLists(new datacontext.createCheckoutList(datacontext.checkoutListResult));
                    }
                });
            });



        };

    datacontext.getShoppingCategoryLists(shoppingCategoryLists, error); // load shoppingCategoryLists

    return {
        shoppingCategoryLists: shoppingCategoryLists,
        checkoutLists: checkoutLists,
        error: error,
        addshoppingCategoryList: addshoppingCategoryList,
        deleteShoppingCategoryItem: deleteShoppingCategoryItem,
        addCheckoutList: addCheckoutList,
        finishCheckoutList: finishCheckoutList,
        showshoppingCategoryList: showshoppingCategoryList,
        showCheckoutList: showCheckoutList
    };

})(ko, shoppingApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.shoppingApp.shoppingCategoryListViewModel);

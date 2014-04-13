window.shoppingApp = window.shoppingApp || {};

window.shoppingApp.datacontext = (function () {

    var datacontext = {
        getShoppingCategoryLists: getShoppingCategoryLists,
        createShoppingItem: createShoppingItem,
        createShoppingCategoryList: createShoppingCategoryList,
        saveNewShoppingItem: saveNewShoppingItem,
        saveNewShoppingCategoryList: saveNewShoppingCategoryList,
        saveChangedShoppingItem: saveChangedShoppingItem,
        saveChangedShoppingCategoryList: saveChangedShoppingCategoryList,
        deleteShoppingItem: deleteShoppingItem,
        deleteShoppingCategoryItem: deleteShoppingCategoryItem,

        createCheckoutItem: createCheckoutItem,
        createCheckoutList: createCheckoutList,
        saveNewCheckoutList: saveNewCheckoutList,
        readAddedProductItemsFromCookie: readAddedProductItemsFromCookie,
        clearAddedProductItemsFromCookie: clearAddedProductItemsFromCookie,
        saveAddedProductItemsToCookie: saveAddedProductItemsToCookie,
        productsBoughtCookieName: 'productsBought'
    };

    return datacontext;

    function getShoppingCategoryLists(shoppingCategoryListsObservable, errorObservable) {
        return ajaxRequest("get", shoppingCategoryListUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedShoppingCategoryLists = $.map(data, function (list) {
                return new createShoppingCategoryList(list);
            });
            shoppingCategoryListsObservable(mappedShoppingCategoryLists);
        }

        function getFailed() {
            errorObservable("Error retrieving shoppingcategory lists.");
        }
    }
    function createShoppingItem(data) {
        return new datacontext.shoppingItem(data); // shoppingItem is injected by shoppingItem.model.js
    }
    function createShoppingCategoryList(data) {
        return new datacontext.shoppingCategoryList(data); // shoppingCategoryList is injected by shoppingItem.model.js
    }
    function createCheckoutItem(data) {
        return new datacontext.checkoutItem(data); // checkoutItem is injected by shoppingItem.model.js
    }
    function createCheckoutList(data) {
        return new datacontext.checkoutList(data); // checkoutList is injected by shoppingItem.model.js
    }
    function saveNewShoppingItem(shoppingItem) {
        clearErrorMessage(shoppingItem);
        return ajaxRequest("post", shoppingItemUrl(), shoppingItem)
            .done(function (result) {
                shoppingItem.shoppingItemId = result.shoppingItemId;
            })
            .fail(function () {
                shoppingItem.errorMessage("Error adding a new shoppingcategory item.");
            });
    }
    function saveNewShoppingCategoryList(shoppingCategoryList) {
        clearErrorMessage(shoppingCategoryList);
        return ajaxRequest("post", shoppingCategoryListUrl(), shoppingCategoryList)
            .done(function (result) {
                shoppingCategoryList.shoppingCategoryListId = result.shoppingCategoryListId;
                shoppingCategoryList.userId = result.userId;
            })
            .fail(function () {
                shoppingCategoryList.errorMessage("Error adding a new shoppingcategory list.");
            });
    }
    function saveNewCheckoutList(checkoutList) {        
        return ajaxRequest("post", checkoutListUrl(), checkoutList)
            .done(function (result) {
                checkoutList.checkoutListId = result.checkoutListId;
                checkoutList.userId = result.userId;
                datacontext.checkoutListResult = result;

            })
            .fail(function (obj1, obj) {
                alert('Error occured during checkout.')
            });
    }
    function deleteShoppingItem(shoppingItem) {
        return ajaxRequest("delete", shoppingItemUrl(shoppingItem.shoppingItemId))
            .fail(function () {
                shoppingItem.errorMessage("Error removing shoppingcategory item.");
            });
    }
    function deleteShoppingCategoryItem(shoppingCategoryList) {
        return ajaxRequest("delete", shoppingCategoryListUrl(shoppingCategoryList.shoppingCategoryListId))
            .fail(function () {
                shoppingCategoryList.errorMessage("Error removing shoppingcategory list.");
            });
    }
    function saveChangedShoppingItem(shoppingItem) {
        clearErrorMessage(shoppingItem);
        return ajaxRequest("put", shoppingItemUrl(shoppingItem.shoppingItemId), shoppingItem, "text")
            .fail(function () {
                shoppingItem.errorMessage("Error updating shoppingcategory item.");
            });
    }
    function saveChangedShoppingCategoryList(shoppingCategoryList) {
        clearErrorMessage(shoppingCategoryList);
        return ajaxRequest("put", shoppingCategoryListUrl(shoppingCategoryList.shoppingCategoryListId), shoppingCategoryList, "text")
            .fail(function () {
                shoppingCategoryList.errorMessage("Error updating the shoppingcategory list title. Please make sure it is non-empty.");
            });
    }
    function readAddedProductItemsFromCookie() {
        var boughtCookieVal = $.cookie(datacontext.productsBoughtCookieName);
        var addedProductItems = boughtCookieVal ? jQuery.parseJSON(boughtCookieVal) : null;
        return addedProductItems;
    }
    function clearAddedProductItemsFromCookie() {
        $.cookie(datacontext.productsBoughtCookieName, null);
    }
    function saveAddedProductItemsToCookie(serialisedCookieVal) {
        $.cookie(datacontext.productsBoughtCookieName, serialisedCookieVal);
    }

    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            }
        }
        return $.ajax(url, options);
    }
    // routes
    function checkoutItemUrl(id) { return "/api/checkoutitem/" + (id || ""); }
    function checkoutListUrl(id) { return "/api/checkoutlist/" + (id || ""); }
    function shoppingCategoryListUrl(id) { return "/api/shoppingcategorylist/" + (id || ""); }
    function shoppingItemUrl(id) { return "/api/shoppingitem/" + (id || ""); }

})();
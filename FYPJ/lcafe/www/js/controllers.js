angular.module('starter.controllers', [])
  .constant('salt', 'bfawuiobwfb79237bf2628vf763vf32if732vfb937bvf89g7ase89vf2378vf812v783g92g793')

  .controller('AuthCtrl', function($scope, $ionicHistory, Account, $rootScope, $state, $ionicPopup, md5, salt, Cart) {

    $scope.Account = {};
    $scope.dataSend = {};

    function popupLoginFail() {
      $ionicPopup.alert({
        title: 'Login Fail',
        subTitle: 'Invalid Email or Password.',
        okType: 'button-royal'
      }).then(function(res) {
        console.log("Login Fail");
      });
    }

    function popupEmailTaken() {
      $ionicPopup.alert({
        title: 'Sign Up Fail',
        subTitle: 'This is email is already taken!',
        okType: 'button-royal'
      }).then(function(res) {
        console.log('Sign Up Fail');
      });
    }

    function popupSuccessSignUp() {
      $ionicPopup.alert({
        title: 'Sign Up Sucesssful',
        subTitle: 'You may now procceed to login.',
        okType: 'button-royal'
      }).then(function(res) {
        $state.go('login');
        console.log('Sign Up Successful');
      });
    }

    $scope.createAcc = function(acc, form) {
      $scope.dataSend.fullname = $scope.Account.fullname;
      $scope.dataSend.email = $scope.Account.email;
      $scope.dataSend.phoneNumber = $scope.Account.phoneNumber;
      $scope.dataSend.password = md5.createHash($scope.Account.password + salt);
      $scope.dataSend.token = localStorage.getItem("accessToken");
      Account.checkDuplicateEmail(acc.email).then(function(res) {
        if (res.message != 'unsuccessful') {
          Account.create($scope.dataSend).then(function(res) {
            if (form) {
              $scope.Account = {};
              form.$setPristine();
              form.$setUntouched();
            }
            popupSuccessSignUp();
          });
        } else {
          popupEmailTaken();
        }
      });
    };

    $scope.login = function(l, form) {
      var passwordHash = md5.createHash(l.password + salt);
      Account.login(l.email, passwordHash).then(function(res) {
        if (res.message != 'unsuccessful') {
          var id = res.message;
          localStorage.setItem("accountId", id);
          Cart.setCustId(id);
          $rootScope.menuState = "login";
          console.log("Your id: " + localStorage.getItem("accountId"));
          Account.get(localStorage.getItem('accountId')).then(function(response) {
            localStorage.setItem("favouriteItemsID", response.favouriteItemsID);
          });
          if ($rootScope.payment != 'true') {
            $state.go('app.food');
          } else {
            $state.go('payment');
          }
        } else {
          popupLoginFail();
        }
      });
    };

  })

  .controller('HomeCtrl', function($scope, $state, FoodMenu, Cart, $rootScope, $ionicSideMenuDelegate, $ionicModal, $rootScope, $filter, $cordovaNetwork, $ionicPlatform, $ionicPopup) {
    document.addEventListener("deviceready", function() {

      // listen for Online event
      $rootScope.$on('$cordovaNetwork:online', function(event, networkState) {
        if ($rootScope.internetConnected == 'true') return;
        $rootScope.internetConnected = 'true';
      });

      // listen for Offline event
      $rootScope.$on('$cordovaNetwork:offline', function(event, networkState) {
        if ($rootScope.internetConnected == 'false') return;
        $rootScope.internetConnected = 'false';
        $ionicPopup.alert({
          title: 'No Internet Connection',
          subTitle: 'Sorry, no Internet connectivity detected. Please reconnect and try again.',
          okType: 'button-assertive'
        }).then(function(res) {
          ionic.Platform.exitApp();
        });
      });

    }, false);

    $scope.cart = Cart.get();

    $scope.sort = {
      value: 'itemSubCategory'
    };

    var tempArr = [];
    var uniqueArr = [];
    var checked = [];
    $scope.filterItems = [];
    $scope.selected = [];
    $scope.default = [];

    Array.prototype.contains = function(v) {
      for (var i = 0; i < this.length; i++) {
        if (this[i] === v) return true;
      }
      return false;
    };

    Array.prototype.unique = function() {
      var arr = [];
      for (var i = 0; i < this.length; i++) {
        if (!arr.contains(this[i])) {
          arr.push(this[i]);
        }
      }
      return arr;
    };

    FoodMenu.all().then(function(menu) {
      $scope.foodMenu = menu;
      for (var i = 0; i < menu.length; i++) {
        tempArr.push(menu[i].itemSubCategory);
      }
      uniqueArr = tempArr.unique();
      for (var j = 0; j < uniqueArr.length; j++) {
        var obj = {};
        obj.text = uniqueArr[j];
        obj.checked = true;
        $scope.filterItems.push(obj);
        $scope.selected.push(uniqueArr[j]);
        $scope.default.push(uniqueArr[j]);
      }
    });

    $scope.customFilter = function(item) {
      return ($scope.selected.indexOf(item.itemSubCategory) !== -1);
    };

    $scope.changeItem = function(item) {
      var enableditem = $filter('filter')($scope.filterItems, {
        checked: true
      });
      $scope.checkedArr = [];
      angular.forEach(enableditem, function(item) {
        $scope.checkedArr.push(item.text);
      });
      checked = $scope.checkedArr;
      console.log(checked);
    };

    $rootScope.logout = function() {
      localStorage.setItem('accountId', '');
      localStorage.setItem('favouriteItemsID', '');
      Cart.setCustId('');
      $rootScope.menuState = "notlogin";
      console.log("Your id: " + localStorage.getItem("accountId"));
      $ionicSideMenuDelegate.toggleLeft();
    };

    // =========================================================================Sorting Modal=========================================================================

    $scope.openSortModal = function() {
      $ionicModal.fromTemplateUrl('templates/sort.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.sortModal = modal;
        $scope.sortBy = angular.copy($scope.sort);
        $scope.sortModal.show();

      });
    };

    $scope.closeSortModal = function() {
      console.log($scope.sortBy);
      $scope.sort = angular.copy($scope.sortBy);
      $scope.sortModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.sortModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Filter Modal=========================================================================

    $scope.openFilterModal = function() {
      $ionicModal.fromTemplateUrl('templates/filter.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.filterModal = modal;
        $scope.filterModal.show();
      });
    };

    $scope.closeFilterModal = function() {
      if (checked.length > 0) {
        $scope.selected = checked;
      } else {
        $scope.selected = $scope.default;
      }
      console.log(checked);
      $scope.filterModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.filterModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Floating action button================================================================
    $scope.openSort = function() {
      $scope.openSortModal();
    };

    $scope.$on('floating-menu:open', function() {
      console.log("open");
    });
    $scope.$on('floating-menu:close', function() {
      console.log("close");
    });

  })

  .controller('DrinksCtrl', function($scope, $state, DrinkMenu, $rootScope, $ionicModal, $filter) {

    $scope.sort = {
      value: 'itemSubCategory'
    };

    var tempArr = [];
    var uniqueArr = [];
    var checked = [];
    $scope.filterItems = [];
    $scope.selected = [];
    $scope.default = [];

    Array.prototype.contains = function(v) {
      for (var i = 0; i < this.length; i++) {
        if (this[i] === v) return true;
      }
      return false;
    };

    Array.prototype.unique = function() {
      var arr = [];
      for (var i = 0; i < this.length; i++) {
        if (!arr.contains(this[i])) {
          arr.push(this[i]);
        }
      }
      return arr;
    };

    DrinkMenu.all().then(function(menu) {
      $scope.drinkMenu = menu;
      for (var i = 0; i < menu.length; i++) {
        tempArr.push(menu[i].itemSubCategory);
      }
      uniqueArr = tempArr.unique();
      for (var j = 0; j < uniqueArr.length; j++) {
        var obj = {};
        obj.text = uniqueArr[j];
        obj.checked = true;
        $scope.filterItems.push(obj);
        $scope.selected.push(uniqueArr[j]);
        $scope.default.push(uniqueArr[j]);
      }
    });

    $scope.customFilter = function(item) {
      return ($scope.selected.indexOf(item.itemSubCategory) !== -1);
    };

    $scope.changeItem = function(item) {
      var enableditem = $filter('filter')($scope.filterItems, {
        checked: true
      });
      $scope.checkedArr = [];
      angular.forEach(enableditem, function(item) {
        $scope.checkedArr.push(item.text);
      });
      checked = $scope.checkedArr;
      console.log(checked);
    };

    // =========================================================================Sorting Modal=========================================================================
    $scope.openSortModal = function() {
      $ionicModal.fromTemplateUrl('templates/sort.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.sortModal = modal;
        $scope.sortBy = angular.copy($scope.sort);
        $scope.sortModal.show();

      });
    };

    $scope.closeSortModal = function() {
      console.log($scope.sortBy);
      $scope.sort = angular.copy($scope.sortBy);
      $scope.sortModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.sortModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Filter Modal=========================================================================

    $scope.openFilterModal = function() {
      $ionicModal.fromTemplateUrl('templates/filter.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.filterModal = modal;
        $scope.filterModal.show();
      });
    };

    $scope.closeFilterModal = function() {
      if (checked.length > 0) {
        $scope.selected = checked;
      } else {
        $scope.selected = $scope.default;
      }
      console.log(checked);
      $scope.filterModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.filterModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Floating action button================================================================
    $scope.openSort = function() {
      $scope.openSortModal();
    };


    $scope.$on('floating-menu:open', function() {
      console.log("open");
    });

    $scope.$on('floating-menu:close', function() {
      console.log("close");
    });

  })

  .controller('SpecialCtrl', function($scope, $state, SpecialMenu, $rootScope, $ionicModal, $timeout, $filter) {

    $scope.sort = {
      value: 'itemSubCategory'
    };

    var tempArr = [];
    var uniqueArr = [];
    var checked = [];
    $scope.filterItems = [];
    $scope.selected = [];
    $scope.default = [];

    Array.prototype.contains = function(v) {
      for (var i = 0; i < this.length; i++) {
        if (this[i] === v) return true;
      }
      return false;
    };

    Array.prototype.unique = function() {
      var arr = [];
      for (var i = 0; i < this.length; i++) {
        if (!arr.contains(this[i])) {
          arr.push(this[i]);
        }
      }
      return arr;
    };

    SpecialMenu.all().then(function(menu) {
      $scope.specialMenu = menu;
      for (var i = 0; i < menu.length; i++) {
        tempArr.push(menu[i].itemSubCategory);
      }
      uniqueArr = tempArr.unique();
      for (var j = 0; j < uniqueArr.length; j++) {
        var obj = {};
        obj.text = uniqueArr[j];
        obj.checked = true;
        $scope.filterItems.push(obj);
        $scope.selected.push(uniqueArr[j]);
        $scope.default.push(uniqueArr[j]);
      }
    });

    $scope.customFilter = function(item) {
      return ($scope.selected.indexOf(item.itemSubCategory) !== -1);
    };

    $scope.changeItem = function(item) {
      var enableditem = $filter('filter')($scope.filterItems, {
        checked: true
      });
      $scope.checkedArr = [];
      angular.forEach(enableditem, function(item) {
        $scope.checkedArr.push(item.text);
      });
      checked = $scope.checkedArr;
      console.log(checked);
    };

    // =========================================================================Sorting Modal=========================================================================

    $scope.openSortModal = function() {
      $ionicModal.fromTemplateUrl('templates/sort.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.sortModal = modal;
        $scope.sortBy = angular.copy($scope.sort);
        $scope.sortModal.show();

      });
    };

    $scope.closeSortModal = function() {
      console.log($scope.sortBy);
      $scope.sort = angular.copy($scope.sortBy);
      $scope.sortModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.sortModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Filter Modal=========================================================================

    $scope.openFilterModal = function() {
      $ionicModal.fromTemplateUrl('templates/filter.html', {
        scope: $scope,
        animation: 'slide-in-up'
      }).then(function(modal) {
        $scope.filterModal = modal;
        $scope.filterModal.show();
      });
    };

    $scope.closeFilterModal = function() {
      if (checked.length > 0) {
        $scope.selected = checked;
      } else {
        $scope.selected = $scope.default;
      }
      console.log(checked);
      $scope.filterModal.remove();
    };

    $scope.$on('$destroy', function() {
      $scope.filterModal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    // =========================================================================Floating action button================================================================
    $scope.openSort = function() {
      $scope.openSortModal();
    };


    $scope.$on('floating-menu:open', function() {
      console.log("open");
    });

    $scope.$on('floating-menu:close', function() {
      console.log("close");
    });

  })

  .controller('ItemCtrl', function($scope, $state, Items, Cart, $stateParams, $ionicPopup, $timeout, $ionicHistory, Favourite, $rootScope) {

    var id = $stateParams.id;
    var itemToCart;
    var quantitySelected;

    $scope.dataSend = {};
    $scope.item = {};
    $scope.input = {};
    $scope.request = {
      special: '',
      item: false
    };

    function checkFav(id) {
      var FavItems = localStorage.getItem("favouriteItemsID").split(',');
      for (var i = 0; i < FavItems.length; i++) {
        if (FavItems[i] == id) {
          $scope.isFavourite = true;
          break;
        } else {
          $scope.isFavourite = false;
        }
      }
    }

    Items.getItem(id).then(function(response) {
      itemToCart = angular.copy(response);
      response.itemPrice = parseFloat(response.itemPrice).toFixed(2);
      $scope.item = response;
      console.log("Item id: " + id);
      checkFav(id);
      if (itemToCart.itemQty == 0) {
        quantitySelected = 0;
      } else {
        quantitySelected = 1;
      }

    }).then(function(response) {
      $scope.input.quantity = quantitySelected;
    });

    function showDuplicateAlert() {
      $ionicPopup.alert({
        title: 'Error',
        template: '<p class="dark">You have already added this item.</b>',
        buttons: [{
          text: 'Ok',
          type: 'button-royal'
        }]
      }).then(function(res) {
        console.log('Duplicate');
        // window.history.back();
        $ionicHistory.goBack();
      });
    }

    function showAddedAlert() {
      $ionicPopup.alert({
        title: 'Success',
        template: '<p class="dark">Item successfully added to cart.</b>',
        buttons: [{
          text: 'Ok',
          type: 'button-royal'
        }]
      }).then(function(res) {
        console.log('Added');
        // window.history.back();
        $ionicHistory.goBack();
      });
    }

    $scope.plusQty = function(item) {
      console.log("+");
      quantitySelected++;
      $scope.input.quantity = quantitySelected;
    };

    $scope.minusQty = function(item) {
      console.log("-");
      if ($scope.input.quantity > 1) {
        quantitySelected--;
        $scope.input.quantity = quantitySelected;
      }
    };

    $scope.addCart = function() {
      itemToCart.quantity = $scope.input.quantity;
      if ($scope.request.special == '') {
        itemToCart.specialRequest = "None";
      } else {
        itemToCart.specialRequest = $scope.request.special;
      }
      if ($scope.request.item) {
        itemToCart.itemRequest = $scope.item.itemRequest;
      } else {
        itemToCart.itemRequest = "None";
      }
      if (Cart.isDuplicate(itemToCart)) {
        showDuplicateAlert();
      } else {
        Cart.add(itemToCart);
        console.log("Before item: " + JSON.stringify($scope.item));
        console.log("After item: " + JSON.stringify(itemToCart));
        showAddedAlert();
      }
    };

    $scope.setFavTrue = function(itemId) {
      $scope.isFavourite = true;
      var FavItems = localStorage.getItem("favouriteItemsID");
      var updateFav = FavItems.concat(itemId + ',');
      localStorage.setItem("favouriteItemsID", updateFav);
      console.log(localStorage.getItem("favouriteItemsID"));
      console.log('set to true');
    };

    $scope.setFavFalse = function(itemId) {
      $scope.isFavourite = false;
      var FavItems = localStorage.getItem("favouriteItemsID").split(/(,)/);
      for (var i = 0; i < FavItems.length; i = i + 2) {
        if (FavItems[i] == itemId) {
          FavItems.splice(i, 2);
          break;
        }
      }
      localStorage.setItem("favouriteItemsID", FavItems.join(""));
      console.log(localStorage.getItem("favouriteItemsID"));
      console.log('set to false');
    };

    $scope.$on('$ionicView.leave', function() {
      if ($rootScope.menuState == 'login') {
        $scope.dataSend.custID = localStorage.getItem("accountId");
        $scope.dataSend.favouriteItemsID = localStorage.getItem("favouriteItemsID");
        $scope.dataSend.token = localStorage.getItem("accessToken");
        Favourite.updateFavourites($scope.dataSend).then(function(response) {
          console.log(response);
        });
      }
    });

  })

  .controller('FeedbackCtrl', function($scope, Feedback, $state, $ionicPopup) {

    $scope.fb = {
      subject: '',
      name: '',
      email: '',
      comment: '',
      service: 0,
      food: 0
    };

    $scope.rating = {};
    $scope.rating.food = 0;
    $scope.rating.service = 0;
    $scope.rating.max = 5;

    function popupSuccessFeedback() {
      $ionicPopup.alert({
        title: 'Thank you for your response',
        subTitle: 'Your response has been submitted successfully.',
        okType: 'button-royal'
      }).then(function(res) {
        console.log("Submitted");
      });
    }

    $scope.save = function(form) {
      $scope.fb.service = $scope.rating.service;
      $scope.fb.food = $scope.rating.food;
      $scope.fb.token = localStorage.getItem("accessToken");
      Feedback.create($scope.fb).then(function() {
        if (form) {
          $scope.fb = {};
          $scope.rating = {};
          form.$setPristine();
          form.$setUntouched();
        }
        popupSuccessFeedback();
      });
    };

  })

  .controller('CartCtrl', function($scope, $rootScope, $state, $ionicPopup, Cart, Promotions, $ionicContentBanner, $timeout) {

    var contentBannerInstance;

    $scope.cart = Cart.get();

    function popupOrdTypePrompt() {
      $ionicPopup.show({
        title: '<b>Confirmation</b>',
        subTitle: 'Do you want to dine-in or pre-order?',
        scope: $scope,
        buttons: [{
            text: 'Dine-in',
            type: 'button-royal',
            onTap: function(e) {
              $rootScope.ordType = "dine-in";
              $state.go('dine-in');
            }
          },
          {
            text: 'Pre-order',
            type: 'button-royal',
            onTap: function(e) {
              $rootScope.ordType = "pre-order";
              $state.go('preorder');
            }
          }
        ]
      });
    }

    function showBanner(bannerType, transition) {
      if (contentBannerInstance) {
        contentBannerInstance();
        contentBannerInstance = null;
      }
      contentBannerInstance = $ionicContentBanner.show({
        text: ['Hint : Swipe left to remove'],
        autoClose: 2000,
        type: bannerType,
        transition: transition || 'vertical'
      });
    }

    if ($scope.cart.items.length > 0) {
      $timeout(function() {
        showBanner('info', 'vertical');
      }, 250);
    }

    $scope.cartEmpty = function() {
      if ($scope.cart.items.length > 0) {
        return false;
      } else if ($scope.cart.items.length == 0) {
        return true;
      }
    };

    $scope.plusQty = function(item) {
      item.quantity++;
      Cart.get();
    };

    $scope.minusQty = function(item) {
      if (item.quantity == 2) {
        showBanner('info', 'vertical');
      }
      if (item.quantity > 1) {
        item.quantity--;
        Cart.get();
      }
    };

    $scope.remove = function(itemID) {
      Cart.remove(itemID);
      Cart.get();
    };

    $scope.submitOrder = function() {
      popupOrdTypePrompt();
    };

  })

  .controller('PromoCtrl', function($scope, $state, Promotions, $ionicSideMenuDelegate) {

    Promotions.getAll().then(function(res) {
      $scope.list = [];
      for (var i = 0; i < res.length; i++) {
        if (res[i].status == 1) {
          $scope.list.push(res[i]);
        }
      }
    });

    $scope.hasPromo = function(length) {
      if (length < 1) {
        return true;
      } else {
        return false;
      }
    }

    $ionicSideMenuDelegate.canDragContent(false);

  })

  .controller('topCtrl', function($scope, $state, Top, $ionicPopup) {

    $scope.items = [];

    Top.get().then(function(res) {
      for (var i = 0; i < res.length; i++) {
        res[i].number = i + 1;
        $scope.items.push(res[i]);
      }
      console.log($scope.items);
    });

  })

  .controller('favouriteCtrl', function($scope, $state, $ionicPopup, Favourite, $rootScope) {

    $scope.items = [];
    $scope.dataSend = {};

    localStorage.setItem("favIndex", '');

    Favourite.get(localStorage.getItem('accountId')).then(function(res) {
      for (var i = 0; i < res.length; i++) {
        $scope.items.push(res[i]);
      }
      console.log($scope.items);
    });

    $scope.init = function(itemId, index) {
      var FavItems = localStorage.getItem("favouriteItemsID").split(',');
      var FavIndex = localStorage.getItem("favIndex");
      var isFound = true;
      if (FavItems.length < 2) {
        $scope.items.splice(index, 1);
      }
      for (var i = 0; i < FavItems.length; i++) {
        if (FavItems[i] == itemId) {
          isFound = true;
          break;
        } else {
          isFound = false;
        }
      }
      if (isFound == false) {
        $scope.items.splice(index, 1);
      }
    };

    $scope.checkFav = function(id) {
      var isFavourite = true;
      var FavItems = localStorage.getItem("favouriteItemsID").split(',');
      for (var i = 0; i < FavItems.length; i++) {
        if (FavItems[i] == id) {
          isFavourite = true;
          break;
        } else {
          isFavourite = false;
        }
      }
      return isFavourite;
    };

    $scope.setFavTrue = function(itemId, index) {
      var FavItems = localStorage.getItem("favouriteItemsID");
      var FavIndex = localStorage.getItem("favIndex").split(/(,)/);
      var updateFav = FavItems.concat(itemId + ',');
      for (var i = 0; i < FavIndex.length; i = i + 2) {
        if (FavIndex[i] == index) {
          FavIndex.splice(i, 2);
          break;
        }
      }
      localStorage.setItem("favouriteItemsID", updateFav);
      localStorage.setItem("favIndex", FavIndex.join(""));
      console.log('favItem: ' + localStorage.getItem("favouriteItemsID"));
      console.log('favIndex: ' + localStorage.getItem("favIndex"));
      console.log('set to true: ' + index);
    };

    $scope.setFavFalse = function(itemId, index) {
      var FavItems = localStorage.getItem("favouriteItemsID").split(/(,)/);
      var FavIndex = localStorage.getItem("favIndex");
      var updateIndex = FavIndex.concat(index + ',');
      for (var i = 0; i < FavItems.length; i = i + 2) {
        if (FavItems[i] == itemId) {
          FavItems.splice(i, 2);
          break;
        }
      }
      localStorage.setItem("favouriteItemsID", FavItems.join(""));
      localStorage.setItem("favIndex", updateIndex);
      console.log('favItem: ' + localStorage.getItem("favouriteItemsID"));
      console.log('favIndex: ' + localStorage.getItem("favIndex"));
      console.log('set to false: ' + index);

    };

    function refresh() {
      if ($rootScope.menuState == 'login') {
        $scope.dataSend.custID = localStorage.getItem("accountId");
        $scope.dataSend.favouriteItemsID = localStorage.getItem("favouriteItemsID");
        $scope.dataSend.token = localStorage.getItem("accessToken");
        Favourite.updateFavourites($scope.dataSend).then(function(response) {
          console.log(response);
        }).then(function(res) {
          var FavIndex = localStorage.getItem("favIndex").split(',');
          FavIndex.sort(function(a, b) {
            return b - a;
          });
          for (var i = 0; i < FavIndex.length; i++) {
            if (FavIndex[i] != '') {
              $scope.items.splice(FavIndex[i], 1);
            }
          }
          localStorage.setItem("favIndex", '');
        });
      }
    }

    $scope.doRefresh = function() {
      refresh();
      $scope.$broadcast('scroll.refreshComplete');
    };

    $scope.$on('$ionicView.leave', function() {
      refresh();
    });


  })

  .controller('historyCtrl', function($scope, $state, $ionicSideMenuDelegate, MyHistory, $ionicContentBanner, $timeout) {

    var contentBannerInstance;

    function showBanner(bannerType, transition) {
      if (contentBannerInstance) {
        contentBannerInstance();
        contentBannerInstance = null;
      }
      contentBannerInstance = $ionicContentBanner.show({
        text: ['You have not order any items.'],
        autoClose: 2000,
        type: bannerType,
        transition: transition || 'vertical'
      });
    }

    $scope.orders = [];
    MyHistory.getOrders(localStorage.getItem('accountId')).then(function(res) {
      console.log(res);
      if (res != 'Empty') {
        for (var i = 0; i < res.length; i++) {
          // split opening hours by ":" delimeters
          // var arrayOfSplitsDateTime = res[i].orderDateTime.split(/( )/);
          // var date = arrayOfSplitsDateTime[0];
          var date = new Date(res[i].orderDateTime);
          res[i].orderDateTime = date;
          $scope.orders.push(res[i]);
        }
      } else {
        $timeout(function() {
          showBanner('info', 'vertical');
        }, 250);
      }
    });


  })

  .controller('ordDetailsCtrl', function($scope, $state, $stateParams, $ionicSideMenuDelegate, MyHistory) {

    var id = $stateParams.id;
    $scope.ordId = id;
    $scope.items = [];

    console.log("Order item Id: " + id);

    MyHistory.getOrderItems(id).then(function(res) {
      $scope.ordGrandTotal = parseFloat(res[0].grandTotal).toFixed(2);
      $scope.ordDiscount = parseFloat(res[0].discount).toFixed(2);
      $scope.ordGST = parseFloat(res[0].GST).toFixed(2);
      $scope.ordTotal = parseFloat(res[0].grandTotal - res[0].GST + res[0].discount).toFixed(2);
      $scope.orderType = res[0].orderType;
      $scope.preorderDateTime = res[0].preorderDateTime;
      $scope.tableNumber = res[0].tableNumber;
      $scope.orderDateTime = res[0].orderDateTime;
      $scope.InvoiceNo = res[0].InvoiceNo;
      $scope.PIN = res[0].PIN;
      for (var i = 0; i < res.length; i++) {
        res[i].itemPrice = parseFloat(res[i].itemPrice).toFixed(2);
        $scope.items.push(res[i]);
      }
    });

  })

  .controller('detailsCtrl', function($scope, $state, $ionicSideMenuDelegate, Account, $ionicModal, $ionicPopup, md5, salt) {

    $scope.Password = {};
    $scope.dataSend = {};

    Account.get(localStorage.getItem('accountId')).then(function(response) {
      $scope.details = response;
    });

    function popupInvalidCurrent() {
      $ionicPopup.alert({
        title: 'Password Change Fail',
        subTitle: 'Your current password is invalid!',
        okType: 'button-royal'
      }).then(function(res) {
        console.log('Password Change Fail');
      });
    }

    function popupSuccessReset() {
      $ionicPopup.alert({
        title: 'Password Change Sucesssful',
        subTitle: 'You may now try to login with new password.',
        okType: 'button-royal'
      }).then(function(res) {
        $scope.closeModal();
        console.log('Password Change Successful');
      });
    }

    $ionicModal.fromTemplateUrl('templates/changePassword.html', {
      scope: $scope,
      animation: 'slide-in-up'
    }).then(function(modal) {
      $scope.modal = modal;
    });

    $scope.openModal = function() {
      $scope.modal.show();
    };

    $scope.closeModal = function() {
      $scope.modal.hide();
    };

    $scope.$on('$destroy', function() {
      $scope.modal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });
    // Execute action on remove modal
    $scope.$on('modal.removed', function() {
      // Execute action
    });

    $scope.changePassword = function(acc, form) {
      $scope.dataSend.custID = localStorage.getItem("accountId");
      $scope.dataSend.custPassword = md5.createHash($scope.Password.new + salt);
      $scope.dataSend.token = localStorage.getItem("accessToken");
      var currentPassordHash = md5.createHash($scope.Password.current + salt);
      Account.checkCurrentPassword($scope.dataSend.custID, currentPassordHash).then(function(res) {
        if (res.message != 'unsuccessful') {
          Account.changePassword($scope.dataSend).then(function(response) {
            if (form) {
              $scope.Password = {};
              form.$setPristine();
              form.$setUntouched();
            }
            popupSuccessReset();
          });
        } else {
          popupInvalidCurrent();
        }
      });
    };

  })

  .controller('AddressCtrl', function($scope, $state) {

    function initialize() {

      // set up begining position
      var myLatlng = new google.maps.LatLng(1.380082, 103.849564);

      // set option for map
      var mapOptions = {
        center: myLatlng,
        zoom: 18,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };

      // init map
      var map = new google.maps.Map(document.getElementById("map"), mapOptions);

      var marker = new google.maps.Marker({
        position: myLatlng,
        title: "l'Cafe"
      });

      marker.setMap(map);

      // assign to stop
      $scope.map = map;
    }

    // load map when the ui is loaded
    $scope.init = function() {
      initialize();
    };

    $scope.openLink = function(url) {
      window.open(url, '_blank');
    };

  })

  .controller('AboutCtrl', function($scope, $state, CafeInfo, moment) {

    CafeInfo.get().then(function(res) {
      var day;
      var id;
      var isTermBreak = false;
      $scope.cafeInfos = res;

      // check today's day
      switch (moment().day()) {
        case 1:
          day = "Monday";
          id = 0;
          break;
        case 2:
          day = "Tuesday";
          id = 1;
          break;
        case 3:
          day = "Wednesday";
          id = 2;
          break;
        case 4:
          day = "Thursday";
          id = 3;
          break;
        case 5:
          day = "Friday";
          id = 4;
          break;
        default:
          id = -1;
      }

      CafeInfo.getTermBreak().then(function(res) {
        var a = moment();
        for (var i = 0; i < res.length; i++) {
          var from = new Date(res[i].DateFrom);
          var to = new Date(res[i].DateTo);
          to.setDate(to.getDate() + 1);
          var withinTermBreak = moment(a).isBetween(from, to);
          console.log("Is within term break: " + withinTermBreak);
          if (withinTermBreak) {
            isTermBreak = true;
            break;
          } else {
            isTermBreak = false;
          }
        }
        console.log(res);
      }).then(function() {
        if (isTermBreak) {
          $scope.status = 'Closed';
        } else {
          // get today's remarks
          if (id != -1) {
            $scope.todayRemark = res[id].remarks;
            $scope.todayDay = day;
            // split opening hours by ":" delimeters
            var todayOpeningHours = res[id].openinghours;
            var arrayOfSplitsOpen = todayOpeningHours.split(/(:)/);
            var hrOpen = arrayOfSplitsOpen[0];
            var minOpen = arrayOfSplitsOpen[2];

            // split closing hours by ":" delimeters
            var todayOpeningHours = res[id].closinghours;
            var arrayOfSplitsClose = todayOpeningHours.split(/(:)/);
            var hrClose = arrayOfSplitsClose[0];
            var minClose = arrayOfSplitsClose[2];

            // check between time
            var d = moment();
            var afterTime = moment().hour(hrOpen).minutes(minOpen);
            var beforeTime = moment().hour(hrClose).minutes(minClose);


            var thirtyMinutesFromNow = moment().hour(hrClose).minutes(minClose).subtract(1800, 'seconds');
            console.log("Is within an hour before closing: " + moment(d).isBetween(thirtyMinutesFromNow, beforeTime));
            console.log("Is between time: " + moment(d).isBetween(afterTime, beforeTime));

            if (moment(d).isBetween(afterTime, beforeTime)) {
              if (moment(d).isBetween(thirtyMinutesFromNow, beforeTime)) {
                $scope.status = 'ClosingSoon';
              } else {
                $scope.status = 'Open';
              }
            } else {
              $scope.status = 'Closed';
            }

          } else {
            $scope.status = 'Closed';
          }
        }
      });

    });

  })

  .controller('dineInCtrl', function($scope, $state, $cordovaBarcodeScanner, Cart, $ionicPopup, $rootScope, CafeInfo, moment, $q) {

    $scope.$on('$ionicView.enter', function() {
      $rootScope.ordType = "dine-in";
    });

    function cafeIsOpen() {
      var deferred = $q.defer();
      CafeInfo.get().then(function(res) {
        var day;
        var id;
        var isTermBreak = false;
        $scope.cafeInfos = res;

        // check today's day
        switch (moment().day()) {
          case 1:
            day = "Monday";
            id = 0;
            break;
          case 2:
            day = "Tuesday";
            id = 1;
            break;
          case 3:
            day = "Wednesday";
            id = 2;
            break;
          case 4:
            day = "Thursday";
            id = 3;
            break;
          case 5:
            day = "Friday";
            id = 4;
            break;
          default:
            id = -1;
        }

        CafeInfo.getTermBreak().then(function(res) {
          var a = moment();
          for (var i = 0; i < res.length; i++) {
            var from = new Date(res[i].DateFrom);
            var to = new Date(res[i].DateTo);
            to.setDate(to.getDate() + 1);
            var withinTermBreak = moment(a).isBetween(from, to);
            console.log("Is within term break: " + withinTermBreak);
            if (withinTermBreak) {
              isTermBreak = true;
              break;
            } else {
              isTermBreak = false;
            }
          }
          console.log(res);
        }).then(function() {
          if (isTermBreak) {
            $scope.status = 'Closed';
          } else {
            // get today's remarks
            if (id != -1) {
              // split opening hours by ":" delimeters
              var todayOpeningHours = res[id].openinghours;
              var arrayOfSplitsOpen = todayOpeningHours.split(/(:)/);
              var hrOpen = arrayOfSplitsOpen[0];
              var minOpen = arrayOfSplitsOpen[2];

              // split closing hours by ":" delimeters
              var todayOpeningHours = res[id].closinghours;
              var arrayOfSplitsClose = todayOpeningHours.split(/(:)/);
              var hrClose = arrayOfSplitsClose[0];
              var minClose = arrayOfSplitsClose[2];

              // check between time
              var d = moment();
              var afterTime = moment().hour(hrOpen).minutes(minOpen);
              var beforeTime = moment().hour(hrClose).minutes(minClose);

              console.log("Is between time: " + moment(d).isBetween(afterTime, beforeTime));
              if (moment(d).isBetween(afterTime, beforeTime)) {
                $scope.status = 'Open';
              } else {
                $scope.status = 'Closed';
              }

            } else {
              $scope.status = 'Closed';
            }
          }
          console.log('Current status is: ' + $scope.status);
          if ($scope.status == 'Closed') {
            deferred.resolve(false);
          } else {
            deferred.resolve(true);
          }
        });
      });
      return deferred.promise;
    }

    cafeIsOpen().then(function(res) {
      console.log(res);
    });

    function popupCafeClose() {
      $ionicPopup.alert({
        title: 'Cafe is closed now',
        subTitle: 'Please check out our opening hours.',
        okType: 'button-royal'
      }).then(function(res) {
        console.log("Cafe closed");
      });
    }

    function popupScanSucess(value) {
      $ionicPopup.alert({
        title: 'Scan Successful',
        template: 'Your table number is : ' + value,
        okType: 'button-royal'
      }).then(function(res) {
        console.log('Table number: ' + value);
        if (localStorage.getItem("accountId") == "") {
          $rootScope.payment = "true";
          $state.go('login');
        } else {
          $state.go('payment');
        }
      });
    }

    $scope.scan = function() {
      cafeIsOpen().then(function(res) {
        if (res) {
          $cordovaBarcodeScanner
            .scan()
            .then(function(barcodeData) {
              Cart.setDineIn(barcodeData.text);
              popupScanSucess(barcodeData.text);
            }, function(error) {
              console.log(error);
            });
        } else {
          popupCafeClose();
        }
      });
    };

    $scope.enterTableNum = function() {
      cafeIsOpen().then(function(res) {
        if (res) {
          $scope.data = {};
          $ionicPopup.show({
            templateUrl: 'templates/tableNumber.html',
            title: 'Enter your table number',
            scope: $scope,
            buttons: [{
              text: 'Cancel'
            }, {
              text: '<b>Okay</b>',
              type: 'button-royal',
              onTap: function(e) {
                if (!$scope.data.tempTable) {
                  e.preventDefault();
                } else {
                  return $scope.data.tempTable;
                }
              }
            }, ]
          }).then(function(res) {
            if (res != undefined) {
              Cart.setDineIn(res);
              popupScanSucess(res);
            }
          });
        } else {
          popupCafeClose();
        }
      });
    };

  })

  .controller('preorderCtrl', function($scope, $state, $rootScope, Cart, $ionicPopup, CafeInfo, $q) {

    $scope.$on('$ionicView.enter', function() {
      $rootScope.ordType = "pre-order";
    });

    function cafeIsOpen(selectedDT) {
      var deferred = $q.defer();
      CafeInfo.get().then(function(res) {
        var day;
        var id;
        var isTermBreak = false;
        $scope.cafeInfos = res;

        // check today's day
        switch (parseInt(moment(selectedDT).format('E'))) {
          case 1:
            day = "Monday";
            id = 0;
            break;
          case 2:
            day = "Tuesday";
            id = 1;
            break;
          case 3:
            day = "Wednesday";
            id = 2;
            break;
          case 4:
            day = "Thursday";
            id = 3;
            break;
          case 5:
            day = "Friday";
            id = 4;
            break;
          default:
            id = -1;
        }

        CafeInfo.getTermBreak().then(function(res) {
          var a = moment(selectedDT);
          for (var i = 0; i < res.length; i++) {
            var from = new Date(res[i].DateFrom);
            var to = new Date(res[i].DateTo);
            to.setDate(to.getDate() + 1);
            var withinTermBreak = moment(a).isBetween(from, to);
            console.log("Is within term break: " + withinTermBreak);
            if (withinTermBreak) {
              isTermBreak = true;
              break;
            } else {
              isTermBreak = false;
            }
          }
          console.log(res);
        }).then(function() {
          if (isTermBreak) {
            $scope.status = 'Closed';
          } else {
            // get today's remarks
            if (id != -1) {
              // split opening hours by ":" delimeters
              var todayOpeningHours = res[id].openinghours;
              var arrayOfSplitsOpen = todayOpeningHours.split(/(:)/);
              var hrOpen = arrayOfSplitsOpen[0];
              var minOpen = arrayOfSplitsOpen[2];

              // split closing hours by ":" delimeters
              var todayOpeningHours = res[id].closinghours;
              var arrayOfSplitsClose = todayOpeningHours.split(/(:)/);
              var hrClose = arrayOfSplitsClose[0];
              var minClose = arrayOfSplitsClose[2];

              // check between time
              var selectedDate = new Date(selectedDT);
              var date = new Date();
              var datestring = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " +
                selectedDate.getHours() + ":" + selectedDate.getMinutes();
              var formatDate = new Date(datestring);
              var d = moment(formatDate);
              var afterTime = moment().hour(hrOpen).minutes(minOpen);
              var beforeTime = moment().hour(hrClose).minutes(minClose);
              console.log("Is between time: " + moment(d).isBetween(afterTime, beforeTime));
              if (moment(d).isBetween(afterTime, beforeTime)) {
                $scope.status = 'Open';
              } else {
                $scope.status = 'Closed';
              }
            } else {
              $scope.status = 'Closed';
            }
          }
          console.log(id);
          console.log('That day status is: ' + $scope.status);
          if ($scope.status == 'Closed') {
            deferred.resolve(false);
          } else {
            deferred.resolve(true);
          }
        });
      });
      return deferred.promise;
    }

    function popupCafeClose() {
      $ionicPopup.alert({
        title: 'Cafe is closed on selected datetime',
        subTitle: 'Please check out our opening hours.',
        okType: 'button-royal'
      }).then(function(res) {
        console.log("Cafe closed");
      });
    }

    // set pre-order date and time
    var coeff = 1000 * 60 * 5;
    var date = new Date();
    date.setHours(date.getHours() + 1);
    var rounded = new Date(Math.round(date.getTime() / coeff) * coeff);

    $scope.preorder = {
      value: rounded
    };

    $scope.continue = function() {
      var dt = moment($scope.preorder.value).format('YYYY-MM-DD HH:mm:ss');
      cafeIsOpen(dt).then(function(res) {
        if (res) {
          Cart.setPreorder(dt);
          console.log("Pre-order dateTime: " + dt);
          if (localStorage.getItem("accountId") == "") {
            $rootScope.payment = "true";
            $state.go('login');
          } else {
            $state.go('payment');
          }
        } else {
          popupCafeClose();
        }
      });


    };

  })

  .controller('paymentCtrl', function($scope, $rootScope, $state, $ionicPopup, $ionicLoading, Cart, Account, $ionicModal) {

    var cart = Cart.get();

    Account.get(localStorage.getItem("accountId")).then(function(res) {
      $scope.paymentForm = {
        reference: 'CC843154841254',
        name: res.custFullName,
        identifier: 'CC6541564531641321',
        amount: '$' + cart.grandTotal
      };
    });

    function popupPaymentSuccess() {
      $ionicPopup.alert({
        title: 'Order Submitted',
        subTitle: '<p class="dark">Your order has been submitted successfully.</p>',
        okType: 'button-royal'
      }).then(function(res) {
        console.log("Order type: " + $rootScope.ordType);
        if ($rootScope.ordType == 'dine-in') {
          $scope.openModal();
        } else {
          $state.go('app.food');
        }
      });
    }

    function popupPaymentFail() {
      $ionicPopup.alert({
        title: 'Order not Submitted',
        subTitle: 'Not enough stock, please re-select your food items',
        okType: 'button-royal'
      }).then(function(res) {
        $state.go('app.food');
        console.log("Not enough stock");
      });
    }

    function load() {
      $ionicLoading.show({
        template: '<p>Payment Processing...</p><ion-spinner></ion-spinner>',
        noBackdrop: true,
        animation: 'fade-in'
      }).then(function() {
        Cart.create(cart).then(function(response) {
          console.log("Message: " + JSON.stringify(response));
          $rootScope.payment = "false";
          $ionicLoading.hide();
          Cart.empty();
          if (response.message != -1) {
            $scope.pin = response.message;
            if ($scope.pin != null) {
              popupPaymentSuccess();
            }
          } else {
            popupPaymentFail();
          }

        });
      });
    }

    $ionicModal.fromTemplateUrl('templates/pin.html', {
      scope: $scope,
      animation: 'slide-in-up'
    }).then(function(modal) {
      $scope.modal = modal;
    });

    $scope.openModal = function() {
      $scope.modal.show();
    };

    $scope.closeModal = function() {
      $scope.modal.hide();
      $state.go('app.food');
    };

    $scope.$on('$destroy', function() {
      $scope.modal.remove();
    });

    $scope.$on('modal.hidden', function() {
      // Execute action
    });

    $scope.$on('modal.removed', function() {
      // Execute action
    });

    $scope.pay = function() {
      load();
    };

  });

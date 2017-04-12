var app = angular.module('lcafe', ['ionic', 'starter.controllers', 'starter.services', 'ngCordova', 'ion-datetime-picker', 'ion-floating-menu', 'angular-md5', 'ionic.rating', 'jett.ionic.content.banner', 'angularMoment'])
  .config(function($stateProvider, $urlRouterProvider) {
    $stateProvider.state('app', {
      url: '/app',
      abstract: true,
      templateUrl: 'templates/tabs.html'
    });
    $stateProvider.state('app.food', {
      url: '/food',
      views: {
        'tab-home': {
          templateUrl: 'templates/food.html',
          controller: 'HomeCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('app.drink', {
      url: '/drink',
      views: {
        'tab-drink': {
          templateUrl: 'templates/drink.html',
          controller: 'DrinksCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('app.special', {
      url: '/special',
      views: {
        'tab-special': {
          templateUrl: 'templates/special.html',
          controller: 'SpecialCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('app.foodItem', {
      url: '/item/:id',
      views: {
        'tab-home': {
          templateUrl: 'templates/item.html',
          controller: 'ItemCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('app.drinkItem', {
      url: '/item/:id',
      views: {
        'tab-drink': {
          templateUrl: 'templates/item.html',
          controller: 'ItemCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('app.specialItem', {
      url: '/item/:id',
      views: {
        'tab-special': {
          templateUrl: 'templates/item.html',
          controller: 'ItemCtrl',
          cache: false
        }
      }
    });
    $stateProvider.state('item', {
      url: '/item/:id',
      templateUrl: 'templates/item.html',
      controller: 'ItemCtrl',
      cache: false
    });
    $stateProvider.state('promotions', {
      url: '/promotions',
      templateUrl: 'templates/promotions.html',
      controller: 'PromoCtrl',
      cache: false
    });
    $stateProvider.state('cart', {
      url: '/cart',
      templateUrl: 'templates/cart.html',
      controller: 'CartCtrl',
      cache: false
    });
    $stateProvider.state('history', {
      url: '/history',
      templateUrl: 'templates/history.html',
      controller: 'historyCtrl',
      cache: false
    });
    $stateProvider.state('order-details', {
      url: '/order-details/:id',
      templateUrl: 'templates/order-details.html',
      controller: 'ordDetailsCtrl',
      cache: false
    });
    $stateProvider.state('about-us', {
      url: '/about-us',
      templateUrl: 'templates/about-us.html',
      controller: 'AboutCtrl',
      cache: false
    });
    $stateProvider.state('contact-us', {
      url: '/contact-us',
      templateUrl: 'templates/contact-us.html',
      controller: 'AddressCtrl',
      cache: false
    });
    $stateProvider.state('login', {
      url: '/login',
      templateUrl: 'templates/login.html',
      controller: 'AuthCtrl',
      cache: false
    });
    $stateProvider.state('register', {
      url: '/register',
      templateUrl: 'templates/register.html',
      controller: 'AuthCtrl',
      cache: false
    });
    $stateProvider.state('feedback', {
      url: '/feedback',
      templateUrl: 'templates/feedback.html',
      controller: 'FeedbackCtrl',
      cache: false
    });
    $stateProvider.state('my-details', {
      url: '/my-details',
      templateUrl: 'templates/my-details.html',
      controller: 'detailsCtrl',
      cache: false
    });
    $stateProvider.state('top', {
      url: '/top',
      templateUrl: 'templates/top.html',
      controller: 'topCtrl',
      cache: false
    });
    $stateProvider.state('favourite', {
      url: '/favourite',
      templateUrl: 'templates/favourite.html',
      controller: 'favouriteCtrl',
      cache: false
    });
    $stateProvider.state('dine-in', {
      url: '/dine-in',
      templateUrl: 'templates/dine-in.html',
      controller: 'dineInCtrl',
      cache: false
    });
    $stateProvider.state('preorder', {
      url: '/preorder',
      templateUrl: 'templates/preorder.html',
      controller: 'preorderCtrl',
      cache: false
    });
    $stateProvider.state('payment', {
      url: '/payment',
      templateUrl: 'templates/payment.html',
      controller: 'paymentCtrl',
      cache: false
    });
    $urlRouterProvider.otherwise('/app/food');
  })
  .config(function($ionicConfigProvider) {
    // $ionicConfigProvider.tabs.position('top');
    // $ionicConfigProvider.tabs.style('striped')
    // $ionicConfigProvider.backButton.text('Go Back').icon('ion-chevron-left');
  })
  .run(function($ionicPlatform, $http, Token, $rootScope, Cart) {
    $rootScope.favouriteState = 'false';
    // initialize
    var acccountId = localStorage.getItem('accountId');
    console.log("Your token: " + localStorage.getItem("accessToken"));
    console.log("Your id：" + acccountId);
    if (acccountId == null) {
      localStorage.setItem('accountId', '');
    }

    var favourite = localStorage.getItem("favouriteItemsID");
    if (favourite == null) {
      localStorage.setItem("favouriteItemsID", '');
    }

    //check token if it is stored in localDB
    if (localStorage.getItem("accessToken") == null) {
      Token.get().then(function(response) {
        localStorage.setItem("accessToken", response.token);
        console.log("New token: " + localStorage.getItem("accessToken"));
        console.log("Your id：" + localStorage.getItem('accountId'));
      });
    } else {
      //if there is a token, check if token is valid
      Token.checkToken(localStorage.getItem("accessToken")).then(function(response) {
        if (response.message == "unsuccessful") {
          Token.get().then(function(response) {
            //not valid
            console.log("Old token: " + response.token);
            localStorage.setItem("accessToken", response.token);
          });
        }
      });
    }

    // run ionic platform
    $ionicPlatform.ready(function() {

      //check if logged in
      if (localStorage.getItem("accountId") == null || localStorage.getItem("accountId") == '') {
        $rootScope.menuState = "notlogin"
      } else {
        $rootScope.menuState = "login"
        Cart.setCustId(localStorage.getItem("accountId"));
      }
      if (window.cordova && window.cordova.plugins.Keyboard) {
        // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
        // for form inputs)
        cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
        // Don't remove this line unless you know what you are doing. It stops the viewport
        // from snapping when text inputs are focused. Ionic handles this internally for
        // a much nicer keyboard experience.
        cordova.plugins.Keyboard.disableScroll(true);
      }
      if (window.cordova && window.cordova.InAppBrowser) {
        window.open = window.cordova.InAppBrowser.open;
      }
      if (window.StatusBar) {
        StatusBar.styleDefault();
      }
    });


  });

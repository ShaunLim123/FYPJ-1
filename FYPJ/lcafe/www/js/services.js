angular.module('starter.services', [])
  .constant('APIurl', 'http://172.20.129.181/WebServiceLCafe3/')
  .constant('Accesskey', 'W7djP08dGhXqld1g3RXpz')
  .directive('errSrc', function() {
    return {
      link: function(scope, element, attrs) {
        element.bind('error', function() {
          if (attrs.src != attrs.errSrc) {
            attrs.$set('src', attrs.errSrc);
          }
        });
      }
    };
  })
  .directive('confirmPassword', function() {
    return {
      require: 'ngModel',
      scope: {
        password: '=confirmPassword'
      },
      link: function(scope, elm, attrs, ctrl) {
        ctrl.$validators.confirmPassword = function(modelValue, viewValue) {
          if (ctrl.$isEmpty(modelValue)) {
            // consider empty models to be valid
            return true;
          }

          return (modelValue === scope.password);
        };

        // If the user changes the password, then we have to re-validate the confirm password input
        scope.$watch('password', function() {
          if (scope.password) ctrl.$validate();
        });
      }
    };
  });

app.factory('FoodMenu', function($http, APIurl) {
  return {
    all: function() {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveMenu?itemCategory=';
      return $http({
        method: 'GET',
        url: url + "food&label=" + "&token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('DrinkMenu', function($http, APIurl) {
  return {
    all: function() {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveMenu?itemCategory=';
      return $http({
        method: 'GET',
        url: url + "beverages&label=" + "&token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('SpecialMenu', function($http, APIurl) {
  return {
    all: function() {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveMenu?itemCategory=';
      return $http({
        method: 'GET',
        url: url + "&label=special&token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('Items', function($http, APIurl) {
  return {
    getItem: function(itemId) {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveMenuItem';
      return $http({
        method: 'GET',
        url: url,
        params: {
          itemID: itemId,
          token: localStorage.getItem("accessToken")
        },
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('Account', function($http, Cart, APIurl) {
  return {
    login: function(username, password) {
      var url = APIurl + 'WebserviceCafe.asmx/login';
      return $http({
        method: 'GET',
        url: url,
        params: {
          custEmail: username,
          custPassword: password,
          token: localStorage.getItem("accessToken")
        },
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    },
    create: function(a) {
      var url = APIurl + 'WebserviceCafe.asmx/createAccount';
      return $http.post(url, a);
    },
    get: function(id) {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveAccountById?';
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken") + "&id=" + id,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    },
    checkDuplicateEmail: function(email) {
      var url = APIurl + "WebserviceCafe.asmx/checkDuplicateEmail?";
      return $http({
        method: 'GET',
        url: url + "email=" + email + "&token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data
      });
    },
    checkCurrentPassword: function(custId, currentPassword) {
      var url = APIurl + 'WebserviceCafe.asmx/checkCurrentPassword';
      return $http({
        method: 'GET',
        url: url,
        params: {
          custID: custId,
          custPassword: currentPassword,
          token: localStorage.getItem("accessToken")
        },
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    },
    changePassword: function(a) {
      var url = APIurl + 'WebserviceCafe.asmx/resetPassword';
      return $http.post(url, a);
    },
  };
});

app.factory('CafeInfo', function($http, APIurl) {
  return {
    get: function(itemId) {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveAllInfo?';
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    },
    getTermBreak: function(itemId) {
      var url = APIurl + 'WebserviceCafe.asmx/retrieveAllTermBreaks?';
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('Cart', function($http, moment, APIurl, $q) {
  var cartObject = {
    discount: 0.0,
    GST: 0.0,
    orderTotalPrice: 0.0,
    grandTotal: 0.0,
    tableNumber: '',
    orderType: '',
    preorderDateTime: '',
    custID: '',
    items: [],
    comboMessage: []
  };

  var cart = JSON.parse(localStorage.getItem('cartItems'));

  if (cart == null) {
    localStorage.setItem('cartItems', JSON.stringify(cartObject));
    cart = JSON.parse(localStorage.getItem('cartItems'));
  }

  function persist() {
    localStorage.setItem('cartItems', JSON.stringify(cart));
  }

  function clear() {
    cart.discount = 0.0;
    cart.GST = 0.0;
    cart.orderTotalPrice = 0.0;
    cart.grandTotal = 0.0;
    cart.tableNumber = '';
    cart.orderType = '',
      cart.preorderDateTime = '';
    cart.items = [];
    cart.comboMessage = [];
  }

  function checkPromoStatus() {
    var url = APIurl + "WebserviceCafe.asmx/retrievePromoActivation";
    return $http({
      method: 'GET',
      url: url,
      params: {
        token: localStorage.getItem("accessToken")
      }
    }).then(function successCallback(response) {
      return response.data;
    }, function errorCallback(response) {
      return response.data;
    });
  }

  function checkTime() {
    var d = moment();
    var afterTime = moment().hour(07).minutes(30).seconds(0);
    var beforeTime = moment().hour(18).minutes(30).seconds(0);
    return moment(d).isBetween(afterTime, beforeTime);
  }

  function checkCombo3(array) {
    var valuesSoFar = Object.create(null);
    for (var i = 0; i < array.length; ++i) {
      var value = array[i];
      if (value in valuesSoFar) {
        return true;
      }
      valuesSoFar[value] = true;
    }
    return false;
  }

  function combo1n2n3(combo1, combo2, combo3) {

    var longblackIce = 0;
    var longblackHot = 0;
    var iceTea = 0;
    var hotTea = 0;
    var iceBlended = 0;
    var food = 0;
    var drinks = 0;
    var discount = 0;

    // categorize them accordingly and get their quantity
    for (var i = 0; i < cart.items.length; i++) {
      var quantity = cart.items[i].quantity;
      var itemName = cart.items[i].itemName;
      var itemPrice = cart.items[i].itemPrice;
      var itemCategory = cart.items[i].itemCategory;
      var itemSubCategory = cart.items[i].itemSubCategory;

      if (itemName == "Coffee / Long Black (Hot)") {
        longblackHot += quantity;
      } else if (itemName == "Coffee / Long Black (Cold)") {
        longblackIce += quantity;
      } else if (itemSubCategory == "Tea") {
        if (itemName.toLowerCase().includes("hot")) {
          hotTea += quantity;
        } else {
          iceTea += quantity;
        }
      } else if (itemSubCategory == "Ice Blended") {
        iceBlended += quantity;
      } else if (itemCategory == "Food") {
        food += quantity;
      }
    }

    // calculate combo 3 first
    if (combo3 && checkTime()) { // after 5.30
      var discountItems = [];
      var combo3Arr = [];
      var combo3Objects = {};
      var combo3ObjectsToBeDiscount = {};

      // put all combo 3 items into array (item name only)
      for (var i = 0; i < cart.items.length; i++) {
        var combo = cart.items[i].combo;
        var itemName = cart.items[i].itemName;
        var quantity = cart.items[i].quantity;
        if (combo == 3) {
          for (var j = 0; j < quantity; j++) {
            combo3Arr.push(itemName);
          }
        }
      }

      // convert and categorize items from array to objects
      combo3Objects = combo3Arr.reduce(function(prev, item) {
        if (item in prev)
          prev[item]++;
        else
          prev[item] = 1;

        return prev;
      }, {});

      // clone new object for discount
      combo3ObjectsToBeDiscount = Object.assign({}, combo3Objects);

      // 1) identify discount quantity for respective items (buy 1 get 1 free logic)
      // 2) retrieve the price of each discount item by name
      // 3) put all the price into array
      for (var key in combo3ObjectsToBeDiscount) {
        if (combo3ObjectsToBeDiscount.hasOwnProperty(key)) {
          var qty = combo3ObjectsToBeDiscount[key];
          combo3ObjectsToBeDiscount[key] = Math.floor(qty / 2);
          var discountQty = combo3ObjectsToBeDiscount[key];
          for (var i = 0; i < cart.items.length; i++) {
            var itemName = cart.items[i].itemName;
            var itemPrice = cart.items[i].itemPrice;
            if (key == itemName) {
              for (var j = 0; j < discountQty; j++) {
                discountItems.push(itemPrice);
              }
            }
          }
        }
      }

      for (var i = 0; i < discountItems.length; i++) {
        discount += discountItems[i];
      }

      food -= discountItems.length;
    }

    // calculate combo 1n2 next
    drinks = longblackIce + longblackHot + iceTea + hotTea + iceBlended;
    if (food > 0 && drinks > 0) {
      if (combo1 && combo2) { // combo 1,2 are active
        var totalNumDrinks = longblackIce + longblackHot + iceTea + hotTea + iceBlended;
        var hotDrinks = hotTea + longblackHot;
        var coldDrinks = iceTea + longblackIce + iceBlended;
        if (food >= totalNumDrinks) {
          discount += (hotDrinks * 0.80) + (coldDrinks * 1.00);
        } else if (food < totalNumDrinks) {
          var coldDrinkDiscount = 0.0;
          var hotDrinkDiscount = 0.0;
          if (coldDrinks > 0) {
            if (coldDrinks >= food) {
              coldDrinkDiscount = food * 1.00;
              food -= food;
            } else {
              coldDrinkDiscount = coldDrinks * 1.00;
              food -= coldDrinks;
            }
            hotDrinkDiscount = food * 0.80;
            discount += hotDrinkDiscount + coldDrinkDiscount;
          } else {
            discount += food * 0.80;
          }
        }
      } else if (combo1 && !combo2) { // only combo 1 is active
        var totalNumDrinks = longblackIce + longblackHot + iceTea + hotTea;
        var hotDrinks = hotTea + longblackHot;
        var coldDrinks = iceTea + longblackIce;
        if (food >= totalNumDrinks) {
          discount += (hotDrinks * 0.80) + (coldDrinks * 1.00);
        } else if (food < totalNumDrinks) {
          var coldDrinkDiscount = 0.0;
          var hotDrinkDiscount = 0.0;
          if (hotDrinks > 0) {
            if (coldDrinks > food) {
              coldDrinkDiscount = food * 1.00;
              food -= food;
            } else {
              coldDrinkDiscount = coldDrinks * 1.00;
              totalNum -= coldDrinks;
            }
            hotDrinkDiscount = food * 0.80;
            discount += hotDrinkDiscount + coldDrinkDiscount;
          } else {
            discount += food * 0.80;
          }
        }
      } else if (!combo1 && combo2) { // only combo2 is active
        var totalNumDrinks = iceBlended;
        if (food >= totalNumDrinks) {
          discount += totalNumDrinks * 1.00;
        } else if (food < totalNumDrinks) {
          discount += food * 1.00;
        }
      }
    }
    return discount;
  }

  function displayDiscount(combo1, combo2, combo3) {

    var food = 0
    var combolist = [];
    var combo1Items = 0;
    var combo2Items = 0;
    var combo3Arr = [];
    var freeFood = 0;
    var combo3Objects = {};
    var hasCombo3 = false;

    for (var i = 0; i < cart.items.length; i++) {
      var quantity = cart.items[i].quantity;
      var combo = cart.items[i].combo;
      var itemName = cart.items[i].itemName;
      var itemCategory = cart.items[i].itemCategory;
      if (combo == 1) {
        combo1Items += quantity;
      } else if (combo == 2) {
        combo2Items += quantity;
      } else if (combo == 3 || itemCategory == "Food") {
        food += quantity;
        if (combo == 3 && checkTime()) {
          for (var j = 0; j < quantity; j++) {
            combo3Arr.push(itemName);
          }
        }
      }

    }

    // convert and categorize items from array to objects
    combo3Objects = combo3Arr.reduce(function(prev, item) {
      if (item in prev)
        prev[item]++;
      else
        prev[item] = 1;

      return prev;
    }, {});

    // 1) identify discount quantity for respective items (buy 1 get 1 free logic)
    // 2) calculate the number of free food
    for (var key in combo3Objects) {
      if (combo3Objects.hasOwnProperty(key)) {
        var qty = combo3Objects[key];
        freeFood += Math.floor(qty / 2);
      }
    }

    hasCombo3 = checkCombo3(combo3Arr);

    if (combo1 && !combo2 && !combo3) { // only combo 1 is active
      if (combo1Items > 0 && food > 0) {
        combolist.push("Combo 1");
      }
    } else if (!combo1 && combo2 && !combo3) { // only combo 2 is active
      if (combo2Items > 0 && food > 0) {
        combolist.push("Combo 2");
      }
    } else if (!combo1 && !combo2 && combo3) { // only combo 3 is active
      if (hasCombo3) {
        combolist.push("1 FOR 1");
      }
    } else if (combo1 && combo2 && !combo3) { // Combo 1,2 are active
      if (combo1Items > 0 && food > 0 && combo2Items > 0) {
        combolist.push("Combo 2");
        if (food > combo2Items) {
          combolist.push("Combo 1");
        }
      } else if (combo1Items == 0 && food > 0 && combo2Items > 0) {
        combolist.push("Combo 2");
      } else if (combo1Items > 0 && food > 0 && combo2Items == 0) {
        combolist.push("Combo 1");
      }
    } else if (combo1 && !combo2 && combo3) { // cobo 1,3 are active
      if (hasCombo3) {
        combolist.push("1 FOR 1");
      }
      if (combo1Items > 0 && food > 0) {
        combolist.push("Combo 1");
      }
    } else if (!combo1 && combo2 && combo3) { // combo 2,3 are active
      if (hasCombo3) {
        combolist.push("1 FOR 1");
      }
      if (combo2Items > 0 && food > 0) {
        combolist.push("Combo 2");
      }
    } else if (combo1 && combo2 && combo3) { // combo 1,2,3 are active
      if (hasCombo3) {
        combolist.push("1 FOR 1");
      }
      if (combo1Items > 0 && food > 0 && combo2Items > 0) {
        combolist.push("Combo 2");
        food -= freeFood;
        if (food > combo2Items) {
          combolist.push("Combo 1");
        }
      } else if (combo1Items == 0 && food > 0 && combo2Items > 0) {
        combolist.push("Combo 2");
      } else if (combo1Items > 0 && food > 0 && combo2Items == 0) {
        combolist.push("Combo 1");
      }
    }
    return combolist;
  }

  function calculateTotalCost() {
    var deferred = $q.defer();
    if (cart.items.length > 0) {
      var total = 0.0;
      for (var i = 0; i < cart.items.length; i++) {
        total += cart.items[i].itemPrice * cart.items[i].quantity;
      }
      cart.orderTotalPrice = parseFloat(total).toFixed(2);
      persist();
      deferred.resolve('===========================================Total is done: ' + cart.orderTotalPrice);
    }
    return deferred.promise;
  }

  function getDiscountAndComboMessage() {
    var deferred = $q.defer();
    var discount = 0.0;
    var comboMessage = [];
    checkPromoStatus().then(function(res) {
      if (res[0].status & !res[1].status & !res[2].status) {
        discount = combo1n2n3(true, false, false);
        comboMessage = displayDiscount(true, false, false);
        console.log("only combo 1 is active ");
      } else if (!res[0].status & res[1].status & !res[2].status) {
        discount = combo1n2n3(false, true, false);
        comboMessage = displayDiscount(false, true, false);
        console.log("only combo 2 is active");
      } else if (!res[0].status & !res[1].status & res[2].status) {
        discount = combo1n2n3(false, false, true);
        comboMessage = displayDiscount(false, false, true);
        console.log("only combo 3 is active");
      } else if (res[0].status & res[1].status & !res[2].status) {
        discount = combo1n2n3(true, true, false);
        comboMessage = displayDiscount(true, true, false);
        console.log("combo 1,2 are active");
      } else if (!res[0].status & res[1].status & res[2].status) {
        discount = combo1n2n3(false, true, true);
        comboMessage = displayDiscount(false, true, true);
        console.log("combo 2,3 are active only");
      } else if (res[0].status & !res[1].status & res[2].status) {
        discount = combo1n2n3(true, false, true);
        comboMessage = displayDiscount(true, false, true);
        console.log("combo 1,3 are active only");
      } else if (res[0].status & res[1].status & res[2].status) {
        discount = combo1n2n3(true, true, true);
        comboMessage = displayDiscount(true, true, true);
        console.log("all combo are active");
      } else {
        console.log('no promos are active');
      }
      cart.discount = parseFloat(discount).toFixed(2);
      cart.comboMessage = comboMessage;
      persist();
      deferred.resolve('===========================================Discount is done :' + cart.discount);
    });
    return deferred.promise;
  }

  function calculateGST() {
    var deferred = $q.defer();
    if (cart.orderTotalPrice != 0.0) {
      var gst = (cart.orderTotalPrice - cart.discount) * 0.07;
      cart.GST = parseFloat(gst).toFixed(2);
      persist();
      deferred.resolve('===========================================GST is done: ' + cart.GST);
    }
    return deferred.promise;
  }

  function calculateGrandTotal() {
    var deferred = $q.defer();
    if (cart.orderTotalPrice != 0) {
      var grandtotal = (parseFloat(cart.GST) + (parseFloat(cart.orderTotalPrice - cart.discount)));
      cart.grandTotal = parseFloat(grandtotal).toFixed(2);
      persist();
      deferred.resolve('===========================================Grand is done: ' + cart.grandTotal);
    }
    return deferred.promise;
  }

  function calculatePrice() {
    if (cart.items.length > 0) {
      calculateTotalCost().then(function(res1) {
        console.log(res1);
        return getDiscountAndComboMessage();
      }).then(function(res2) {
        console.log(res2);
        return calculateGST();
      }).then(function(res3) {
        console.log(res3);
        return calculateGrandTotal();
      }).then(function(res4) {
        console.log(res4);
      });
    } else {
      clear();
    }
  }

  return {
    get: function() {
      calculatePrice();
      return cart;
    },
    create: function(c) {
      var url = APIurl + "WebserviceCafe.asmx/CreateOrder";
      return $http({
        method: 'GET',
        url: url,
        params: {
          jsonString: c,
          token: localStorage.getItem("accessToken")
        }
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    },
    add: function(item) {
      cart.items.push(item);
      calculatePrice();
      persist();
    },
    remove: function(itemID) {
      for (var i = 0; i < cart.items.length; i++) {
        if (cart.items[i].itemID === itemID) {
          cart.items.splice(i, 1);
          calculatePrice();
          persist();
          return;
        }
      }
    },
    empty: function() {
      clear();
      persist();
    },
    setDineIn: function(tableNumber) {
      cart.orderType = 'Dine-In';
      cart.tableNumber = tableNumber;
      delete cart.preorderDateTime;
      persist();
    },
    setPreorder: function(datetime) {
      cart.orderType = 'Pre-Order';
      cart.preorderDateTime = datetime;
      delete cart.tableNumber;
      persist();
    },
    setCustId: function(id) {
      cart.custID = id;
      persist();
    },
    isDuplicate: function(item) {
      var isDuplicate = false;
      for (var i = 0; i < cart.items.length; i++) {
        if (cart.items[i].itemID === item.itemID) {
          isDuplicate = true;
          break;
        } else {
          isDuplicate = false;
        }
      };
      return isDuplicate;
    },
  };
});

app.factory('Feedback', function($http, APIurl) {
  return {
    create: function(f) {
      var url = APIurl + "WebserviceCafe.asmx/CreateFeedback";
      return $http.post(url, f);
    }
  };
});

app.factory('MyHistory', function($http, Cart, APIurl) {
  return {
    getOrders: function(custId) {
      var url = APIurl + "WebserviceCafe.asmx/retrieveOrderList?";
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken") + "&custID=" + custId,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback() {
        return response.data;
      });
    },
    getOrderItems: function(orderId) {
      var url = APIurl + "WebserviceCafe.asmx/retrieveOrderItemList?";
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken") + "&orderID=" + orderId,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback() {
        return response.data;
      });
    }
  };
});

app.factory('Promotions', function($http, APIurl) {
  return {
    getAll: function() {
      var url = APIurl + "WebserviceCafe.asmx/retrieveAllPromos?";
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('Top', function($http, APIurl) {
  return {
    get: function() {
      var url = APIurl + "WebserviceCafe.asmx/retrieveTop10MenuItems?";
      return $http({
        method: 'GET',
        url: url + "token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback() {
        return response.data;
      });
    }
  };
});

app.factory('Favourite', function($http, APIurl) {
  return {
    updateFavourites: function(f) {
      var url = APIurl + 'WebserviceCafe.asmx/updateFavourites';
      return $http.post(url, f);
    },
    get: function(id) {
      var url = APIurl + "WebserviceCafe.asmx/retrieveFav?";
      return $http({
        method: 'GET',
        url: url + "custID=" + id + "&token=" + localStorage.getItem("accessToken"),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback(response) {
        return response.data;
      });
    }
  };
});

app.factory('Token', function($http, APIurl, Accesskey) {
  return {
    get: function() {
      var url = APIurl + "WebserviceCafe.asmx/getToken?";
      return $http({
        method: 'GET',
        url: url + "accessKey=" + Accesskey,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback() {
        return response.data;
      });
    },
    checkToken: function(tokenString) {
      var url = APIurl + "WebserviceCafe.asmx/checkToken?";
      return $http({
        method: 'GET',
        url: url + "token=" + tokenString,
        dataType: "json",
        contentType: "application/json; charset=utf-8"
      }).then(function successCallback(response) {
        return response.data;
      }, function errorCallback() {
        return response.data;
      });
    }
  };
});

var shopApp = angular.module('shopApp', ['ngRoute', 'ui.bootstrap']);

shopApp.baseUrl = "/api/";

shopApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/customer', {
            templateUrl: 'content/app/customer/customer.html',
            controller: 'CustomerCtrl'
        }).
        when('/product', {
            templateUrl: 'content/app/product/product.html',
            controller: 'ProductCtrl'
        }).
        when('/order', {
            templateUrl: 'content/app/order/order.html',
            controller: 'OrderCtrl'
        }).
        otherwise({
            redirectTo: '/'
        });
  }]);

shopApp.directive('commandContainer', function () {
    return {
        restrict: 'E',
        transclude: 'true',
        scope: {
            submitText: "@",
            commandName: "@",
            command: "="
        },
        template: '<form role="form" ng-submit="sendCommand(command, commandName)">' +
            '<div class="my-transclude"></div>' +
            '<input type="submit" class="btn btn-success" value="{{submitText}}" />' +
            '</div>',
        controller: 'CommandContainerController',
        link: function (scope, element, attrs, ctrl, transclude) {
            transclude(scope.$parent, function (clone) {
                element.find(".my-transclude").replaceWith(clone);
            });
        }
    };
});

shopApp.controller('CommandContainerController', function ($scope, $http) {
    $scope.sendCommand = function (command, commandName) {
        var url = shopApp.baseUrl + commandName;

        var guid = (function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                           .toString(16)
                           .substring(1);
            }
            return function () {
                return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                       s4() + '-' + s4() + s4() + s4();
            };
        })();

        command.Id = guid();
        $http.post(url, command).success(function (data) {
            $scope.command = {};
        }).error(function () {
            alert("Failed to execute command: " + commandName);
            console.log(command);
        });
    }
});

shopApp.controller('CustomerCtrl', ['$scope',
    function($scope) {
    }
]);

shopApp.controller('ProductCtrl', ['$scope', '$http',
  function ($scope) {
  }]);

shopApp.controller('OrderCtrl', ['$scope', '$http', 'debounce',
    function($scope, $http, debounce) {
        $scope.message = "This is a order thingy";
        $scope.order = {
            Items: []
        };
        $scope.addItem = function() {
            $scope.order.Items.push({});
        };

        var searchForRecommendations = function () {
            var productIds = $scope.order.Items.map(function(item) {
                return item.ProductId;
            });
            var query = { ProductIds: productIds };
            $http.post(shopApp.baseUrl + "recommendation", query).then(function(response) {
                $scope.recommendations = response.data;
            });
        };

        var lazyGetCustomers = debounce(function (filter) {
            return $http.get(shopApp.baseUrl + "customer?query=" + filter).then(function(response) {
                return response.data;
            });
        }, 400);
        $scope.getCustomers = function(filter) {
            return lazyGetCustomers(filter);
        };
        $scope.customerSelected = function(order, $item) {
            order.CustomerId = $item.Id;
        };

        var lazyGetProducts = debounce(function (filter) {
            return $http.get(shopApp.baseUrl + "product?query=" + filter).then(function (response) {
                return response.data;
            });
        }, 400);
        $scope.getProducts = function (filter) {
            return lazyGetProducts(filter);
        };
        $scope.productSelected = function (item, $item) {
            item.ProductId = $item.Id;
            searchForRecommendations();
        };

        $scope.deleteItem = function (item) {
            var index = $scope.order.Items.indexOf(item);
            if (index > -1) {
                $scope.order.Items.splice(index, 1);
            }
            searchForRecommendations();
        }
    }
]);

shopApp.factory('debounce', ['$timeout', '$q', function ($timeout, $q) {
    // The service is actually this function, which we call with the func
    // that should be debounced and how long to wait in between calls
    return function debounce(func, wait, immediate) {
        var timeout;
        // Create a deferred object that will be resolved when we need to
        // actually call the func
        var deferred = $q.defer();
        return function () {
            var context = this, args = arguments;
            var later = function () {
                timeout = null;
                if (!immediate) {
                    deferred.resolve(func.apply(context, args));
                    deferred = $q.defer();
                }
            };
            var callNow = immediate && !timeout;
            if (timeout) {
                $timeout.cancel(timeout);
            }
            timeout = $timeout(later, wait);
            if (callNow) {
                deferred.resolve(func.apply(context, args));
                deferred = $q.defer();
            }
            return deferred.promise;
        };
    };
}]);

shopApp.controller('ESPollingController', function ($scope, $http) {
    $scope.events = [];
    function getEvents(stream) {
        var currentLink = stream;
        var config = { headers: { "ES-LongPoll": 15 } };
        $http
            .get(stream + "?embed=body", config)
            .success(function (data) {
                var events = data.entries.map(function (e) {
                    return JSON.parse(e.data);
                });
                events.reverse().forEach(function (e) {
                    $scope.events.unshift(e);
                });
                var previousLink = data.links.filter(function (l) {
                    return l.relation === "previous";
                });
                currentLink = stream;
                if (previousLink.length > 0) {
                    currentLink = previousLink[0].uri;
                }
                getEvents(currentLink);
            })
        .error(function () {
            getEvents(currentLink);
        });
    }

    getEvents("http://192.168.99.100:2113/" + "streams/%24ce-PolyglotHeaven");
});

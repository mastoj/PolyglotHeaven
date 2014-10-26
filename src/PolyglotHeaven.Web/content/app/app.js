var shopApp = angular.module('shopApp', ['ngRoute']);

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

shopApp.controller('CustomerCtrl', ['$scope', '$http',
    function($scope, $http) {
        $scope.message = "This is a customer thingy";
    }
]);

shopApp.controller('ProductCtrl', ['$scope', '$http',
  function ($scope, $http) {
      $scope.message = "This is a product thingy";
  }]);

shopApp.controller('OrderCtrl', ['$scope', '$http',
    function($scope, $http) {
        $scope.message = "This is a order thingy";
    }
]);
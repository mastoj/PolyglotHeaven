var shopApp = angular.module('shopApp', ['ngRoute', 'ui.bootstrap']);

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
            '<input type="submit" class="btn btn-default" value="{{submitText}}" />' +
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
    var baseUrl = "/api/";
    $scope.sendCommand = function (command, commandName) {
        var url = baseUrl + commandName;
        command.Id = "0C446CE4-4A12-492D-A624-C54CBF32DFC9";
        $http.post(url, command).success(function (data) {
            $scope.command = {};
        }).error(function () {
            alert("Failed to execute command: " + commandName);
            console.log(command);
        });
    }
});

shopApp.controller('CustomerCtrl', ['$scope', 'executeCommandService',
    function($scope, executeCommandService) {
        $scope.message = "This is a customer thingy";

        $scope.createCustomer = function(customer) {
            executeCommandService.execute(customer, 'customer');
        }
    }
]);

shopApp.controller('ProductCtrl', ['$scope', '$http',
  function ($scope, $http) {
      $scope.message = "This is a product thingy";
  }]);

shopApp.controller('OrderCtrl', ['$scope', '$http',
    function($scope, $http) {
        $scope.message = "This is a order thingy";
        $scope.order = {
            OrderItems: []
        };
        $scope.addItem = function() {
            $scope.order.OrderItems.push({});
        };
        $scope.getCustomers = function (filter) {
            return $http.get("http://localhost:60843/api/customer");
        }
        $scope.customerSelected = function (order, $item) {
            order.CustomerId = $item.Id;
        }
    }
]);

shopApp.service('executeCommandService', ['$http', function($http) {
    var baseUrl = "http://localhost:60843/api/";

    function createUrl(commandName) {
        return baseUrl + commandName;
    }

    function execute(command, commandName) {
        var url = createUrl(commandName);
        command.Id = "0C446CE4-4A12-492D-A624-C54CBF32DFC9";
        $http.post(url, command).then(function(data) {
            alert(command.id + " " + command.name);
        });
    }

    return {
        execute: execute
    };
}])
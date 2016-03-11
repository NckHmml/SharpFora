(function (angular, document, $scope, $http, undefined) {
    // Globals
    var app = angular.module("foraApp", []);
    // Content will be inserted below
    <%= contents %>
})(angular, document, "$scope", "$http");
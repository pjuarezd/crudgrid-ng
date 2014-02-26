var app = angular.module('CrudGridExample', ['ngResource', 'ngRoute', 'ngSanitize', 'localytics.directives', 'bootstrap-tagsinput', 'ui.bootstrap']);

app.config(['$routeProvider', function($routeProvider) {
    $routeProvider.
        when('/home', { templateUrl: '/Scripts/App/Templates/Home.html' }).
        when('/simpleExample', { templateUrl: '/Scripts/app/Templates/SimpleExample.html', controller: "SimpleExampleController" }).
        when('/customExample', { templateUrl: '/Scripts/app/Templates/CustomExample.html', controller: "CustomExampleController" }).
        otherwise({ redirectTo: '/home' });
}]);

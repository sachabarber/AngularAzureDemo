// Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('main',
    [   'ngRoute',
        'ngGrid',
        'ngResource',
        'ngCookies',
        'angularAzureDemo.services',
        'angularAzureDemo.factories',
        'angularAzureDemo.directives',
        'colorpicker.module'
    ]);     //Define the main module

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
        $routeProvider

            //home routes
            .when('/subscriptions', { templateUrl: '/home/subscriptions', controller: 'SubscriptionsController' })
            .when('/create', { templateUrl: '/home/create', controller: 'CreateController' })
            .when('/viewall', { templateUrl: '/home/viewall', controller: 'ViewAllController' })
            .when('/sketcheractions', { templateUrl: '/home/sketcheractions', controller: 'SketcherActionsController' })

            .when('/viewsingleimage/:id',
                {
                    templateUrl: function (params) { return '/home/viewsingleimage/' + params.id; },
                    controller: 'ViewSingleImageController'
                }
            )

            //account routes
            .when('/login', { templateUrl: '/account/login', controller: 'LoginController' })

            //default
            .otherwise({ redirectTo: '/login' });
    }])
    .controller('RootController', ['$scope', '$route', '$routeParams', '$location', function ($scope, $route, $routeParams, $location) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $scope.activeViewPath = $location.path();
        });
    }]);


// grab underscore from window (where it attaches itself)
appRoot.constant('_', window._);

// add on underscore to global scope
appRoot.run(function ($rootScope) {
    $rootScope._ = window._;
});

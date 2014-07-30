angular.module('main').controller('ViewSingleImageController', ['$scope', '$log', '$window', '$location', '$routeParams','loginService',
    function ($scope, $log, $window, $location, $routeParams, loginService) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }


        $scope.id = $routeParams.id;

        $log.log('Route params = ', $routeParams);
        $log.log('$scope.id = ', $scope.id);


    }]);



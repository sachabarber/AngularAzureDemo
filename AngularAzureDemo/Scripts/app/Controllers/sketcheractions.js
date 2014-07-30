angular.module('main').controller('SketcherActionsController', ['$scope', '$log', '$window', '$location', 'loginService', 'imageBlobComment',
    function ($scope, $log, $window, $location, loginService, imageBlobComment) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }

        $('.actionImages').tooltip();

    }]);
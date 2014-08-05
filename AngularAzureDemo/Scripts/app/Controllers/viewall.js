angular.module('main').controller('ViewAllController',
    ['$scope', '$log', '$window', '$location', 'loginService', 'imageBlobComment','dialogService',
    function ($scope, $log, $window, $location, loginService, imageBlobComment, dialogService) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }

        $scope.storedBlobs = [];
        $scope.tableItems = [];
        $scope.hasItems = false;

        dialogService.showPleaseWait();
        getAllBlobs();

        function getAllBlobs() {
            imageBlobComment.get(function (result) {
                $scope.storedBlobs = [];
                if (result.BlobComments.length == 0) {
                    dialogService.hidePleaseWait();
                    dialogService.showAlert('Info', 'There are no items stored right now');
                    $scope.hasItems = false;
                } else {
                    $scope.hasItems = true;
                    $scope.tableItems = result.BlobComments;
                    dialogService.hidePleaseWait();
                }
            }, function (error) {
                $scope.hasItems = false;
                dialogService.hidePleaseWait();
                dialogService.showAlert('Error', 'Unable to load stored image data: ' + error.message);
            });
        }
    }]);
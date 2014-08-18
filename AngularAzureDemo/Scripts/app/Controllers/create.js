angular.module('main').controller('CreateController',
    ['$scope', '$log', '$window', '$location', 'loginService', 'imageBlob','dialogService',
    function ($scope, $log, $window, $location, loginService, imageBlob, dialogService) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }

        $scope.changeLog = function () {
            console.log('change');
        };

        $scope.strokeColor = "#a00000";
        $scope.strokeWidth = 5;
        $scope.canvasData = null;
        $scope.title = 'New Image';

        $scope.acceptCanvas = function (newCanvasData) {
            $scope.canvasData = newCanvasData;
        }


        $scope.save = function () {
            if (typeof $scope.canvasData !== 'undefined' && $scope.canvasData != null) {

                var imageBlobToSave = {
                        "UserId": loginService.currentlyLoggedInUser().Id,
                        "UserName": loginService.currentlyLoggedInUser().Name,
                        "CanvasData": $scope.canvasData,
                        "Title": $scope.title,
                        "CreatedOn" : new Date()
                };

                imageBlob.save(imageBlobToSave, function (result) {

                    $log.log('save blobs result : ', result);
                    if (result) {
                        dialogService.showAlert('Success','Sucessfully saved image data');
                    } else {
                        dialogService.showAlert('Error','Unable to save image data');
                    }
                }, function (error) {
                    dialogService.showAlert('Error',
                        'Unable to save image data: ' + error.message);
                });
            }
        };
    }]);
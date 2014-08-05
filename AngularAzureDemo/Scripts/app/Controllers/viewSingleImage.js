angular.module('main').controller('ViewSingleImageController',
    ['$scope', '$log', '$window', '$location', '$routeParams', 'loginService','imageBlobComment','dialogService',
    function ($scope, $log, $window, $location, $routeParams, loginService, imageBlobComment, dialogService) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }


        $scope.id = $routeParams.id;
        $scope.storedBlob = null;
        $scope.hasItem = false;
        $scope.commentText = '';

        $log.log('Route params = ', $routeParams);
        $log.log('ViewSingleImageController id = ', $scope.id);

        dialogService.showPleaseWait();

        getBlob($scope.id);


        $scope.hasComment = function () {
            return typeof $scope.commentText !== 'undefined' &&
                $scope.commentText != null
                && $scope.commentText != '';
        };

        $scope.saveComment = function () {
            
            if ($scope.hasComment()) {

                var imageBlobCommentToSave = {
                    "UserId": loginService.currentlyLoggedInUser().Id,
                    "UserName": loginService.currentlyLoggedInUser().Name,
                    "CreatedOn": new Date(),
                    "AssociatedBlobId": $scope.storedBlob.Blob.Id,
                    "Comment" : $scope.commentText
                };

                imageBlobComment.save(imageBlobCommentToSave, function (result) {

                    $log.log('save imageBlobComments result : ', result);
                    if (result.SuccessfulAdd) {
                        $scope.storedBlob.Comments.unshift(result.Comment);
                    } else {
                        dialogService.showAlert('Error', 'Unable to save comment');
                    }
                }, function (error) {
                    $log.log('save imageBlobComments error : ', error);
                    dialogService.showAlert('Error', 'Unable to save comment: ' + error.message);
                });
            }



        };

        function getBlob(id) {

            imageBlobComment.fetchSingle(id)
                .success(function (result) {
                    $scope.hasItem = true;
                    $scope.storedBlob = result;

                    $log.log("xxxxxx results", result);
                    $log.log("SCOPE BLOB", $scope.storedBlob);


                    dialogService.hidePleaseWait();

                }).error(function (error) {
                    $scope.hasItem = false;
                    dialogService.hidePleaseWait();
                    dialogService.showAlert('Error', 'Unable to load stored image data: ' + error.message);
                });
        }

    }]);



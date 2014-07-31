angular.module('main').controller('ViewAllController',
    ['$scope', '$log', '$window', '$location', 'loginService', 'imageBlobComment','dialogService',
    function ($scope, $log, $window, $location, loginService, imageBlobComment, dialogService) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }

        $scope.storedBlobs = [];
        $scope.tableItems = [];
        $scope.columnCount = 3;



        $('.results').tooltip();

        while ($scope.tableItems.push([]) < $scope.columnCount);

        dialogService.showPleaseWait();


        getAllBlobs();


        function getAllBlobs() {
            imageBlobComment.get(function (result) {

                $log.log("results", result);

                $scope.storedBlobs = [];
                $log.log('blobs count : ', result.BlobComments.length);

                if (result.BlobComments.length == 0) {
                    dialogService.hidePleaseWait();
                    dialogService.showAlert('Info', 'There are no items stored right now');
                } else {
                    //createTableOfResults(result.BlobComments);

                    $scope.tableItems = result.BlobComments;


                   

                    setTimeout(function () {
                        $('.infoIcon').tooltip();
                    }, 1);

                    dialogService.hidePleaseWait();
                }

            }, function (error) {
                dialogService.hidePleaseWait();
                dialogService.showAlert('Error', 'Unable to load stored image data: ' + error.message);
            });
        }

        $scope.showBlogTooltip = function (blog) {
            return 'Created On :' + blog.CreatedOnPreFormatted +
                '\x0A\x0DCreated By :' + blog.UserName;
        }
    }]);
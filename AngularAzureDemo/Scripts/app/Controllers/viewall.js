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
                    createTableOfResults(result.BlobComments);
                }

            }, function (error) {
                dialogService.hidePleaseWait();
                dialogService.showAlert('Error', 'Unable to load stored image data: ' + error.message);
            });
        }


        function createTableOfResults(blobComments) {
            var maxRows = Math.ceil(blobComments.length / $scope.columnCount);
            if (blobComments.length < $scope.columnCount) {
                $scope.tableItems[0] = [];
                for (var i = 0; i < blobComments.length; i++) {
                    $scope.tableItems[0].push(blobComments[i]);
                }
            } else {
                var originalIndexCounter = 0;
                for (var r = 0; r < maxRows; r++) {
                    for (var c = 0; c < $scope.columnCount; c++) {
                        if (originalIndexCounter < blobComments.length) {
                            $scope.tableItems[r][c] = blobComments[originalIndexCounter];
                            originalIndexCounter++;
                        }
                    }
                }
            }
            setTimeout(function () {
                $('.results').tooltip();
            }, 1);
            dialogService.hidePleaseWait();
        }


        $scope.showBlogTooltip = function (blog) {
            return 'Created On :' + blog.CreatedOnPreFormatted +
                '\x0A\x0DCreated By :' + blog.UserName;
        }
    }]);
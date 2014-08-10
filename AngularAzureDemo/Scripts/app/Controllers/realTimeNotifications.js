angular.module('main').controller('RealTimeNotificationsController',
        ['$rootScope','$scope', '$log', '$window', '$location', '$cookieStore', '_',
    function ($rootScope, $scope, $log, $window, $location, $cookieStore, _) {

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": navigate,
            "showDuration": "5000000",
            "hideDuration": "1000",
            "timeOut": "5000000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        $scope.latestBlobId = '';


        $(function () {

            // Declare a proxy to reference the hub.
            var blobProxy = $.connection.blobHub;

            // Create a function that the hub can call to broadcast messages.
            blobProxy.client.latestBlobMessage = function (latestBlob) {

                //do this here as this may have changed since this controller first started. Its quick lookup
                //so no real harm doing it here
                $scope.allFriendsSubscriptionsCookie = $cookieStore.get('allFriendsSubscriptions');

                var userSubscription = _.findWhere($scope.allFriendsSubscriptionsCookie,
                    { Id: latestBlob.UserId });

                if (userSubscription != undefined) {


                    $scope.latestBlobId = latestBlob.Id;
                    $scope.$apply();

                    //show toast notification
                    var text = latestBlob.UserName + ' has just created a new image called "' +
                        latestBlob.Title + '", click here to view it HHHHH';
                    toastr['info'](text, "New image added");
                }
            };

            //start the SignalR hub comms
            $.connection.hub.start(
                {
                    transport: ['longPolling', 'webSockets'],
                    waitForPageLoad: false
                });
            
            $.connection.hub.disconnected(function () {
                $log.log('*** BlobHub Disconnected');
                setTimeout(function () {
                    $.connection.hub.start(
                    {
                        transport: ['longPolling', 'webSockets'],
                        waitForPageLoad: false
                    });
                }, 1000); // Restart connection after 1 seconds.
            });

        });

        function navigate() {
            $rootScope.$apply(function() {
                $location.path("viewsingleimage/" + $scope.latestBlobId);
                $location.replace();
            });
        } 
    }]);
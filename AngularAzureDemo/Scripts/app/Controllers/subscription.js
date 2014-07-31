angular.module('main').controller('SubscriptionsController', ['$scope', '$log', '$window', '$location', 'loginService','$cookieStore',
    'userService', 'dialogService', 'userSubscription',
    function ($scope, $log, $window, $location, loginService, $cookieStore, userService, dialogService, userSubscription) {

        if (!loginService.isLoggedIn()) {
            $location.path("login");
        }

            
        $scope.storedSubscriptions = [];
        $scope.allFriendsSubscriptions = [];

        $log.log('Logged in user Id : ',  loginService.currentlyLoggedInUser().Id);


        $scope.hasSubscriptions = false;


        dialogService.showPleaseWait();
        getAllFriends(loginService.currentlyLoggedInUser().Id);


      

        function getAllFriends(id) {
            userService.getFriends(id)
                .success(function (friends) {
                    $log.log('friends count : ', friends.length);

                    $scope.storedSubscriptions = [];

                    for (var i = 0; i < friends.length; i++) {
                        friends[i].IsActive = false;
                        $scope.storedSubscriptions.push(friends[i]);
                    }

                    //get all actual stored subscriptions
                    getAllSubscriptions(id);
                        
                })
                .error(function (error) {
                    dialogService.hidePleaseWait();
                    dialogService.showAlert('Error', 'Unable to load friend data: ' + error.message);
                });
        }


        function getAllSubscriptions(id) {

            //NOTE : userSubscription.get(..) returns a promise so we could also do this
            //and then use it as you would a regular promise, for now we will just use callbacks
            //
            //var userSubscriptionPromise = userSubscription.get({ id: id }).$promise;
            //userSubscriptionPromise.then(function (result) {
            //    $log.log('Success');
            //}, function (error) {
            //   $log.log('error', error);
            //});


            userSubscription.get({ id: id }, function (result) {

                var savedSubscriptions = result.Subscriptions;

                $log.log('subscription count : ', savedSubscriptions.length);

                for (var i = 0; i < savedSubscriptions.length; i++) {
                    var friendSubscription = _.findWhere($scope.storedSubscriptions, { Id: savedSubscriptions[i].FriendId });

                    if (typeof friendSubscription !== 'undefined' && friendSubscription != null) {
                        friendSubscription.IsActive = true;
                    } else {
                        $log.log('could not find friend', savedSubscriptions[i].FriendId);
                    }
                }

                $scope.allFriendsSubscriptions = $scope.storedSubscriptions;

                $cookieStore.put('allFriendsSubscriptions', $scope.allFriendsSubscriptions);
                var allFriendsSubscriptionsCookie = $cookieStore.get('allFriendsSubscriptions');


                $scope.hasSubscriptions = true;
                dialogService.hidePleaseWait();
            }, function (error) {
                dialogService.hidePleaseWait();
                dialogService.showAlert('Error', 'Unable to load subscription data: ' + error.message);

            });
        }


        $scope.updateSubscriptions = function () {


            dialogService.showPleaseWait();

            $log.log('Updating the subscriptions');

            var subscriptionsToSave = [];
            for (var i = 0; i < $scope.allFriendsSubscriptions.length; i++) {
                subscriptionsToSave.push(
                {
                    "UserId": loginService.currentlyLoggedInUser().Id,
                    "FriendId": $scope.allFriendsSubscriptions[i].Id,
                    "IsActive": $scope.allFriendsSubscriptions[i].IsActive
                });
            }

            $log.log('subscriptionsToSave', subscriptionsToSave);
            
            var userSubscriptions = {
                Subscriptions : subscriptionsToSave
            }

            //NOTE : userSubscription.save(..) returns a promise so we could also do this
            //and then use it as you would a regular promise, for now we will just use callbacks
            //
            //var userSubscriptionPromise = userSubscription.save(userSubscriptions).$promise;
            //userSubscriptionPromise.then(function (result) {
            //    $log.log('Success');
            //}, function (error) {
            //   $log.log('error', error);
            //});

            userSubscription.save((userSubscriptions), function (result) {
                $log.log('saveSubscriptions result : ', result);
                if (result) {
                    dialogService.hidePleaseWait();
                    dialogService.showAlert('Success', 'Successfully saved all subscriptions');
                    //$window.alert('Successfully saved all subscriptions');
                } else {
                    dialogService.hidePleaseWait();
                    $window.alert('Unable to save subscription data');
                    dialogService.showAlert('Error', 'Unable to save subscription data');
                }
            }, function (error) {
                dialogService.hidePleaseWait();
                dialogService.showAlert('Error', 'Unable to save subscription data: ' + error.message);
            });



        };

    }]);



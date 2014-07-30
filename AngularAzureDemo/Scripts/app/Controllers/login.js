appRoot.controller('LoginController', ['$scope', '$log', '$location', '$resource',
    '$window', 'loginService', 'dialogService','userService', '_',
    function ($scope, $log, $location, $resource, $window, loginService, dialogService, userService, _) {

        $scope.usersList = [];
        $scope.selectedPerson = null;
        $scope.isLoggedIn = false;

        $log.log("logged in " + loginService.isLoggedIn());

        dialogService.showPleaseWait();

        getAllPeople();

        $scope.login = function () {
            loginService.login($scope.selectedPerson);
            $scope.isLoggedIn = true;
            $location.path("sketcheractions");
        };

        $scope.logout = function () {
            $scope.selectedPerson = null;
            loginService.logout();
            $scope.isLoggedIn = false;
        };

        function getPersonFromList(userName) {
            $scope.selectedPerson = _.findWhere($scope.usersList, { Name: userName });
        }

        function getAllPeople() {
            userService.getAll()
                .success(function (users) {
                    $scope.usersList = users;
                    if (loginService.isLoggedIn()) {
                        getPersonFromList(loginService.currentlyLoggedInUser().Name);
                        $scope.isLoggedIn = true;
                        $log.log("selected person " + $scope.selectedPerson.Name);
                    }
                    dialogService.hidePleaseWait();
                })
                .error(function (error) {
                    dialogService.hidePleaseWait();
                    $window.alert('Error', 'Unable to load user data: ' + error.message);
                });
        }
    }]);
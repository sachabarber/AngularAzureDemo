angularAzureDemoServices.service('loginService', ['$log', function ($log) {

    this.loggedInUser = null;

    this.login = function (currentUser) {
        this.loggedInUser = currentUser;
        $log.log('Logged in user ' + this.loggedInUser.name);
    }

    this.logout = function () {
        this.loggedInUser = null;
        $log.log('User has been logged out');
    }

    this.isLoggedIn = function() {
        return typeof this.loggedInUser !== 'undefined' && this.loggedInUser != null;
    }

    this.currentlyLoggedInUser = function () {
        return this.loggedInUser;
    }

}]);

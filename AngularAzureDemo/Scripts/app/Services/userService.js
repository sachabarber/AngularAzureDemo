// http://weblogs.asp.net/dwahlin/using-an-angularjs-factory-to-interact-with-a-restful-service
angularAzureDemoServices.service('userService', ['$http', '$window', function ($http, $window) {
    
    var urlBase = '/api/user';

    this.getAll = function () {
        return $http.get(urlBase);
    };

    this.getFriends = function (id) {
        return $http.get(urlBase + '/' + id);
    };
}]);



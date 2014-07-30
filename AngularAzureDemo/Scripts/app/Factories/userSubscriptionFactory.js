//http://odetocode.com/blogs/scott/archive/2013/02/28/mapping-an-angular-resource-service-to-a-web-api.aspx
angularAzureDemoFactories.factory('userSubscription', ['$resource', function ($resource) {

    var urlBase = '/api/usersubscription/:id';

    return $resource(
        urlBase,
        { id: "@id" },
        {
            "save": { method: "POST", isArray: false }
        });
}]);



//**********************************************************************************************************
//NOTE : If you wanted to use angular $http instead of angular $resource you could have done something like this instead
//**********************************************************************************************************


// http://weblogs.asp.net/dwahlin/using-an-angularjs-factory-to-interact-with-a-restful-service
//angularAzureDemoFactories.factory('userSubscription', ['$http', '$window', '$log', function ($http, $window, $log) {
//    var urlBase = '/api/usersubscription';
//
//    this.getSubscriptions = function (id) {
//        return $http.get(urlBase + '/' + id);
//    };
//
//
//    this.saveSubscriptions = function (userSubscriptions) {
//        $log.log('userSubscriptionService.saveSubscriptions()', userSubscriptions);
//        return $http({
//            url: urlBase,
//            method: "POST",
//            data: userSubscriptions,
//        });
//    };
//}]);






//http://odetocode.com/blogs/scott/archive/2013/02/28/mapping-an-angular-resource-service-to-a-web-api.aspx
angularAzureDemoFactories.factory('imageBlobComment', ['$resource', '$http', function ($resource, $http) {

    var urlBase = '/api/imageblobcomment/:id';

    var ImageBlobComment = $resource(
        urlBase,
        { id: "@id" },
        {
            "save": { method: "POST", isArray: false }
        });

    ImageBlobComment.fetchSingle = function(id) {
        return $http.get('/api/imageblobcomment/' + id);
    }

    return ImageBlobComment;
}]);



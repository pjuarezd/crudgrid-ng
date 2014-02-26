app.factory('crudGridMetadataFactory', ['$http', '$resource', function ($http, $resource) {
    return function (type, parameters) {

        var baseUrl = 'api/' + type;
        if (parameters != null) {
            baseUrl += '?';
            $.each(parameters, function (k, v) {
                baseUrl += encodeURI(k) + '=' + encodeURI(v) + '&';
            });
            baseUrl = baseUrl.substr(0, baseUrl.length - 1);
        }

        return $resource(baseUrl, {},
             {
                 'metadata': { method: 'GET', isArray: false }
             });
    };
}]);
app.factory('crudGridDataFactory', ['$http', '$resource', function ($http, $resource) {
    return function (type, parameters) {

        var baseUrl = 'api/' + type + '/:id';
        
        if (parameters != null) {
            baseUrl += '?';
            $.each(parameters, function (k, v) {
                baseUrl += encodeURI(k) + '=' + encodeURI(v) + '&';
            });
            baseUrl = baseUrl.substr(0, baseUrl.length - 1);
        }

        return $resource(baseUrl,
            { id: '@id'},
            { 'update': { method: 'PUT' } },
            { 'query': { method: 'GET', isArray: false } }
        );
    };
}]);

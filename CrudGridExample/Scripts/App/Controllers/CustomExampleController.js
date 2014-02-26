app.controller('CustomExampleController', ['$scope',
    function ($scope) {

        init = function () {
            $scope.gridOptions = {
                routes: {
                    query: '/GetWithFullname/action',
                    metadata: '/GetMetadata/x',
                }
            };
        };
        init();
    }]);
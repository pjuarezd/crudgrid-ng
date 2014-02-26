app.controller('SimpleExampleController', ['$scope', 
        function ($scope) {

            init = function () {
                $scope.gridOptions = {visuals:{showEdit:false}};

            };
            init();
        }]);


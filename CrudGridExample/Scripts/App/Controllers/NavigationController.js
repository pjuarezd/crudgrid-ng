app.controller('NavigationCtrl', ['$scope', '$location',
    function ($scope, $location) {

        $scope.activeTab = function(tab) {
            var currentTab = $location.path().split('/')[1];
			return tab === currentTab;            
        };
        
        //Ignore this section, is only indeed to highlight the sintaxys documentation sections
        $scope.$on('$routeChangeSuccess', function () {
            prettyPrint();
        });
        //Ignore this section, is only indeed to highlight the sintaxys documentation sections
    }]);
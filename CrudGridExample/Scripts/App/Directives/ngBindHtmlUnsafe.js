app.directive('bindHtml', function ($parse, $compile) {
    return {
        restrict: 'AE',
        link: function($scope, $element, $attrs) {
            var compile = function(newHtml) {
                newHtml = $compile(newHtml)($scope);
                $element.html('').append(newHtml);
            };

            var htmlName = $attrs.bindHtml;

            $scope.$watch(htmlName, function(newHtml) {
                if (!newHtml) return;
                compile(newHtml);
            });

        }
    };
});
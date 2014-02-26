app.directive('crudGrid', function () {
    return {
        restrict: 'A',
        replace: false,
        scope: { options: "=" },
        templateUrl: '/Scripts/app/Directives/Templates/crud-grid-directive-template.html',
        controller: ['$scope', '$element', '$attrs', 'crudGridDataFactory', 'notificationFactory', 'crudGridMetadataFactory',
            function ($scope, $element, $attrs, crudGridDataFactory, notificationFactory, crudGridMetadataFactory) {

                //here will be placed default values grid parameters
                var defaultOptions = {
                    gridInitialized : null,
                    visuals: {
                        showEdit: true,
                        showRemove: true,
                        showAdd: true,
                        showPagination: true,
                        showRecordLegend: true,
                        showSearch : false,
                        caption: '',
                    },
                    pageSize: 10,
                    customColumns: [],
                    getParameters: {},
                    routes: {
                        query: '',
                        create: '',
                        update: '',
                        metadata: '',
                        delete: ''
                    },
                    defaultValues: {},
                    rowCallback : null
                };

                $scope.id = $attrs.id || 'crudGrid';
                $scope.baseUrl = $attrs.tableName || $attrs.url;
                $scope.metadataUrl = '';
                $scope.metadata = new Object();
                $scope.objects = [];
                $scope.object = new Object();
                $scope.addMode = false;
                $scope.fields = new Array();
                $scope.loading = true;
                $scope.orderBy = { field: '', asc: true };
                $scope.caption = '';
                $scope.page = 1;
                $scope.pages = 1;
                $scope.backEnabled = false;
                $scope.forwardEnabled = true;
                $scope.searchText = '';
                
                var loadingFunction = function () { $scope.loading = false; };

                var setNavigationButtons = function() {
                    $scope.getCurrentRowCount();

                    if ($scope.page <= 1) {
                        $scope.backEnabled = false;
                    } else {
                        $scope.backEnabled = true;
                    }

                    if ($scope.page >= $scope.pages) {
                        $scope.forwardEnabled = false;
                    } else {
                        $scope.forwardEnabled = true;
                    }
                };
                
                
                var successCallback = function (e) {
                    notificationFactory.success();
                    $scope.getData();
                };
                
                var errorCallback = function (e) {
                    notificationFactory.error(e.data.ExceptionMessage);
                };
                
                var dataLoadErrorCallback = function (e) {
                    notificationFactory.error(e.data.MessageDetail);
                    loadingFunction();
                };

                var getRequestParams = function () {
                    var pagingParams = { Size: $scope.options.pageSize, Page: $scope.page, Search: $scope.searchText, SortName: $scope.orderBy.field, SortOrder: $scope.orderBy.asc ? "Asc" : "Desc" };
                    var params = $scope.options.getParameters != null ? $.extend({}, pagingParams, $scope.options.getParameters) : pagingParams;
                    return params;
                };

                $scope.deleteObject = function (object) {
                    crudGridDataFactory($scope.baseUrl + ($scope.options.routes.delete || '')).delete({ id: object[$scope.metadata.Key] }, successCallback, errorCallback);
                };
                
                $scope.getData = function () {
                        crudGridDataFactory($scope.baseUrl + ($scope.options.routes.query || ''), getRequestParams()).query(function (data) {
                            $scope.objects = data;
                            setNavigationButtons();
                            loadingFunction();
                        }, dataLoadErrorCallback);
                };

                $scope.showActionsColumn = function () {
                    return ($scope.options.visuals.showEdit | $scope.options.visuals.showRemove | $scope.options.visuals.showAdd);
                };

                $scope.showColumn = function (column) {
                    return column.ColumnVisible;
                };

                $scope.goToPage = function (index) {
                    $scope.page = index;
                    $scope.getData();
                };

                $scope.back = function () {
                    if ($scope.page > 1) {
                        $scope.page = $scope.page - 1;
                        $scope.getData();
                    }
                };

                $scope.forward = function () {
                    if ($scope.page < $scope.pages) {
                        $scope.page = $scope.page + 1;
                        $scope.getData();
                    }
                };

                $scope.setOrderBy = function (field) {
                    var asc = $scope.orderBy.field === field ? !$scope.orderBy.asc : true;
                    $scope.orderBy = { field: field, asc: asc };
                    $scope.getData();
                };

                $scope.customColumValue = function (column, rowData) {
                    if (column.IsCustom && column.Callback) {
                        if ({}.toString.call(column.Callback) === '[object Function]') {
                            return '<span>' + column.Callback(rowData) + '</span>';
                        } else {
                            return '<span>' + (column.Callback || '') + '</span>';
                        }
                    } else {
                        if (column.Datatype == 'Boolean') {
                            return (rowData[column.Name] ? '<i class="glyphicon glyphicon-ok">&nbsp;</i>' : '<i class="glyphicon glyphicon-remove">&nbsp;</i>');
                        }
                        return '<span data-toggle="tooltip" title="' + (rowData[column.Name] || '') + '">' + (rowData[column.Name] || '') + '</span>';
                    }
                };

                $scope.initializeObject = function () {
                    $scope.object = $.extend({}, {}, $scope.options.defaultValues);
                };

                $scope.setMetadataUrl = function () {
                    $scope.metadataUrl = $attrs.tableName != null ?
                       ('Metadata/' + $scope.baseUrl + ($scope.options.routes.metadata || '')) :
                       ($scope.baseUrl + ($scope.options.routes.metadata || ''));
                };

                $scope.getMetadata = function() {
                    crudGridMetadataFactory($scope.metadataUrl, getRequestParams()).metadata(function (data) {
                        $scope.fields = new Array();
                        $scope.metadata = data;
                        $scope.caption = $scope.options.visuals.caption || data.Name;
                        $.each(data.Fields, function (i, field) {
                            field.IsCustom = false;
                            var customField = Enumerable.From($scope.options.customColumns).FirstOrDefault(null, function (x) { return x.Name == field.Name; });
                            if (customField != null) {
                                field.IsCustom = true;
                                var newField = $.extend({}, field, customField);
                                $scope.fields.push(newField);
                            } else {
                                $scope.fields.push(field);
                            }
                        });

                        //Finally fill the other cusotm fields
                        $.each($scope.options.customColumns, function (i, field) {
                            field.IsCustom = true;
                            if (!Enumerable.From($scope.fields).Any(function (x) { return x.Name == field.Name; })) {
                                if (field.ColumnVisible == null) field.ColumnVisible = true;
                                if (field.FieldVisible == null) field.FieldVisible = true;
                                $scope.fields.push(field);
                            }
                        });

                        //Order fields
                            $scope.fields.sort(function (obj1, obj2) {
                                return obj1.Order - obj2.Order;
                            });

                        $scope.orderBy = { field: $scope.fields[0].Name, asc: true };
                        $scope.pages = Math.ceil((Number($scope.metadata.RowCount) / $scope.options.pageSize));
                    });
                };

                $scope.getCurrentRowCount = function() {
                    crudGridMetadataFactory($scope.metadataUrl, getRequestParams()).metadata(function (data) {
                        $scope.metadata.RowCount = data.RowCount;
                        $scope.pages = Math.ceil((Number($scope.metadata.RowCount) / $scope.options.pageSize));
                    });
                };

                init = function () {
                    $scope.options =
                    $scope.options != null ?
                        (($scope.options.constructor == "string".constructor) ?
                            $.extend({}, defaultOptions, $.parseJSON($scope.options)) :
                            $.extend({}, defaultOptions, $scope.options))
                        : defaultOptions;
                    if ($scope.options.routes == null) $scope.options.routes = defaultOptions.routes;
                    $scope.options.visuals = $.extend({}, defaultOptions.visuals, $scope.options.visuals);
                    $scope.initializeObject();
                    $scope.setMetadataUrl();
                    $scope.getMetadata();
                };
                
                init();

                $scope.$watch("options.getParameters", function () {
                    $scope.getMetadata();
                    $scope.getData();
                });
                
                $scope.$watch("options.routes.metadata", $scope.setMetadataUrl);
            }]
    };
});
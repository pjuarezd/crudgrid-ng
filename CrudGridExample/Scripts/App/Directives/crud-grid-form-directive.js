app.directive('crudGridForm', function ($timeout) {
    return {
        restrict: 'E',
        replace: false,
        scope: false,
        templateUrl: '/Scripts/app/Directives/Templates/crud-grid-form-template.html',
        controller: ['$scope', '$element', '$attrs', 'crudGridDataFactory', 'notificationFactory', 'crudGridMetadataFactory',
         function ($scope, $element, $attrs, crudGridDataFactory, notificationFactory, crudGridMetadataFactory) {
             $scope.modalId = $attrs.modalId;

             $scope.fieldType = function (field) {
                 var fieldType = '';
                 var dotNetObjectType = field.Datatype;
                 if (typeof field.Caption !== "undefined")
                     if (field.Caption.indexOf('Email') > 0) dotNetObjectType = 'Email';

                 switch (dotNetObjectType) {
                     case 'Email':
                         fieldType = 'email';
                         break;
                     case 'String':
                         fieldType = 'text';
                         break;
                     case 'DateTime':
                         fieldType = 'datetime';
                         break;
                     case 'Int32':
                         fieldType = 'number';
                         break;
                     case 'Decimal':
                         fieldType = 'number';
                         break;
                     case 'Single':
                         fieldType = 'number';
                         break;
                     case 'Boolean':
                         fieldType = 'checkbox';
                         break;

                     default:
                         fieldType = 'text';
                 }
                 return fieldType;
             };

             $scope.dateOptions = {
                 'year-format': "'yyyy'",
                 'starting-day': 1
             };

             $scope.toggleAddMode = function () {
                 $scope.addMode = !$scope.addMode;
             };

             $scope.toggleEditMode = function (object) {
                 if ($attrs.modalId == 'Edit')
                     $scope.editMode = !$scope.editMode;

                 if ($attrs.modalId == 'Add')
                     $scope.addMode = !$scope.addMode;
             };

             $scope.showLabel = function (field) {
                 return ($scope.showCustomField(field) | $scope.showField(field));
             };

             $scope.showCustomField = function (field) {
                 if (field.IsCustom) {
                     return field.FieldVisible;
                 }
                 return false;
             };

             $scope.showField = function (field) {
                 if (field.IsCustom && field.Editor != null) {
                     return false;
                 }

                 return field.FieldVisible;
             };

             var successUpdateCallback = function (e, cb) {
                 var modalForm = $('#modal' + $scope.modalId + e[$scope.metadata.Key]);
                 if (modalForm.length > 0) {
                     $(modalForm).on('hidden.bs.modal', function (f) {
                         notificationFactory.success();
                         $scope.getData(cb);
                     });
                     $(modalForm).modal('hide');
                 } else {
                     notificationFactory.success();
                     $scope.getData(cb);
                 }
             };

             var successPostCallback = function (e) {
                 $('#modal' + $scope.id + $scope.modalId).modal('hide');
                 $scope.initializeObject();
                 $scope.getData();
                 notificationFactory.success();
             };

             var errorCallback = function (e) {
                 notificationFactory.error(e.data.ExceptionMessage);
             };
             
             $scope.saveOrUpdateObject = function (object) {
                 if ($attrs.modalId == 'Add') {
                     $scope.add($scope.object);
                 } else if ($attrs.modalId == 'Edit') {
                     $scope.update(object);
                 }
                 $scope.toggleEditMode();
             };

             $scope.add = function (object) {
                 crudGridDataFactory($scope.baseUrl + ($scope.options.routes.create || '')).save(object, successPostCallback, errorCallback);
             };

             $scope.update = function (object) {
                 crudGridDataFactory($scope.baseUrl + ($scope.options.routes.update || '')).update({ id: object[$scope.metadata.Key] }, object, successUpdateCallback, errorCallback);
             };

             $scope.customFieldEditor = function (field, rowData) {
                 if (field.IsCustom) {
                     if (field.Editor && {}.toString.call(field.Editor) === '[object Function]') {
                         return field.Editor(rowData);
                     } else {
                         return field.Editor;
                     }
                 } else {
                     return rowData[field.Name];
                 }
             };

             $timeout(function () {

                 if ($scope.options.gridInitialized) {
                     $scope.options.gridInitialized($scope, $element, $attrs);
                 }
             });
         }]
    };
});
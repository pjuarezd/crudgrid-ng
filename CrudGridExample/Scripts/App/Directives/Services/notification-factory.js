app.factory('notificationFactory', function () {
    return {
        success: function () {
            //toastr.options.positionClass = "toast-top-full-width";
            toastr.success("Success");
        },
        error: function (text) {
            toastr.error(text, "Error");
        }
    };
});
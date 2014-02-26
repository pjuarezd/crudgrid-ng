app.filter('pagesRange', function() {
    return function(input, params) {
        var total = parseInt(params.total);
        var current = parseInt(params.current);
        var i = 1;
        if (current >= 10) {
            if ((total - current) > 5) {
                i = current - 5;
            } else {
                i = current - ( 10 - (total - current)) + 1;
            }
        }
        for (i; i < (total + 1) ; i++) {
            if (input.length < 10) {
                input.push(i);
            } else {
                break;
            }
            
        }
        return input;
    };
});
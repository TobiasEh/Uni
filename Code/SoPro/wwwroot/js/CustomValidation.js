
$.validator.addMethod("starttime", function (value, element) {
    return Date.parse(value) > Date.now();
});


$.validator.addMethod("endtime", function (value, element) {
    return Date.parse(value) > Date.parse($('#booking_startTime').val());
});


$.validator.addMethod('socend', function (value, element) {
    return parseInt(value) > parseInt($('#booking_socStart').val());
});



$.validator.unobtrusive.adapters.addBool('starttime');
$.validator.unobtrusive.adapters.addBool('endtime');
$.validator.unobtrusive.adapters.addBool('socend');

$.validator.unobtrusive.parse('form');



$.validator.addMethod('BookingStartTimeValidation', function (value, element) {
        return Date.parse(value) <= Date.now;
});
$.validator.addMethod("BookingEndTimeValidation", function (value, element) {
    return Date.parse(value) > Date.parse($('#booking_startTime').val());
});

$.validator.addMethod('BookingSocEndValidation', function (value, element) {
    return parseInt(value) > parseInt($('#booking_socStart').val());
});






$.validator.unobtrusive.adapters.addBool('BookingStartTimeValidation');
$.validator.unobtrusive.adapters.addBool('BookingEndTimeValidation');
$.validator.unobtrusive.adapters.addBool('BookingSocEndValidation');

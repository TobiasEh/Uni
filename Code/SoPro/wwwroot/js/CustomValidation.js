$.validator.addMethod('BookingStartTimeValidation', function (value, element) {
        return Date.parse(value) <= Date.now;
});
$.validator.addMethod("BookingEndTimeValidation", function (value, element) {
    return Date.parse(value) > Date.parse($('#booking_startTime').val());
});

$.validator.addMethod('BookingSocEndValidation', function (value, element) {
    return parseInt(value) > parseInt($('#booking_socStart').val());
});
$.validator.addMethod('minPlugtype2Validation', function (value, element) {
    if ($('#ccs').val() == 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.addMethod('minPlugccsValidation', function (value, element) {
    if ($('#type2').val() == 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.addMethod('ccsPowerValidation', function (value, element) {
    if ($('#ccs').val() != 0 && parseInt(value) == 0 ) {
        return false;
    }
    return true;
});
$.validator.addMethod('type2PowerValidation', function (value, element) {
    if ($('#type2').val() != 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.addMethod('parallelUsableValidation', function (value, element) {
    return (parseInt($('#type2').val()) + parseInt($('#ccs').val())) > parseInt(value);
});





$.validator.unobtrusive.adapters.addBool('BookingStartTimeValidation');
$.validator.unobtrusive.adapters.addBool('BookingEndTimeValidation');
$.validator.unobtrusive.adapters.addBool('BookingSocEndValidation');

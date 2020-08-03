$.validator.setDefaults({
    debug: true
});
$.validator.addMethod("starttime", function (value, element) {
    return Date.parse(value) > Date.now();
});


$.validator.addMethod("endtime", function (value, element) {
    return Date.parse(value) > Date.parse($('#booking_startTime').val());
});


$.validator.addMethod('socend', function (value, element) {
    return parseInt(value) > parseInt($('#booking_socStart').val());
});


$.validator.addMethod('minPlugtype2Validation', function (value, element) {
    if ($('#ccs').checked() == true && element.checked() == false) {
        return false;
    }
    return true;
});


$.validator.addMethod('minPlugccsValidation', function (value, element) {
    if ($('#type2').checked() == true && element.checked() == false) {
        return false;
    }
    return true;
});


$.validator.addMethod('ccsPowerValidation', function (value, element) {
    if ($('#ccs').val() != 0 && parseInt(value) == 0) {
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

$.validator.unobtrusive.adapters.addBool('starttime');
$.validator.unobtrusive.adapters.addBool('endtime');
$.validator.unobtrusive.adapters.addBool('socend');
$.validator.unobtrusive.adapters.addBool('minPlugtype2Validation');
$.validator.unobtrusive.adapters.addBool('minPlugccsValidation');
$.validator.unobtrusive.adapters.addBool('ccsPowerValidation');
$.validator.unobtrusive.adapters.addBool('type2PowerValidation');
$.validator.unobtrusive.parse('form');



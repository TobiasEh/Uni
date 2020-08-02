
$.validator.addMethod("starttime", function (value, element) {
    return false;
});
$.validator.unobtrusive.adapters.addBool(starttime);

$.validator.addMethod("endtime", function (value, element) {
    return Date.parse(value) > Date.parse($('#booking_startTime').val());
});
$.validator.unobtrusive.adapters.addBool(endtime);

$.validator.addMethod('socend', function (value, element) {
    return parseInt(value) > parseInt($('#booking_socStart').val());
});
$.validator.unobtrusive.adapters.addBool(socend);

$.validator.addMethod('minPlugtype2Validation', function (value, element) {
    if ($('#ccs').val() == 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool(minPlugtype2Validation);

$.validator.addMethod('minPlugccsValidation', function (value, element) {
    if ($('#type2').val() == 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool(minPlugccsValidation);

$.validator.addMethod('ccsPowerValidation', function (value, element) {
    if ($('#ccs').val() != 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool(ccsPowerValidation);

$.validator.addMethod('type2PowerValidation', function (value, element) {
    if ($('#type2').val() != 0 && parseInt(value) == 0) {
        return false;
    }
    return true;
});
$.validator.unobtrusive.adapters.addBool(type2PowerValidation);



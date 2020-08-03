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


$.validator.addMethod('minplug', function (value, element) {
    if ($('input:checkbox:checked').length > 0) {
        return true;
    }
    return false;
});

$.validator.addMethod('atleastoneplug', function (value, element) {
    if ($('#ccs').val() + $('#type2').val() > 0) {
        return true;
    }
    return false;
});

$.validator.addMethod('notzeropower', function (value, element) {
    var id = '#' + element.attr('id').replace('Power', '');
    if ($(id).val() > 0 && value == 0) {
        return false;
    }
    return true;
});

$.validator.unobtrusive.adapters.addBool('starttime');
$.validator.unobtrusive.adapters.addBool('endtime');
$.validator.unobtrusive.adapters.addBool('socend');
$.validator.unobtrusive.adapters.addBool('minplug');
$.validator.unobtrusive.adapters.addBool('atleastoneplug');
$.validator.unobtrusive.adapters.addBool('notzeropower');
$.validator.unobtrusive.parse('form');



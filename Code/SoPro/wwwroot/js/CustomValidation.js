$.validator.addMethod('BookingStartTimeValidation', function (value, element) {
        return Date.parse(value) <= Date.now;
});
$.validator.addMethod("BookingEndTimeValidation", function (value, element, params) {
    return Date.parse(value) > Date.parse($(params).val());
});



$.validator.unobtrusive.adapters.addBool('BookingStartTimeValidation');
$.validator.unobtrusive.adapters.add('classicmovie', ['year'], function (options) {
    var element = $(options.form).find('select#Movie_Genre')[0];

    options.rules['BookingStartTimeValidation'] = [element, parseInt(options.params['year'])];
    options.messages['BookingStartTimeValidation'] = options.message;
});
// Value is the element to be validated, params is the array of name/value pairs of the parameters extracted from the HTML, element is the HTML element that the validator is attached to
booking_startTime
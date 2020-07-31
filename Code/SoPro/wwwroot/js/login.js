$(document).ready(function () {
    $('#navlinkB').click(function () {
        $('#loginBox').addClass('animate__animated animate__shakeX rounded border border-danger');
        $('.alert').fadeIn(500);
        setTimeout("$('.alert').fadeOut(1500);", 3000);
    });
    $('.form-control').keypress(function () {
        $('#loginBox').removeClass('animate__animated animate__shakeX rounded border border-danger');
    });

});
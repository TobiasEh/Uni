// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sidebar
$(document).ready(function () {

    var anzahl = 1;
    $('#sidebarCollapse').on('click', function () {

        $('#sidebar').toggleClass('active');

        var text = $(this).text();
        if (text == ">") {
            $(this).text("<");
        } else {
            $(this).text(">");
        }

    });

    $('#plus').click(function () {
        anzahl++;
        peakLayout()
    });

    $('#minus').click(function () {
        if (anzahl > 0) {
            anzahl--;
        }
        peakLayout();
    });

    function peakLayout() {
        var htmlString = "";
        for (i = 0; i < anzahl; i++) {
            htmlString = htmlString + '<div class="flex-column mb-3">' +
                '<div>Peak ' + (i + 1) + '</div>' +
                '<div class="form-group mb-2 ml-5">' +
                '<div>Zeitpunkt</div>' +
                '<input form="" type="datetime-local" class="form-control" />' +
                '<small></small>' +
                '</div>' +
                '<div class="form-group mb-2 ml-5">' +
                '<div>Dauer</div>' +
                '<input form="" class="form-control" />' +
                '<small></small>' +
                '</div>' +
                '<div class="form-group mb-2 ml-5">' +
                '<div>Spread</div>' +
                '<input form="" type="datetime-local" class="form-control" />' +
                '<small></small>' +
                '</div>' +
                '</div>';
        }
        $('#peak').html(htmlString);

    }
});
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sidebar
$(document).ready(function () {

    var anzahl = 1;
    $('#sidebarCollapse').on('click', function () {

        $('#sidebar').toggleClass('active');
        $(this).toggleClass('addPadding');
        if ($('#sidebar').hasClass("active"))
            $('#carreticon').css({ "transform": "scaleX(-1)" });
        else
            $('#carreticon').css({ "transform": "scaleX(1)" });
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
            htmlString = htmlString + '<div class="flex-column mb-3">'+
                '<div> Peak' + (i + 1) + '</div >' +
                '@if(Model.startRushours.Count==' + i +'){Model.startRushours.Add(new DateTime(0)); {Model.endRushours.Add(new DateTime(0)); model.bookingsRushours.Add(-1)}'+
                    '<div class="form-group mb-2 ml-5">'+
                        '<div>Start Zeitpunkt</div>'+
                       '<input form="create" type="datetime-local" class="form-control" asp-for="@Model.startRushours['+i+']" />'+
                        '<small></small>'+
                    '</div>'+
                    '<div class="form-group mb-2 ml-5">'+
                        '<div>Ende</div>'+
                            '<input form="create" type="time" class="form-control" asp-for="@Model.endRushours[' + i +']" />'+
                        '<small></small>'+
                            '</div>' + 
                    '<div class="form-group mb-2 ml-5">'+
                        '<div>Anzahl der Buchungen ín dieser Zeit</div>'+
                            '<input form="create" type="datetime-local" class="form-control" asp-for="@Model.bookingsRushours[' + i +']" />'+
                        '<small></small>'+
                            '</div>' + 
        '</div >';
        }
        $('#peak').html(htmlString);

    }
});

var now = new Date().toISOString().slice(0, 16);
document.getElementById("booking_startTime").min = now;
document.getElementById("booking_startTime").value = now;
document.getElementById("booking_endTime").min = now;
document.getElementById("booking_endTime").value = now;

function socMin(e) {
    document.getElementById("booking_socEnd").min = e.target.value;
    document.getElementById("booking_socEnd").value = e.target.value;
}
function endTimeMIN(e) {
    document.getElementById("booking_endTime").min = e.target.value;
    document.getElementById("booking_endTime").value = e.target.value;
}

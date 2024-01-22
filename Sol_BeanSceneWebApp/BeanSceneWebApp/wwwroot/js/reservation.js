


var WeekDays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

$(document).ready(function () {
    updateSitting();

    $('#BookingDate').change(function () {
        updateSitting();
    })

    $('#error-message').change(function () {
        alert($(this).text());
    })

    $('#number-of-guest').change(function () {
        $("#error-message").text("");
    })

    function updateSitting() {
        $.ajax({
            type: "post",
            url: "/Reservation/GetSittingsByBookingDate",
             
            data: { bookingDate: $('#BookingDate').val(), reservationId: $('#Id').val() },
            datatype: "json",
            traditional: true,
            success: function (data) {

           


                var sitting = "<select id='sittings-ddl' class='form-control' asp-for='SittingId' name='Sittings'>";
                sitting = sitting + '<option value="">--Select sitting--</option>';
                for (var i = 0; i < data.length; i++) {
                    sitting = sitting +
                        (data[i].isSelected ?  
                        '<option asp-for="SittingId"  value=' + data[i].id + ' selected >' + data[i].name :
                        '<option asp-for="SittingId" value=' + data[i].id + ' >' + data[i].name) +
                         

                        " - capacity: " + data[i].capacity +
                        " -Week day: " + WeekDays[new Date(data[i].startTime).getDay()] +
                        " - From: " + new Date(data[i].startTime).getHours() + ":" + new Date(data[i].startTime).getMinutes() +
                        " - to: " + new Date(data[i].endTime).getHours() + ":" + new Date(data[i].endTime).getMinutes() +
                        '</option>';
                }
                sitting = sitting + '</select>';
                $('#sitting-list').html(sitting);

                $('#sittings-ddl').change(function () {
                    $("#SelectedSittingId").val($(this).val());
                    $("#SelectedStartDate").val(data[this.selectedIndex - 1].startTime);
                    $("#SelectedEndDate").val(data[this.selectedIndex - 1].endTime);
                    $("#error-message").text("");
                });
            }
        });
    }
});
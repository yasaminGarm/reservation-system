document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    $('#sitting-create').hide();

    getSittingReport();

    function getSittingReport() {
        $.ajax({
            type: "get",
            url: "/Administration/Sitting/GetSittings",
            data: {},
            datatype: "json",
            traditional: true,
            success: function (data) {
                //Data from controller ready to build chart
                displayInCalendar(data);
            }
        });
    }

    $('#cancel-create-sitting').click(function () {
        $('#sitting-create').hide();
    })

    function displayInCalendar(data) {


        var sittings = data.map(function (d) { return { title: d.name, start: d.startDateTime, end: d.endDateTime } });

        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            selectable: true,
            initialDate: new Date(),
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            events: sittings,
           dateClick: function (info) {
                //alert('clicked ' + info.dateStr);
            },
            select: function (info) {
                
                $('#sitting-create').show();

                
                var startDatePicker = document.getElementById("StartDateTime");
                startDatePicker.value = info.startStr.replace('+11:00', '');

                var endDatePicker = document.getElementById("EndDateTime");
                endDatePicker.value = info.endStr.replace('+11:00', '');
                
            }
        });

        calendar.render();
    }

   

    
});
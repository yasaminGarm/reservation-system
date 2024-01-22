$(document).ready(function () {

    GetStatusReport();


    function GetStatusReport() {
        $.ajax({
            type: "get",
            url: "/Administration/StatusReport/GetStatusReport",
            data: {},
            datatype: "json",
            traditional: true,
            success: function (data) {
                //Data from controller ready to build chart
                displayChart(data);
            }
        });
    }

    function displayChart(data) {
        const ctx = document.getElementById('myChart');

        var chartLabels = data.map(d => d.statusName);
        var numberOfBooking = data.map(d => d.bookingNumber);
        

        new Chart(ctx, {
            type: 'pie',
            
            data: {
                labels: chartLabels,
                datasets: [{
                    label: '# of status',
                    data: numberOfBooking,
                    backgroundColor: ["orange", "green", "blue", "yellow", "red"],
                    borderWidth: 1
                },
                 
                ]
            },
            options: {
                //responsive: true,
                //aintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
});

$(document).ready(function () {

    getSittingReport();


    //We call a method inside controller(sittingREport) to get data, and we build chart with data
    function getSittingReport() {
        $.ajax({
            type: "get",
            url: "/Administration/SittingReport/GetSittingReport",
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
  
       var chartLabels = data.map(d => d.sittingName);
       
        var numberOfBooking = data.map(d => d.bookingNumber);
        var capacityOfSitting = data.map(d => d.capacityOfSitting);
        var RestCapacity = data.map(d => d.restCapacity);
   

        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: chartLabels,
                datasets: [{
                    label: '# Number of Reservations',
                    data: numberOfBooking,
                   

                    borderWidth: 1
                }, {
                        label: 'capacity',
                        data: capacityOfSitting,
                    borderWidth: 1 
                       
                       


                            
                    },
                    {

                        label: '# Rest of capacity',
                        data: RestCapacity,
                        borderWidth: 1


                    }



                ]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
});
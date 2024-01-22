$(() => {

    let selectedTableIds = [];

    // Use event delegation to handle clicks on .area-table elements
    $('.area-table').on('click', function () {
        let tableId = $(this).data('table-id');

        // Toggle the selection state of the clicked table
        if (selectedTableIds.includes(tableId)) {
            selectedTableIds = selectedTableIds.filter(id => id !== tableId);
            $(this).removeClass('selected');
        } else {
            selectedTableIds.push(tableId);
            $(this).addClass('selected');
        }
    });

    $('#saveButton').click(() => {
        // Get the selected table IDs and reservation ID
        let reservationId = $('#ReservationId').val();

        // Send an AJAX request to save the data
        $.ajax({
            url: '/Reservation/SaveTableSelection',
            method: 'POST',
            data: {
                tableIds: selectedTableIds,
                reservationId: reservationId
            },
            success: function (response) {
                // Handle the response from the server if needed
                console.log(response);
                alert('Table selection saved successfully');
            },
            error: function (error) {
                console.error(error);
            }
        });
    }); 



    var buttons = document.querySelectorAll('[id^="addTablesButton_"]');

    buttons.forEach(function (button) {
        var reservationTablesId = button.getAttribute('data-reservation-tables-id');

        if (reservationTablesId === "True" || reservationTablesId === "true") {
            button.setAttribute('disabled', 'true');
        }
    });


});

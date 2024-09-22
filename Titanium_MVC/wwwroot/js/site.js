//
$(document).ready(function () {
   
    $('#deleteProperty').click(function () {
        var propertyId = $('#d1').val(); // Get the integer value

        $.ajax({
            type: 'POST',
            url: '/Admin/DeleteProperty',
            data: { propertyId: propertyId }, // Send as plain data
            success: function (result) {
                alert('Successfully received Data');
                console.log(result);
            },
            error: function () {
                alert('Failed to receive the Data');
                console.log('Failed');
            }
        });
    });
    $("#productButton").click(function () {
        var form = $('#productForm')[0];
        var formData = new FormData(form);

        $.ajax({
            type: 'POST',
            url: '/Admin/AddProperty',
            data: formData,
            contentType: false, // Tell jQuery not to set contentType
            processData: false, // Tell jQuery not to process the data
            success: function (response) {
                alert('Successfully received Data: ' + response.message);
                console.log(response);
            },
            error: function () {
                alert('Failed to receive the Data');
                console.log('Failed');
            }
        });
    });

});
document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.start()
        .then(() => console.log("SignalR connection established"))
        .catch(err => console.error('SignalR Connection Error: ', err));

    connection.on("ReceiveNotification", function (message) {
        console.log('Received notification:', message);
        alert(message);
    });
});



$("#make-self-admin").click(function () {
    $.ajax({
        type: "POST",
        url: "/Account/AddThisUserToRole",
        data: {
            roleName: "Admin"
        },
        dataType: "json",
        timeout: configuration.timeout, // in milliseconds
        statusCode: {
            500: function () {
                alert("Internal Server error occurred!");
            },
            200: function () { // success
                location.reload(true);
            }
        },
        error: function (request, status, err) {
            if (status == "timeout") {
                alert("Timeout occurred sending message to web service. ");
            }
        }
    });
});

$("#manage-team-users").click(function () {
    var dataUrl = $(this).data("url");
    var partial;

    var teamId = $(this).data("team-id");

    $.get(dataUrl, function (data) {
        partial = data;
    });

    var userArray = [];

    var defaultMessage = "<i class=\"fa fa-pulse fa-spinner fa-4x\"></i>";

    var dialog = new BootstrapDialog({
        title: "Manage Team",
        message: defaultMessage,
        buttons: [{
            // button 1
            icon: "fa fa-floppy-o",
            label: "Save Changes",
            cssClass: "btn-primary",
            autospin: true,
            action: function (dialogRef) {
                dialogRef.enableButtons(false);
                dialogRef.setClosable(false);

                $("#manage-user-assigned-team").children(".user-container").each(function () {
                    userArray.push($(this).html().trim());
                });

                sendUsersToServer(userArray, teamId, dialogRef);
            }
            // button 2
        }, {
            label: 'Close',
            action: function (dialogRef) {
                dialogRef.close();
                window.location.reload();
            }
        }],
        onshown: function (dialogRef) {
            dialogRef.getModalBody().html(partial);
        }
    });
    dialog.realize();
    dialog.open();

    function sendUsersToServer(users, teamId, dialog) {
        $.ajax({
            type: "POST",
            url: "/Team/TeamUsers",
            data: {
                users: users,
                teamId: teamId
            },
            dataType: "json",
            timeout: configuration.timeout, // in milliseconds
            statusCode: {
                500: function () {
                    alert("Internal Server error occurred! ");
                },
                200: function () { // success
                    dialog.close();
                    window.location.reload();
                }
            },
            error: function (request, status, err) {
                if (status == "timeout") {
                    // reenable buttons
                    dialog.enableButtons(true);
                    dialog.setClosable(true);
                }
            }
        });
    }
});
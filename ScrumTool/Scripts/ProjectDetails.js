$("#manage-assigned-project-teams").click(function () {
    var dataUrl = $(this).data("url");
    var partial;

    var projectId = $(this).data("project-id");

    $.get(dataUrl, function (data) {
        partial = data;
    });

    var defaultMessage = "<i class=\"fa fa-pulse fa-spinner fa-4x\"></i>";

    function sendProjectDetailsToServer(teamsArray, usersArray, projectId, dialog) {
        $.ajax({
            type: "POST",
            url: "/Project/SubmitProjectTeamsUsers",
            data: {
                projectId: projectId,
                teamsArray: teamsArray,
                usersArray: usersArray
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

    var dialog = new BootstrapDialog({
        title: "Manage Assigned Project Teams and Users",
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

                var teamsArray = [];
                var usersArray = [];

                $("#manage-team-assigned-project").children(".team-container").each(function () {
                    teamsArray.push(Number($(this).children(".team-id").html()));
                });

                $("#manage-user-assigned-project").children(".user-container").each(function () {
                    usersArray.push($(this).html().trim());
                });

                sendProjectDetailsToServer(teamsArray,usersArray, projectId, dialogRef);
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

    
});
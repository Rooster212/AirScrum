$(".modify-roles-button").click(function () {
    var dataUrl = $(this).data("url");
    var partial;

    var username = $(this).data("username");

    $.get(dataUrl, function (data) {
        partial = data;
    });

    var defaultMessage = "<i class=\"fa fa-pulse fa-spinner fa-4x\"></i>";

    function sendRolesToServer(roles, username, dialog) {
        $.ajax({
            type: "POST",
            url: "/Settings/ChangeUserRoles",
            data: {
                roles: roles,
                username: username
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
        title: "Manage User Roles",
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

                var rolesArray = [];

                $("#manage-user-assigned-role").children(".role-container").each(function() {
                    rolesArray.push($(this).html().trim());
                });

                sendRolesToServer(rolesArray, username, dialogRef);
            }
            // button 2
        }, {
            label: 'Close',
            action: function (dialogRef) {
                dialogRef.close();
                window.location.reload();
            }
        }],
        onshown: function(dialogRef) {
            dialogRef.getModalBody().html(partial);
        }
    });
    dialog.realize();
    dialog.open();

    
});
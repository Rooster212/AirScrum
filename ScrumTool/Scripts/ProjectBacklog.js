$(document).ready(function () {
    $("#sortable-project-backlog").sortable({
        placeholder: "backlog-sort-placeholder",
        forcePlaceholderSize: true,
        update: function(event, ui) {
            //console.log("Testing 1");
            var priorityOrder = $("#sortable-project-backlog").sortable("serialize", {
                attribute: "data-sort-id",
                key: "id"
            });
            var splitPriority = priorityOrder.split("&");
            for (var i = 0; i < splitPriority.length; i++) {
                // have to cast to a number to allow string to be sent to server successfully
                splitPriority[i] = Number(splitPriority[i].slice(3));
            }
            $.ajax({
                type: "POST",
                url: "/BacklogItem/PriorityChange",
                data: JSON.stringify({
                    projId: $("#proj-id").html(),
                    idArray: splitPriority
                }),
                dataType: "json",
                contentType: "application/json",
                timeout: 5000, // in milliseconds
                statusCode: {
                    500: function () {
                        alert("Internal Server error occurred! ");
                    },
                    200: function () { // success
                        //alert("Success");
                        //TODO: change this to have visual feedback
                    }
                },
                error: function (request, status, err) {
                    if (status == "timeout") {
                        alert("Timeout occurred sending message to web service. ");
                    }
                }
            });
        }
    });


    $(".backlog-item-accordion").accordion({
        collapsible: true,
        active: false,
        heightStyle: "content",
        activate: function (event, ui) {
            $.ajax({
                type: "POST",
                url: "/BacklogItem/BacklogItemTasks",
                data: JSON.stringify({
                    id: Number($(this).attr("data-id"))
                }),
                dataType: "json",
                contentType: "application/json",
                timeout: configuration.timeout, // in milliseconds
                statusCode: {
                    500: function() {
                        alert("Internal Server error occurred! ");
                    },
                    200: function(data) { // success
                        $(ui.newHeader[0]).next().html(data.responseText).find(".task-list").bootstrapTable();
                    }
                },
                error: function(request, status, err) {
                    if (status == "timeout") {
                        alert("Timeout occurred sending message to web service. ");
                    }
                }
            });
        }
    });

    $("#sortable").disableSelection();

    $("#sortable-project-backlog li").contextMenu({
        menuSelector: "#contextMenu",
        menuSelected: function(invokedOn, selectedMenu) {
            //var msg = "You selected the menu item '" + selectedMenu.text() +
            //    "' on the value '" + invokedOn.text() + "'";
            //alert(msg);

            var thisItem = $(invokedOn);
            var backlogItemId = thisItem.attr("id");
            if (selectedMenu.hasClass("context-remove-item")) {
                if (confirm("Are you sure you want to remove this backlog item?")) {
                    $.ajax({
                        type: "POST",
                        url: "/BacklogItem/StateChange",
                        data: {
                            backlogItemId: backlogItemId,
                            newState: 10
                        },
                        dataType: "json",
                        timeout: 500, // in milliseconds
                        statusCode: {
                            404: function () {
                                alert("404 Internal Server error occurred! ");
                            },
                            200: function () { // success
                                $("#sortable-project-backlog li").remove("#" + backlogItemId);
                            }
                        },
                        error: function (request, status, err) {
                            if (status == "timeout") {
                                alert("Timeout occurred sending message to web service. ");
                            }
                        }
                    });
                }
            }
            else if (selectedMenu.hasClass("context-edit")) {
                location.href = "/BacklogItem/Edit/"+backlogItemId;
            }
            else if (selectedMenu.hasClass("context-details")) {
                location.href = "/BacklogItem/Details/" + backlogItemId;
            }
    }
    });
});
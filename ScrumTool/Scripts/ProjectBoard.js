$(document).ready(function() {

    $(".rear-button").button();

    $(".backlog-item").draggable({
        appendTo: "body",
        cancel: "fa", // clicking an icon won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        //helper: "clone",
        cursor: "move"
    });

    $(".backlog-item").click(function() {
        if ($(this).hasClass("ui-draggable-dragging")) {
            return;
        } else {
            $(this).find(".flipper").toggleClass("flip");
        }
    });

    $(".rear-button-details").click(function () {
        location.href = "/BacklogItem/Details/" + $(this).attr("data-backlog-id");
    });

    $(".rear-button-edit").click(function() {
        location.href = "/BacklogItem/Edit/" + $(this).attr("data-backlog-id");
    });

    $(".rear-button-remove").click(function () {
        var backlogItemId = $(this).attr("data-backlog-id");
        if (confirm("Are you sure you want to remove this backlog item?")) {
            $.ajax({
                type: "POST",
                url: "/BacklogItem/StateChange",
                data: {
                    backlogItemId: backlogItemId,
                    newState: 10
                },
                dataType: "json",
                timeout: configuration.timeout, // in milliseconds
                statusCode: {
                    404: function () {
                        alert("404 Internal Server error occurred! ");
                    },
                    200: function () { // success
                        $(".backlog-item").remove("#" + backlogItemId);
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


    $(".board-area").droppable({
        accept: ".backlog-item",
        tolerance: "pointer",
        greedy: true,
        activeClass: "custom-state-active",
        hoverClass: "ui-state-highlight",
        drop: function (event, ui) {
            // send request to server
            $.ajax({
                type: "POST",
                url: "/BacklogItem/StateChange",
                data: {
                    backlogItemId: Number(ui.draggable.attr("id")),
                    newState: Number($(event.target).attr("data-state"))
                },
                dataType: "json",
                timeout: configuration.timeout, // in milliseconds
                statusCode: {
                    404: function () {
                        alert("404 Internal Server error occurred! ");
                    },
                    200: function () { // success
                        var $draggable = ui.draggable;
                        // we detach it so that we ignore the jQuery way of dropping it where the mouse pointer is
                        $draggable.detach().css({ top: 0, left: 0 }).appendTo(event.target);
                        $(document).remove($draggable);
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
});
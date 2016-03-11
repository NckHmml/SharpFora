function ChatController() {
    var $ctrl = this;
    var location = window.location,
        protocol;
    if (location.protocol === "http:") {
        protocol = "ws:";
    } else {
        protocol = "wss:";
    }

    var connection = new WebSocket(protocol + "//" + location.host);
    connection.onopen = function () {
        connection.send("Hello");
    };
}

var messageInput = document.getElementById('messageInput');
var name = prompt('Enter your name:', '');
messageInput.focus();
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44375/MessHub").build();

connection.on("Message", function (user, message) {
    var list = document.getElementById("messagesList");
    if (user == name) {
        user = "You";
        list.innerHTML = list.innerHTML + "<div class='chat-message-right'>"
            + "<div class='card-body'>"
            + "<h6 class='card-subtitle float-right'>" + `${user}` + "</h6><br>"
            + "<p class='card-text float-right'>"
            + "<div class='flex-shrink-1 bg-light rounded'>" + `${message}` + "</div>"
            + "</p>"
            + "</div > "
            +"</div>";
    }
    else {
        list.innerHTML = list.innerHTML + "<div class='chat-message-left'>"
            + "<div class='card-body'>"
            + "<h6 class='card-subtitle float-left'>" + `${user}` + "</h6><br>"
            + "<p class='card-text float-left'>"
            + "<div class='flex-shrink-1 bg-light rounded'>" + `${message}` + "</div>"
            + "</p>"
            + "</div > "
            + "</div>";
    }

});
connection.start();

document.getElementById("sendButton").addEventListener("click", function (event) {
    if (messageInput.value != null && messageInput.value != "") {
        connection.invoke("Send", name, messageInput.value);
        messageInput.value = "";
    }
    event.preventDefault();
});
messageInput.addEventListener("keypress", function (event) {
    if (event.key == "Enter") {
        event.preventDefault();
        document.getElementById("sendButton").click();
    }

})


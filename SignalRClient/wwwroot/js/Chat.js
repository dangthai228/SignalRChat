
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44375/MessHub").build();

connection.on("Message", function (user, message) {
   
    var list = document.createElement("ds");
    document.getElementById("messagesList").appendChild(list);
    list.textContent = `${user} says ${message}`;
});
connection.start();

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var msg = document.getElementById("messageInput").value;
    connection.invoke("Send", user, msg);
    event.preventDefault();
});
